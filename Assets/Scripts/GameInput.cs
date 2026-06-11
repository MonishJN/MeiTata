using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputActions inputActions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
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
        Vector2 joystickInput = inputActions.Player.movement.ReadValue<Vector2>();
        Vector2 lockedInput = Vector2.zero;

        // 3. Compare which direction the thumb is pushing harder
        if (joystickInput.magnitude > 0.1f)
        {
            // Is the horizontal push stronger than the vertical push?
            if (Mathf.Abs(joystickInput.x) > Mathf.Abs(joystickInput.y))
            {
                // Snap strictly Left (-1) or Right (1), and completely kill vertical movement
                lockedInput.x = joystickInput.x > 0 ? 1f : -1f;
            }
            else // Otherwise, the vertical push is stronger
            {
                // Snap strictly Down (-1) or Up (1), and completely kill horizontal movement
                lockedInput.y = joystickInput.y > 0 ? 1f : -1f;
            }
        }
        return lockedInput;
    }
}
