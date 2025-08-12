using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalController : MonoBehaviour
{
    public Transform centerObject;
    public float orbitDistance = 2f;
    public float rotationSpeed = 50f;
    public float acceleration = 100f;
    public float maxSpeed = 200f;
    public float minSpeed = -200f;
    public Vector3 rotationAxis = Vector3.forward;

    private void Start()
    {
        if (centerObject != null)
        {
            transform.position = centerObject.position + new Vector3(orbitDistance, 0f, 0f);
        }
    }

    private void Update()
    {
        if (centerObject == null) return;

        float verticalInput = (Keyboard.current.upArrowKey.isPressed ? 1f : 0f) -
                              (Keyboard.current.downArrowKey.isPressed ? 1f : 0f);

        rotationSpeed += verticalInput * acceleration * Time.deltaTime;
        rotationSpeed = Mathf.Clamp(rotationSpeed, minSpeed, maxSpeed);

        float angleThisFrame = rotationSpeed * Time.deltaTime;
        transform.RotateAround(centerObject.position, rotationAxis, angleThisFrame);
    }
}
