using System;
using UnityEditor;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class ChangeMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip music;

        private void OnTriggerEnter2D(Collider2D other)
        {
            MusicHolder.Instance.SetMusic(music);
        }
    }

    [CustomEditor(typeof(ChangeMusic))]
    public class ChangeMusicEditor : Editor
    {
        private new ChangeMusic target;

        private void OnEnable()
        {
            target = base.target as ChangeMusic;
        }

        private void OnSceneGUI()
        {
            Handles.BeginGUI();
            /*
            Gizmos.DrawIcon(
                target.transform.position,
                "Assets/MarioForever/Art/Sprites/Enemies/Mole2.png"
            );
            */
            Handles.EndGUI();
        }
    }
}