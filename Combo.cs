using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class Combo : MonoBehaviour
{

    public static bool NextLevel;

    public bool TimerEnabled;
    public bool TargetScoreEnabled;
    public float TimeLimit;
    public float TargetScore;
    public TextMeshProUGUI Timer;

    public static int ComboCount;
    public static int ScoreAdd;
    int HighScore;
    public static Vector3 DeathPos = new Vector3(0,0,0);

    public GameObject Text;
    public TextMeshProUGUI TextText;

    public int FrameCheck;
    public bool IsVisible;
    public bool Instantiated;

    public TextMeshProUGUI TempScore;
    public int TempScoreCount;

    public GameObject ScoreText;
    public TextMeshProUGUI ScoreTextText;
    public TextMeshProUGUI HighScoreDisplay;
    public TextMeshProUGUI TargetScoreDisplay;

    public Button NextLevelButton;

    public TextMeshProUGUI ButtonText;

    void Start()
    {

        //Basic UI Management, Setting Text and Buttons that only appear after certain events to be invisible, setting display titles of text

        TargetScoreDisplay.text = "Target Score: " + TargetScore.ToString();
        NextLevelButton.GetComponent<Image>().enabled = false;
        ButtonText.GetComponent<TextMeshProUGUI>().enabled = false;

        Text.SetActive(false);
        ScoreText.SetActive(false);
        IsVisible = false;

        //Check for if the game is running as an application or in editor because of how file pathing works the saved high score can only be retrieved if the game is running as an application

        if (Application.isPlaying && !Application.isEditor)
        {

            HighScore = GetHighScore();

        }
        else
        {

            HighScore = 30000;

        }

    }

    void FixedUpdate()
    {

        //Timer management, ammount of time on the timer can be edited in the inspector, this simply counts down and formats the time properly 

        if (TimeLimit <= 0.0f)
        {

            if (TimerEnabled == true)
            {

                gameObject.GetComponent<PlayerMovement>().MuteAudio();

                TargetScoreCheck();

                Time.timeScale = 0;

            }

        }
        else
        {

            TimeLimit -= Time.deltaTime;

            int TimeRound = Mathf.RoundToInt(TimeLimit);

            Timer.text = TimeRound.ToString();

        }

        //Check whether multiple enemies have been killed in quick succession, every time an enemy dies they increment combo count which is reset aproximately every frame

        if (ComboCount > 1)
        {

            //Finds the position of the last enemy to die, moves the combo and score display elements to that position and sets them to represent the correct data

            if (DeathPos != new Vector3(0, 0, 0))
            {

                Text.transform.position = DeathPos;
                TextText.text = (ComboCount + "x Multiplier");
                ScoreTextText.text = ((ComboCount * 2) + "00");
                TempScoreCount += ((ComboCount * 2) * ScoreAdd);
                ScoreText.transform.position = new Vector3(DeathPos.x, (DeathPos.y + 4f), DeathPos.z);
                
                if (ComboCount > 2)
                {

                    SecondCamView.EnableActionCam = true;

                }

                TempScore.text = ("Score: " + TempScoreCount);

                Text.SetActive(true);
                ScoreText.SetActive(true);

                IsVisible = true;
                FrameCheck = 0;

            }

        }
        else if (ComboCount == 1)
        {

            //Does the same as the previous piece of code but only shows the score as no multiplier has been achieved

            if (DeathPos != new Vector3(0, 0, 0))
            {

                ScoreTextText.text = (ScoreAdd.ToString());
                TempScoreCount += ScoreAdd;
                ScoreText.transform.position = new Vector3(DeathPos.x, (DeathPos.y + 2f), DeathPos.z);

                TempScore.text = ("Score: " + TempScoreCount);

                ScoreText.SetActive(true);

                IsVisible = true;
                FrameCheck = 0;

            }

        }

        //Frame counter which deals with a prior check on whether the action cam, which enables after a 3x or higher mult, needs to be enabled, is disabled after 250 frames

        if (FrameCheck > 250 && IsVisible == true)
        {

            SecondCamView.EnableActionCam = false;

        }

        //Frame counter which keeps the score and mult text on screen for 500 frames

        if (FrameCheck > 500 && IsVisible == true)
        {

            Text.SetActive(false);
            ScoreText.SetActive(false);
            DeathPos = new Vector3(0, 0, 0);

            IsVisible = false;
            FrameCheck = 0;

        }

        ComboCount = 0;
        FrameCheck++;

        //Check for whether the score is higher than the current high score & various other pieces of high score management 

        if (TempScoreCount > HighScore)
        {

            HighScore = TempScoreCount;

            if (Application.isPlaying && !Application.isEditor)
            {

                WriteHighScore();

            }

        }

        HighScoreDisplay.text = ("High Score: " + HighScore);

    }

    //Subroutine which reads the high score from the file 

    int GetHighScore()
    {

        string CurrentHighScore;

        CurrentHighScore = File.ReadAllText(Application.dataPath + "/HighScores.txt");


        return int.Parse(CurrentHighScore);

    }

    //Subroutine which writes the high score to the file 

    void WriteHighScore()
    {

        string HighScoreSave = HighScore.ToString();

        File.WriteAllText(Application.dataPath + "/HighScores.txt", HighScoreSave);

    }

    //Check for whether the target score has been met at the end of the time period, if it has the game continues to the next level

    void TargetScoreCheck()
    {

        NextLevelButton.GetComponent<Image>().enabled = true;
        ButtonText.GetComponent<TextMeshProUGUI>().enabled = true;

        if (TempScoreCount > TargetScore)
        {

            NextLevel = true;

            ButtonText.text = "Next Level";

        }
        else
        {

            NextLevel = false;

            ButtonText.text = "Restart";

        }

    }

}
