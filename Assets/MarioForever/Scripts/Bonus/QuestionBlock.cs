using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Base;
using System.Collections;
using SweetMoleHouse.MarioForever.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Bonus
{
    /// <summary>
    /// 一个可以顶的东西
    /// </summary>
    public class QuestionBlock : MonoBehaviour, IHitable
    {
        protected Animator Animation;
        protected SpriteRenderer Renderer;
        [CanBeNull, SerializeField, RenameInInspector("顶后的残余")]
        protected GameObject afterHit;
        [SerializeField, RenameInInspector("顶出的物品")]
        protected GameObject[] outputs;
        [SerializeField, RenameInInspector("音效")]
        protected AudioClip sfx;

        protected int OutputIndex;
        protected const float AnimLen = 0.3f;
        private float cd;
        private Vector2 size;
        private static readonly int BLOCK_HIT_ANIM = Animator.StringToHash("顶起");

        protected virtual void Start()
        {
            Animation = GetComponentInChildren<Animator>();
            Renderer = GetComponentInChildren<SpriteRenderer>();
            var bounds = GetComponent<Collider2D>().bounds;
            size = bounds.size;
            // 创建子对象
            // 隐藏块顶后的方块，隐藏块里的内容物
            if (afterHit != null)
            {
                CreateChild(ref afterHit, 1);
            }
            for (int i = 0; i < outputs.Length; i++)
            {
                CreateChild(ref outputs[i], i + 2);
            }
        }
        private void CreateChild(ref GameObject input,in int delta)
        {
            var cloned = Instantiate(input, transform.parent);
            
            // 设置y坐标为问号块顶部
            cloned.transform.position = transform.position;
            cloned.SetActive(false);
            MoveToBack(cloned, delta);
            
            input = cloned;
        }

        private void MoveToBack(in GameObject obj, in int delta)
        {
            var thatSr = obj.TryGetComponent(out SpriteRenderer sr)
                ? sr : obj.GetComponentInChildren<SpriteRenderer>();
            if (thatSr == null) return;
            thatSr.sortingLayerID = Renderer.sortingLayerID;
            thatSr.sortingOrder = Renderer.sortingOrder - delta;
        }
        public virtual bool OnHit(Transform hitter)
        {
            if (cd > 0)
            {
                return true;
            }
            if (OutputIndex < outputs.Length)
            {
                StartCoroutine(ProcessAppearing(outputs[OutputIndex]));
                Animation.SetTrigger(BLOCK_HIT_ANIM);
                cd = AnimLen;
                ++OutputIndex;
            }
            if (OutputIndex >= outputs.Length && afterHit != null)
            {
                afterHit.SetActive(true);
                Renderer.enabled = false;
            }
            return false;
        }

        private void FixedUpdate()
        {
            if (cd >= 0)
            {
                cd -= Time.fixedDeltaTime;
            }
        }

        private IEnumerator ProcessAppearing(GameObject bonus)
        {
            yield return new WaitForSeconds(AnimLen);
            if (sfx != null)
            {
                Global.PlaySound(sfx);
            }
            bonus.SetActive(true);
            var yOffset = bonus.TryGetComponent(out Rigidbody2D r2d) ? MFUtil.Height(r2d) : size.y;
            bonus.transform.Translate(0, -yOffset, 0);
            
            var appearable = bonus.GetComponent<IAppearable>();
            appearable?.Appear(Vector2.up, new Vector2(0, yOffset));
        }
    }
}