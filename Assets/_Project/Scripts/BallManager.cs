using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab;       // Prefab of the ball
    public Transform pitchPosition;    // Position where the ball is spawned
    public float pitchSpeed = 5f;
    public float curveIntensity = 0.75f;
    public float pitchInterval = 5f;

    private bool isPitching = false;

    public void StartPitching(int difficulty, int pitchesPerGame)
    {
        if (isPitching) return;

        isPitching = true;
        StartCoroutine(PitchSequence(difficulty, pitchesPerGame));
    }

    public void StopPitching()
    {
        isPitching = false;
        StopAllCoroutines();
    }

    private IEnumerator PitchSequence(int difficulty, int pitchesToThrow)
    {
        for (int i = 0; i < pitchesToThrow; i++)
        {
            if (!isPitching) yield break;

            // Spawn a new ball
            GameObject ballInstance = Instantiate(ballPrefab, pitchPosition.position, Quaternion.identity);

            // Initialize the ball with pitching parameters
            BallPitch ballScript = ballInstance.GetComponent<BallPitch>();
            ballScript.Initialize(pitchPosition, pitchSpeed, curveIntensity, difficulty);

            // Notify GameManager about the pitch
            GameManager.Instance.HandlePitchOutcome(false);

            // Wait before pitching the next ball
            yield return new WaitForSeconds(pitchInterval);
        }

        // Notify GameManager that pitching is done
        GameManager.Instance.EndGame();
    }
}