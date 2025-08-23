using UnityEngine;

public class OrbitalRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 180f; 
    [SerializeField] private Vector3 rotationAxis = Vector3.forward; 
    [SerializeField] private Space rotationSpace = Space.Self; 

    [Header("Optional Target")]
    [SerializeField] private Transform orbitalTransform; 

    private void Awake()
    {
        if (orbitalTransform == null)
        {
            orbitalTransform = transform;
        }
    }

    private void Update()
    {
        RotateOrbital();
    }

    private void RotateOrbital()
    {
        orbitalTransform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, rotationSpace);
    }

    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

}