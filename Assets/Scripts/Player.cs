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
    //[SerializeField] private GameObject Raycaster;

    //public LayerMask raycastMask;
    //[SerializeField] private float rayDistance;

    private RedAnimation redAnimation;
    private BlueAnimation blueAnimation;

    //[SerializeField] private Animator redAnimator;
    //[SerializeField] private Animator blueAnimator;
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
        //playerOrientation = "North";
        list = new List<GameObject>();
        //magneticLayer = LayerMask.NameToLayer("Magnetic");
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

        if (state == PlayerState.Force || state == PlayerState.Force)
        {
            ExecuteForce(playerDirection);
        }

    }
    void Update()   
    {
        switch (state)
        {
            case PlayerState.NoForce:
                movement.x = Input.GetAxis("Horizontal");
                movement.y = Input.GetAxis("Vertical");
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
                if (Input.GetKeyDown(KeyCode.Space) && list.Count != 0)
                {
                    Shoot();
                }
                break;

            default:
            case PlayerState.Force:
            case PlayerState.PlayerRestricted:
                break;
        }
        //Vector2 origin = (Vector2)transform.position;
        //Vector2 dir = playerDirection.normalized;

        //// Draw a red line in the Scene view
        //Debug.DrawRay(origin, dir * rayDistance, Color.red);
    }
  
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Red Pickup"))
        {
            list.Add(chargeParticleRed);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Blue Pickup"))
        {
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
        Debug.Log("Execute Force is called!");
        movement = Vector2.zero;
        rigidBody.AddForce(direction * forceSpeed, ForceMode2D.Impulse);
        //StartCoroutine(SetStateAfterDelay(.01f));
        //if (state == PlayerState.PushForce)
        //{
        //    CheckForBounce(playerDirection);
        //}
        state = PlayerState.NoForce;

        if (poleSwitcherCanBeCalled)
        {
            PoleSwitcher();
        }
    }
    //private void CheckForBounce(Vector2 direction)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance,raycastMask);
       
    //    if (hit.collider != null)
    //    {
    //        Debug.Log("Ray hit: " + hit.transform.gameObject);

    //        if (state == PlayerState.NoForce)
    //        {
    //            // Normal collision → allow pushing/moving
    //            hit.collider.attachedRigidbody?.AddForce(direction * forceSpeed, ForceMode2D.Impulse);
    //        }
    //        else if (hit.collider.gameObject.name == "Object")
    //        {
    //            // Repel bounce → reflect velocity
    //            Vector2 reflect = Vector2.Reflect(rigidBody.linearVelocity, hit.normal);
    //            rigidBody.linearVelocity = reflect;
    //        }
    //    }
    //}
    //IEnumerator SetStateAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    state = PlayerState.NoForce;
    //}
    private void Shoot() {
        TriggerOnShoot();
        Instantiate(list[0], Shooter.transform.position , Quaternion.identity);
        list.RemoveAt(0);
    }
    public void TriggerOnGameOver() {
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
