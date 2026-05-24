using UnityEngine;

public class UIManager : MonoBehaviour
{
   [SerializeField] private GameObject tutorialUI;
private int currentChild = 1;

public void Learnt()
{
    // If we've reached the last child, hide the whole tutorial UI
    if (currentChild >= tutorialUI.transform.childCount - 1)
    {
        // Disable the current child
        // tutorialUI.transform.GetChild(currentChild).gameObject.SetActive(false);

        // Finally disable the whole tutorial panel
        tutorialUI.SetActive(false);
    }
    else
    {
        // Disable the current child
        tutorialUI.transform.GetChild(currentChild).gameObject.SetActive(false);

        // Move to the next child
        currentChild++;

        // Enable the next child
        tutorialUI.transform.GetChild(currentChild).gameObject.SetActive(true);
    }
}
}
