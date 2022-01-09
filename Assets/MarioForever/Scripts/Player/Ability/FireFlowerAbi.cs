using SweetMoleHouse.MarioForever.Scripts.Base.Physics;
using SweetMoleHouse.MarioForever.Scripts.Constants;
using SweetMoleHouse.MarioForever.Scripts.Util;
using UnityEngine;

namespace SweetMoleHouse.MarioForever.Scripts.Player.Ability
{
    public class FireFlowerAbi : BaseAnimatedAbility
    {
        [SerializeField, RenameInInspector("火球上限")]
        private int maxFireballs = 2;

        [SerializeField] private MarioFireball fireball;

        public override MarioPowerup CorrespondingPowerup => MarioPowerup.FIRE_FLOWER;

        public override void OnShoot()
        {
            if (MarioFireball.Count >= maxFireballs) return;
            base.OnShoot();
            var bullet = Instantiate(
                fireball.gameObject,
                Manager.Mario.transform.parent
            );
            bullet.transform.position = Manager.GetMuzzle().position;
            bullet.BfsComponentInChildren<BasePhysics>().SetDirection(Manager.Mario.Direction);
        }
    }
}
