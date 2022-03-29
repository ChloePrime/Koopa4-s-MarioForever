using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SweetMoleHouse.MarioForever.Scripts.Audio {
[CreateAssetMenu(fileName = null, menuName = "MarioForever/" + nameof(AudioGroup), order = 0)]
public class AudioGroup : ScriptableObject {
    [SerializeField] private AudioClip[] audioClips;

    public void Play() {
        Global.PlaySound(Pick());
    }

    public AudioClip Pick() {
        int clipCount = audioClips.Length;
        switch (clipCount) {
            case 0:
                throw new Exception("AudioClips are empty");
            case 1:
                return audioClips[0];
        }
        int selectedIndex;
        if (_bFirst) {
            _bFirst = false;
            selectedIndex = Random.Range(0, clipCount);
        } else {
            selectedIndex = Random.Range(0, clipCount - 1);
        }
        ref AudioClip selected = ref audioClips[selectedIndex];
        ref AudioClip last = ref audioClips[^1];
        (selected, last) = (last, selected);
        return last;
    }

    private bool _bFirst = true;
}
}
