using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionalOrbitalController : MonoBehaviour
{
    public Transform target; 
    public float orbitRadius = 2f;  
    public float moveSpeed = 10f;  

    private Vector2 currentOffset;
    private Vector2 desiredOffset; 

    private void Start()
    {
        if (target != null)
        {
            currentOffset = Vector2.right * orbitRadius;
            desiredOffset = currentOffset;
        }
    }

    private void Update()
    {
        if (target == null) return;

        Vector2 inputDirection = Vector2.zero;

        if (Keyboard.current.upArrowKey.isPressed) inputDirection.y += 1f;
        if (Keyboard.current.downArrowKey.isPressed) inputDirection.y -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed) inputDirection.x += 1f;
        if (Keyboard.current.leftArrowKey.isPressed) inputDirection.x -= 1f;

        inputDirection = inputDirection.normalized;

        if (inputDirection != Vector2.zero)
        {
            desiredOffset = inputDirection * orbitRadius;
        }

        currentOffset = Vector2.Lerp(currentOffset, desiredOffset, Time.deltaTime * moveSpeed);

        transform.position = target.position + (Vector3)currentOffset;
    }
}
