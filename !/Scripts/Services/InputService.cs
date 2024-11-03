using UnityEngine;

public class InputService 
{
    public PlayerInput playerInput;

    public float xMouseSensitivity { get; private set; } = 0.1f;
    public float yMouseSensitivity { get; private set; } = 0.1f;

    public InputService()
    {
        playerInput = new PlayerInput();
    }

    public Vector2 GetMovementNormalizedVector()
    {
        Vector2 inputVector = playerInput.Movement.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetMouseVector()
    {
        Vector2 mouseVector = playerInput.CameraLook.Look.ReadValue<Vector2>();

        return mouseVector;
    }

    public void SetCursorMode(CursorMode cursorMode)
    {
        switch(cursorMode)
        {
            case CursorMode.Game:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case CursorMode.Menu:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }

    public void EnableMovement(bool isEnabled)
    {
        if (isEnabled)
            playerInput.Movement.Enable();
        else
            playerInput.Movement.Disable();
    }

    public void EnableCameraLook(bool isEnabled)
    {
        if (isEnabled)
            playerInput.CameraLook.Enable();
        else
            playerInput.CameraLook.Disable();
    }

    public void EnableInteractions(bool isEnabled)
    {
        if (isEnabled)
            playerInput.Interactions.Enable();
        else
            playerInput.Interactions.Disable();
    }

    public void EnableAttack(bool isEnabled)
    {
        if (isEnabled)
            playerInput.Attack.Enable();
        else
            playerInput.Attack.Disable();
    }

    public void EnableExit(bool isEnabled)
    {
        if (isEnabled)
            playerInput.Exit.Enable();
        else
            playerInput.Exit.Disable();
    }
}
