using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 一个可以顶的东西
    /// </summary>
    public class QuestionBlock : MonoBehaviour, IHitable
    {
        protected new Animator animation;
        protected new SpriteRenderer renderer;
        [CanBeNull, SerializeField, RenameInInspector("顶后的残余")]
        protected GameObject afterHit;
        [SerializeField, RenameInInspector("顶出的物品")]
        protected GameObject[] outputs;
        [SerializeField, RenameInInspector("音效")]
        protected AudioClip sfx = null;

        protected int outputIndex = 0;
        protected static float ANIM_LEN = 0.3f;
        private float cd;
        private Vector2 size;

        protected virtual void Start()
        {
            animation = GetComponentInChildren<Animator>();
            renderer = GetComponentInChildren<SpriteRenderer>();
            var c2d = GetComponent<Collider2D>();
            size = c2d.bounds.max - c2d.bounds.min;
            if (afterHit != null)
            {
                CreateChild(ref afterHit, 1);
            }
            for (int i = 0; i < outputs.Length; i++)
            {
                CreateChild(ref outputs[i], i + 2);
            }
        }
        private void CreateChild(ref GameObject input, int delta)
        {
            var cloned = Instantiate(input, transform.parent);
            cloned.transform.position = transform.position;
            cloned.SetActive(false);
            var thatSr = cloned.TryGetComponent(out SpriteRenderer sr) 
                ? sr : cloned.GetComponentInChildren<SpriteRenderer>();
            if (thatSr != null)
            {
                thatSr.sortingLayerID = renderer.sortingLayerID;
                thatSr.sortingOrder = renderer.sortingOrder - delta;
            }
            input = cloned;
        }
        public virtual bool OnHit(Transform hitter)
        {
            if (cd > 0)
            {
                return true;
            }
            if (outputIndex < outputs.Length)
            {
                StartCoroutine(ProcessAppearing(outputs[outputIndex]));
                animation.SetTrigger("顶起");
                cd = ANIM_LEN;
                ++outputIndex;
            }
            if ((outputIndex >= outputs.Length) && (afterHit != null))
            {
                afterHit.SetActive(true);
                renderer.enabled = false;
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
            yield return new WaitForSeconds(ANIM_LEN);
            if (sfx != null)
            {
                Global.PlaySound(sfx);
            }
            bonus.SetActive(true);
            var apperable = bonus.GetComponent<IAppearable>();
            if (apperable != null)
            {
                apperable.Appear(Vector2.up, size);
            }
        }
    }
} 