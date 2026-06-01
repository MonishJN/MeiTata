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
    public void BackgroundMusicForwarder(bool isEnabled) {
        AudioManager.Instance.BackgroundMusic(isEnabled);
    }
    public void BGVolumeForwarder(float volume) {
        AudioManager.Instance.SetBGVolume(volume);
    }
    public void SFXForwarder(bool isEnabled) {
        AudioManager.Instance.SFX(isEnabled);
    }
    public void SFXVolumeForwarder(float volume) {
        AudioManager.Instance.SetSFXVolume(volume);
    }
    public void GoTOThatLevelForwarder(int level) {
        GameManager.Instance.GotoThatLevel(level);
    }
   public void BackToMainMenu() {
        levelMenuUI.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        mainMenuUI.SetActive(true);
    }
    public void GoToOptions() {
        mainMenuUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }
    public void GoToCredits() {
        mainMenuUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        mainMenuUI.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
    }
}
