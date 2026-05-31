using UnityEngine;

public class Forwarder : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject levelMenuUI;

    int [] playersBestScore = new int[20];
    int levelsUnlocked;
    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        levelMenuUI.SetActive(true);   
        Data loadedData = SaveSystem.LoadData();
        if (loadedData != null) {
            playersBestScore = loadedData.highScores;
            levelsUnlocked = loadedData.levelsUnlocked;
        } 
        for(int i =2; i<=20; i++) {
            if (i <= GameManager.Instance.levelsUnlocked) {
                levelMenuUI.transform.GetChild(0).GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = true;
            } else {
                levelMenuUI.transform.GetChild(0).GetChild(i).GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }

    }
    public void GoTOThatLevelForwarder(int level) {
        GameManager.Instance.GotoThatLevel(level);
    }
   public void BackToMainMenu() {
        levelMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}
