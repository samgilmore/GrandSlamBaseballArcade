using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPitch : MonoBehaviour
{
    public Transform pitchPosition;
    public Transform hittingZone;
    public float throwForce = 20f;
    public float pitchDelay = 2f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody>();
        InvokeRepeating("Pitch", 1f, pitchDelay);
    }

    void Pitch()
    {
        // Reset previous forces on ball
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Position ball back at starting point
        transform.position = pitchPosition.position;

        // Calculate direction to hitting zone
        Vector3 direction = (hittingZone.position - pitchPosition.position).normalized;

        // Add randomness to pitch
        direction += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

        // Apply force to ball
        rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
