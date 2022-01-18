using System;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Util {
[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSpriteAtRuntime : MonoBehaviour {
    [SerializeField] private Sprite runtimeSprite;
    
    private void Start() {
        GetComponent<SpriteRenderer>().sprite = runtimeSprite;
    }
}
}
