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
    public class BaseHitable : MonoBehaviour, IHitable
    {
        protected new Animator animation;
        protected new SpriteRenderer renderer;
        [CanBeNull, SerializeField, RenameInInspector("顶后的残余")]
        protected GameObject afterHit;
        [SerializeField, RenameInInspector("顶出的物品")]
        protected GameObject[] outputs;
        protected int outputIndex = 0;

        protected virtual void Start()
        {
            animation = GetComponentInChildren<Animator>();
            renderer = GetComponentInChildren<SpriteRenderer>();
            if (afterHit != null)
            {
                CreateChild(ref afterHit);
            }
            for (int i = 0; i < outputs.Length; i++)
            {
                CreateChild(ref outputs[i]);
            }
        }
        private void CreateChild(ref GameObject input)
        {
            var cloned = Instantiate(input, transform.parent);
            cloned.transform.position = transform.position;
            cloned.SetActive(false);
            var thatSr = cloned.TryGetComponent(out SpriteRenderer sr) 
                ? sr : cloned.GetComponentInChildren<SpriteRenderer>();
            if (thatSr != null)
            {
                thatSr.sortingLayerID = renderer.sortingLayerID;
                thatSr.sortingOrder = renderer.sortingOrder - 1;
            }
            input = cloned;
        }
        public virtual bool OnHit(Transform hitter)
        {
            if (outputIndex < outputs.Length)
            {
                animation.SetTrigger("顶起");
                print("顶起");
                outputs[outputIndex].SetActive(true);
                ++outputIndex;
            }
            if ((outputIndex >= outputs.Length) && (afterHit != null))
            {
                afterHit.SetActive(true);
                Destroy(gameObject);
            }
            return true;
        }
    }
}