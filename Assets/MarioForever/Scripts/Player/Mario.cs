using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Effect;
using SweetMoleHouse.MarioForever.Scripts.Enemy;
using SweetMoleHouse.MarioForever.Scripts.Level;
using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Player.Ability;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player {
/// <summary>
/// 马里奥主物体
/// </summary>
public class Mario : MonoBehaviour {
    [Header("音效设置")]
    [SerializeField, RenameInInspector("受伤音效")]
    private AudioClip hurtSound;

    [SerializeField, RenameInInspector("死亡音效")]
    private AudioClip deathSound;

    [SerializeField] private GameObject corpse;
    [SerializeField] private DamageSource stompSource;

    /// <summary>
    /// 掉悬崖死亡判定线和屏幕底部的距离差
    /// </summary>
    private const float CliffKillBias = 2F;

    #region 子对象引用

    private Transform Hitboxes { get; set; }
    public MarioMove Mover { get; private set; }
    public MarioJump Jumper { get; private set; }
    public MarioCrouch Croucher { get; private set; }
    public ComboCalculator ComboInfo { get; private set; }
    public readonly Dictionary<MarioSize, Collider2D> Sizes = new();

    public MarioAnim Anims { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Flashing FlashCtrl { get; private set; }

    public AbilityManager PowerupManager { get; private set; }

    public DamageSource StompDamageSource => stompSource;

    #endregion

    public static float DeltaSizeSmallToBig { get; private set; }

    #region 字段，部分摘自HelloMarioEngine

    public MarioPowerup Powerup {
        get => MarioProperty.CurPowerup;
        private set {
            MarioProperty.CurPowerup = value;
            Size = value == MarioPowerup.SMALL ? MarioSize.SMALL : MarioSize.BIG;
        }
    }

    public int Direction { get; set; }
    public MarioState State { get; set; }

    public bool IsSkidding { get; set; }
    public bool SwimmingNow { get; set; }
    public bool Crouching => Croucher.Crouching;
    public MarioHoldingState Holding { get; set; }
    public MarioStompStyle StompStyle { get; set; }

    /// <summary>
    /// 摩擦系数，越小越滑
    /// </summary>
    public float Friction { get; set; }

    public bool ControlDisabled { get; set; }
    public bool IsWallJumping { get; set; }
    public bool Invulnerable => FlashCtrl.FlashTime > 0 && FlashIsInvul;
    private bool FlashIsInvul { get; set; }
    private readonly HashSet<Transform> invulnerableFrom = new HashSet<Transform>();

    #endregion


    private MarioSize size;

    public MarioSize Size {
        get => size;
        set {
            size = value;
            int i = 0;
            foreach (Transform obj in Hitboxes) {
                obj.gameObject.SetActive(i == (int)value);
                i++;
            }
        }
    }

    public void RefreshSize() {
        Size = GetRealSize();
    }

    /// <summary>
    /// 获取非下蹲时的马里奥个子
    /// </summary>
    /// <returns>非下蹲时的马里奥个子</returns>
    public MarioSize GetRealSize() {
        return Powerup == MarioPowerup.SMALL ? MarioSize.SMALL : MarioSize.BIG;
    }

    public void Damage(DamageSource damager, float flashTime = 2) {
        if (Invulnerable || invulnerableFrom.Contains(damager.Host)) {
            return;
        }

        if (Powerup == MarioPowerup.SMALL) {
            Kill();
        } else {
            Global.PlaySound(hurtSound);
            SetPowerup(Powerup == MarioPowerup.BIG ? MarioPowerup.SMALL : MarioPowerup.BIG);

            FlashCtrl.FlashTime = flashTime;
            FlashIsInvul = true;
        }
    }

    public async void SetInvulnerableFrom(Transform targetHost, TimeSpan time) {
        invulnerableFrom.Add(targetHost);
        await UniTask.Delay(time);
        invulnerableFrom.Remove(targetHost);
    }

    public void SetPowerup(MarioPowerup target, float rainbowTime = 0) {
        if (Powerup == target) return;
        Powerup = target;

        if (rainbowTime > 0) {
            FlashCtrl.RainbowFlashTime = rainbowTime;
        }

        PowerupManager.SetPowerup(target);

        // 检测马里奥大小，
        // 以防止小个子变大个子时卡在墙里
        if (Mover.OverlappingAnything()) {
            Croucher.Crouching = true;
        }
    }

    public void OnStomp(in float power, bool shouldCalcCombo) {
        Jumper.Jump(power);
        if (shouldCalcCombo) {
            ComboInfo.Hit(transform);
        }
    }

    public void Kill() {
        corpse = Instantiate(corpse);
        corpse.transform.position = transform.position;
        Global.PlaySound(deathSound);
        FindObjectOfType<MarioCamera>().Stop();
        Destroy(gameObject);

        Powerup = MarioPowerup.SMALL;
    }

    private void Awake() {
        InitChildren();
    }

    private void Start() {
        // 刷新玩家能力
        PowerupManager.SetPowerup(Powerup);
        RefreshSize();
    }

    private void InitChildren() {
        Hitboxes = transform.GetChild(0);
        //子脚本的引用
        Mover = GetComponent<MarioMove>();
        Jumper = GetComponent<MarioJump>();
        Croucher = transform.GetComponent<MarioCrouch>();
        ComboInfo = GetComponent<ComboCalculator>();

        //附属组件
        Anims = transform.GetChild(1).GetComponent<MarioAnim>();
        Renderer = Anims.GetComponent<SpriteRenderer>();
        FlashCtrl = Anims.GetComponent<Flashing>();
        PowerupManager = GetComponentInChildren<AbilityManager>();
        foreach (MarioSize item in Enum.GetValues(typeof(MarioSize))) {
            Sizes.Add(item, Hitboxes.GetChild((int)item).GetChild(0).GetComponent<Collider2D>());
        }

        DeltaSizeSmallToBig = Sizes[MarioSize.BIG].bounds.size.y - Sizes[MarioSize.SMALL].bounds.size.y;
    }

    private void FixedUpdate() {
        if (transform.position.y <= ScrollInfo.Bottom - CliffKillBias) {
            Kill();
        }
    }
}
}