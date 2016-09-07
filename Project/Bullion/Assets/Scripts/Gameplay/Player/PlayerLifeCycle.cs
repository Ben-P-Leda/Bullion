using UnityEngine;
using Assets.Scripts.EventHandling;
using Assets.Scripts.Gameplay.Player.Interfaces;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerLifeCycle : MonoBehaviour, IAnimated
    {
        private Transform _transform;
        private GameObject _aliveModel;
        private GameObject _deadModel;
        private Animator _aliveModelAnimator;

        private bool _immuneToDamage;

        private void Start()
        {
            _transform = transform;

            _immuneToDamage = false;
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
            EventDispatcher.FloatEventHandler += FloatEventHandler;
        }

        private void OnDisable()
        {
            EventDispatcher.MessageEventHandler -= MessageEventHandler;
            EventDispatcher.FloatEventHandler -= FloatEventHandler;
        }

        private void MessageEventHandler(Transform originator, Transform target, string message)
        {
            if (target == _transform)
            {
                switch (message)
                {
                    case EventMessage.Has_Died: StartDeathSequence(); break;
                    case EventMessage.Enter_Dead_Mode: ActivateModel(_deadModel); break;
                    case EventMessage.Respawn: Respawn(); break;
                }
            }
        }

        private void FloatEventHandler(Transform originator, Transform target, string message, float value)
        {
            if ((target == _transform) && (message == EventMessage.Inflict_Damage) && (!_immuneToDamage))
            {
                _aliveModelAnimator.SetBool("DamageTaken", true);
            }
        }

        private void StartDeathSequence()
        {
            _immuneToDamage = true;
            _aliveModelAnimator.CrossFade("dead", 0.5f);
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

        private void Respawn()
        {
            ActivateModel(_aliveModel);

            _immuneToDamage = false;
            _aliveModelAnimator.Play("salute");
        }

        private readonly Vector3 Out_Of_Shot = new Vector3(0.0f, 0.0f, -30000.0f);
    }
}
