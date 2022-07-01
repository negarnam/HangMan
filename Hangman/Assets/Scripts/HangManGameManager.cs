using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using SimpleJSON;
using System.Linq;
using UnityEngine.SceneManagement;

public class HangManGameManager : MonoBehaviour
{
    private Dictionary<string, string> gameDict;
    [Header("Text placeholders")]
    public Text questionText;
    public Text answerText;
    [Header("Keyboard buttons")]
    public List<Button> keyboardButtons;
    private string correctAnswer;
    private string userInput;
    private const char placeHolder = '_';
    [Header("Sound clip")]
    public AudioClip buttonSoundClip;
    private AudioSource audioSource;
    [Header("Hangman sprites")]
    public GameObject[] HangmanParts;
    public static int loseCount = 0;
    public static int winCount = 0;
    private const int HangManPartsNum = 7; //more as a reminder for myself
    private int wrongAnswersNum = 0;

    void Start()
    {
        gameDict = new Dictionary<string, string>();
        audioSource = GetComponent<AudioSource>();
        CreateDictionary();
        SpawnRandomQuestion();
        foreach (var button in keyboardButtons)
        {
            button.onClick.AddListener(delegate { CheckUserAnswer(button.name.ToString()); });
        }
    }

    /// <summary>
    /// parses the json and creates a dictionary base on it
    /// </summary>
    private void CreateDictionary()
    {
        TextAsset temp = Resources.Load("MyDictionary") as TextAsset;
        string jsonFile = temp.ToString();
        JSONObject jsonObj = (JSONObject)JSON.Parse(jsonFile);
        foreach (var key in jsonObj.GetKeys())
        {
            gameDict[key] = jsonObj[key];
        }
    }

    /// <summary>
    /// spawns a random question and answer from dictionary
    /// </summary>
    private void SpawnRandomQuestion()
    {
        var randomKey = gameDict.Keys.ToArray()[(int)Random.Range(0, gameDict.Count)];

        questionText.text = randomKey;
        correctAnswer = gameDict[randomKey].ToUpper();

        //create dashes & initial userinput
        StringBuilder sb = new StringBuilder("");
        StringBuilder sb1 = new StringBuilder("");
        for (int i = 0; i < correctAnswer.Length; i++)
        {
            sb.Append(placeHolder);
            sb.Append(" ");
            sb1.Append(placeHolder);
        }
        answerText.text = sb.ToString();
        userInput = sb1.ToString();
    }

    public void CheckUserAnswer(string buttonName)
    {
        audioSource.PlayOneShot(buttonSoundClip, 1);
        var currentUserInputLetter = buttonName;
        if (correctAnswer.Contains(currentUserInputLetter))
        {
            UpdateAnswerTextField(currentUserInputLetter[0]);
            if (correctAnswer.Equals(userInput))
            {
                Button[] buttons = FindObjectsOfType<Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }

                winCount++;
                Invoke("LoadWinScene", 0.5f);
            }
        }
        else
        {

            WrongAnswer();
            if (wrongAnswersNum == HangManPartsNum)
            {
                foreach (var button in keyboardButtons)
                {
                    button.interactable = false;
                }
                loseCount++;
                Invoke("LoadLoseScene", 0.5f);
            }
        }
    }

    private void UpdateAnswerTextField(char inputLetter)
    {
        //split the string to chars
        char[] temp = userInput.ToCharArray();
        for (int i = 0; i < correctAnswer.Length; i++)
        {
            if (temp[i] != inputLetter) // already gussed
            {
                if (correctAnswer[i] == inputLetter)
                {
                    temp[i] = inputLetter;
                }
            }
        }
        //rebuild the chars into new string-Update Answer
        userInput = new string(temp);
        //Build the string for showing in UI
        StringBuilder sb = new StringBuilder();
        for (int j = 0; j < correctAnswer.Length; j++)
        {
            sb.Append(temp[j]);
            sb.Append(" ");
        }
        answerText.text = sb.ToString();
    }

    private void WrongAnswer()
    {
        if (wrongAnswersNum < HangManPartsNum)
        {
            HangmanParts[wrongAnswersNum].SetActive(true);
            wrongAnswersNum++;
        }

    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
    }
    private void LoadLoseScene()
    {
        SceneManager.LoadScene("Lose");
    }

}



