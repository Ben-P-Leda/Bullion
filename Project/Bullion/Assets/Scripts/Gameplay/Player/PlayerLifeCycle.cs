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

            ActivateModel(_aliveModel);
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
                    case EventMessage.Enter_Dead_Mode: ActivateModel(_deadModel); break;
                    case EventMessage.Respawn: ActivateModel(_aliveModel); _aliveModelAnimator.Play("salute"); break;
                }
            }
        }

        private void ActivateModel(GameObject modelToActivate)
        {
            if (modelToActivate == _deadModel)
            {
                _deadModel.transform.localPosition = Vector3.zero;
                _aliveModel.transform.localPosition = Out_Of_Shot;
            }
            else
            {
                _deadModel.transform.localPosition = Out_Of_Shot;
                _aliveModel.transform.localPosition = Vector3.zero;
            }
        }

        private readonly Vector3 Out_Of_Shot = new Vector3(0.0f, 0.0f, -30000.0f);
    }
}
