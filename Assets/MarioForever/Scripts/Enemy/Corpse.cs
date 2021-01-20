﻿using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Level;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy
{
    /// <summary>
    /// 怪物尸体
    /// </summary>
    public class Corpse : MonoBehaviour 
    {
        private new SpriteRenderer renderer;
        private BasePhysics physics;
        private bool inited;

        private void Start()
        {
            if (inited) return;
            
            physics = GetComponent<BasePhysics>();
            
            renderer = GetComponentInChildren<SpriteRenderer>();
            
            inited = true;
        }

        private void Update()
        {
            if (transform.position.y <= ScrollInfo.Bottom - 8)
            {
                Destroy(gameObject);
            }
        }

        public void AcceptBody(in SpriteRenderer sr)
        {
            // 此函数调用时 Start 可能未执行
            if (!inited) Start(); 
            
            physics.TeleportTo(sr.transform.position);
            renderer.sprite = sr.sprite;
        }
    }
}