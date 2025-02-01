using System;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public string Name; // Name of the button or axis
    public float steeringSpeed = 2f; // Steering speed - the higher, the faster the turn
    public float smoothingFactor = 0.1f; // Smoothing factor for steering
    public float returnSpeed = 2f; // Speed of returning to the neutral position
    public float reverseThreshold = 0.1f; // Threshold below which the car can reverse
    public float brakeToReverseDelay = 0.5f; // Time before switching to reverse after braking to zero speed

    private float currentAxisValue = 0f; // Current axis value (from -1 to 1)
    private float targetAxisValue = 0f; // Target axis value
    private bool isBraking = false; // Whether braking is happening
    private float brakeTimer = 0f; // Timer to track delay for reverse activation

    public void SetDownState()
    {
        InputManager.SetButtonDown(Name);
    }

    public void SetUpState()
    {
        InputManager.SetButtonUp(Name);
    }

    public void SetAxisPositiveState()
    {
        InputManager.SetAxisPositive(Name);
    }

    public void SetAxisNeutralState()
    {
        InputManager.SetAxisZero(Name);
    }

    public void SetAxisNegativeState()
    {
        InputManager.SetAxisNegative(Name);
    }

    private void Update()
    {
        // Determine target axis state based on input
        if (Input.GetKey(KeyCode.A)) // Turn left
        {
            targetAxisValue = -1f; // Left turn
        }
        else if (Input.GetKey(KeyCode.D)) // Turn right
        {
            targetAxisValue = 1f; // Right turn
        }
        else // No input, smooth back to neutral
        {
            targetAxisValue = Mathf.Lerp(targetAxisValue, 0f, returnSpeed * Time.deltaTime);
        }

        // Smooth interpolation of the axis value
        currentAxisValue = Mathf.Lerp(currentAxisValue, targetAxisValue, steeringSpeed * Time.deltaTime);

        // Check for braking and manage reverse state
        if (Input.GetKey(KeyCode.S)) // Braking input (or use appropriate brake key)
        {
            isBraking = true;
            brakeTimer += Time.deltaTime;

            // If the brake timer exceeds the delay and the car's speed is below the threshold, enable reverse
            if (brakeTimer >= brakeToReverseDelay && Mathf.Abs(currentAxisValue) < reverseThreshold)
            {
                SetAxisNegativeState(); // Reverse
            }
            else
            {
                SetAxisNeutralState(); // Braking without reversing yet
            }
        }
        else
        {
            isBraking = false;
            brakeTimer = 0f; // Reset the timer when braking is released
        }

        // Update the axis state based on the current axis value
        if (!isBraking)
        {
            if (currentAxisValue > 0)
            {
                SetAxisPositiveState();
            }
            else if (currentAxisValue < 0)
            {
                SetAxisNegativeState();
            }
            else
            {
                SetAxisNeutralState();
            }
        }
    }
}
