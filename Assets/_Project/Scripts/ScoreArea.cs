using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ballScript = other.GetComponent<Ball>();
            if (ballScript != null && !ballScript.HasScored)
            {
                ballScript.HasScored = true;
                Debug.Log("Homerun");
                GameManager.Instance.HandlePitchOutcome(isHomeRun: true);
                Destroy(other.gameObject, 5);
            }
        }
    }
}
