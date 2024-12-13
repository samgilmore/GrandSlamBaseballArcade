using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonScoreArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ballScript = other.GetComponent<Ball>();
            if (ballScript != null && !ballScript.HasScored)
            {
                ballScript.HasScored = true;
                Debug.Log("groundball/foul");
                GameManager.Instance.HandlePitchOutcome(isHomeRun: false);
                Destroy(other.gameObject, 5);
            }
        }
    }
}

