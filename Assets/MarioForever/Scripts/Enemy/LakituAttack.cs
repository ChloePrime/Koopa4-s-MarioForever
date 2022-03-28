using System;
using Cysharp.Threading.Tasks;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
/// <summary>
/// 扔刺猬
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class LakituAttack : MonoBehaviour {
    [SerializeField] private float minimumAttackDelay = 4;
    [SerializeField] private float maximumAttackDelay = 6;

    [SerializeField, Min(0.2f)]
    private float chargeTime = 0.8f;

    [SerializeField] private GameObject projectileObject;
    [SerializeField] private float projectileSpeed = 4.5f;

    [Header("高级设置")] [SerializeField] private Transform muzzle;
    
    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        
        if (ReferenceEquals(muzzle, null)) {
            Debug.LogError("Lakitu's muzzle object is not assigned!");
        }
    }

    private void Start() {
        RefreshAttackDelay();
    }

    private void RefreshAttackDelay() {
        _etaNextAttack = Time.time + Random.Range(minimumAttackDelay, maximumAttackDelay);
    }

    private void FixedUpdate() {
        if (Time.time < _etaNextAttack) {
            return;
        }
        if (transform.DistanceOutOfScreen() > 0) {
            _etaNextAttack = Time.time + ScanDelay;
        }
        _etaNextAttack = float.PositiveInfinity;
        ThrowHedgehogAsync();
    }

    private async void ThrowHedgehogAsync() {
        while (true) {
            // 蓄力动画（压下头）
            _animator.SetTrigger(StartChargingTrigger);
            await UniTask.Delay(TimeSpan.FromSeconds(chargeTime));
            if (this == null) {
                return;
            }
            // 如果自己位于实心中，则等待至自身未与任何实心碰撞。
            await WaitUntilNotCollided();
            if (this == null) {
                return;
            }
            // 弹起动画
            _animator.SetTrigger(EndChargingTrigger);
            const float endChargeToThrowDelay = 0.2f;
            await UniTask.Delay(TimeSpan.FromSeconds(endChargeToThrowDelay));
            if (this == null) {
                return;
            }
            // 如果延迟过后本刺猬云与实心发生重叠，则再次按下头并等待自己离开实心。
            if (IsOverlappingAnything()) {
                continue;
            }
            break;
        }
        // 创建刺猬
        CreateHedgehogObject();
        // 刷新下次攻击计时
        RefreshAttackDelay();
    }

    private async UniTask WaitUntilNotCollided() {
        while (IsOverlappingAnything()) {
            await UniTask.Delay(TimeSpan.FromSeconds(ScanDelay));
        }
    }

    private bool IsOverlappingAnything() {
        if (this == null) {
            return false;
        }
        return _rigidbody.OverlapCollider(BasePhysics.GlobalFilter, OverlapBuffer) > 0;
    }

    private void CreateHedgehogObject() {
        var myTransform = transform;
        GameObject hedgehog = Instantiate(projectileObject, muzzle.transform.position, myTransform.rotation, myTransform.parent);
        if (hedgehog.TryGetComponent(out BasePhysics bp)) {
            // MF Unity 定制物理
            bp.YSpeed = projectileSpeed;
        } else if (hedgehog.TryGetComponent(out Rigidbody2D r2d) && r2d.bodyType == RigidbodyType2D.Dynamic) {
            // Unity 原生物理
            r2d.velocity = new Vector2(0, projectileSpeed);
        }
    }

    private float _etaNextAttack;
    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private const float ScanDelay = 0.25f;
    private static readonly int StartChargingTrigger = Animator.StringToHash("StartCharging");
    private static readonly int EndChargingTrigger = Animator.StringToHash("EndCharging");
    private static readonly Collider2D[] OverlapBuffer = new Collider2D[1];
}
}
