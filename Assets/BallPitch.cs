using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class BallPitch : MonoBehaviour
{
    public Transform pitchPosition;
    public Transform hittingZone;
    public float pitchSpeed = 5f;       // How fast the ball moves
    public float pitchInterval = 5f;    // Time in seconds between each pitch (reset time)
    public float curveIntensity = .75f;   // Intensity of the curve (higher values = more curve)
    public int difficulty = 2;          // 0 = Easy, 1 = Medium, 2 = Hard

    private float startTime;            // Time at which the pitch started
    private float pitchLength;        // Total distance between the pitching and hitting positions
    private bool isPitching;            // Whether the ball is currently in the process of pitching
    private bool hasReachedBatter;      // Whether the ball has reached the batter

    private enum PitchType { Fastball, LeftCurveball, RightCurveball, UpCurveball, WavePitch }
    private PitchType currentPitchType; // Current type of pitch (Fastball or Curveball)

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the total distance between the pitching and hitting positions
        pitchLength = Vector3.Distance(pitchPosition.position, hittingZone.position);
        StartCoroutine(PitchSequence()); // Start the first pitch sequence
    }

    // Update is called once per frame
    void Update()
    {
        // If the ball is currently pitching
        if (isPitching && !hasReachedBatter)
        {
            // Calculate how far along the path we are (from 0 to 1)
            float distanceCovered = (Time.time - startTime) * pitchSpeed;
            float fractionOfJourney = distanceCovered / pitchLength;

            // Decide which type of pitch to throw
            if (currentPitchType == PitchType.Fastball)
            {
                // Move the ball along a straight line (fastball)
                transform.position = Vector3.Lerp(pitchPosition.position, hittingZone.position, fractionOfJourney);
            }
            else if (currentPitchType == PitchType.RightCurveball)
            {
                // Move the ball with a curve (curveball)
                Vector3 currentPosition = Vector3.Lerp(pitchPosition.position, hittingZone.position, fractionOfJourney);

                currentPosition.x += Mathf.Sin(fractionOfJourney * Mathf.PI) * curveIntensity;

                transform.position = currentPosition;
            }
            else if (currentPitchType == PitchType.LeftCurveball)
            {
                // Move the ball with a curve (curveball)
                Vector3 currentPosition = Vector3.Lerp(pitchPosition.position, hittingZone.position, fractionOfJourney);

                currentPosition.x -= Mathf.Sin(fractionOfJourney * Mathf.PI) * curveIntensity;

                transform.position = currentPosition;
            }
            else if (currentPitchType == PitchType.UpCurveball)
            {
                Vector3 currentPosition = Vector3.Lerp(pitchPosition.position, hittingZone.position, fractionOfJourney);

                currentPosition.y += Mathf.Sin(fractionOfJourney * Mathf.PI);

                transform.position = currentPosition;
            }
            else if (currentPitchType == PitchType.WavePitch)
            {
                // Apply a full sine wave oscillation (new wave pitch)
                Vector3 currentPosition = Vector3.Lerp(pitchPosition.position, hittingZone.position, fractionOfJourney);

                // Apply sine wave to the X, Y, or Z axis (we'll use the Y axis for this example)
                currentPosition.x += Mathf.Sin(fractionOfJourney * Mathf.PI * 2) * curveIntensity;  // Complete sine wave oscillation

                transform.position = currentPosition;
            }

            // If the ball reaches the hitting position, stop the movement and set position
            if (fractionOfJourney >= 1f)
            {
                transform.position = hittingZone.position; // Ensure it's at the hitting position
                hasReachedBatter = true; // Mark that the ball has reached the batter
            }
        }
    }

    // Coroutine to handle pitching sequence
    private IEnumerator PitchSequence()
    {
        while (true)
        {
            // Reset the ball position to the pitching position
            transform.position = pitchPosition.position;

            // Randomly choose the next pitch type based on difficulty
            PitchType newPitch;

            if (difficulty == 0) // Easy
            {
                // Only Fastball
                newPitch = PitchType.Fastball;
            }
            else if (difficulty == 1) // Medium
            {
                // Randomly choose between Fastball, LeftCurveball, or RightCurveball
                int randomPitch = Random.Range(0, 3); // 0, 1, or 2
                newPitch = (PitchType)randomPitch;
            }
            else if (difficulty == 2) // Hard
            {
                // Randomly choose from all available pitches (Fastball, LeftCurveball, RightCurveball, UpCurveball, WavePitch)
                int randomPitch = Random.Range(0, 5); // 0 to 4
                newPitch = (PitchType)randomPitch;
            }
            else
            {
                // Default to Fastball if difficulty is unrecognized (just in case)
                newPitch = PitchType.Fastball;
            }

            currentPitchType = newPitch; // Set the new pitch type

            // Start the pitch
            startTime = Time.time;
            isPitching = true; // Start the pitching process
            hasReachedBatter = false; // Reset the flag to track if the ball has reached the batter

            // Wait for the ball to reach the batter (fractionOfJourney >= 1)
            yield return new WaitUntil(() => hasReachedBatter);

            // Wait for the interval before starting the next pitch
            yield return new WaitForSeconds(pitchInterval);
        }
    }
}
