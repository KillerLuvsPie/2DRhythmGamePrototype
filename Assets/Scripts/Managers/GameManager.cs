using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager Instance;
    //UI ELEMENTS
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI comboUI;
    public TextMeshProUGUI timerUI;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI hitIndicator;
    public Transform hitLine;
    //TIME VARIABLES
    private int seconds;
    private int minutes;
    private double startTime;
    private double currentTime;
    private double targetTime;
    private bool started;

    //GAME VARIABLES
    private int score = 0;
    private int scoreIncrement = 1;
    private int scoreDecrementIfMiss = 3;
    private int scoreDecrementIfWrong = 5;
    private int combo = 0;
    private float bpm = 150;
    private float scrollSpeed;
    private float inputRange = 0.50f;
    //FALLING INPUT VARIABLES
    public Transform inputGroup;
    private List<SpriteRenderer> circleList = new List<SpriteRenderer>();
    private List<SpriteRenderer> squareList = new List<SpriteRenderer>();
    private List<SpriteRenderer> triangleList = new List<SpriteRenderer>();
    private List<SpriteRenderer> diamondList = new List<SpriteRenderer>();
    private int circleListIndex = 0;
    private int squareListIndex = 0;
    private int triangleListIndex = 0;
    private int diamondListIndex = 0;

    
    //TIMER FUNCTION
    void CalculateTime()
    {
        currentTime = AudioSettings.dspTime - startTime;
        seconds = Mathf.RoundToInt((float)currentTime % 60f);
        minutes = Mathf.FloorToInt((float)currentTime / 60f);
        if (seconds >= 60)
        {
            seconds = 0;
            minutes += 1;
        }
        timerUI.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    //NOTE SCROLL FUNCTION
    private void ScrollNotes()
    {
        inputGroup.Translate(0,-scrollSpeed * Time.deltaTime,0);
    }
    private void CheckIfMusicIsDone()
    {
        
    }
    //POINTS FUNCTIONS
    public void IncreaseScore(int increment)
    {
        score += increment + combo/10;
        scoreUI.text = "Score:" + score; 
    }

    public void DecreaseScore(int decrement)
    {
        score -= decrement;
        scoreUI.text = "Score:" + score;
    }
    public void IncreaseCombo()
    {
        combo++;
    }

    public void ResetCombo()
    {
        combo = 0;
    }
    
    //FALLING INPUTS MANAGEMENT FUNCTIONS
    public Color AddTransparencyToUsedNote(Color color, float alpha)
    {
        return color = new Color(color.r, color.g, color.b, alpha);
    }
    //CIRCLE INPUT WAS PRESSED
    public void CirclePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(circleList[circleListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            IncreaseScore(scoreIncrement*2);
            circleList[circleListIndex].color = AddTransparencyToUsedNote(circleList[circleListIndex].color, 0.70f);
            circleList[circleListIndex].GetComponent<FallingInputController>().enabled = false;
            circleListIndex++;
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(circleList[circleListIndex].transform.position.y - hitLine.position.y) <= inputRange)
        {
            IncreaseScore(scoreIncrement);
            circleList[circleListIndex].color = AddTransparencyToUsedNote(circleList[circleListIndex].color, 0.45f);
            circleList[circleListIndex].GetComponent<FallingInputController>().enabled = false;
            circleListIndex++;
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            DecreaseScore(scoreDecrementIfWrong);
        }
    }
    //IF CIRCLE NOTE WAS MISSED
    public void MissedCircle()
    {
        if(circleList[circleListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            DecreaseScore(scoreDecrementIfMiss);
            circleList[circleListIndex].color = AddTransparencyToUsedNote(circleList[circleListIndex].color, 0.20f);
            circleList[circleListIndex].GetComponent<FallingInputController>().enabled = false;
            circleListIndex++;
        }
    }
    //SQUARE LIST

    //TRIANGLE LIST

    //DIAMOND LIST

    //START OF GAME COUNTDOWN
    private IEnumerator Countdown()
    {
        int seconds = 5;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            --seconds;
            if(seconds <= 3 && seconds > 0)
                countdown.text = seconds.ToString();
            else if(seconds == 0)
            {
                countdown.text = "START";
                startTime = AudioSettings.dspTime;
                started = true;
                yield return new WaitForSeconds(1.5f);
                countdown.text = "";
            }
        }
    }
    //UNITY FUNCTIONS
    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        for(int i = 0; i < inputGroup.childCount; i++)
        {
            FallingInputController fic = inputGroup.GetChild(i).GetComponent<FallingInputController>();
            switch(fic.fallingInput.inputName)
            {
                case "Circle":
                    circleList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                case "Square":
                    squareList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                case "Triangle":
                    triangleList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                case "Diamond":
                    diamondList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                default:
                    print("ERROR: Invalid falling input name at Awake function in GameManager");
                    break;
            }
        }
        for(int i = 0; i < circleList.Count; i++)
        {
            print("Circle List, Pos" + i + ":" + circleList[circleListIndex]);
        }
        for(int i = 0; i < squareList.Count; i++)
        {
            print(squareList[squareListIndex]);
        }
        for(int i = 0; i < triangleList.Count; i++)
        {
            print(triangleList[triangleListIndex]);
        }
        for(int i = 0; i < diamondList.Count; i++)
        {
            print(diamondList[diamondListIndex]);
        }
    }

    void Start()
    {
        scrollSpeed = bpm / 60;
        countdown.text = "";
        hitIndicator.text = "";
        StartCoroutine(Countdown());
        circleList[circleListIndex].color = AddTransparencyToUsedNote(circleList[circleListIndex].color, 50);
    }
    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            CalculateTime();
            ScrollNotes();
        }
    }
}
