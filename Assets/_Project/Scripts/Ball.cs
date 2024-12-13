using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool hasScored = false;
    public System.Action onBallHandled;

    public bool HasScored
    {
        get => hasScored;
        set
        {
            if (!hasScored)
            {
                hasScored = value;
                onBallHandled?.Invoke();
            }
        }
    }
}


