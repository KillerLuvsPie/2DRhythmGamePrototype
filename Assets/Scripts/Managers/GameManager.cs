using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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
    public Transform hitLine;
    //TIME VARIABLES
    private int seconds;
    private int minutes;
    private double startTime;
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
    private float scrollSpeed = 1;
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
        currentTime = AudioSettings.dspTime - startTime;
        seconds = (int)(currentTime % 60);
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
        inputChart.Translate(0,-scrollSpeed * Time.deltaTime,0);
    }
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
                circleList[circleListIndex].GetComponent<FallingInputController>().enabled = false;
                distance = circleList[circleListIndex].transform.position.y - hitLine.position.y;
                if(circleListIndex < circleList.Count-1)
                    circleListIndex++;
                break;
            case InputType.Square:
                squareList[squareListIndex].color = AddTransparencyToUsedNote(squareList[squareListIndex].color, alpha);
                squareList[squareListIndex].GetComponent<FallingInputController>().enabled = false;
                distance = squareList[squareListIndex].transform.position.y - hitLine.position.y;
                if(squareListIndex < squareList.Count-1)
                    squareListIndex++;
                break;
            case InputType.Triangle:
                triangleList[triangleListIndex].color = AddTransparencyToUsedNote(triangleList[triangleListIndex].color, alpha);
                triangleList[triangleListIndex].GetComponent<FallingInputController>().enabled = false;
                distance = triangleList[triangleListIndex].transform.position.y - hitLine.position.y;
                if(triangleListIndex < triangleList.Count-1)
                    triangleListIndex++;
                break;
            case InputType.Diamond:
                diamondList[diamondListIndex].color = AddTransparencyToUsedNote(diamondList[diamondListIndex].color, alpha);
                diamondList[diamondListIndex].GetComponent<FallingInputController>().enabled = false;
                distance = diamondList[diamondListIndex].transform.position.y - hitLine.position.y;
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
        if(Mathf.Abs(circleList[circleListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2, /*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(circleList[circleListIndex].transform.position.y - hitLine.position.y) <= inputRange)
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
    public void MissedCircle()
    {
        if(circleList[circleListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Circle, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------SQUARE LIST----------
    //SQUARE INPUT WAS PRESSED
    public void SquarePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(squareList[squareListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(squareList[squareListIndex].transform.position.y - hitLine.position.y) <= inputRange)
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
    public void MissedSquare()
    {
        if(squareList[squareListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Square, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }

    //----------TRIANGLE LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void TrianglePressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(triangleList[triangleListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(triangleList[triangleListIndex].transform.position.y - hitLine.position.y) <= inputRange)
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
    public void MissedTriangle()
    {
        if(triangleList[triangleListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
            ProcessNoteList(/*ListType*/InputType.Triangle, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss, /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
        }
    }
    
    //----------DIAMOND LIST----------
    //TRIANGLE INPUT WAS PRESSED
    public void DiamondPressed()
    {
        //CHECK IF INPUT IS VERY ACCURATE
        if(Mathf.Abs(diamondList[diamondListIndex].transform.position.y - hitLine.position.y) <= Mathf.Lerp(0, inputRange, 0.5f))
        {
            ProcessNoteList(/*ListType*/InputType.Diamond, /*Add score?*/true, /*How many points*/scoreIncrement, /*Score multiplier*/ 2,/*Alpha change*/ perfectHitAlpha);
        }
        //CHECK IF INPUT IS NOT AS ACCURATE
        else if (Mathf.Abs(diamondList[diamondListIndex].transform.position.y - hitLine.position.y) <= inputRange)
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
    public void MissedDiamond()
    {
        if(diamondList[diamondListIndex].transform.position.y - hitLine.position.y < -inputRange)
        {
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
                startTime = AudioSettings.dspTime;
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
        /*for(int i = 0; i < inputChart.childCount; i++)
        {
            FallingInputController fic = inputChart.GetChild(i).GetComponent<FallingInputController>();
            switch(fic.fallingInput.inputName)
            {
                case "Circle":
                    fic.transform.localPosition = new Vector2(0, fic.inputTime * scrollSpeed);
                    circleList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                case "Square":
                    fic.transform.localPosition = new Vector2(2, fic.inputTime * scrollSpeed);
                    squareList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                case "Triangle":
                    fic.transform.localPosition = new Vector2(4, fic.inputTime * scrollSpeed);
                    triangleList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                case "Diamond":
                    fic.transform.localPosition = new Vector2(6, fic.inputTime * scrollSpeed);
                    diamondList.Add(fic.GameObject().GetComponent<SpriteRenderer>());
                    break;
                default:
                    print("ERROR: Invalid falling input name at Awake function in GameManager");
                    break;
            }
        }*/
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
            ScrollNotes();
        }
    }
}
