using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonScoreArea : MonoBehaviour
{
    public AudioSource ballSource;
    public AudioClip thud;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballSource.clip = thud;
            ballSource.Play();

            Ball ballScript = other.gameObject.GetComponent<Ball>();
            if (ballScript != null && !ballScript.HasScored)
            {
                ballScript.HasScored = true;
                Debug.Log("groundball/foul");
                GameManager.Instance.HandlePitchOutcome(isHomeRun: false);
                Destroy(other.gameObject, 3);
            }
        }
    }
}

