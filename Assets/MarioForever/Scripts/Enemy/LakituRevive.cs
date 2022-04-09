using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
public class LakituRevive : MonoBehaviour {
    /// <summary>
    /// 复活等待时间
    /// 单位：秒
    /// </summary>
    [SerializeField]
    private float reviveDelay = 5;

    [Header("高级设置")] [SerializeField] private DamageReceiver myDamageHandler;

    private void Awake() {
        myDamageHandler.OnDeath += _ => {
            if (enabled) {
                var myTransform = transform;
                ScheduleReviveAsync(
                    reviveDelay, _newLakitu, myTransform.parent,
                    myTransform.position, myTransform.rotation,
                    SceneManager.GetActiveScene()
                );
            }
            return ActionResult.PASS;
        };
    }

    private void Start() {
        // 关卡开始复制自己，以备复活
        _newLakitu = Instantiate(gameObject, transform.parent);
        _newLakitu.SetActive(false);
    }

    /* 快乐云死后会被销毁，所以要把复活需要的信息作为函数参数传递 */
    private static async void ScheduleReviveAsync(
        float delay,
        GameObject newLakitu, Transform parent,
        Vector3 deathAt, Quaternion rotation,
        Scene sceneWhereLakituDied
    ) {
        if (delay > 0) {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }

        if (newLakitu == null) {
            return;
        }

        // 等待复活的过程中切换了 Scene, 终止复活。
        if (SceneManager.GetActiveScene() != sceneWhereLakituDied) {
            return;
        }

        // 复活后快乐云的位置
        // 离离屏幕右侧的距离
        // 单位：米
        const float offset = 6;

        Vector3 spawnLocation = deathAt;
        spawnLocation.x = ScrollInfo.Right + offset;

        newLakitu.transform.position = spawnLocation;
        newLakitu.transform.rotation = rotation;
        newLakitu.transform.parent = parent;
        newLakitu.SetActive(true);
    }

    private GameObject _newLakitu;
}
}