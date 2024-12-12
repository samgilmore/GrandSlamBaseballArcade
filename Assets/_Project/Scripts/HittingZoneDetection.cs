using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittingZoneDetection : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // enable hitting physics here
        }
    }

    // debugging function to see hitting zone while editing
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
