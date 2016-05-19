using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerRushCharge : MonoBehaviour, IConfigurable
    {
        private Transform _transform;

        private float _rushChargeLevel;
        private bool _rechargeBlocked;

        public CharacterConfiguration Configuration { private get; set; }

        private void Start()
        {
            _transform = transform;

            _rushChargeLevel = 0.0f;
            _rechargeBlocked = false;
        }

        private void FixedUpdate()
        {
            if ((_rushChargeLevel < Maximum_Rush_Charge) && (!_rechargeBlocked))
            {
                _rushChargeLevel = Mathf.Min(_rushChargeLevel + Configuration.RushRechargeSpeed, Maximum_Rush_Charge);
                EventDispatcher.FireEvent(_transform, _transform, EventMessage.Update_Rush_Charge, _rushChargeLevel);
            }
        }

        private const float Maximum_Rush_Charge = 100.0f;
    }
}