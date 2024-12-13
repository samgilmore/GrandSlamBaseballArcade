using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab;       // Prefab of the ball
    public Transform pitchPosition;     // Position where the ball is spawned
    public float pitchSpeed = 5f;
    public float curveIntensity = 0.75f;
    public float pitchInterval = 5f;
    public GameObject fireEffectPrefab; // The fire particle system prefab
    private bool isPitching = false;
    private bool fireEnabled = false;   // Flag to track if fire effect is enabled

    [Header("Robot Voice Line Settings")]
    public AudioSource robotAudioSource; // AudioSource reference
    public AudioClip[] voiceLines;       // Array for the voice line clips

    public void StartPitching(int difficulty)
    {
        if (isPitching) return;

        isPitching = true;
        StartCoroutine(PitchSequence(difficulty));
    }

    public void StopPitching()
    {
        isPitching = false;
        StopAllCoroutines();
    }

    private IEnumerator PitchSequence(int difficulty)
    {
        while (GameManager.Instance.pitchesRemaining > 0)
        {
            if (!isPitching || !GameManager.Instance.isGameActive) yield break;

            // Spawn a new ball
            GameObject ballInstance = Instantiate(ballPrefab, pitchPosition.position, Quaternion.identity);

            // Initialize the ball with pitching parameters
            BallPitch ballScript = ballInstance.GetComponent<BallPitch>();
            ballScript.Initialize(pitchPosition, pitchSpeed, curveIntensity, difficulty);

            // If fire effect is enabled, instantiate the fire effect on the ball
            if (fireEnabled)
            {
                GameObject fireEffect = Instantiate(fireEffectPrefab, ballInstance.transform);
                fireEffect.transform.localPosition = Vector3.zero; // Position the fire effect relative to the ball
            }

            // Attach a callback to the ball to notify when it's handled
            Ball ball = ballInstance.GetComponent<Ball>();
            bool ballHandled = false;
            ball.onBallHandled = () => ballHandled = true;

            // Wait until the ball is handled
            yield return new WaitUntil(() => ballHandled);

            yield return new WaitForSeconds(1.5f);
            // Play a random voice line after every hit
            PlayRandomVoiceLine();

            // Add a short delay (1-2 seconds) before pitching the next ball
            yield return new WaitForSeconds(1.5f);
        }

        StopPitching();
    }

    // Method to enable or disable fire effect
    public void EnableFireEffect(bool enable)
    {
        fireEnabled = enable;
    }

    // Method to play a random voice line from the array
    private void PlayRandomVoiceLine()
    {
        if (voiceLines.Length == 0) return;

        AudioClip randomClip = voiceLines[Random.Range(0, voiceLines.Length)];
        robotAudioSource.clip = randomClip;
        robotAudioSource.Play();
    }
}