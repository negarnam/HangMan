using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.IO;
using SimpleJSON;
using System.Linq;
using UnityEngine.SceneManagement;

public class HangManGameManager : MonoBehaviour
{
    private Dictionary<string, string> gameDict;
    public Text questionText;
    public Text answerText;
    private string correctAnswer;
    private string userInput;
    private const char placeHolder = '_';
    public AudioClip buttonSoundClip;
    
    private AudioSource audioSource;
    private const int HangManPartsNum = 7; //more as a reminder for myself
    private int wrongAnswersNum = 0;
    public GameObject[] HangmanParts;
    public static int LoseCount=0;
    public static int WinCount=0;
    // Start is called before the first frame update
    void Start()
    {
        gameDict = new Dictionary<string, string>();
        audioSource = GetComponent<AudioSource>();
        CreateDictionary();
        RandomQuestion();
       
  
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void CreateDictionary()
    {
        //Read the JsonFile and Parse it
        //string jsonFile = File.ReadAllText(Application.dataPath + "/Resources/MyDictionary.json");// doesn't work when built to Android
        TextAsset temp = Resources.Load("MyDictionary") as TextAsset;
        string jsonFile = temp.ToString();
       // Debug.Log(jsonFile);
        JSONObject jsonObj = (JSONObject)JSON.Parse(jsonFile);
      //  Debug.Log(jsonObj.GetKeys()[2]);
        //Create Dictionary 
        foreach (var key in jsonObj.GetKeys())
        {
            gameDict[key] = jsonObj[key];
            
        }

      /* foreach (KeyValuePair<string, string> dicvalue in gameDict)
        {
            Debug.Log(dicvalue.Key);
            Debug.Log(dicvalue.Value);
        }*/

    }

    private void RandomQuestion()
    {
        var randomKey = gameDict.Keys.ToArray()[(int)Random.Range(0, gameDict.Count)];

        questionText.text = randomKey;
        correctAnswer = gameDict[randomKey].ToUpper();
        Debug.Log(correctAnswer);

        
        //create dashes & initial userinput
        StringBuilder sb = new StringBuilder("");
        StringBuilder sb1 = new StringBuilder("");
        for (int i =0; i<correctAnswer.Length; i++)
        {
            sb.Append(placeHolder);
            sb.Append(" ");
            sb1.Append(placeHolder);

        }
        answerText.text = sb.ToString();
        userInput = sb1.ToString();
        Debug.Log(userInput);
    }

    public void CheckUserAnswer(Button button)
    {
        audioSource.PlayOneShot(buttonSoundClip,1);
        var currentUserInputLetter = button.name.ToString();
        if(correctAnswer.Contains(currentUserInputLetter))
        {
            Debug.Log("correct letter");
            Debug.Log(currentUserInputLetter);
            UpdateAnswer(currentUserInputLetter[0]);
            
            if(correctAnswer.Equals(userInput))
            {
                Debug.Log("you won");
                Button[] buttons = FindObjectsOfType<Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
              
                WinCount++;
                Invoke("LoadWinScene",0.5f);
               
            }
        }
        else
        {
            Debug.Log("Wrong Letter");
            WrongAnswer();
            Debug.Log("you lost");
            if(wrongAnswersNum==HangManPartsNum)
            {
                Button[] buttons = FindObjectsOfType<Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
                LoseCount++;
                Invoke("LoadLoseScene", 0.5f);
            }
            


        }
    }

    private void UpdateAnswer(char inputLetter)
    {
        //split the string to chars
        char[] temp = userInput.ToCharArray();
        for(int  i = 0; i<correctAnswer.Length; i++)
        {
            if(temp[i]!=inputLetter) // already gussed
            {
                if(correctAnswer[i]==inputLetter)
                {
                    temp[i] = inputLetter;
                    Debug.Log(temp[i]);
                   

                }
            }
        }
        
        //rebuild the chars into new string-Update Answer
        userInput = new string(temp);
        Debug.Log(userInput);
        //Build the string for showing in UI
        StringBuilder sb = new StringBuilder();
        for(int j= 0; j<correctAnswer.Length;j++)
        {
            sb.Append(temp[j]);
            sb.Append(" ");
        }
        answerText.text = sb.ToString();
    }

    private void WrongAnswer()
    {
        if(wrongAnswersNum<HangManPartsNum)
        {
            HangmanParts[wrongAnswersNum].SetActive(true);
            wrongAnswersNum++;
        }
        else
        {

           
            
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



