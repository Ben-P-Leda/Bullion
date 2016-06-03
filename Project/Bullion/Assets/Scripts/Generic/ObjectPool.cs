using UnityEngine;

namespace Assets.Scripts.Generic
{
    public class ObjectPool
    {
        public delegate GameObject PoolObjectCreator();
        public delegate void PoolObjectActivator(GameObject objectToActivate);

        private GameObject[] _objects;
        private PoolObjectActivator _activateObjectCallback;

        public ObjectPool(Transform parent, int poolSize, PoolObjectCreator createObjectCallback, PoolObjectActivator activateObjectCallback)
        {
            _objects = new GameObject[poolSize];

            for (int i = 0; i < poolSize; i++)
            {
                _objects[i] = createObjectCallback();
                _objects[i].transform.parent = parent;
            }

            _activateObjectCallback = activateObjectCallback;
        }

        public GameObject GetNextAvailableObject()
        {
            GameObject nextAvailable = null;
            for (int i = 0; ((i < _objects.Length) && (nextAvailable == null)); i++)
            {
                if (!_objects[i].activeInHierarchy)
                {
                    nextAvailable = _objects[i];
                }
            }

            return nextAvailable;
        }

        public int GetAvailableObjectCount()
        {
            int availableCount = 0;
            for (int i = 0; i < _objects.Length; i++)
            {
                if (!_objects[i].activeInHierarchy)
                {
                    availableCount++;
                }
            }

            return availableCount;
        }

        public int AttemptMultipleActivation(int totalToActivate)
        {
            int numberActivated = 0;
            for (int i = 0; (i < _objects.Length) && (numberActivated < totalToActivate); i++)
            {
                if (!_objects[i].activeInHierarchy)
                {
                    _objects[i].SetActive(true);
                    if (_activateObjectCallback != null)
                    {
                        _activateObjectCallback(_objects[i]);
                    }
                    numberActivated++;
                }
            }

            return numberActivated;
        }
    }
}