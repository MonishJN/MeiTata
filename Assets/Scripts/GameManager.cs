using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NUnit.Framework.Constraints;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup faderImage;
    [SerializeField] private GameObject joyStickUI;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject scoreBoard;

    private bool joyStickNeeded = false;
    private int firedShots;
    private int forceExperienced;
    private int score;
    public int[] playersBestScore = new int[20];
    public int levelsUnlocked ;
    private string currentScene = "Main Menu";
    private string previousScene;
    private int currentLevel = 1;

    public static GameManager Instance { get; private set; }
    private void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        // Check if we're on a mobile platform (Android or iOS)
        #if UNITY_ANDROID || UNITY_IOS
            joyStickNeeded = true;
        #elif UNITY_WEBGL && !UNITY_EDITOR
            joyStickNeeded = Application.isMobilePlatform; 
        #else
            joyStickNeeded = false;
        #endif


        //Load Saved Data cleanly
        Data loadedData = SaveSystem.LoadData(); 
        if (loadedData != null) {
            playersBestScore = loadedData.highScores;
            levelsUnlocked = loadedData.levelsUnlocked;
        } else {
            playersBestScore = new int[20];
            levelsUnlocked = 1;
            SaveSystem.SaveData(this);
        }
    }
    private void Start()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Additive);

    }


    public void GotoThatLevel(int level) {
        
        //Reset the score and other variables for the new level
        firedShots = 0;
        forceExperienced = 0;

        previousScene = currentScene;
        currentLevel = level;
        currentScene = "Level" + currentLevel;
        
        StartCoroutine(SceneTransition(previousScene, currentScene));
    }
    public void PauseGame() {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void LevelCompleted() { 
        joyStickUI.SetActive(false);
        pauseButton.SetActive(false);
        levelCompleteUI.SetActive(true);
        AudioManager.Instance.PlayLevelComplete();
        DisplayScore();
        levelsUnlocked = Mathf.Max(levelsUnlocked, currentLevel + 1);
        SaveSystem.SaveData(this);
        if (currentScene == "Level20") {
            levelCompleteUI.transform.GetChild(0).GetChild(3).gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
            return;
        }
    }
    public void BackToMainMenu() {
        Time.timeScale = 1f;
        levelCompleteUI.SetActive(false);
        joyStickUI.SetActive(false);
        pauseButton.SetActive(false);
        pauseMenu.SetActive(false);
        levelCompleteUI.transform.GetChild(0).GetChild(3).gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
        previousScene = currentScene;
        currentScene = "Main Menu";
        StartCoroutine(SceneTransition(previousScene, currentScene));
    }
    public void ReloadLevel() {
        Time.timeScale = 1f;
        levelCompleteUI.SetActive(false);
        pauseMenu.SetActive(false);
        //Reset the score and other variables for the new level
        firedShots = 0;
        forceExperienced = 0;

        StartCoroutine(SceneTransition(currentScene, currentScene));
    }
    public void NextLevel()
    {
        levelCompleteUI.SetActive(false);
        //Reset the score and other variables for the new level
        firedShots = 0;
        forceExperienced = 0;

        // 3. Load the new level
        previousScene = currentScene;
        currentLevel++;
        currentScene = "Level" + currentLevel;
        StartCoroutine(SceneTransition(previousScene,currentScene));
    }
    public IEnumerator SceneTransition(string from,string to)
    {
        // 1. Start Fading to Black
        yield return StartCoroutine(Fade(1f)); // 1f = full black

        // 2. Unload the current level
        AsyncOperation unload = SceneManager.UnloadSceneAsync(from);
        while (!unload.isDone) yield return null;

        AsyncOperation load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        while (!load.isDone) yield return null;

        // 4. Set the new scene as active (for lighting/physics)
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(to));

        // 5. Fade back in
        yield return StartCoroutine(Fade(0f)); // 0f = transparent
        if(to != "Main Menu")
        {
            joyStickUI.SetActive(joyStickNeeded);
            pauseButton.SetActive(true);
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        CanvasGroup cg = faderImage.GetComponent<CanvasGroup>();
        while (!Mathf.Approximately(cg.alpha, targetAlpha))
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, targetAlpha, Time.deltaTime * 2f);
            yield return null;
        }
    }
    private void DisplayScore() {
    CalculateScore();
    StartCoroutine(ShowScoreLinesSequentially());
}

    private IEnumerator ShowScoreLinesSequentially() {
        Transform scoreboardParent = scoreBoard.transform;

        string[] values = new string[] {
            firedShots.ToString(),
            forceExperienced.ToString(),
            playersBestScore[currentLevel-1].ToString(),
            score.ToString()
        };

        for (int i = 0; i < 4; i++) {
            scoreboardParent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < 4; i++) {
            Transform row = scoreboardParent.GetChild(i);
            
            var valueTextMesh = row.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            
            valueTextMesh.text = values[i];

            row.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f); 
        }
    }

    public void IncrementShoots() {
        firedShots++;
    }
    public void IncrementForceExperienced() {
        forceExperienced++;
    }
    public void CalculateScore() {
        score = 10000 - (firedShots * 150 + forceExperienced * 300);
        if (score <= 1500) {
            score = 1500;
        }
        if(playersBestScore[currentLevel-1] < score) {
            playersBestScore[currentLevel-1] = score;
        }

    }

}
