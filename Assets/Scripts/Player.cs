using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject Red;
    [SerializeField] private GameObject Blue;
    [SerializeField] private GameObject Shooter;

    private RedAnimation redAnimation;
    private BlueAnimation blueAnimation;
    private Rigidbody2D rigidBody;

    public int forceSpeed = 0;

    [SerializeField] private Vector2 movement;
    [SerializeField] private float speed;
    [SerializeField] private PlayerState state;

    public GameObject chargeParticleRed;
    public GameObject chargeParticleBlue;
    public Vector2 particleDirection;

    public string playerOrientation;
    public Vector2 playerDirection;
    public bool hasKey;
    public bool poleSwitcherCanBeCalled = false;

    private List<GameObject> list;
    public static Player Instance { get; private set; }

    public enum PlayerState { 
        NoForce,
        Force,
        PlayerRestricted
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        state = PlayerState.NoForce;
        list = new List<GameObject>();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
       
        rigidBody.freezeRotation = true;
        redAnimation = GetComponentInChildren<RedAnimation>(true);
        blueAnimation = GetComponentInChildren<BlueAnimation>(true);
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(movement.x * speed, movement.y * speed);
        TriggerPlayerTree();

        if (state == PlayerState.Force)
        {
            ExecuteForce(playerDirection);
        }

    }
    void Update()   
    {
        switch (state)
        {
            case PlayerState.NoForce:
                movement = Vector2.zero;

                Vector2 inputVector = GameInput.Instance.GetMovementVector();

                if (inputVector != Vector2.zero)
                {
                    movement = inputVector;
                }
                else
                {
                    if (GameInput.Instance.isRightPressed()) movement.x = 1;
                    if (GameInput.Instance.isLeftPressed())  movement.x = -1;
                    if (GameInput.Instance.isUpPressed())    movement.y = 1;
                    if (GameInput.Instance.isDownPressed())  movement.y = -1;
                }
                if (movement.x != 0)
                {
                    if (movement.x < 0)
                    {
                        particleDirection = Vector2.left;
                    }
                    else {
                        particleDirection = Vector2.right;
                    }
                }
                else if (movement.y != 0) {
                    if (movement.y < 0)
                    {
                        particleDirection = Vector2.down;
                    }
                    else { 
                        particleDirection = Vector2.up;
                    }
                }
                if (GameInput.Instance.IsShootPressed() && list.Count != 0)
                {
                    Shoot();
                    GameManager.Instance.IncrementShoots();
                }
                break;

            default:
            case PlayerState.Force:
            case PlayerState.PlayerRestricted:
                break;
        }
    }
  
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Red Pickup"))
        {
            AudioManager.Instance.PlayPickup();
            list.Add(chargeParticleRed);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Blue Pickup"))
        {
            AudioManager.Instance.PlayPickup();
            list.Add(chargeParticleBlue);
            Destroy(collision.gameObject);
        }
    }
    public void SetPlayerState(PlayerState state) {
        this.state = state;
    }
    public PlayerState GetPlayerState()
    {
        return state;
    }
    public void SetPlayerMovement() { 
        this.movement = Vector2.zero;
    }
    private void PoleSwitcher() {

        if (playerOrientation == "North")
        {
            Red.SetActive(false);
            Blue.SetActive(true);
            playerOrientation = "South";
        }
        else
        {
            Blue.SetActive(false);
            Red.SetActive(true);
            playerOrientation = "North";
        }
    }
    public void ExecuteForce(Vector2 direction) {
        movement = Vector2.zero;
        AudioManager.Instance.PlayForceExecution();
        rigidBody.AddForce(direction * forceSpeed, ForceMode2D.Impulse);
        state = PlayerState.NoForce;

        if (poleSwitcherCanBeCalled)
        {
            PoleSwitcher();
        }
    }
    private void Shoot() {
        TriggerOnShoot();
        Instantiate(list[0], Shooter.transform.position , Quaternion.identity);
        list.RemoveAt(0);
    }
    public void TriggerOnGameOver() {
        AudioManager.Instance.PlayPortalEnter();
        if (playerOrientation == "North")
        {
            redAnimation.OnGameOver();
        }
        else { 
            blueAnimation.OnGameOver();
        }
    }
    private void TriggerPlayerTree() {
        if (playerOrientation == "North")
        {
            redAnimation.PlayerTree(movement);
        }
        else
        {
            blueAnimation.PlayerTree(movement);
        }
    }
    private void TriggerOnShoot() {
        AudioManager.Instance.PlayShoot();
        if (playerOrientation == "North")
        {
            redAnimation.OnShoot();
        }
        else
        {
            blueAnimation.OnShoot();
        }
    }
}
