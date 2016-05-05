using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        public List<GameObject> Avatars;

        public float Distance;

        private Camera _camera;
        private Transform _transform;
        private Vector3 _unitVector;

        private void Start()
        {
            _transform = transform;
            _camera = GetComponent<Camera>();
            _unitVector = new Vector3(0.0f, Mathf.Sin(Camera_Angle * Mathf.Deg2Rad), -Mathf.Cos(Camera_Angle * Mathf.Deg2Rad));
        }

        private void FixedUpdate()
        {
            Vector3 center = GetCenter();
            float containmentRadius = GetContainmentRadius(center);
            Distance = containmentRadius / Mathf.Abs(Mathf.Sin(_camera.fieldOfView * Mathf.Deg2Rad) / 2.0f);

            _transform.position = center + (_unitVector * Distance);
            _transform.LookAt(center + new Vector3(0.0f, Vertical_Focus_Offset, 0.0f));
        }

        private Vector3 GetCenter()
        {
            Vector3 center = Vector3.zero;
            for (int i=0; i<Avatars.Count; i++)
            {
                center += Avatars[i].transform.position;
            }

            return center / Avatars.Count;
        }

        private float GetContainmentRadius(Vector3 center)
        {
            float containmentRadius = 0;
            for (int i = 0; i < Avatars.Count; i++)
            {
                containmentRadius = Mathf.Max(Vector3.Distance(center, Avatars[i].transform.position), containmentRadius);
            }

            return containmentRadius;
        }

        private const float Camera_Angle = 25.0f;
        private const float Vertical_Focus_Offset = 0.5f;
    }
}