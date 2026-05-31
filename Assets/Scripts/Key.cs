using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            AudioManager.Instance.PlayKey();
            Player.Instance.hasKey = true;
            Destroy(gameObject);
        }
    }
}
