using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager Instance;
    //ENUMS
    public enum InputType {Circle, Square, Triangle, Diamond}
    //UI ELEMENTS
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI comboUI;
    public TextMeshProUGUI timerUI;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI hitIndicator;
    //REMOVE hitLine VARIABLE AS IT IS NOW OBSOLETE AFTER NEW MECHANICS ARE IMPLEMENTED
    public Transform hitLine;
    //HIT MARKERS FOR NOTES (TO REPLACE hitLine)
    public Transform circleHitZone;
    public Transform squareHitZone;
    public Transform triangleHitZone;
    public Transform diamondHitZone;

    //TIME VARIABLES
    private int seconds;
    private int minutes;
    private double currentTime;
    public bool started;
    //AUDIO VARIABLES
    public AudioSource audioSource;
    //GAME VARIABLES
    private int score = 0;
    [SerializeField]
    private int scoreIncrement = 1;
    [SerializeField]
    private int scoreDecrementIfMiss = 3;
    [SerializeField]
    private int scoreDecrementIfWrong = 5;
    private int combo = 0;
    [SerializeField]
    public float scrollSpeed = 1;
    [SerializeField]
    private float inputRange = 0.25f;
    [SerializeField]
    private float perfectHitAlpha = 0f;
    [SerializeField]
    private float normalHitAlpha = 0f;
    [SerializeField]
    private float missedAlpha = 0.1f;

    //FALLING INPUT VARIABLES
    public GameObject circleInput;
    public GameObject squareInput;
    public GameObject triangleInput;
    public GameObject diamondInput;
    public Transform inputChart;
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
        currentTime = audioSource.time;
        seconds = (int)(currentTime % 60);
        minutes = Mathf.FloorToInt((float)currentTime / 60f);
        if (seconds >= 60)
        {
            seconds = 0;
            minutes += 1;
        }
        timerUI.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    //NOTE SCROLL FUNCTION (OBSOLETE ... DELETE THIS AFTER NEW MOVE METHOD IS COMPLETE OR CHANGE TO SOMETHING ELSE)
    private void ScrollNotes()
    {
        inputChart.Translate(0,-scrollSpeed * Time.deltaTime,0);
        //inputChart.position = new Vector2(0, -currentTime * scrollSpeed);
    }

    //END OF MUSIC FUNCTION
    private void CheckIfMusicIsDone()
    {
        
    }
    
    //POINTS FUNCTIONS
    public void IncreaseScore(int increment)
    {
        score += increment + combo/10;
        scoreUI.text = "Score:" + score.ToString("000000"); 
    }

    public void DecreaseScore(int decrement)
    {
        score -= decrement;
        scoreUI.text = "Score:" + score.ToString("000000");
    }
    public void IncreaseCombo()
    {
        combo++;
        comboUI.text = "Combo:" + combo.ToString("000");
    }

    public void ResetCombo()
    {
        combo = 0;
        comboUI.text = "Combo:" + combo.ToString("000");
    }
    
    //FALLING NOTE LISTS MANAGEMENT FUNCTIONS
    private void NoteSetup(GameObject obj, float xPos, float inputTime, List<SpriteRenderer> list)
    {
        GameObject instance = Instantiate(obj, inputChart);
        instance.transform.localPosition = new Vector2(xPos, inputTime * scrollSpeed);
        //COMMENT THIS NEXT LINE WHEN DEBUG IS DONE (LESS LOAD)
        instance.GetComponent<FallingInputController>().inputTime = inputTime;
        //-----------------------------------------
        list.Add(instance.GetComponent<SpriteRenderer>());
    }

    private Color AddTransparencyToUsedNote(Color color, float alpha)
    {
        return color = new Color(color.r, color.g, color.b, alpha);
    }

    private void ShowHitMessage(float distance)
    {
        if(distance <= Mathf.Lerp(0, inputRange, 0.5f))
            hitIndicator.text = (-distance).ToString("0.000") + "\nNice!";
        else if(distance <= inputRange)
            hitIndicator.text = (-distance).ToString("0.000") + "\nOk";
        else
            hitIndicator.text = "\nMiss";
    }

    private void ShowEarlyHitMessage()
    {
        hitIndicator.text = "Too\nEarly";
    }

    private void ProcessNoteList(InputType inputType, bool addScore, int score, int scoreMultiplier, float alpha)
    {
        float distance = 0;
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
        switch (inputType)
        {
            case InputType.Circle:
                circleList[circleListIndex].color = AddTransparencyToUsedNote(circleList[circleListIndex].color, alpha);
                circleList[circleListIndex].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(circleList[circleListIndex].transform.position, circleHitZone.position);
                if(circleListIndex < circleList.Count-1)
                    circleListIndex++;
                break;
            case InputType.Square:
                squareList[squareListIndex].color = AddTransparencyToUsedNote(squareList[squareListIndex].color, alpha);
                squareList[squareListIndex].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(squareList[squareListIndex].transform.position, squareHitZone.position);
                if(squareListIndex < squareList.Count-1)
                    squareListIndex++;
                break;
            case InputType.Triangle:
                triangleList[triangleListIndex].color = AddTransparencyToUsedNote(triangleList[triangleListIndex].color, alpha);
                triangleList[triangleListIndex].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(triangleList[triangleListIndex].transform.position, triangleHitZone.position);
                if(triangleListIndex < triangleList.Count-1)
                    triangleListIndex++;
                break;
            case InputType.Diamond:
                diamondList[diamondListIndex].color = AddTransparencyToUsedNote(diamondList[diamondListIndex].color, alpha);
                diamondList[diamondListIndex].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(diamondList[diamondListIndex].transform.position, diamondHitZone.position);
                if(diamondListIndex < diamondList.Count-1)
                    diamondListIndex++;
                break;
        }
        ShowHitMessage(distance);
    }

    //----------CIRCLE LIST----------
    //CIRCLE INPUT WAS PRESSED
    public void CirclePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(circleList[circleListIndex].transform.position, circleHitZone.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2, /*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(circleList[circleListIndex].transform.position, circleHitZone.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
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
    public void MissedCircle(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - circleHitZone.position).normalized;
        if(-direction != noteDirection && Vector2.Distance(circleList[circleListIndex].transform.position, circleHitZone.position) > inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------SQUARE LIST----------
    //SQUARE INPUT WAS PRESSED
    public void SquarePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(squareList[squareListIndex].transform.position, squareHitZone.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(squareList[squareListIndex].transform.position, squareHitZone.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
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
    public void MissedSquare(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - squareHitZone.position).normalized;
        if(-direction != noteDirection && Vector2.Distance(squareList[squareListIndex].transform.position, squareHitZone.position) > inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------TRIANGLE LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void TrianglePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(triangleList[triangleListIndex].transform.position, triangleHitZone.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(triangleList[triangleListIndex].transform.position, triangleHitZone.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
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
    public void MissedTriangle(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - triangleHitZone.position).normalized;
        if(-direction != noteDirection && Vector2.Distance(triangleList[triangleListIndex].transform.position, triangleHitZone.position) > inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }
    
    //----------DIAMOND LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void DiamondPressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(diamondList[diamondListIndex].transform.position, diamondHitZone.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(diamondList[diamondListIndex].transform.position, diamondHitZone.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF DIAMOND NOTE WAS MISSED
    public void MissedDiamond(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - diamondHitZone.position).normalized;
        print("HitZone Direction: " + direction + "\tNote Direction: " + noteDirection);
        if(-direction != noteDirection && Vector2.Distance(diamondList[diamondListIndex].transform.position, diamondHitZone.position) > inputRange)
        {
            print(Vector2.Distance(diamondList[diamondListIndex].transform.position, diamondHitZone.position));
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
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
                audioSource.Play();
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
        SaveLoadManager.LoadChart(audioSource.clip.name);
        int indexOffset = 0;
        for(int i = 0; i < SaveLoadManager.inputTypeCountList.Count; i++)
        {
            for(int j = indexOffset; j < SaveLoadManager.inputTypeCountList[i] + indexOffset; j++)
            {
                switch(i)
                {
                    case 0:
                        NoteSetup(circleInput, 0, SaveLoadManager.inputTimeList[j], circleList);
                        break;
                    case 1:
                        NoteSetup(squareInput, 2, SaveLoadManager.inputTimeList[j], squareList);
                        break;
                    case 2:
                        NoteSetup(triangleInput, 4, SaveLoadManager.inputTimeList[j], triangleList);
                        break;
                    case 3:
                        NoteSetup(diamondInput, 6, SaveLoadManager.inputTimeList[j], diamondList);
                        break;
                    default:
                        print("ERROR: inputTypeCountList in SaveLoadManager has more than 4 entries");
                        break;
                }
            }
            indexOffset += SaveLoadManager.inputTypeCountList[i];
        }
        circleList.Add(LastNotes[0]);
        squareList.Add(LastNotes[1]);
        triangleList.Add(LastNotes[2]);
        diamondList.Add(LastNotes[3]);
        /* UNCOMMENT IF IT IS NECESSARY TO CHECK WHAT IS IN THE LISTS
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
        }*/
    }

    void Start()
    {
        countdown.text = "";
        hitIndicator.text = "";
        StartCoroutine(Countdown());
    }
    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            CalculateTime();
        }
    }
}
