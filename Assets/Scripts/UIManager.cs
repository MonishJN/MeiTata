using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;
    public void Learnt() { 
        tutorialUI.SetActive(false);
    }
}
