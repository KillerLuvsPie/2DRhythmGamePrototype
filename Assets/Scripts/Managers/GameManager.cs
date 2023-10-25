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
    public bool started;

    //GAME VARIABLES
    private int score = 0;
    private int scoreIncrement = 1;
    private int scoreDecrementIfMiss = 3;
    private int scoreDecrementIfWrong = 5;
    private int combo = 0;
    private float bpm = 150;
    private float scrollSpeed;
    private float inputRange = 0.25f;
    private float perfectHitAlpha = 0f;
    private float normalHitAlpha = 0f;
    private float missedAlpha = 0.1f;

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
    public SpriteRenderer[] LastNotes;
    
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
        comboUI.text = "Combo:" + combo;
    }

    public void ResetCombo()
    {
        combo = 0;
        comboUI.text = "Combo:" + combo;
    }
    
    //FALLING NOTE LISTS MANAGEMENT FUNCTIONS
    private Color AddTransparencyToUsedNote(Color color, float alpha)
    {
        return color = new Color(color.r, color.g, color.b, alpha);
    }

    private void ShowHitMessage(float distance)
    {
        if(Mathf.Abs(distance) <= Mathf.Lerp(0, inputRange, 0.5f))
            hitIndicator.text = (-distance).ToString("0.000") + "\nNice!";
        else if(Mathf.Abs(distance) <= inputRange)
            hitIndicator.text = (-distance).ToString("0.000") + "\nOk";
        else
            hitIndicator.text = "\nMiss";
    }
    private void ShowEarlyHitMessage()
    {
        hitIndicator.text = "Too\nEarly";
    }
    private void ProcessNoteList(List<SpriteRenderer> noteList, int inputIndex, bool addScore, int score, int scoreMultiplier, float alpha)
    {
        if(addScore)
        {
            IncreaseScore(score * scoreMultiplier);
            IncreaseCombo();
        }
        else
        {
            DecreaseScore(score * scoreMultiplier);
            ResetCombo();
        }
        noteList[inputIndex].color = AddTransparencyToUsedNote(noteList[inputIndex].color, alpha);
        noteList[inputIndex].GetComponent<FallingInputController>().enabled = false;
        float distance = noteList[inputIndex].transform.position.y - hitLine.position.y;
        ShowHitMessage(distance);
        if(inputIndex < noteList.Count-1)
            inputIndex++;
    }

    //----------CIRCLE LIST----------
    //CIRCLE INPUT WAS PRESSED
    public void CirclePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(circleList[circleListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*List:*/circleList, /*ListIndex:*/circleListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 2, /*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(circleList[circleListIndex].transform.position.y - hitLine.position.y) <= inputRange)
        {
            ProcessNoteList(/*List:*/circleList, /*ListIndex:*/circleListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF CIRCLE NOTE WAS MISSED
    public void MissedCircle()
    {
        if(circleList[circleListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {

            ProcessNoteList(/*List:*/circleList, /*ListIndex:*/circleListIndex, /*Add score?*/false,
            /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
            
        }
    }

    //----------SQUARE LIST----------
    //SQUARE INPUT WAS PRESSED
    public void SquarePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(squareList[squareListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*List:*/squareList, /*ListIndex:*/squareListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(squareList[squareListIndex].transform.position.y - hitLine.position.y) <= inputRange)
        {
            ProcessNoteList(/*List:*/squareList, /*ListIndex:*/squareListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF SQUARE NOTE WAS MISSED
    public void MissedSquare()
    {
        if(squareList[squareListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            ProcessNoteList(/*List:*/squareList, /*ListIndex:*/squareListIndex, /*Add score?*/false,
            /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------TRIANGLE LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void TrianglePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(triangleList[triangleListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*List:*/triangleList, /*ListIndex:*/triangleListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(triangleList[triangleListIndex].transform.position.y - hitLine.position.y) <= inputRange)
        {
            ProcessNoteList(/*List:*/triangleList, /*ListIndex:*/triangleListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF TRIANGLE NOTE WAS MISSED
    public void MissedTriangle()
    {
        if(triangleList[triangleListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            ProcessNoteList(/*List:*/triangleList, /*ListIndex:*/triangleListIndex, /*Add score?*/false,
            /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }
    
    //----------DIAMOND LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void DiamondPressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(diamondList[diamondListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*List:*/diamondList, /*ListIndex:*/diamondListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(diamondList[diamondListIndex].transform.position.y - hitLine.position.y) <= inputRange)
        {
            ProcessNoteList(/*List:*/diamondList, /*ListIndex:*/diamondListIndex, /*Add score?*/true,
            /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF TRIANGLE NOTE WAS MISSED
    public void MissedDiamond()
    {
        if(diamondList[diamondListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            ProcessNoteList(/*List:*/diamondList, /*ListIndex:*/diamondListIndex, /*Add score?*/false,
            /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

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
        circleList.Add(LastNotes[0]);
        squareList.Add(LastNotes[1]);
        triangleList.Add(LastNotes[2]);
        diamondList.Add(LastNotes[3]);
        for(int i = 0; i < circleList.Count; i++)
        {
            print("Circle List, Pos " + i + ": " + circleList[i].name);
        }
        for(int i = 0; i < squareList.Count; i++)
        {
            print("Square List, Pos " + i + ": " + squareList[i].name);
        }
        for(int i = 0; i < triangleList.Count; i++)
        {
            print("Triangle List, Pos " + i + ": " + triangleList[i].name);
        }
        for(int i = 0; i < diamondList.Count; i++)
        {
            print("Diamond List, Pos " + i + ": " + diamondList[i].name);
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
