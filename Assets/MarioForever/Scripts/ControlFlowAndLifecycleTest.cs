using UnityEngine;

namespace SweetMoleHouse
{
    /// <summary>
    /// 测试 
    /// </summary>
    public class ControlFlowAndLifecycleTest : MonoBehaviour
    {
        private static bool _initted = false;

        private void Awake()
        {
            DoTest("Awaking", "Awaken");
        }

        private void Start()
        {
            DoTest("Starting", "Started");
        }

        private void DoTest(string ing, string ed)
        {
            print($"{gameObject.name} {ing}");
            
            if (!_initted)
            {
                _initted = true;
                GameObject other = Instantiate(gameObject);
                other.name = gameObject.name + " (2)";
            }

            print($"{gameObject.name} {ed}");
        }
    }
}
