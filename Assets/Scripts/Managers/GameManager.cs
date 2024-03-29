using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    #region VARIABLES
    //SINGLETON
    public static GameManager Instance;
    //UI ELEMENTS
    public TextMeshProUGUI timerUI;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI[] scoreUIS = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] comboUIS = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] hitIndicators = new TextMeshProUGUI[2];
    
    //HIT MARKERS FOR NOTES
    public Transform[] circleHitMarks = new Transform[2];
    public Transform[] squareHitMarks = new Transform[2];
    public Transform[] triangleHitMarks = new Transform[2];
    public Transform[] diamondHitMarks = new Transform[2];

    //TIME VARIABLES
    private int seconds;
    private int minutes;
    public double currentTime;
    public bool started;

    //AUDIO VARIABLES
    public AudioSource audioSource;

    //GAME VARIABLES
    private int[] scores = new int[2];
    [SerializeField]
    private int scoreIncrement = 1;
    [SerializeField]
    private int scoreDecrementIfMiss = 3;
    [SerializeField]
    private int scoreDecrementIfWrong = 2;
    private int[] combos = new int[2];
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
    //PREFABS
    public GameObject circleNote;
    public GameObject squareNote;
    public GameObject triangleNote;
    public GameObject diamondNote;
    public GameObject missInputEffect;
    public Sprite circleOutline;
    public Sprite squareOutline;
    public Sprite triangleOutline;
    public Sprite diamondOutline;

    //BACKGROUND EFFECT VARIABLES
    public List<SpriteRenderer> bgObjects = new List<SpriteRenderer>();
    public GameObject bgObjectWrapper;
    public Image[] bgEffects;
    private IEnumerator blueBGEffectResetter;
    private IEnumerator greenBGEffectResetter;
    private bool isBlueBGCoroutineRunning = false;
    private bool isGreenBGCoroutineRunning = false;
    [SerializeField]
    private int blueEffectCounter = 0;
    [SerializeField]
    private int greenEffectCounter = 0;

    //NOTE CHART PARENT OBJECTS
    public Transform[] noteCharts = new Transform[2];

    //NOTE LISTS
    public List<SpriteRenderer>[] circleLists = new List<SpriteRenderer>[2] {new List<SpriteRenderer>(), new List<SpriteRenderer>()};
    public List<SpriteRenderer>[] squareLists = new List<SpriteRenderer>[2] {new List<SpriteRenderer>(), new List<SpriteRenderer>()};
    public List<SpriteRenderer>[] triangleLists = new List<SpriteRenderer>[2] {new List<SpriteRenderer>(), new List<SpriteRenderer>()};
    public List<SpriteRenderer>[] diamondLists = new List<SpriteRenderer>[2] {new List<SpriteRenderer>(), new List<SpriteRenderer>()};

    //NOTE LIST INDEX (NEXT NOTE TO BE PRESSED)
    private int[] circleListIndexes = new int[2] {0,0};
    private int[] squareListIndexes = new int[2] {0,0};
    private int[] triangleListIndexes = new int[2] {0,0};
    private int[] diamondListIndexes = new int[2] {0,0};

    //NOTE ACTIVATION INDEX (NEXT NOTE TO BE ACTIVATED AND TRANSLATED)
    private int[] circleActivationIndexes = new int[2] {0,0};
    private int[] squareActivationIndexes = new int[2] {0,0};
    private int[] triangleActivationIndexes = new int[2] {0,0};
    private int[] diamondActivationIndexes = new int[2] {0,0};

    //NOTE ACTIVATION TIMES
    private double[] circleActivationTimes = new double[2];
    private double[] squareActivationTimes = new double[2];
    private double[] triangleActivationTimes = new double[2];
    private double[] diamondActivationTimes = new double[2];

    //LAST NOTES (USED TO AVOID INDEX OUT OF RANGE ERROR, RESULTS IN A "TOO EARLY" WHEN THERE ARE NO NOTES LEFT) | 0 = CIRCLE | 1 = SQUARE | 2 = TRIANGLE | 3 = DIAMOND |
    public SpriteRenderer[] LastNotes;
    #endregion VARIABLES

    #region CUSTOM FUNCTIONS
    //PRINT NOTES FOR DEBUGGING PURPOSES
    private void PrintNoteLists()
    {
        for(int i = 0; i < circleLists.Length; i++)
        {
            for(int j = 0; j < circleLists[i].Count; j++)
            {
                print("Player " + i + " list | Circle List, Pos " + j + ": " + circleLists[i][j].name);
            }
            for(int j = 0; j < squareLists[i].Count; j++)
            {
                print("Player " + i + " list | Square List, Pos " + j + ": " + squareLists[i][j].name);
            }
            for(int j = 0; j < triangleLists[i].Count; j++)
            {
                print("Player " + i + " list | Triangle List, Pos " + j + ": " + triangleLists[i][j].name);
            }
            for(int j = 0; j < diamondLists[i].Count; j++)
            {
                print("Player " + i + " list | Diamond List, Pos " + j + ": " + diamondLists[i][j].name);
            }
        }
    }

    //ROTATE BG EFFECT
    private void RotateBGEffect()
    {
        for(int i = 0; i < bgObjects.Count; i++)
            bgObjects[i].transform.Rotate(0,0, Random.Range(75, 126) * Time.deltaTime);
    }

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

    //END OF MUSIC FUNCTION
    private void CheckIfMusicIsDone()
    {
        
    }
    
    //POINTS FUNCTIONS
    public void IncreaseScore(int index, int increment)
    {
        if(index == 0)
        {
            AddBGObjectToBlue();
            if(isGreenBGCoroutineRunning)
                bgEffects[0].enabled = true;
            else
                ResetBlueBGEffect();
        }
            
        else
        {
            AddBGObjectToGreen();
            if(isBlueBGCoroutineRunning)
                bgEffects[1].enabled = true;
            else
                ResetGreenBGEffect();
        }
            
        scores[index] += increment + combos[index]/10;
        scoreUIS[index].text = "Score:" + scores[index].ToString("000000");
    }

    public void DecreaseScore(int index, int decrement)
    {
        //ChangeBGEffectAlpha(index, false);
        scores[index] -= decrement;
        scoreUIS[index].text = "Score:" + scores[index].ToString("000000");
    }
    public void IncreaseCombo(int index)
    {
        combos[index]++;
        comboUIS[index].text = "Combo:" + combos[index].ToString("000");
    }

    public void ResetCombo(int index)
    {
        if(index == 0)
            ResetAllBlueObjects();
        else
            ResetAllGreenObjects();
        combos[index] = 0;
        comboUIS[index].text = "Combo:" + combos[index].ToString("000");
    }
    
    //FALLING NOTE LISTS MANAGEMENT FUNCTIONS
    private void NoteSetup(int index, GameObject obj, float inputTime, List<SpriteRenderer> list)
    {
        GameObject instance = Instantiate(obj, noteCharts[index]);
        instance.transform.position = new Vector2(0,0);
        //instance.transform.localPosition = new Vector2(xPos, inputTime * scrollSpeed);
        FallingInputController fic = instance.GetComponent<FallingInputController>();
        fic.inputTime = inputTime;
        fic.outlineRenderer.color = HelperClass.outlineColors[index];
        fic.playerNum = index;
        list.Add(instance.GetComponent<SpriteRenderer>());
    }

    private Color AddTransparencyToUsedNote(Color color, float alpha)
    {
        return color = new Color(color.r, color.g, color.b, alpha);
    }

    private void ShowHitMessage(int index, float distance)
    {
        if(distance <= Mathf.Lerp(0, inputRange, 0.5f))
            hitIndicators[index].text = (-distance).ToString("0.000") + "\nNice!";
        else if(distance <= inputRange)
            hitIndicators[index].text = (-distance).ToString("0.000") + "\nOk";
        else
            hitIndicators[index].text = "\nMiss";
    }

    private void ShowEarlyHitMessage(int index)
    {
        hitIndicators[index].text = "Too\nEarly";
    }

    private void ProcessNoteList(int index, HelperClass.InputType inputType, bool addScore, int score, int scoreMultiplier, float alpha)
    {
        float distance = 0;
        if(addScore)
        {
            IncreaseScore(index, score * scoreMultiplier);
            IncreaseCombo(index);
        }
        else
        {
            DecreaseScore(index, score * scoreMultiplier);
            ResetCombo(index);
        }
        switch (inputType)
        {
            case HelperClass.InputType.Circle:
                circleLists[index][circleListIndexes[index]].color = AddTransparencyToUsedNote(circleLists[index][circleListIndexes[index]].color, alpha);
                circleLists[index][circleListIndexes[index]].GetComponent<FallingInputController>().outlineRenderer.enabled = false;
                circleLists[index][circleListIndexes[index]].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(circleLists[index][circleListIndexes[index]].transform.position, circleHitMarks[index].position);
                if(circleListIndexes[index] < circleLists[index].Count-1)
                    circleListIndexes[index]++;
                break;
            case HelperClass.InputType.Square:
                squareLists[index][squareListIndexes[index]].color = AddTransparencyToUsedNote(squareLists[index][squareListIndexes[index]].color, alpha);
                squareLists[index][squareListIndexes[index]].GetComponent<FallingInputController>().outlineRenderer.enabled = false;
                squareLists[index][squareListIndexes[index]].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(squareLists[index][squareListIndexes[index]].transform.position, squareHitMarks[index].position);
                if(squareListIndexes[index] < squareLists[index].Count-1)
                    squareListIndexes[index]++;
                break;
            case HelperClass.InputType.Triangle:
                triangleLists[index][triangleListIndexes[index]].color = AddTransparencyToUsedNote(triangleLists[index][triangleListIndexes[index]].color, alpha);
                triangleLists[index][triangleListIndexes[index]].GetComponent<FallingInputController>().outlineRenderer.enabled = false;
                triangleLists[index][triangleListIndexes[index]].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(triangleLists[index][triangleListIndexes[index]].transform.position, triangleHitMarks[index].position);
                if(triangleListIndexes[index] < triangleLists[index].Count-1)
                    triangleListIndexes[index]++;
                break;
            case HelperClass.InputType.Diamond:
                diamondLists[index][diamondListIndexes[index]].color = AddTransparencyToUsedNote(diamondLists[index][diamondListIndexes[index]].color, alpha);
                diamondLists[index][diamondListIndexes[index]].GetComponent<FallingInputController>().outlineRenderer.enabled = false;
                diamondLists[index][diamondListIndexes[index]].GetComponent<FallingInputController>().isActive = false;
                distance = Vector3.Distance(diamondLists[index][diamondListIndexes[index]].transform.position, diamondHitMarks[index].position);
                if(diamondListIndexes[index] < diamondLists[index].Count-1)
                    diamondListIndexes[index]++;
                break;
        }
        ShowHitMessage(index, distance);
    }

    //INPUT WAS PRESSED
    public void InputPressed(int index, HelperClass.InputType inputType)
    {
        int scoreMultiplier;
        float alphaValue, distance = 0;
        switch(inputType)
        {
            case HelperClass.InputType.Circle:
                distance = Vector2.Distance(circleLists[index][circleListIndexes[index]].transform.position, circleHitMarks[index].position);
                break;
            case HelperClass.InputType.Square:
                distance = Vector2.Distance(squareLists[index][squareListIndexes[index]].transform.position, squareHitMarks[index].position);
                break;
            case HelperClass.InputType.Triangle:
                distance = Vector2.Distance(triangleLists[index][triangleListIndexes[index]].transform.position, triangleHitMarks[index].position);
                break;
            case HelperClass.InputType.Diamond:
                distance = Vector2.Distance(diamondLists[index][diamondListIndexes[index]].transform.position, diamondHitMarks[index].position);
                break;
        }
        //CHECK IF INPUT IS INSIDE RANGE
        if (distance <= inputRange)
        {
            //CHECK IF IT'S 50% OF INPUTRANGE VALUE
            if(distance <= inputRange / 2)
            {
                scoreMultiplier = 2;
                alphaValue = perfectHitAlpha;
            }
            //OR IF NOT 50% OF INPUTRANGE VALUE
            else
            {
                scoreMultiplier = 1;
                alphaValue = normalHitAlpha;
            }
            ProcessNoteList(/*Index*/index, /*ListType*/inputType, /*Add score?*/true, /*How many points*/scoreIncrement,
            /*Score multiplier*/ scoreMultiplier,/*Alpha change*/ alphaValue);
        }
        //ELSE IF INPUT WAS TOO EARLY
        else
        {
            ShowEarlyHitMessage(index);
            DecreaseScore(index, scoreDecrementIfWrong);
            ResetCombo(index);
        }
    }
    //IF A NOTE WAS MISSED
    public void MissedInput(int index, HelperClass.InputType inputType, Transform noteTransform, Vector2 noteDirection)
    {
        Vector2 direction;
        float distance;
        switch(inputType)
        {
            case HelperClass.InputType.Circle:
                direction = (noteTransform.position - circleHitMarks[index].position).normalized;
                distance = Vector2.Distance(circleLists[index][circleListIndexes[index]].transform.position, circleHitMarks[index].position);
                break;
            case HelperClass.InputType.Square:
                direction = (noteTransform.position - squareHitMarks[index].position).normalized;
                distance = Vector2.Distance(squareLists[index][squareListIndexes[index]].transform.position, squareHitMarks[index].position);
                break;
            case HelperClass.InputType.Triangle:
                direction = (noteTransform.position - triangleHitMarks[index].position).normalized;
                distance = Vector2.Distance(triangleLists[index][triangleListIndexes[index]].transform.position, triangleHitMarks[index].position);
                break;
            case HelperClass.InputType.Diamond:
                direction = (noteTransform.position - diamondHitMarks[index].position).normalized;
                distance = Vector2.Distance(diamondLists[index][diamondListIndexes[index]].transform.position, diamondHitMarks[index].position);
                break;
            default:
                direction = Vector2.zero;
                distance = 0;
                break;
        }
        if(Vector2.Dot(direction, noteDirection) > 0.999f && distance > inputRange)
        {
            ProcessNoteList(/*Index*/index, /*ListType*/inputType, /*Add score?*/false, /*How many points*/scoreDecrementIfMiss,
            /*Score multiplier*/ 1,/*Alpha change*/ missedAlpha);
            MissedInputEffect(index, inputType);
        }
    }

    private void MissedInputEffect(int index, HelperClass.InputType inputType)
    {
        GameObject gameObject;
        switch(inputType)
        {
            case HelperClass.InputType.Circle:
                gameObject = Instantiate(missInputEffect, circleHitMarks[index].transform.position, Quaternion.identity);
                gameObject.GetComponent<SpriteRenderer>().sprite = circleOutline;
                gameObject.GetComponent<SpriteRenderer>().color = HelperClass.red;
                gameObject.transform.localScale = HelperClass.circleOutlineScale;
                break;
            case HelperClass.InputType.Square:
                gameObject = Instantiate(missInputEffect, squareHitMarks[index].transform.position, Quaternion.identity);
                gameObject.GetComponent<SpriteRenderer>().sprite = squareOutline;
                gameObject.GetComponent<SpriteRenderer>().color = HelperClass.red;
                gameObject.transform.localScale = HelperClass.squareOutlineScale;
                break;
            case HelperClass.InputType.Triangle:
                Vector3 offset = new Vector3(0,-0.045f,0);
                gameObject = Instantiate(missInputEffect, triangleHitMarks[index].transform.position + offset, Quaternion.identity);
                gameObject.GetComponent<SpriteRenderer>().sprite = triangleOutline;
                gameObject.GetComponent<SpriteRenderer>().color = HelperClass.red;
                gameObject.transform.localScale = HelperClass.triangleOutlineScale;
                break;
            case HelperClass.InputType.Diamond:
                gameObject = Instantiate(missInputEffect, diamondHitMarks[index].transform.position, Quaternion.identity);
                gameObject.GetComponent<SpriteRenderer>().sprite = diamondOutline;
                gameObject.GetComponent<SpriteRenderer>().color = HelperClass.red;
                gameObject.transform.localScale = HelperClass.diamondOutlineScale;
                break;
        }
    }
    //NOTE ACTIVATION FUNCTIONS
    private void ActivateNote(int index, string noteType)
    {
        switch(noteType)
        {
            case "Circle":
                circleLists[index][circleActivationIndexes[index]].GetComponent<FallingInputController>().canMove = true;
                circleLists[index][circleActivationIndexes[index]].GetComponent<SpriteRenderer>().enabled = true;
                circleLists[index][circleActivationIndexes[index]].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                circleActivationIndexes[index]++;
                break;
            case "Square":
                squareLists[index][squareActivationIndexes[index]].GetComponent<FallingInputController>().canMove = true;
                squareLists[index][squareActivationIndexes[index]].GetComponent<SpriteRenderer>().enabled = true;
                squareLists[index][squareActivationIndexes[index]].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                squareActivationIndexes[index]++;
                break;
            case "Triangle":
                triangleLists[index][triangleActivationIndexes[index]].GetComponent<FallingInputController>().canMove = true;
                triangleLists[index][triangleActivationIndexes[index]].GetComponent<SpriteRenderer>().enabled = true;
                triangleLists[index][triangleActivationIndexes[index]].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                triangleActivationIndexes[index]++;
                break;
            case "Diamond":
                diamondLists[index][diamondActivationIndexes[index]].GetComponent<FallingInputController>().canMove = true;
                diamondLists[index][diamondActivationIndexes[index]].GetComponent<SpriteRenderer>().enabled = true;
                diamondLists[index][diamondActivationIndexes[index]].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                diamondActivationIndexes[index]++;
                break;
            default:
                print("ActivateNote has received an invalid string: " + noteType);
                break;  
        }
        SetActivationTime(index, noteType);
    }

    private void SetActivationTime(int index, string noteType)
    {
        switch(noteType)
        {
            case "Circle":
                if(circleActivationIndexes[index] < circleLists[index].Count-1)
                    circleActivationTimes[index] = circleLists[index][circleActivationIndexes[index]].GetComponent<FallingInputController>().inputTime;
                break;
            case "Square":
                if(squareActivationIndexes[index] < squareLists[index].Count-1)
                    squareActivationTimes[index] = squareLists[index][squareActivationIndexes[index]].GetComponent<FallingInputController>().inputTime;
                break;
            case "Triangle":
                if(triangleActivationIndexes[index] < triangleLists[index].Count-1)
                    triangleActivationTimes[index] = triangleLists[index][triangleActivationIndexes[index]].GetComponent<FallingInputController>().inputTime;
                break;
            case "Diamond":
                if(diamondActivationIndexes[index] < diamondLists[index].Count-1)
                    diamondActivationTimes[index] = diamondLists[index][diamondActivationIndexes[index]].GetComponent<FallingInputController>().inputTime;
                break;
            default:
                print("SetActivationTime has received an invalid string: " + noteType);
                break;
        }
    }
    //SET SORTING LAYER INDEX OF NOTES
    private void SetSortingLayers()
    {
        for(int i = 0; i < circleLists.Length; i++)
        {
            for(int j = 0; j < circleLists[i].Count - 1; j++)
            {
                circleLists[i][j].sortingOrder = circleLists[i].Count * 2 - 1 - j * 2;
                circleLists[i][j].GetComponent<FallingInputController>().outlineRenderer.sortingOrder = circleLists[i].Count * 2 - j * 2;
            }
            for(int j = 0; j < squareLists[i].Count - 1; j++)
            {
                squareLists[i][j].sortingOrder = squareLists[i].Count * 2 - 1 - j * 2;
                squareLists[i][j].GetComponent<FallingInputController>().outlineRenderer.sortingOrder = squareLists[i].Count * 2 - j + 2;
            }
            for(int j = 0; j < triangleLists[i].Count - 1; j++)
            {
                triangleLists[i][j].sortingOrder = triangleLists[i].Count * 2 - 1 - j * 2;
                triangleLists[i][j].GetComponent<FallingInputController>().outlineRenderer.sortingOrder = triangleLists[i].Count * 2 - j * 2;
            }
            for(int j = 0; j < diamondLists[i].Count - 1; j++)
            {
                diamondLists[i][j].sortingOrder = diamondLists[i].Count * 2 - 1 - j * 2;
                diamondLists[i][j].GetComponent<FallingInputController>().outlineRenderer.sortingOrder = diamondLists[i].Count * 2 - j * 2;
            }
        }
    }

    //BACKGROUND EFFECTS
    private void ResetBlueBGEffect()
    {
        StopCoroutine(blueBGEffectResetter);
        Camera.main.backgroundColor = new Color(Camera.main.backgroundColor.r, Camera.main.backgroundColor.g, 0, Camera.main.backgroundColor.a);
        bgEffects[1].color = new Color(bgEffects[1].color.r, bgEffects[1].color.g, bgEffects[1].color.b, 0);
        bgEffects[1].enabled = false;
        blueBGEffectResetter = BackgroundEffectBlue();
        StartCoroutine(blueBGEffectResetter);
    }

    private void ResetGreenBGEffect()
    {
        StopCoroutine(greenBGEffectResetter);
        Camera.main.backgroundColor = new Color(Camera.main.backgroundColor.r, 0, Camera.main.backgroundColor.b, Camera.main.backgroundColor.a);
        bgEffects[0].color = new Color(bgEffects[0].color.r, bgEffects[0].color.g, bgEffects[0].color.b, 0);
        bgEffects[0].enabled = false;
        greenBGEffectResetter = BackgroundEffectGreen();
        StartCoroutine(greenBGEffectResetter);
    }

    private void AddBGObjectToBlue()
    {
        if(blueEffectCounter < bgObjects.Count)
        {
            if(bgObjects[blueEffectCounter].enabled == true)
                greenEffectCounter--;
            bgObjects[blueEffectCounter].color = HelperClass.blue;
            bgObjects[blueEffectCounter].enabled = true;
            blueEffectCounter++;
        }
        
    }

    private void AddBGObjectToGreen()
    {
        if(bgObjects.Count - greenEffectCounter > 0)
        {
            if(bgObjects[bgObjects.Count - 1 - greenEffectCounter].enabled == true)
                blueEffectCounter--;
            bgObjects[bgObjects.Count - 1 - greenEffectCounter].color = HelperClass.green;
            bgObjects[bgObjects.Count - 1 - greenEffectCounter].enabled = true;
            greenEffectCounter++;
        } 
    }

    private void ResetAllBlueObjects()
    {
        for(int i = 0; i < blueEffectCounter; i++)
        {
            bgObjects[i].enabled = false;
            bgObjects[i].color = HelperClass.white;
        }
        blueEffectCounter = 0;
    }
    private void ResetAllGreenObjects()
    {
        for(int i = bgObjects.Count; i > bgObjects.Count - greenEffectCounter; i--)
        {
            bgObjects[i-1].enabled = false;
            bgObjects[i-1].color = HelperClass.white;
        }
        greenEffectCounter = 0;
    }
    #endregion CUSTOM FUNCTIONS

    #region COROUTINES
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

    private IEnumerator BackgroundEffectBlue()
    {
        float interval = 0.02f;
        isBlueBGCoroutineRunning = true;
        while(Camera.main.backgroundColor.b < 1)
        {
            Camera.main.backgroundColor += HelperClass.playerBGEffectColorSlide[0];
            bgEffects[1].color += HelperClass.bgAlphaChange;
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForEndOfFrame();
        while(Camera.main.backgroundColor.b > 0)
        {
            Camera.main.backgroundColor -= HelperClass.playerBGEffectColorSlide[0];
            bgEffects[1].color -= HelperClass.bgAlphaChange;
            yield return new WaitForSeconds(interval);
        }
        bgEffects[1].enabled = false;
        isBlueBGCoroutineRunning = false;
    }
    private IEnumerator BackgroundEffectGreen()
    {
        float interval = 0.02f;
        isGreenBGCoroutineRunning = true;
        while(Camera.main.backgroundColor.g < 1)
        {
            Camera.main.backgroundColor += HelperClass.playerBGEffectColorSlide[1];
            bgEffects[0].color += HelperClass.bgAlphaChange;
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForEndOfFrame();
        while(Camera.main.backgroundColor.g > 0)
        {
            Camera.main.backgroundColor -= HelperClass.playerBGEffectColorSlide[1];
            bgEffects[0].color -= HelperClass.bgAlphaChange;
            yield return new WaitForSeconds(interval);
        }
        bgEffects[0].enabled = false;
        isGreenBGCoroutineRunning = false;
    }
    #endregion COROUTINES

    #region UNITY FUNCTIONS
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
                        for(int k = 0; k < circleLists.Length; k++)
                            NoteSetup(k, circleNote, SaveLoadManager.inputTimeList[j], circleLists[k]);
                        break;
                    case 1:
                        for(int k = 0; k < squareLists.Length; k++)
                            NoteSetup(k, squareNote, SaveLoadManager.inputTimeList[j], squareLists[k]);
                        break;
                    case 2:
                        for(int k = 0; k < triangleLists.Length; k++)
                            NoteSetup(k, triangleNote, SaveLoadManager.inputTimeList[j], triangleLists[k]);
                        break;
                    case 3:
                        for(int k = 0; k < diamondLists.Length; k++)
                            NoteSetup(k, diamondNote, SaveLoadManager.inputTimeList[j], diamondLists[k]);
                        break;
                    default:
                        print("ERROR: inputTypeCountList in SaveLoadManager has more than 4 entries");
                        break;
                }
            }
            indexOffset += SaveLoadManager.inputTypeCountList[i];
        }
        for(int i = 0; i < circleLists.Length; i++)
        {
            circleLists[i].Add(LastNotes[0]);
            squareLists[i].Add(LastNotes[1]);
            triangleLists[i].Add(LastNotes[2]);
            diamondLists[i].Add(LastNotes[3]);
        }
        // UNCOMMENT IF IT'S NECESSARY TO CHECK WHAT IS IN THE LISTS
        //PrintNoteLists();

        for(int i = 0; i < bgObjectWrapper.transform.childCount; i++)
            bgObjects.Add(bgObjectWrapper.transform.GetChild(i).GetComponent<SpriteRenderer>());
    }

    void Start()
    {
        blueBGEffectResetter = BackgroundEffectBlue();
        greenBGEffectResetter = BackgroundEffectGreen();
        SetSortingLayers();
        countdown.text = "";
        for(int i = 0; i < hitIndicators.Length; i++)
        {
            hitIndicators[i].text = "";
            SetActivationTime(i, "Circle");
            SetActivationTime(i, "Square");
            SetActivationTime(i, "Triangle");
            SetActivationTime(i, "Diamond");
        }
        StartCoroutine(Countdown());
    }

    void Update()
    {
        RotateBGEffect();
        if(started)
        {
            CalculateTime();
        }
        for(int i = 0; i < circleActivationTimes.Length; i++)
        {
            if(currentTime >= circleActivationTimes[i] - baseTranslateDuration / scrollSpeed && circleActivationIndexes[i] < circleLists[i].Count-1)
                ActivateNote(i, "Circle");
            if(currentTime >= squareActivationTimes[i] - baseTranslateDuration / scrollSpeed && squareActivationIndexes[i] < squareLists[i].Count-1)
                ActivateNote(i, "Square");
            if(currentTime >= triangleActivationTimes[i] - baseTranslateDuration / scrollSpeed && triangleActivationIndexes[i] < triangleLists[i].Count-1)
                ActivateNote(i, "Triangle");
            if(currentTime >= diamondActivationTimes[i] - baseTranslateDuration / scrollSpeed && diamondActivationIndexes[i] < diamondLists[i].Count-1)
                ActivateNote(i, "Diamond");
        }
    }

    #endregion UNITY FUNCTIONS
}
