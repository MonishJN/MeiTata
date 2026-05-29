using UnityEngine;

[System.Serializable]
public class Data
{
    public int [] highScores = new int[20];
    public int levelsUnlocked;

    public Data(GameManager gameManager)
    {
       highScores = gameManager.playersBestScore;
       levelsUnlocked = gameManager.levelsUnlocked;
    }
}
