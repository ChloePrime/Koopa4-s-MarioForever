using System;
using SweetMoleHouse.MarioForever.Base;
using SweetMoleHouse.MarioForever.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SweetMoleHouse.MarioForever.Constants
{
    public enum ScoreType
    {

        S100 = 0,
        S200,
        S500,
        S1000,
        S2000,
        S5000,
        S10000,
        ONE_UP
    }

    public static class ScoreObjOperations
    {
        private static readonly ScoreTypeData ByType = ScoreTypeData.Instance;
        public static void Summon(this ScoreType type, Transform pos)
        {
            var obj = ByType[(int) type];
            obj = Object.Instantiate(obj, pos.parent);
            obj.transform.position = pos.transform.position;
        }
    }

    public class ScoreTypeData : GlobalSingleton<ScoreTypeData>
    {
        private const int TypeNum = 8;
        [SerializeField] private GameObject[] objectByType = new GameObject[TypeNum];

        public GameObject this[int index]
        {
            get
            {
                if (index >= TypeNum)
                {
                    throw new ArgumentOutOfRangeException();
                }
                var result = objectByType[index];
                if (result == null)
                {
                    throw new IncompleteSetupException($"Score object bind not found for index {index}");
                }
                return result;
            }
        }
    }
}
