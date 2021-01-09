using TMPro;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.UI
{
    /// <summary>
    /// 使用 TextMeshPro 显示数字的计数器
    /// </summary>
    public class Counter : MonoBehaviour
    {
        [SerializeField]
        private long value;
        public long Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    tmp.text = value.ToString();
                }
                this.value = value;
            }
        }

        private TMP_Text tmp;
        
        public virtual void Start()
        {
            tmp = GetComponent<TMP_Text>();
        }
        
        public string DisplayText
        {
            set => tmp.text = value;
        }
    }
}
