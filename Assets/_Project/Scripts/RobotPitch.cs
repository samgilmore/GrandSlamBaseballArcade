using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RobotPitch : MonoBehaviour
{
    public GameObject ballPrefab;      // Reference to the ball prefab
    public Transform handTransform;    // The robot's hand (where the ball will be attached)
    public Transform targetPosition;    // Position ball is pitched to (e.g. hittingZone)
    public float pitchSpeed = 5f;      // Speed of the ball when pitched
    public float curveIntensity = 0.75f; // Curve intensity of the ball's path
    public float pitchInterval = 5f;   // Time interval between pitches
    public Animator robotAnimator;       // Reference to the robot's

    private GameObject currentBall;    // The ball currently attached to the hand
    
    private void Start()
    {
        // Optionally, start the pitching sequence
        StartCoroutine(PitchSequence());
    }

    private IEnumerator PitchSequence()
    {
        while (true)
        {
            // Play the pitching animation (trigger the StartPitching animation state)
            if (robotAnimator != null)
            {
                robotAnimator.SetTrigger("StartPitch");
            }

            // Simulate the robot pitching the ball
            // You can set up an animation event here to know when to release the ball
            yield return new WaitForSeconds(1f); // Wait for a moment before releasing the ball

            // Wait before the next pitch
            yield return new WaitForSeconds(pitchInterval);
        }
    }
}
