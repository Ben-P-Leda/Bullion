using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay
{
    public class CameraMovement : MonoBehaviour
    {
        private Camera _camera;
        private List<Transform> _avatars;
        private Transform _transform;
        private Vector3 _unitVector;
        private Vector3 _currentcenter;
        private float _currentDistance;

        public void AddAvatar(GameObject avatar)
        {
            if (_avatars == null)
            {
                _avatars = new List<Transform>();
            }

            _avatars.Add(avatar.transform);
        }

        private void Start()
        {
            _transform = transform;
            _camera = GetComponent<Camera>();
            _unitVector = new Vector3(0.0f, Mathf.Sin(Camera_Angle * Mathf.Deg2Rad), -Mathf.Cos(Camera_Angle * Mathf.Deg2Rad));
        }

        private void FixedUpdate()
        {
            Vector3 targetCenter = GetCenter();
            _currentcenter = (_currentcenter * 0.8f) + (targetCenter * 0.2f);
            float containmentRadius = GetContainmentRadius(_currentcenter);
            float targetDistance = containmentRadius / Mathf.Abs(Mathf.Sin(_camera.fieldOfView * Mathf.Deg2Rad) / 2.0f);
            _currentDistance = (_currentDistance * 0.8f) + (targetDistance * 0.2f);

            _transform.position = _currentcenter + (_unitVector * (Mathf.Max(_currentDistance, Minimum_Distance) + Distance_Margin));
            _transform.LookAt(_currentcenter + new Vector3(0.0f, Vertical_Focus_Offset, 0.0f));
        }

        private Vector3 GetCenter()
        {
            Vector3 center = Vector3.zero;
            for (int i = 0; i < _avatars.Count; i++)
            {
                center += _avatars[i].position;
            }

            return center / _avatars.Count;
        }

        private float GetContainmentRadius(Vector3 center)
        {
            float containmentRadius = 0;
            for (int i = 0; i < _avatars.Count; i++)
            {
                containmentRadius = Mathf.Max(Vector3.Distance(center, _avatars[i].position), containmentRadius);
            }

            return containmentRadius;
        }

        private const float Camera_Angle = 25.0f;
        private const float Vertical_Focus_Offset = 0.5f;
        private const float Minimum_Distance = 10.0f;
        private const float Distance_Margin = 2.0f;
    }
}