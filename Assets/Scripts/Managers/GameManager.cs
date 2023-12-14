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
    public TextMeshProUGUI timerUI;
    public TextMeshProUGUI countdown;
    //PLAYER 1 UI ELEMENTS
    public TextMeshProUGUI scoreUI_P1;
    public TextMeshProUGUI comboUI_P1;
    public TextMeshProUGUI hitIndicator_P1;
    //PLAYER 2 UI ELEMENTS
    public TextMeshProUGUI scoreUI_P2;
    public TextMeshProUGUI comboUI_P2;
    public TextMeshProUGUI hitIndicator_P2;
    
    //HIT MARKERS FOR NOTES
    //PLAYER 1
    public Transform circleHitMark_P1;
    public Transform squareHitMark_P1;
    public Transform triangleHitMark_P1;
    public Transform diamondHitMark_P1;
    //PLAYER 2
    public Transform circleHitMark_P2;
    public Transform squareHitMark_P2;
    public Transform triangleHitMark_P2;
    public Transform diamondHitMark_P2;

    //TIME VARIABLES
    private int seconds;
    private int minutes;
    public double currentTime;
    public bool started;

    //AUDIO VARIABLES
    public AudioSource audioSource;

    //GAME VARIABLES
    private int score_P1 = 0;
    private int score_P2 = 0;
    [SerializeField]
    private int scoreIncrement = 1;
    [SerializeField]
    private int scoreDecrementIfMiss = 3;
    [SerializeField]
    private int scoreDecrementIfWrong = 5;
    private int combo_P1 = 0;
    private int combo_P2 = 0;
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

    //FALLING NOTES VARIABLES
    public float baseTranslateDuration = 2;

    //REFERENCES
    public GameObject circleNote;
    public GameObject squareNote;
    public GameObject triangleNote;
    public GameObject diamondNote;
    public Transform noteChart_P1;
    public Transform noteChart_P2;

    //NOTE LISTS
    //PLAYER 1
    public List<SpriteRenderer> circleList_P1 = new List<SpriteRenderer>();
    public List<SpriteRenderer> squareList_P1 = new List<SpriteRenderer>();
    public List<SpriteRenderer> triangleList_P1 = new List<SpriteRenderer>();
    public List<SpriteRenderer> diamondList_P1 = new List<SpriteRenderer>();
    //PLAYER 2
    public List<SpriteRenderer> circleList_P2 = new List<SpriteRenderer>();
    public List<SpriteRenderer> squareList_P2 = new List<SpriteRenderer>();
    public List<SpriteRenderer> triangleList_P2 = new List<SpriteRenderer>();
    public List<SpriteRenderer> diamondList_P2 = new List<SpriteRenderer>();

    //NOTE LIST INDEX (NEXT NOTE TO BE PRESSED)
    //PLAYER 1
    private int circleListIndex_P1 = 0;
    private int squareListIndex_P1 = 0;
    private int triangleListIndex_P1 = 0;
    private int diamondListIndex_P1 = 0;
    //PLAYER 2
    private int circleListIndex_P2 = 0;
    private int squareListIndex_P2 = 0;
    private int triangleListIndex_P2 = 0;
    private int diamondListIndex_P2 = 0;

    //NOTE ACTIVATION INDEX (NEXT NOTE TO BE ACTIVATED AND TRANSLATED)
    //PLAYER 1
    private int circleActivationIndex_P1 = 0;
    private int squareActivationIndex_P1 = 0;
    private int triangleActivationIndex_P1 = 0;
    private int diamondActivationIndex_P1 = 0;
    //PLAYER 2
    private int circleActivationIndex_P2 = 0;
    private int squareActivationIndex_P2 = 0;
    private int triangleActivationIndex_P2 = 0;
    private int diamondActivationIndex_P2 = 0;

    //NOTE ACTIvATION TIMES
    //PLAYER 1
    private double circleActivationTime_P1;
    private double squareActivationTime_P1;
    private double triangleActivationTime_P1;
    private double diamondActivationTime_P1;
    //PLAYER 2
    private double circleActivationTime_P2;
    private double squareActivationTime_P2;
    private double triangleActivationTime_P2;
    private double diamondActivationTime_P2;

    //LAST NOTES (USED TO AVOID INDEX OUT OF RANGE ERROR, RESULTS IN A "TOO EARLY" WHEN THERE ARE NO NOTES LEFT) | 0 = CIRCLE | 1 = SQUARE | 2 = TRIANGLE | 3 = DIAMOND |
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
        noteChart_P1.Translate(0,-scrollSpeed * Time.deltaTime,0);
        //inputChart.position = new Vector2(0, -currentTime * scrollSpeed);
    }

    //END OF MUSIC FUNCTION
    private void CheckIfMusicIsDone()
    {
        
    }
    
    //POINTS FUNCTIONS
    public void IncreaseScore(bool p1, int increment)
    {
        if(p1)
        {
            score_P1 += increment + combo_P1/10;
            scoreUI_P1.text = "Score:" + score_P1.ToString("000000"); 
        }
        else
        {
            score_P2 += increment + combo_P1/10;
            scoreUI_P2.text = "Score:" + score_P2.ToString("000000");
        }
    }

    public void DecreaseScore(bool p1, int decrement)
    {
        if(p1)
        {
            score_P1 -= decrement;
            scoreUI_P1.text = "Score:" + score_P1.ToString("000000");
        }
        else
        {
            score_P2 -= decrement;
            scoreUI_P2.text = "Score:" + score_P2.ToString("000000");
        }
    }
    public void IncreaseCombo(bool p1)
    {
        if(p1)
        {
            combo_P1++;
            comboUI_P1.text = "Combo:" + combo_P1.ToString("000");
        }
        else
        {
            combo_P2++;
            comboUI_P2.text = "Combo:" + combo_P2.ToString("000");
        }
    }

    public void ResetCombo()
    {
        combo_P1 = 0;
        comboUI_P1.text = "Combo:" + combo_P1.ToString("000");
    }
    
    //FALLING NOTE LISTS MANAGEMENT FUNCTIONS
    private void NoteSetup(GameObject obj, float xPos, float inputTime, List<SpriteRenderer> list)
    {
        GameObject instance = Instantiate(obj, noteChart_P1);
        instance.transform.position = new Vector2(0,0);
        //instance.transform.localPosition = new Vector2(xPos, inputTime * scrollSpeed);
        instance.GetComponent<FallingInputController>().inputTime = inputTime;
        instance.GetComponent<FallingInputController>().outlineRenderer.color = new Color(0, 0.75f, 1, 1);
        list.Add(instance.GetComponent<SpriteRenderer>());
    }

    private Color AddTransparencyToUsedNote(Color color, float alpha)
    {
        return color = new Color(color.r, color.g, color.b, alpha);
    }

    private void ShowHitMessage(float distance)
    {
        if(distance <= Mathf.Lerp(0, inputRange, 0.5f))
            hitIndicator_P1.text = (-distance).ToString("0.000") + "\nNice!";
        else if(distance <= inputRange)
            hitIndicator_P1.text = (-distance).ToString("0.000") + "\nOk";
        else
            hitIndicator_P1.text = "\nMiss";
    }

    private void ShowEarlyHitMessage()
    {
        hitIndicator_P1.text = "Too\nEarly";
    }

    private void ProcessNoteList(InputType inputType, bool addScore, int score, int scoreMultiplier, float alpha)
    {
        float distance = 0;
        if(addScore)
        {
            IncreaseScore(true, score * scoreMultiplier);
            IncreaseCombo(true);
        }
        else
        {
            DecreaseScore(true, score * scoreMultiplier);
            ResetCombo();
        }
        switch (inputType)
        {
            case InputType.Circle:
                circleList_P1[circleListIndex_P1].color = AddTransparencyToUsedNote(circleList_P1[circleListIndex_P1].color, alpha);
                circleList_P1[circleListIndex_P1].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(circleList_P1[circleListIndex_P1].transform.position, circleHitMark_P1.position);
                if(circleListIndex_P1 < circleList_P1.Count-1)
                    circleListIndex_P1++;
                break;
            case InputType.Square:
                squareList_P1[squareListIndex_P1].color = AddTransparencyToUsedNote(squareList_P1[squareListIndex_P1].color, alpha);
                squareList_P1[squareListIndex_P1].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(squareList_P1[squareListIndex_P1].transform.position, squareHitMark_P1.position);
                if(squareListIndex_P1 < squareList_P1.Count-1)
                    squareListIndex_P1++;
                break;
            case InputType.Triangle:
                triangleList_P1[triangleListIndex_P1].color = AddTransparencyToUsedNote(triangleList_P1[triangleListIndex_P1].color, alpha);
                triangleList_P1[triangleListIndex_P1].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(triangleList_P1[triangleListIndex_P1].transform.position, triangleHitMark_P1.position);
                if(triangleListIndex_P1 < triangleList_P1.Count-1)
                    triangleListIndex_P1++;
                break;
            case InputType.Diamond:
                diamondList_P1[diamondListIndex_P1].color = AddTransparencyToUsedNote(diamondList_P1[diamondListIndex_P1].color, alpha);
                diamondList_P1[diamondListIndex_P1].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(diamondList_P1[diamondListIndex_P1].transform.position, diamondHitMark_P1.position);
                if(diamondListIndex_P1 < diamondList_P1.Count-1)
                    diamondListIndex_P1++;
                break;
        }
        ShowHitMessage(distance);
    }

    //----------CIRCLE LIST----------
    //CIRCLE INPUT WAS PRESSED
    public void CirclePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(circleList_P1[circleListIndex_P1].transform.position, circleHitMark_P1.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2, /*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(circleList_P1[circleListIndex_P1].transform.position, circleHitMark_P1.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(true, scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF CIRCLE NOTE WAS MISSED
    public void MissedCircle(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - circleHitMark_P1.position).normalized;
        if(Vector2.Dot(direction, noteDirection) > 0.999f && Vector2.Distance(circleList_P1[circleListIndex_P1].transform.position, circleHitMark_P1.position) > inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------SQUARE LIST----------
    //SQUARE INPUT WAS PRESSED
    public void SquarePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(squareList_P1[squareListIndex_P1].transform.position, squareHitMark_P1.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(squareList_P1[squareListIndex_P1].transform.position, squareHitMark_P1.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(true, scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF SQUARE NOTE WAS MISSED
    public void MissedSquare(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - squareHitMark_P1.position).normalized;
        if(Vector2.Dot(direction, noteDirection) > 0.999f && Vector2.Distance(squareList_P1[squareListIndex_P1].transform.position, squareHitMark_P1.position) > inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------TRIANGLE LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void TrianglePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(triangleList_P1[triangleListIndex_P1].transform.position, triangleHitMark_P1.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(triangleList_P1[triangleListIndex_P1].transform.position, triangleHitMark_P1.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(true, scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF TRIANGLE NOTE WAS MISSED
    public void MissedTriangle(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - triangleHitMark_P1.position).normalized;
        if(Vector2.Dot(direction, noteDirection) > 0.999f && Vector2.Distance(triangleList_P1[triangleListIndex_P1].transform.position, triangleHitMark_P1.position) > inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }
    
    //----------DIAMOND LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void DiamondPressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Vector2.Distance(diamondList_P1[diamondListIndex_P1].transform.position, diamondHitMark_P1.position) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Vector2.Distance(diamondList_P1[diamondListIndex_P1].transform.position, diamondHitMark_P1.position) <= inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 1,/*Alpha change*/ normalHitAlpha);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage();
            DecreaseScore(true, scoreDecrementIfWrong);
            ResetCombo();
        }
    }
    //IF DIAMOND NOTE WAS MISSED
    public void MissedDiamond(Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction = (noteTransform.position - diamondHitMark_P1.position).normalized;
        //print("HitZone Direction: " + direction + "\tNote Direction: " + noteDirection + "Dot Product: " + Vector2.Dot(-direction, noteDirection));
        if(Vector2.Dot(direction, noteDirection) > 0.999f && Vector2.Distance(diamondList_P1[diamondListIndex_P1].transform.position, diamondHitMark_P1.position) > inputRange)
        {
            //print(Vector2.Distance(diamondList[diamondListIndex].transform.position, diamondHitZone.position));
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //NOTE ACTIVATION FUNCTIONS
    private void SetActivationTime(string noteType)
    {
        switch(noteType)
        {
            case "Circle":
                if(circleActivationIndex_P1 < circleList_P1.Count-1)
                    circleActivationTime_P1 = circleList_P1[circleActivationIndex_P1].GetComponent<FallingInputController>().inputTime;
                break;
            case "Square":
                print(squareActivationIndex_P1);
                if(squareActivationIndex_P1 < squareList_P1.Count-1)
                    squareActivationTime_P1 = squareList_P1[squareActivationIndex_P1].GetComponent<FallingInputController>().inputTime;
                break;
            case "Triangle":
                if(triangleActivationIndex_P1 < triangleList_P1.Count-1)
                    triangleActivationTime_P1 = triangleList_P1[triangleActivationIndex_P1].GetComponent<FallingInputController>().inputTime;
                break;
            case "Diamond":
                if(diamondActivationIndex_P1 < diamondList_P1.Count-1)
                    diamondActivationTime_P1 = diamondList_P1[diamondActivationIndex_P1].GetComponent<FallingInputController>().inputTime;
                break;
            default:
                print("SetActivationTime has received an invalid string: " + noteType);
                break;
        }
    }

    private void ActivateNote(string noteType)
    {
        switch(noteType)
        {
            case "Circle":
                circleList_P1[circleActivationIndex_P1].GetComponent<FallingInputController>().canMove = true;
                circleActivationIndex_P1++;
                break;
            case "Square":
                squareList_P1[squareActivationIndex_P1].GetComponent<FallingInputController>().canMove = true;
                squareActivationIndex_P1++;
                break;
            case "Triangle":
                triangleList_P1[triangleActivationIndex_P1].GetComponent<FallingInputController>().canMove = true;
                triangleActivationIndex_P1++;
                break;
            case "Diamond":
                diamondList_P1[diamondActivationIndex_P1].GetComponent<FallingInputController>().canMove = true;
                diamondActivationIndex_P1++;
                break;
            default:
                print("ActivateNote has received an invalid string: " + noteType);
                break;  
        }
        SetActivationTime(noteType);
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
                        NoteSetup(circleNote, 0, SaveLoadManager.inputTimeList[j], circleList_P1);
                        break;
                    case 1:
                        NoteSetup(squareNote, 2, SaveLoadManager.inputTimeList[j], squareList_P1);
                        break;
                    case 2:
                        NoteSetup(triangleNote, 4, SaveLoadManager.inputTimeList[j], triangleList_P1);
                        break;
                    case 3:
                        NoteSetup(diamondNote, 6, SaveLoadManager.inputTimeList[j], diamondList_P1);
                        break;
                    default:
                        print("ERROR: inputTypeCountList in SaveLoadManager has more than 4 entries");
                        break;
                }
            }
            indexOffset += SaveLoadManager.inputTypeCountList[i];
        }
        circleList_P1.Add(LastNotes[0]);
        squareList_P1.Add(LastNotes[1]);
        triangleList_P1.Add(LastNotes[2]);
        diamondList_P1.Add(LastNotes[3]);
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
        hitIndicator_P1.text = "";
        SetActivationTime("Circle");
        SetActivationTime("Square");
        SetActivationTime("Triangle");
        SetActivationTime("Diamond");
        StartCoroutine(Countdown());
    }
    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            CalculateTime();
        }
        if(currentTime >= circleActivationTime_P1 - baseTranslateDuration / scrollSpeed && circleActivationIndex_P1 < circleList_P1.Count-1)
        {
            ActivateNote("Circle");
        }
        if(currentTime >= squareActivationTime_P1 - baseTranslateDuration / scrollSpeed && squareActivationIndex_P1 < squareList_P1.Count-1)
        {
            print("Square Activation Time: " + squareActivationTime_P1 + "Condition result: " + (squareActivationTime_P1 - baseTranslateDuration / scrollSpeed));
            ActivateNote("Square");
        }
            
        if(currentTime >= triangleActivationTime_P1 - baseTranslateDuration / scrollSpeed && triangleActivationIndex_P1 < triangleList_P1.Count-1)
            ActivateNote("Triangle");
        if(currentTime >= diamondActivationTime_P1 - baseTranslateDuration / scrollSpeed && diamondActivationIndex_P1 < diamondList_P1.Count-1)
            ActivateNote("Diamond");
    }
}
