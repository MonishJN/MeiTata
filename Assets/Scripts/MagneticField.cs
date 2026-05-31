using UnityEngine;
using UnityEngine.UIElements;

public class MagneticField : MonoBehaviour
{
    [SerializeField] private new BoxCollider2D collider;
    [SerializeField] private GameObject RedField;
    [SerializeField] private GameObject BlueField;

    [SerializeField] private string fieldOrientation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player Entered Magnetic Field!");
            GameManager.Instance.IncrementForceExperienced();
            Player.Instance.SetPlayerState(Player.PlayerState.Force);
            Player.Instance.playerDirection = GetDirectionToClosestPoint();
        }
    }

    private Vector2 GetDirectionToClosestPoint()
    {
        if (Player.Instance == null) return Vector2.zero;

        Vector2 playerPosition = Player.Instance.transform.position;
        Vector2 closestPoint = Physics2D.ClosestPoint(playerPosition, collider);
        if(fieldOrientation == Player.Instance.playerOrientation){
            Player.Instance.poleSwitcherCanBeCalled = false;
            Player.Instance.forceSpeed = 300;
            return -(closestPoint - playerPosition).normalized; 
        }
        Player.Instance.poleSwitcherCanBeCalled = true;
        Player.Instance.forceSpeed = 150;
        return (closestPoint - playerPosition).normalized; 
    }
    public string GetFieldOrientation() { 
        return fieldOrientation;
    }
    public void ChangeFieldOrientation()
    {
        // if (fieldOrientation == "Neutral") {
        //     return;
        // }
        if (fieldOrientation == "North")
        {
            fieldOrientation = "South";
            RedField.SetActive(false);
            BlueField.SetActive(true);
        }
        else
        {
            fieldOrientation = "North";
            RedField.SetActive(true);
            BlueField.SetActive(false);
           
        }
    }
}
