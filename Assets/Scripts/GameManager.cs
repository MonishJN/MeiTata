using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private CanvasGroup faderImage;

    [SerializeField] private GameObject levelCompleteUI;
    //[SerializeField] private GameObject tutorialUI;
    private string currentScene = "Main Menu";
    private string previousScene;
    private int currentLevel = 1;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void StartGame() {
        //SceneManager.LoadSceneAsync("Level" + currentLevel, LoadSceneMode.Additive);
        //currentScene = "Level" + currentLevel;
        //SceneManager.UnloadSceneAsync("Main Menu");
        previousScene = currentScene;
        currentScene = "Level" + currentLevel;
        StartCoroutine(TransitionToNextLevel(previousScene, currentScene));
    }
    public void LevelCompleted() { 
        levelCompleteUI.SetActive(true);
    }
    public void ReloadLevel() {
        levelCompleteUI.SetActive(false);
        StartCoroutine(TransitionToNextLevel(currentScene, currentScene));

    }
    public void NextLevel()
    {
        levelCompleteUI.SetActive(false);

        // 3. Load the new level
        previousScene = currentScene;
        currentLevel++;
        if (currentLevel == 15)
        {
            //Debug.Log("Work in Progress! Just Play from the Start!");
            //currentLevel = 1;
            levelCompleteUI.transform.GetChild(3).gameObject.SetActive(false);
        }
        currentScene = "Level" + currentLevel;
        StartCoroutine(TransitionToNextLevel(previousScene,currentScene));
    }
    //private void TutorialUI() {
    //    if (currentLevel <= 1) { 
    //        tutorialUI.SetActive(true);
    //    }
    //}
    public IEnumerator TransitionToNextLevel(string from,string to)
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
        //TutorialUI();
    }

    private IEnumerator Fade(float targetAlpha)
    {
        CanvasGroup cg = faderImage.GetComponent<CanvasGroup>();
        while (!Mathf.Approximately(cg.alpha, targetAlpha))
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, targetAlpha, Time.deltaTime * 2f); // Adjust speed here
            yield return null;
        }
    }

}
