using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class Suspension : MonoBehaviour
    {
        private List<WheelCollider> wheelColliders = new List<WheelCollider>();
        public float suspensionTravelMultiplier = 0.2f; // Adjust the suspension travel effect
        public float rotationOffset = 0; // Offset for wheel rotation

        // Store references to the wheel's visual transforms
        private List<Transform> wheelVisuals = new List<Transform>();

        private void Start()
        {
            // Get all WheelColliders in the children of this GameObject
            WheelCollider[] colliders = GetComponentsInChildren<WheelCollider>();
            foreach (var collider in colliders)
            {
                wheelColliders.Add(collider);

                // Find the visual transform associated with this WheelCollider
                Transform wheelTransform = collider.transform.GetChild(0); // Assumes the visual wheel is the first child
                if (wheelTransform != null)
                {
                    wheelVisuals.Add(wheelTransform);
                }
            }

            if (wheelColliders.Count != wheelVisuals.Count)
            {
                Debug.LogError("Mismatch between WheelColliders and visual wheels. Check your hierarchy.");
            }
        }

        private void Update()
        {
            for (int i = 0; i < wheelColliders.Count; i++)
            {
                // Get the wheel position and rotation from the WheelCollider
                Vector3 wheelPosition;
                Quaternion wheelRotation;
                wheelColliders[i].GetWorldPose(out wheelPosition, out wheelRotation);

                // Apply the position and rotation to the corresponding visual wheel
                if (wheelVisuals[i] != null)
                {
                    // Adjust position with suspension travel
                    wheelVisuals[i].position = wheelPosition;
                    wheelVisuals[i].localPosition += wheelVisuals[i].transform.up * suspensionTravelMultiplier;

                    // Adjust rotation with optional offset
                    wheelVisuals[i].rotation = wheelRotation * Quaternion.Euler(0, rotationOffset, 0);
                }
            }
        }
    }
}