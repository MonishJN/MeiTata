using UnityEngine;

public class Object : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Rigidbody2D objectRigidBody;
    public new BoxCollider2D collider;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Player.Instance.GetPlayerState() == Player.PlayerState.NoForce)
            {
                Debug.Log("OnCollisionEnter2D is Running!");
                objectRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            //else
            //{
            //    Debug.Log("this else block from object collider eneter2d method is running!");
            //    objectRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            //    //Player.Instance.playerDirection = ApplyPushAway();
            //    //Player.Instance.playerDirection = -Player.Instance.playerDirection;
            //    //Player.Instance.SetPlayerState(Player.PlayerState.PushForce);
            //}
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("OnCollisionExit2D is Running!");
            objectRigidBody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Magnetic Field") || collision.gameObject.CompareTag("Door") || collision.gameObject.CompareTag("Neutral Field")) {
            Destroy(gameObject);
        }
    }
    //private Vector2 ApplyPushAway()
    //{
    //    if (Player.Instance == null) return Vector2.zero;

    //    Vector2 playerPosition = Player.Instance.transform.position;
    //    Vector2 closestPoint = Physics2D.ClosestPoint(playerPosition, collider);

    //    Player.Instance.poleSwitcherCanBeCalled = false;
    //    Player.Instance.forceSpeed = 210;
    //    return -(closestPoint - playerPosition).normalized;
    //}
}
