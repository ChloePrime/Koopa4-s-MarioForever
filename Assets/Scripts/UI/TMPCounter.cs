using TMPro;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.UI
{
    public class TMPCounter : MonoBehaviour
    {
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
        
        protected virtual void Start()
        {
            tmp = GetComponent<TMP_Text>();
            foreach (var obj in GetComponents<MonoBehaviour>())
            {
                print(obj.GetType().ToString());
            }
        }
    }
}
