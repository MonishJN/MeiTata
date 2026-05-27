using UnityEngine;

public class Forwarder : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelMenuUI;
    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        levelMenuUI.SetActive(true);

    }
    public void GoTOThatLevelForwarder(int level) {
        GameManager.Instance.GotoThatLevel(level);
    }
}
