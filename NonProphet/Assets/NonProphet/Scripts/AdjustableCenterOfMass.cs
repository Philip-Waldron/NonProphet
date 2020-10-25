using UnityEngine;

// Expose center of mass to allow it to be set from the inspector.
public class AdjustableCenterOfMass : MonoBehaviour
{
    public Vector3 CenterOfMass;
    public Rigidbody Rigidbody;

    void Start()
    {
        Rigidbody.centerOfMass = CenterOfMass;
    }
}