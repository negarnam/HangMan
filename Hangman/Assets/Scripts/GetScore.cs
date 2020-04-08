using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetScore : MonoBehaviour
{
    
    public Text winCount;
    public Text loseCount;
    // Start is called before the first frame update
    void Start()
    {
        UpdateWinText();
        UpdateLoseText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateWinText()
    {
        winCount.text = HangManGameManager.WinCount.ToString();
    }
   public void UpdateLoseText()
    {
        loseCount.text = HangManGameManager.LoseCount.ToString();
    }
}
