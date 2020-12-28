using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SweetMoleHouse.MarioForever
{
    /// <summary>
    /// 
    /// </summary>
    public class FlagsTest : MonoBehaviour 
    {
        private void Update()
        {
            print($"Update: {UnityEngine.Random.Range(0, 100)}");
        }

        private void FixedUpdate()
        {
            print($"Fixed Update: {UnityEngine.Random.Range(0, 100)}");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("暂停游戏"))
            {
                Time.timeScale = 0;
            }
        }
    }
}