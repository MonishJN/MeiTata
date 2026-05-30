using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Disable();
    }
    public bool IsShootPressed() {
        return inputActions.Player.shoot.triggered;
    }
    public bool isRightPressed() {
        return inputActions.Player.right.IsPressed();
    }
        public bool isLeftPressed() {
        return inputActions.Player.left.IsPressed();
    }    public bool isUpPressed() {
        return inputActions.Player.up.IsPressed();
    }    public bool isDownPressed() {
        return inputActions.Player.down.IsPressed();
    }
    public Vector2  GetMovementVector() {
        return inputActions.Player.movement.ReadValue<Vector2>();
    }
}
