using System.Collections.Generic;
using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Scripts.Base;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Level {
public class AreaManager : Singleton<AreaManager> {
    [SerializeField] private Area current;

    private void Awake() {
        _audio = GetComponent<AudioSource>();
    }

    public void Register(Area area) {
        _registered.Add(area);
        area.SetEnabled(area == current);
    }

    public static void SetCurrentArea(Area areaIn) {
        Instance.SetCurrentAreaImpl(areaIn);
    }

    public static void SetMusic([CanBeNull] AudioClip music) {
        Instance.SetMusicImpl(music);
    }

    public static void SetOverrideMusic(AudioClip music) {
        Instance.SetOverrideMusicImpl(music);
    }

    public static void StopOverrideMusic() {
        Instance.StopOverrideMusicImpl();
    }

    private void SetMusicImpl([CanBeNull] AudioClip music) {
        if (_curMusic == music) {
            return;
        }
        _curMusic = music;

        if (_overrideMusic != null) {
            return;
        }

        _audio.clip = music;
        if (music != null) {
            _audio.Play();
        } else {
            _audio.Stop();
        }
    }

    private void SetCurrentAreaImpl(Area areaIn) {
        if (!_registered.Contains(areaIn)) {
            Debug.LogError("Unregistered Area!");
        }

        if (areaIn == current) return;

        current = areaIn;
        foreach (var area in _registered) {
            area.SetEnabled(area == areaIn);
        }
    }

    private void SetOverrideMusicImpl(AudioClip music) {
        if (_overrideMusic == music) {
            return;
        }
        
        _overrideMusic = music;
        _audio.clip = music;
        _audio.Play();
    }
    
    private void StopOverrideMusicImpl() {
        if (_overrideMusic == null) {
            return;
        }

        _overrideMusic = null;
        _audio.clip = _curMusic;
        if (_curMusic != null) {
            _audio.Play();
        }
    }
    
    private AudioSource _audio;
    private AudioClip _curMusic;
    private AudioClip _overrideMusic;
    private readonly HashSet<Area> _registered = new();
}
}
