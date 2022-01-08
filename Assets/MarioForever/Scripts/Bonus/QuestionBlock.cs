using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 一个可以顶的东西
/// </summary>
public class QuestionBlock : HittableBase {

    [CanBeNull, SerializeField, RenameInInspector("顶后的残余")]
    protected GameObject afterHit;

    [SerializeField, RenameInInspector("顶出的物品")]
    protected GameObject[] outputs;

    [SerializeField, RenameInInspector("音效")]
    protected AudioClip sfx;
    
    protected Animator Animation;
    protected SpriteRenderer Renderer;

    protected int OutputIndex;
    protected static readonly TimeSpan AnimLen = TimeSpan.FromSeconds(0.3f);
    private bool _ready = true;
    private Vector2 _size;
    private static readonly int BlockHitAnim = Animator.StringToHash("顶起");

    protected override void Awake() {
        base.Awake();
        
        Animation = GetComponentInChildren<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        var bounds = GetComponent<Collider2D>().bounds;
        _size = bounds.size;
        // 创建子对象
        // 隐藏块顶后的方块，隐藏块里的内容物
        if (afterHit != null) {
            CreateChild(ref afterHit, 1);
        }

        for (int i = 0; i < outputs.Length; i++) {
            CreateChild(ref outputs[i], i + 2);
        }
    }

    private void CreateChild(ref GameObject input, int depth) {
        GameObject cloned = Instantiate(input, transform.parent);

        // 设置y坐标为问号块顶部
        cloned.transform.position = transform.position;
        cloned.SetActive(false);
        MoveToBack(cloned, depth);

        input = cloned;
    }

    private void MoveToBack(in GameObject obj, in int delta) {
        var thatSr = obj.TryGetComponent(out SpriteRenderer sr)
            ? sr
            : obj.GetComponentInChildren<SpriteRenderer>();
        if (thatSr == null) return;
        thatSr.sortingLayerID = Renderer.sortingLayerID;
        thatSr.sortingOrder = Renderer.sortingOrder - delta;
    }

    public override bool OnHit(Transform hitter) {
        if (!_ready) {
            return true;
        }

        if (OutputIndex < outputs.Length) {
            SpawnContent(outputs[OutputIndex]);
            Animation.SetTrigger(BlockHitAnim);
            DamageEnemiesAbove();
            StartCooldown();
            ++OutputIndex;
        }

        if (OutputIndex >= outputs.Length && afterHit != null) {
            afterHit.SetActive(true);
            Renderer.enabled = false;
        }

        return false;
    }

    private async void StartCooldown() {
        _ready = false;
        await UniTask.Delay(AnimLen);
        _ready = true;
    }

    private async void SpawnContent(GameObject bonus) {
        await UniTask.Delay(AnimLen);
        
        if (sfx != null) {
            Global.PlaySound(sfx);
        }

        bonus.SetActive(true);
        var yOffset = bonus.TryGetComponent(out Rigidbody2D r2d) ? MFUtil.Height(r2d) : _size.y;
        bonus.transform.Translate(0, -yOffset, 0);

        if (bonus.TryGetComponent(out IAppearable appearable)) {
            appearable.Appear(Vector2.up, new Vector2(0, yOffset));
        }
    }
}
}