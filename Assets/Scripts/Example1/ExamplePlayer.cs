using UnityEngine;
using UnityEngine.InputSystem;

public class ExamplePlayer : MonoBehaviour
{
    Vector2 movementInput;
    [SerializeField] private float movementSpeed = 0;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    public void OnMovement_Input(InputAction.CallbackContext value)
    {
        movementInput = value.ReadValue<Vector2>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement.Normalize();
        movement *= movementSpeed;
        cc.Move(movement);
    }
}
