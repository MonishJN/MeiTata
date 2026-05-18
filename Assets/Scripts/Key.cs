using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") {
            Player.Instance.hasKey = true;
            Destroy(gameObject);
        }
    }
}
