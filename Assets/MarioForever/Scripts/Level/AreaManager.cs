using System.Collections.Generic;
using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Level
{
    public class AreaManager : Singleton<AreaManager>
    {
        private new AudioSource audio;
        
        private readonly ISet<Area> registered = new HashSet<Area>();
        [SerializeField] private Area current;

        private void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void Register(Area area)
        {
            registered.Add(area);
            area.SetEnabled(area == current);
        }
        public static void SetMusic([CanBeNull] AudioClip music)
        {
            Instance.SetMusic0(music);
        }
        private void SetMusic0([CanBeNull] AudioClip music)
        {
            if (audio.clip == music) return;

            if (music == null)
            {
                audio.Stop();
                return;
            }
            
            audio.clip = music;
            audio.Play();
        }

        public static void SetCurrentArea(Area areaIn)
        {
            Instance.SetCurrentArea0(areaIn);
        }

        private void SetCurrentArea0(Area areaIn)
        {
            if (!registered.Contains(areaIn))
            {
                UnityEngine.Debug.LogError("Unregistered Area!");
            }
            if (areaIn == current) return;

            current = areaIn;
            foreach (var area in registered)
            {
                area.SetEnabled(area == areaIn);
            }
        }
    }
}
