using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonScoreArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Foul/Groundball");
            GameManager.Instance.HandlePitchOutcome(isHomeRun: false);
            Destroy(other.gameObject);
        }
    }
}
