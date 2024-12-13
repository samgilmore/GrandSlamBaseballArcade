using System.Collections;
using UnityEngine;

public class BallPitch : MonoBehaviour
{
    public Transform pitchPosition;
    public float pitchSpeed = 5f;       // How fast the ball moves
    public float curveIntensity = 0.75f; // Intensity of the curve
    public int difficulty = 2;          // 0 = Easy, 1 = Medium, 2 = Hard

    public AudioClip buzz;

    public AudioSource ballSource;
    public AudioClip hitClip;
    public AudioClip pitchClip;

    private bool isPitching;            // Whether the ball is actively pitching
    private bool hasCollided;           // Flag to check if the ball has collided
    private Rigidbody rb;

    private enum PitchType { Fastball, LeftCurveball, RightCurveball, UpCurveball, WavePitch }
    private PitchType currentPitchType; // Current type of pitch

    public void Initialize(Transform pitchPos, float speed, float curve, int diff)
    {
        pitchPosition = pitchPos;
        pitchSpeed = speed;
        curveIntensity = curve;
        difficulty = diff;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity initially
        hasCollided = false; // Reset collision flag
        StartPitch(); // Start pitching
    }

    private void StartPitch()
    {
        ballSource.clip = pitchClip;
        ballSource.Play();

        currentPitchType = SelectPitchType();
        isPitching = true;
    }

    private void Update()
    {
        if (!isPitching) return;

        // Calculate initial velocity in the negative Z direction
        Vector3 velocity = -pitchPosition.forward * pitchSpeed;

        // Apply pitch-specific modifications
        switch (currentPitchType)
        {
            case PitchType.Fastball:
                break;
            case PitchType.LeftCurveball:
                velocity.x -= Mathf.Sin(Time.time * pitchSpeed) * curveIntensity;
                break;
            case PitchType.RightCurveball:
                velocity.x += Mathf.Sin(Time.time * pitchSpeed) * curveIntensity;
                break;
            case PitchType.UpCurveball:
                velocity.y += Mathf.Sin(Time.time * pitchSpeed) * curveIntensity;
                break;
            case PitchType.WavePitch:
                velocity.x += Mathf.Sin(Time.time * pitchSpeed * 2) * curveIntensity;
                break;
        }

        // Apply velocity to Rigidbody
        rb.velocity = velocity;
    }

    private PitchType SelectPitchType()
    {
        int randomIndex = difficulty switch
        {
            0 => 0, // Easy: Only Fastball
            1 => Random.Range(0, 3), // Medium: Fastball + 2 Curves
            _ => Random.Range(0, 5) // Hard: All Pitches
        };
        return (PitchType)randomIndex;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ensure the ball only reacts to the first collision with "Bat"
        if (hasCollided) return;

        if (collision.gameObject.CompareTag("Bat"))
        {
            hasCollided = true; // Set collision flag to true
            TriggerVibration(buzz);
            ballSource.clip = hitClip;
            ballSource.Play();
            StopPitching(); // Stop the pitching logic
            rb.useGravity = true; // Enable gravity for physics-based behavior
            rb.velocity = collision.relativeVelocity; // Add force from the bat
        }
    }

    private void TriggerVibration(AudioClip vibrationAudio)
    {
        OVRHapticsClip clip = new OVRHapticsClip(vibrationAudio);
        Debug.Log(vibrationAudio);

        OVRHaptics.LeftChannel.Preempt(clip);
        OVRHaptics.RightChannel.Preempt(clip);
    }

    private void StopPitching()
    {
        isPitching = false; // Stop the pitching logic
    }
}