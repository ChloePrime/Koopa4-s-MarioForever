using System;
using SweetMoleHouse.MarioForever.Scripts.Base;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace SweetMoleHouse.MarioForever.Scripts.Constants {
public enum ScoreType {

    S100 = 0,
    S200,
    S500,
    S1000,
    S2000,
    S5000,
    S10000,
    ONE_UP
}

public static class ScoreObjOperations {
    private static readonly ScoreTypeData ByType = ScoreTypeData.Instance;

    public static void Summon(this ScoreType type, Transform pos, float dx = 0, float dy = 0, float dz = 0) {
        var obj = ByType[(int)type];
        obj = Object.Instantiate(obj, pos.parent);
        var actualPos = pos.transform.position;
        actualPos.x += dx;
        actualPos.y += dy;
        actualPos.z += dz;
        obj.transform.position = actualPos;
    }
}

public class ScoreTypeData : GlobalSingleton<ScoreTypeData> {
    private const int TypeNum = 8;
    public GameObject this[int index] => GetScoreImpl(index);
    public static AudioClip CoinSound => Instance.coinSound;
    public static AudioClip OneUpSound => Instance.oneUpSound;
    

    [FormerlySerializedAs("objectByType")]
    [SerializeField]
    private GameObject[] scoreObjectByType = new GameObject[TypeNum];

    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip oneUpSound;

    private GameObject GetScoreImpl(int index) {
        if (index >= TypeNum) {
            throw new ArgumentOutOfRangeException();
        }

        var result = scoreObjectByType[index];
        if (result == null) {
            throw new IncompleteSetupException($"Score object bind not found for index {index}");
        }

        return result;
    }
}
}
