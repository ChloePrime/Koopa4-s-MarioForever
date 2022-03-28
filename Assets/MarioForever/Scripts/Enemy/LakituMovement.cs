using System;
using SweetMoleHouse.MarioForever.Scripts.Player;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy {
public class LakituMovement : MonoBehaviour {
    [SerializeField] private float accNear = 78.125f;
    [SerializeField] private float accFar = 15.625f;
    /// <summary>
    /// ±9 in CTF
    /// </summary>
    [SerializeField] private float maxSpeed = 14f;
    /// <summary>
    /// ±2 in CTF
    /// </summary>
    [SerializeField] private float lowSpeed = 3.125f;

    private void FixedUpdate() {
        Mario mario = FindMario();
        if (mario == null) {
            _speed = 0;
            return;
        }
        float dx = transform.position.x - mario.transform.position.x;
        const float dist1 = 1.5f;
        const float dist2 = 3;

        if (dx > dist1 && _speed > -maxSpeed) {
            _speed -= accFar * Time.fixedDeltaTime;
        }
        if (dx < -dist1 && _speed < maxSpeed) {
            _speed += accFar * Time.fixedDeltaTime;
        }
        if (MathF.Abs(dx) < dist2) {
            if (_speed < -lowSpeed && mario.Direction > 0) {
                _speed += accNear * Time.fixedDeltaTime;
            }
            if (_speed > lowSpeed && mario.Direction < 0) {
                _speed -= accNear * Time.fixedDeltaTime;
            }
        }
    }

    private void Update() {
        transform.Translate(_speed * Time.deltaTime, 0, 0);
    }

    private Mario FindMario() {
        if (_cachedMario != null) {
            return _cachedMario;
        }
        _cachedMario = FindObjectOfType<Mario>();
        return _cachedMario;
    }
    
    private float _speed = 0;
    private Mario _cachedMario;
}
}
