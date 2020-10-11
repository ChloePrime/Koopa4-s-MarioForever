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
        private SizeProfile Profile { get => transform.GetChild((int)size).GetComponent<SizeProfile>(); }
        public Transform Center { get => Profile.Center; }
        public readonly Dictionary<MarioSize, Collider2D> sizes = new Dictionary<MarioSize, Collider2D>();

        public Animator Anims { get; private set; }
        public AudioSource SoundPlayer { get; private set; }
        #endregion

        public static float DeltaSizeSmallToBig { get; private set; }

        #region 字段，部分摘自HelloMarioEngine
        [SerializeField, RenameInInspector("马里奥状态")]
        private MarioPowerup powerup;
        public MarioPowerup Powerup { get => powerup; set => powerup = value; }
        public MarioState State { get; set; }
        public int Dir { get; set; }

        public bool IsSkidding { get; set; }
        public bool SwimmingNow { get; set; }
        public bool DuckingNow { get; set; }
        public MarioHoldingState Holding { get; set; }
        public MarioStompStyle StompStyle { get; set; }
        /// <summary>
        /// 摩擦系数，越小越滑
        /// </summary>
        public float Friction { get; set; }
        public bool ControlDisabled { get; set; }
        public bool IsWallJumping { get; set; }
        public bool IsFlashing { get; set; }
        /// <summary>
        /// 连踩计数
        /// </summary>
        public int ConsecutiveJumpCount { get; set; }
        #endregion
        private MarioSize size;
        public MarioSize Size
        {
            get => size; set
            {
                size = value;
                SetSize(size);
            }
        }
        public void RefreshSize()
        {
            Size = GetRealSize();
        }
        private void SetSize(MarioSize size)
        {
            int i = 0;
            foreach (Transform obj in Hitboxes)
            {
                obj.gameObject.SetActive(i == (int)size);
                i++;
            }
        }
        public MarioSize GetRealSize()
        {
            return (Powerup == MarioPowerup.SMALL) ? MarioSize.SMALL : MarioSize.BIG;
        }

        public void PlaySound(AudioClip sample)
        {
            SoundPlayer.PlayOneShot(sample);
        }
        private void Start()
        {
            Hitboxes = transform.GetChild(0);
            Mover = GetComponent<MarioMove>();
            Jumper = GetComponent<MarioJump>();
            Anims = transform.GetChild(1).GetComponent<Animator>();
            SoundPlayer = GetComponent<AudioSource>();
            foreach (MarioSize item in Enum.GetValues(typeof(MarioSize)))
            {
                sizes.Add(item, Hitboxes.GetChild((int)item).GetChild(0).GetComponent<Collider2D>());
            }
            DeltaSizeSmallToBig = sizes[MarioSize.BIG].bounds.size.y - sizes[MarioSize.SMALL].bounds.size.y;

            InitDiffBetweenEditAndRun();
            Size = MarioSize.BIG;
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