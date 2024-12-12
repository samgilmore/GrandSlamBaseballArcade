using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab;       // Prefab of the ball
    public Transform pitchPosition;    // Position where the ball is spawned
    public float pitchSpeed = 5f;
    public float curveIntensity = 0.75f;
    public int difficulty = 2;
    public float pitchInterval = 5f;

    private void Start()
    {
        StartCoroutine(PitchSequence());
    }

    private IEnumerator PitchSequence()
    {
        while (true)
        {
            // Spawn a new ball
            GameObject ballInstance = Instantiate(ballPrefab, pitchPosition.position, Quaternion.identity);

            // Initialize the ball with pitching parameters
            BallPitch ballScript = ballInstance.GetComponent<BallPitch>();
            ballScript.Initialize(pitchPosition, pitchSpeed, curveIntensity, difficulty);

            // Wait before pitching the next ball
            yield return new WaitForSeconds(pitchInterval);
        }
    }
}