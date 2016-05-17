using UnityEngine;
using Assets.Scripts.Configuration;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerLifeCycle : MonoBehaviour, IAnimated
    {
        private Transform _transform;
        private GameObject _aliveModel;
        private GameObject _deadModel;
        private Animator _aliveModelAnimator;

        private void Start()
        {
            _transform = transform;
        }

        public void WireUpModels(GameObject aliveModel, GameObject deadModel)
        {
            _aliveModel = aliveModel;
            _deadModel = deadModel;
        }

        public void WireUpAnimators(Animator aliveModelAnimator, Animator deadModelAnimator)
        {
            _aliveModelAnimator = aliveModelAnimator;
        }

        private void OnEnable()
        {
            EventDispatcher.MessageEventHandler += MessageEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Inflict_Damage: _aliveModelAnimator.SetBool("DamageTaken", true); break;
                    case EventMessage.Has_Died: _aliveModelAnimator.CrossFade("dead", 0.5f); break;
                    case EventMessage.Enter_Dead_Mode: SwitchToDeadMode(); break;
                    case EventMessage.Respawn: SwitchToAliveMode(); break;
                }
            }
        }

        private void SwitchToDeadMode()
        {
            _deadModel.SetActive(true);
            _aliveModel.SetActive(false);
        }

        private void SwitchToAliveMode()
        {
            _deadModel.SetActive(false);
            _aliveModel.SetActive(true);

            // TODO: Trigger respawn animation
        }
    }
}
