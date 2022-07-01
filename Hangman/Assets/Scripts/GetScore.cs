using UnityEngine;
using UnityEngine.UI;


public class GetScore : MonoBehaviour
{
    [Header("Text placeholders")]
    public Text winCount;
    public Text loseCount;
    // Start is called before the first frame update
    void Start()
    {
        SetWinCount();
        SetLoseCount();
    }

    public void SetWinCount()
    {
        winCount.text = HangManGameManager.winCount.ToString();
    }
    public void SetLoseCount()
    {
        loseCount.text = HangManGameManager.loseCount.ToString();
    }
}
