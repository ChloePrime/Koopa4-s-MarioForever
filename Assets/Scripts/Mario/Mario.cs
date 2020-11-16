using SweetMoleHouse.MarioForever.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 马里奥主物体
    /// </summary>
    public class Mario : MonoBehaviour
    {
        #region 子对象引用
        private Transform Hitboxes { get; set; }
        public MarioMove Mover { get; private set; }
        public MarioJump Jumper { get; private set; }
        public MarioCrouch Croucher { get; private set; }
        private SizeProfile Profile { get => transform.GetChild((int)size).GetComponent<SizeProfile>(); }
        public Transform Center { get => Profile.Center; }
        public readonly Dictionary<MarioSize, Collider2D> sizes = new Dictionary<MarioSize, Collider2D>();

        public Animator Anims { get; private set; }
        public SpriteRenderer Renderer { get; private set; }
        #endregion

        public static float DeltaSizeSmallToBig { get; private set; }

        #region 字段，部分摘自HelloMarioEngine

        [SerializeField, RenameInInspector("马里奥状态")]
        private MarioPowerup powerup;
        public MarioPowerup Powerup
        {
            get => powerup; set
            {
                powerup = value;
                Size = (value == MarioPowerup.SMALL) ? MarioSize.SMALL : MarioSize.BIG;
            }
        }
        public MarioState State { get; set; }
        public int Dir { get; set; }

        public bool IsSkidding { get; set; }
        public bool SwimmingNow { get; set; }
        public bool Crouching { get => Croucher.Crouching; }
        public MarioHoldingState Holding { get; set; }
        public MarioStompStyle StompStyle { get; set; }
        /// <summary>
        /// 摩擦系数，越小越滑
        /// </summary>
        public float Friction { get; set; }
        public bool ControlDisabled { get; set; } = false;
        public bool IsWallJumping { get; set; }
        public bool IsFlashing { get => FlashTime > 0; }
        public float FlashTime { get; private set; }
        /// <summary>
        /// 连踩计数
        /// </summary>
        public int ConsecutiveJumpCount { get; set; }
        #endregion

        [Header("音效设置")]
        [SerializeField, RenameInInspector("受伤音效")]
        private AudioClip hurtSound = null;

        private MarioSize size;
        public MarioSize Size
        {
            get => size; 
            set
            {
                size = value;
                int i = 0;
                foreach (Transform obj in Hitboxes)
                {
                    obj.gameObject.SetActive(i == (int)value);
                    i++;
                }
            }
        }
        public void RefreshSize()
        {
            Size = GetRealSize();
        }
        /// <summary>
        /// 获取非下蹲时的马里奥个子
        /// </summary>
        /// <returns>非下蹲时的马里奥个子</returns>
        public MarioSize GetRealSize()
        {
            return (Powerup == MarioPowerup.SMALL) ? MarioSize.SMALL : MarioSize.BIG;
        }

        public void Damage(in float flashTime = 2)
        {
            if (FlashTime > 0)
            {
                return;
            }
            if (Powerup == MarioPowerup.SMALL)
            {
                Kill();
            }
            else
            {
                Global.PlaySound(hurtSound);
                Powerup = (Powerup == MarioPowerup.BIG) ? MarioPowerup.SMALL : MarioPowerup.BIG;
                FlashTime = flashTime;
            }
        }

        public void OnStomp(in float power, bool shouldCalcCombo)
        {
            Jumper.Jump(power);
        }

        protected static float INVUL_CYCLE = 0.2f;
        private void Update()
        {
            if (IsFlashing)
            {
                var color = Renderer.material.color;
                // 0 ~ INVUL_CYCLE
                var alpha = Time.time % INVUL_CYCLE;
                if (alpha > INVUL_CYCLE / 2)
                {
                    alpha = INVUL_CYCLE - alpha;
                }
                alpha *= 2 / INVUL_CYCLE;
                color.a = alpha;
                FlashTime -= Time.deltaTime;
                if (FlashTime < 0)
                {
                    color.a = 1;
                }
                Renderer.material.color = color;
            }
        }

        public void Kill()
        {

        }

        private void Start()
        {
            Hitboxes = transform.GetChild(0);
            //子脚本的引用
            Mover = GetComponent<MarioMove>();
            Jumper = GetComponent<MarioJump>();
            Croucher = transform.GetComponent<MarioCrouch>();

            //附属组件
            Anims = transform.GetChild(1).GetComponent<Animator>();
            Renderer = Anims.GetComponent<SpriteRenderer>();
            foreach (MarioSize item in Enum.GetValues(typeof(MarioSize)))
            {
                sizes.Add(item, Hitboxes.GetChild((int)item).GetChild(0).GetComponent<Collider2D>());
            }
            DeltaSizeSmallToBig = sizes[MarioSize.BIG].bounds.size.y - sizes[MarioSize.SMALL].bounds.size.y;

            InitDiffBetweenEditAndRun();
            RefreshSize();
        }

        /// <summary>
        /// 处理编辑器和运行时的差异
        /// 包括坐标偏差：
        ///     如果不包括此项，马里奥的出生点会高半格
        /// 动画切换：
        ///     运行时删掉在编辑器模式下显示的尸体贴图（挂在root上的 <see cref="SpriteRenderer"/>），
        /// </summary>
        private void InitDiffBetweenEditAndRun()
        {
            transform.Translate(0, -0.5f + Consts.ONE_PIXEL, 0);
        }
    }
}