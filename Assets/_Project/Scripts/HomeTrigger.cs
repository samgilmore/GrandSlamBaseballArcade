using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTrigger : MonoBehaviour
{
    private bool isGameDone = false;

    public void OnTriggerEnter(Collider other)
    { 
        if (other.transform.root.CompareTag("PlayerStart") && !isGameDone)
        {
            GameManager.Instance.PlayerEnteredTeleportArea();
            isGameDone = true;
        }
    }
}
