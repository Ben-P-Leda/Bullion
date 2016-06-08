using UnityEngine;
using Assets.Scripts.EventHandling;

namespace Assets.Scripts.Gameplay.Chests.Contents.PowerUps
{
    public class PowerUpFactory : MonoBehaviour
    {
        private GameObject[] _powerUps;

        private void Start()
        {
            InitializePowerUps();
        }

        private void InitializePowerUps()
        {
            _powerUps = new GameObject[transform.childCount];
            for (int i = 0; i < _powerUps.Length; i++)
            {
                _powerUps[i] = transform.GetChild(i).gameObject;

                PowerUpLifeCycle lifeCycleController = _powerUps[i].GetComponent<PowerUpLifeCycle>();
                if (lifeCycleController == null)
                {
                    throw new System.Exception("All power ups must have a PowerUpLifeCycle script attached!");
                }
                else
                {
                    lifeCycleController.InitializeComponents();
                }
            }
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
            if (message == EventMessage.Spawn_Power_Up)
            {
                int powerUpIndex = GetPowerUpToLaunch();

                if (powerUpIndex > -1)
                {
                    _powerUps[powerUpIndex].SetActive(true);
                    _powerUps[powerUpIndex].transform.position = originator.transform.position;
                }
                else
                {
                    EventDispatcher.FireEvent(originator, target, EventMessage.Spawn_Treasure);
                }
            }
        }

        private int GetPowerUpToLaunch()
        {
            int powerUpIndex = -1;
            int offset = Random.Range(0, _powerUps.Length);

            for (int i = 0; ((i < _powerUps.Length) && (powerUpIndex < 0)); i++)
            {
                int index = (i + offset) % _powerUps.Length;
                if (!_powerUps[index].gameObject.activeInHierarchy)
                {
                    powerUpIndex = (i + offset) % _powerUps.Length;
                }
            }

            return powerUpIndex;
        }
    }
}