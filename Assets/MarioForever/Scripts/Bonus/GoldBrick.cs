using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
public class GoldBrick : QuestionBlockBase {
    [SerializeField] private float duration = 6;
    [SerializeField] private GameObject bonus;

    protected override bool HasNext() {
        return _enabled || bonus != null;
    }

    protected override bool TryGetContent(out GameObject content) {
        // 顶砖时效内的金币
        if (_enabled) {
            content = Instantiate(bonus, transform.parent);
            MoveToBack(content, 2, true);
            DisableAsync();
            return true;
        }

        // 超时后的最后一枚金币
        if (bonus != null) {
            content = Instantiate(bonus, transform.parent);
            MoveToBack(content, 2, true);
            bonus = null;
            return true;
        }

        content = null;
        return false;
    }

    private async void DisableAsync() {
        if (_disabling) {
            return;
        }

        _disabling = true;

        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        _enabled = false;
    }

    private bool _enabled = true;
    private bool _disabling;
}
}

