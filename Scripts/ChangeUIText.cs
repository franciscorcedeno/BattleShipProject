using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUIText : MonoBehaviour
{

    public TMPro.TextMeshProUGUI playerScoreUpdateText;
    public TMPro.TextMeshProUGUI AIScoreUpdateText;


    // Update is called once per frame
    public void UpdatePlayerScore(int score)
    {
        
        playerScoreUpdateText.text = "Player Score: " + score;
    }

    public void UpdateAIScore(int score)
    {
        AIScoreUpdateText.text = "AI Score: " + score;
    }
}
