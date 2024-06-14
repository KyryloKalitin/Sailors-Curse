using UnityEngine;

public class InputService 
{
    public PlayerInputActions playerInput;

    public float xMouseSensitivity { get; private set; } = 0.1f;
    public float yMouseSensitivity { get; private set; } = 0.1f;

    private InputService()
    {
        playerInput = new PlayerInputActions();

        playerInput.CameraLook.Enable();
        playerInput.Interactions.Enable();
        playerInput.Attack.Enable();

        playerInput.Movement.Jump.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Vector2 GetMovementNormalizedVector()
    {
        Vector2 inputVector = playerInput.Movement.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetMouseVector()
    {
        Vector2 mouseVector = playerInput.CameraLook.Look.ReadValue<Vector2>();

        return mouseVector;
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        if (isEnabled)
            playerInput.Movement.Movement.Enable();
        else
            playerInput.Movement.Movement.Disable();
    }

    public void SetLookingEnabled(bool isEnabled)
    {
        if (isEnabled)
            playerInput.CameraLook.Enable();
        else
            playerInput.CameraLook.Disable();
    }

    public void SetCursorEnabled(bool isEnable)
    {
        if(isEnable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

}
