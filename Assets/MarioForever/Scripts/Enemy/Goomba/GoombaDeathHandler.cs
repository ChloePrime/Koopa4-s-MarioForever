using SweetMoleHouse.MarioForever.Scripts.Constants;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Enemy.Goomba {
public class GoombaDeathHandler : MonoBehaviour {
    [SerializeField] private GameObject goombaPhoto;

    private DamageReceiver dr;

    private void Start() {
        dr = GetComponent<DamageReceiver>();
        dr.OnGenerateCorpse += OnDeath;

        goombaPhoto = Instantiate(goombaPhoto, dr.Host);
        goombaPhoto.SetActive(false);
    }

    private ActionResult OnDeath(DamageSource damager, EnumDamageType damageType) {
        if (!damageType.Contains(EnumDamageType.STOMP)) return ActionResult.PASS;

        goombaPhoto.SetActive(true);
        goombaPhoto.transform.parent = dr.Host.parent;
        goombaPhoto.transform.position = dr.Host.position;
        if (goombaPhoto.TryGetComponent(out Corpse corpse)) {
            corpse.InitCorpse(damager, dr.Renderer);
        }

        return ActionResult.CANCEL;
    }
}
}
