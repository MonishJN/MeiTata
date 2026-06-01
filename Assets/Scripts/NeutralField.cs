using UnityEngine;

public class NeutralField : MonoBehaviour
{
    [SerializeField] private new BoxCollider2D collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.IncrementForceExperienced();
            Player.Instance.playerDirection = GetDirectionToClosestPoint();
            Player.Instance.SetPlayerState(Player.PlayerState.Force);

        }
    }
    private Vector2 GetDirectionToClosestPoint()
    {
        if (Player.Instance == null) return Vector2.zero;

        Vector2 playerPosition = Player.Instance.transform.position;
        Vector2 closestPoint = Physics2D.ClosestPoint(playerPosition, collider);
        Player.Instance.poleSwitcherCanBeCalled = true;
        Player.Instance.forceSpeed = 150;
        return (closestPoint - playerPosition).normalized;
    }
}
