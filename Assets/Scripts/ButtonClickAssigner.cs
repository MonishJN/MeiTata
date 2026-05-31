using UnityEngine;
using UnityEngine.UI; // Required for accessing Button components

public class ButtonSoundAssigner : MonoBehaviour
{
    private void Start()
    {
        // 1. Find all Button components sitting under this UI Canvas/Panel
        Button[] allButtons = GetComponentsInChildren<Button>(true);

        // 2. Loop through every single button found
        foreach (Button button in allButtons)
        {
            // 3. Add a listener to the button's onClick event via code
            button.onClick.AddListener(PlayButtonSound);
        }
    }

    private void PlayButtonSound()
    {
        // Call your central AudioManager instance here
        if (AudioManager.Instance != null)
        {
            // Assuming you add a generic UI click function to your manager
            AudioManager.Instance.PlayButtonClick(); // Or your specific click method
        }
    }
}