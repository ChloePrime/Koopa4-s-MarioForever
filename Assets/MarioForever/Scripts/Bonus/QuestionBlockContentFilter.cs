using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Persistent;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
[RequireComponent(typeof(QuestionBlock))]
public class QuestionBlockContentFilter : MonoBehaviour {
    [SerializeField] private GameObject fallbackWhenMarioIsSmall;

    private void Awake() {
        GetComponent<QuestionBlock>().ModifyContent += ModifyQBlockContent;
    }

    private void ModifyQBlockContent(ref GameObject content) {
        if (fallbackWhenMarioIsSmall == null) {
            return;
        }

        if (MarioProperty.CurPowerup != MarioPowerup.SMALL) {
            return;
        }

        Transform myTransform = transform;
        GameObject old = content;
        GameObject @new = content = Instantiate(
            fallbackWhenMarioIsSmall, myTransform.position, Quaternion.identity, myTransform.parent
        );
        // 复制排序图层
        if (@new.TryBfsComponentInChildren(out SpriteRenderer newSr) && old.TryBfsComponentInChildren(out SpriteRenderer oldSr)) {
            newSr.sortingOrder = oldSr.sortingOrder;
        }

        Destroy(old);
    }
}
}
