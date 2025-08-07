using UnityEngine;
using UnityEngine.InputSystem;

public class RotateChildAroundParent : MonoBehaviour
{
    public GameObject rotationObject;
    public float rotationSpeed = 50f;
    public float acceleration = 100f;
    public float maxSpeed = 200f;
    public float minSpeed = -200f;
    public Vector3 rotationAxis = Vector3.up;
    public float orbitDistance = 5f;

    void Start()
    {
        if (rotationObject != null)
        {
            rotationObject.transform.position = transform.position + new Vector3(orbitDistance, 0f, 0f);
        }
    }

    void Update()
    {
        if (rotationObject == null)
        {
            Debug.LogWarning("Child object not assigned to RotateChildAroundParent script.");
            return;
        }

        float horizontalInput = (Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f);

        rotationSpeed += horizontalInput * acceleration * Time.deltaTime;

        rotationSpeed = Mathf.Clamp(rotationSpeed, minSpeed, maxSpeed);

        float angleThisFrame = rotationSpeed * Time.deltaTime;

        rotationObject.transform.RotateAround(transform.position, rotationAxis, angleThisFrame);
    }
}
