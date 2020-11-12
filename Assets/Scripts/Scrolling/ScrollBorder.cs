﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 
    /// </summary>
    public class ScrollBorder : MonoBehaviour
    {
        private Vector2 min;
        private Vector2 max;
        public void Start()
        {
            var box = GetComponent<Collider2D>();
            min = box.bounds.min;
            max = box.bounds.max;
        }

        /// <summary>
        /// 由camera手动刷新
        /// </summary>
        public void Tick(Camera camera)
        {
            Vector3 pos = camera.transform.position;
            float camHeight = camera.orthographicSize;
            float camWidth = camHeight * camera.rect.width / camera.rect.height;
            pos.x = Mathf.Clamp(pos.x, min.x + camWidth, max.x - camWidth);
            pos.y = Mathf.Clamp(pos.y, min.y + camHeight, max.y - camHeight);
            camera.transform.position = pos;
        }
    }
}