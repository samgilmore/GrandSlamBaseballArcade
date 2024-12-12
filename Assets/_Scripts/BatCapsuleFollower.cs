using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCapsuleFollower : MonoBehaviour
{
    private BatCapsule _batFollower;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    [SerializeField]
    private float _sensitivity = 100f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //private void FixedUpdate()
    //{
    //    Vector3 destination = _batFollower.transform.position;
    //    _rigidbody.transform.rotation = transform.rotation;

    //    _velocity = (destination - _rigidbody.transform.position) * _sensitivity;

    //    _rigidbody.velocity = _velocity;
    //    transform.rotation = _batFollower.transform.rotation;
    //}

    private void FixedUpdate()
    {
        if (_batFollower == null) return; // Safety check

        Vector3 destination = _batFollower.transform.position;
        Quaternion targetRotation = _batFollower.transform.rotation;

        // Smoothly move the Rigidbody to the target position
        Vector3 newPosition = Vector3.MoveTowards(_rigidbody.position, destination, _sensitivity * Time.fixedDeltaTime);
        _rigidbody.MovePosition(newPosition);

        // Smoothly rotate the Rigidbody to the target rotation
        _rigidbody.MoveRotation(targetRotation);
    }


    public void SetFollowTarget(BatCapsule batFollower)
    {
        _batFollower = batFollower;
    }
}
