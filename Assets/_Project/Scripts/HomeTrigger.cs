using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Not yet");


        if (other.transform.root.CompareTag("PlayerStart"))
        {
            Debug.Log("Entered");
            GameManager.Instance.PlayerEnteredTeleportArea();
            
        }
    }
}
