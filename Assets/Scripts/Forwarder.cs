using UnityEngine;

public class Forwarder : MonoBehaviour
{
    public void StartGameForwarder() {
        GameManager.Instance.StartGame();
    }
}
