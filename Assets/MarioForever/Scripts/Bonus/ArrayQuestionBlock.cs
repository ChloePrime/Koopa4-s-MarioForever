using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// 内容物为固定序列的问号块。
/// </summary>
public class ArrayQuestionBlock : QuestionBlockBase {

    [SerializeField, RenameInInspector("顶出的物品")]
    protected GameObject[] outputs;
    
    protected override void Awake() {
        base.Awake();
        
        // 创建子对象
        // 隐藏块顶后的方块，隐藏块里的内容物
        for (int i = 0; i < outputs.Length; i++) {
            CreateChild(ref outputs[i], i + 2, false);
        }
    }

    protected override bool HasNext() {
        return _outputIndex < outputs.Length;
    }

    protected override bool TryGetContent(out GameObject content) {
        if (_outputIndex >= outputs.Length) {
            content = null;
            return false;
        }

        content = outputs[_outputIndex];
        ++_outputIndex;
        return true;
    }
    
    private int _outputIndex;
}
}