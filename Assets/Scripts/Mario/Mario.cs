using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Player
{
    /// <summary>
    /// 
    /// </summary>
    public class Mario : MonoBehaviour
    {
        #region 子对象引用
        private Transform Hitboxes { get; set; }
        private SizeProfile Profile { get => transform.GetChild((int)size).GetComponent<SizeProfile>(); }
        public Collider2D PhysicHitbox { get => Profile.PhysicHitbox.GetComponent<Collider2D>(); }
        public Transform Center { get => Profile.Center; }
        public Collider2D DamageHitbox { get => Profile.DamageHitbox.GetComponent<Collider2D>(); }

        public Transform Anims { get; set; }
        #endregion

        #region 字段，部分摘自HelloMarioEngine
        public MarioState State { get; set; }
        public int Dir { get; set; };

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
        private void SetSize(MarioSize size)
        {
            int i = 0;
            foreach (Transform obj in Hitboxes)
            {
                obj.gameObject.SetActive(i == (int)size);
                i++;
            }
        }
        private void Start()
        {
            Hitboxes = transform.GetChild(0);
            Anims = transform.GetChild(1);
            Size = MarioSize.BIG;

            InitDiffBetweenEditAndRun();

            Anims.GetChild(0).gameObject.SetActive(true);
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
            Destroy(GetComponent<SpriteRenderer>());
        }
    }
}