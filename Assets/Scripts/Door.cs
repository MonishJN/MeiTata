using UnityEngine;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject RedDoor;
    [SerializeField] private GameObject BlueDoor;

    [SerializeField] private string doorOrientaion;

    public new BoxCollider2D collider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (Player.Instance.hasKey)
            {
                if (Player.Instance.playerOrientation == doorOrientaion)
                {
                    Debug.Log("Cant Enter!");
                    Player.Instance.playerDirection = ApplyPushAway();
                    Player.Instance.SetPlayerState(Player.PlayerState.Force);
                    return;
                }
                Player.Instance.SetPlayerState(Player.PlayerState.PlayerRestricted);
                Player.Instance.SetPlayerMovement();
                Player.Instance.transform.position = transform.position;
                Debug.Log("Door Position is :" + transform.position);
                Player.Instance.TriggerOnGameOver();
                Debug.Log("Level Completed!");
            }
            else {
                if (Player.Instance.playerOrientation == doorOrientaion)
                {
                    Debug.Log("Cant Enter!");
                    Player.Instance.playerDirection = ApplyPushAway();
                    Player.Instance.SetPlayerState(Player.PlayerState.Force);
                    return;
                }
                Debug.Log("Need to keyto Enter!");
            }
        }
    }
    private Vector2 ApplyPushAway()
    {
        if (Player.Instance == null) return Vector2.zero;

        Vector2 playerPosition = Player.Instance.transform.position;
        Vector2 closestPoint = Physics2D.ClosestPoint(playerPosition, collider);
        
            Player.Instance.poleSwitcherCanBeCalled = false;
            Player.Instance.forceSpeed = 300;
            return -(closestPoint - playerPosition).normalized;
    }
    public string GetDoorOrientaion() {
        return doorOrientaion;
    }
    public void ChangeDoorOrientation() {
        if (doorOrientaion == "North")
        {
            doorOrientaion = "South";
            RedDoor.SetActive(false);
            BlueDoor.SetActive(true);
        }
        else
        {
            doorOrientaion = "North";
            RedDoor.SetActive(true);
            BlueDoor.SetActive(false);

        }
    }
   
}
