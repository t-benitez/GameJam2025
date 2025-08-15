using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalController : MonoBehaviour
{
    public Transform centerObject;
    public float orbitDistance = 1f;
    public float rotationSpeed = 180f; // Degrees per second

    private float currentAngle; // In degrees
    private float targetAngle;  // In degrees

    private void Start()
    {
        if (centerObject != null)
        {
            Vector3 offset = transform.position - centerObject.position;
            currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            targetAngle = currentAngle;
        }
    }

    private void Update()
    {
        if (centerObject == null) return;

        // 1. Determine input direction
        Vector2 input = Vector2.zero;
        if (Keyboard.current.upArrowKey.isPressed) input.y += 1f;
        if (Keyboard.current.downArrowKey.isPressed) input.y -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
        if (Keyboard.current.leftArrowKey.isPressed) input.x -= 1f;

        // 2. If input, set targetAngle
        if (input != Vector2.zero)
        {
            input.Normalize();
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            if (angle < 0f) angle += 360f;
            targetAngle = angle;
        }

        // 3. Smoothly move currentAngle toward targetAngle (shortest path)
        float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
        float maxStep = rotationSpeed * Time.deltaTime;
        if (Mathf.Abs(angleDiff) > 0.01f)
        {
            float step = Mathf.Clamp(angleDiff, -maxStep, maxStep);
            currentAngle += step;
        }
        else
        {
            currentAngle = targetAngle;
        }

        // 4. Keep angle in [0, 360)
        if (currentAngle < 0f) currentAngle += 360f;
        if (currentAngle >= 360f) currentAngle -= 360f;

        // 5. Update position
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * orbitDistance;
        transform.position = centerObject.position + offset;
    }
}