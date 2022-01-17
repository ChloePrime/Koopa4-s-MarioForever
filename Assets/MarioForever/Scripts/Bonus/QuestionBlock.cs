using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Bonus {
/// <summary>
/// </summary>
public class QuestionBlock : HittableBase {
    [CanBeNull, SerializeField, RenameInInspector("顶后的残余")]
    protected GameObject afterHit;

    [SerializeField, RenameInInspector("顶出的物品")]
    protected GameObject[] outputs;

    [SerializeField, RenameInInspector("音效")]
    protected AudioClip sfx;

    /// <summary>
    /// 顶出物的初始位置。
    /// </summary>
    [SerializeField] private Transform muzzle;
    
    public delegate void ContentModifier(ref GameObject content);

    /// <summary>
    /// 修改顶出物内容。
    /// </summary>
    public event ContentModifier ModifyContent;

    protected override void Awake() {
        base.Awake();
        
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _size = GetComponent<Collider2D>().bounds.size;
        // 创建子对象
        // 隐藏块顶后的方块，隐藏块里的内容物
        if (afterHit != null) {
            CreateChild(ref afterHit, 1, true);
        }

        for (int i = 0; i < outputs.Length; i++) {
            CreateChild(ref outputs[i], i + 2, false);
        }
    }

    private void CreateChild(ref GameObject input, int depth, bool forceMoveBack) {
        GameObject cloned = Instantiate(input, transform.parent);

        // 设置 y 坐标为问号块顶部
        cloned.transform.position = muzzle.transform.position;
        cloned.SetActive(false);
        MoveToBack(cloned, depth, forceMoveBack);

        input = cloned;
    }

    private void MoveToBack(GameObject obj, int delta, bool forceMoveBack) {
        if (!forceMoveBack && !obj.TryGetComponent(out IAppearable _)) {
            return;
        }
        
        SpriteRenderer thatSr = obj.TryGetComponent(out SpriteRenderer sr)
            ? sr
            : obj.GetComponentInChildren<SpriteRenderer>();
        if (thatSr == null) return;
        thatSr.sortingLayerID = _renderer.sortingLayerID;
        thatSr.sortingOrder = _renderer.sortingOrder - delta;
    }

    public override bool OnHit(Transform hitter) {
        if (!Ready) {
            return true;
        }

        if (_outputIndex < outputs.Length) {
            base.OnHit(hitter);
            SpawnContent(outputs[_outputIndex]);
            ++_outputIndex;
        }

        if (_outputIndex >= outputs.Length && afterHit != null) {
            afterHit.SetActive(true);
            _renderer.enabled = false;
        }

        return false;
    }

    private async void SpawnContent(GameObject bonus) {
        await UniTask.Delay(AnimLen);
        
        if (sfx != null) {
            Global.PlaySound(sfx);
        }

        ModifyContent?.Invoke(ref bonus);
        bonus.SetActive(true);
        if (!bonus.TryGetComponent(out IAppearable appearable)) {
            return;
        }

        var yOffset = bonus.TryGetComponent(out Rigidbody2D r2d) ? MFUtil.Height(r2d) : _size.y;
        bonus.transform.Translate(0, -yOffset, 0);
        appearable.Appear(Vector2.up, new Vector2(0, yOffset));
    }

    private SpriteRenderer _renderer;
    private int _outputIndex;
    private Vector2 _size;
}
}