using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using NUnit.Framework.Constraints;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup faderImage;
    [SerializeField] private GameObject joyStickUI;

    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject scoreUI;


    private int firedShots;
    private int forceExperienced;
    private int score;
    public int[] playersBestScore = new int[20];
    public int levelsUnlocked ;
    //[SerializeField] private GameObject tutorialUI;
    private string currentScene = "Main Menu";
    private string previousScene;
    private int currentLevel = 1;

    public static GameManager Instance { get; private set; }
    private void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // <-- destroy the duplicate object
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Data loadedData = SaveSystem.LoadData(); 
        if (loadedData != null) {
            Debug.Log("Data loaded successfully.");
            playersBestScore = loadedData.highScores;
            levelsUnlocked = loadedData.levelsUnlocked;
        } else {
            // File exists but data is null → fallback
            Debug.LogWarning("Data file found but data is null. Initializing defaults.");
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
    public void LevelCompleted() { 
        joyStickUI.SetActive(false);
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
        levelCompleteUI.SetActive(false);
        joyStickUI.SetActive(false);
        levelCompleteUI.transform.GetChild(0).GetChild(3).gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
        previousScene = currentScene;
        currentScene = "Main Menu";
        StartCoroutine(SceneTransition(previousScene, currentScene));
    }
    public void ReloadLevel() {
        levelCompleteUI.SetActive(false);
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
            joyStickUI.SetActive(true);
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
    StartCoroutine(ShowScoreLines());
}

private IEnumerator ShowScoreLines() {
    
    var textMesh = scoreUI.GetComponent<TMPro.TextMeshProUGUI>();

    // Line values
    string[] lines = new string[] {
        firedShots.ToString(),
        forceExperienced.ToString(),
        playersBestScore[currentLevel-1].ToString(),
        score.ToString()
    };

    textMesh.text = ""; // clear first

    // Sequential reveal
    for (int i = 0; i < lines.Length; i++) {
        textMesh.text += lines[i] + "\n";
        yield return new WaitForSeconds(0.5f); // wait before showing next line
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
