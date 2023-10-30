using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEditor;

public class LevelEditorManager : MonoBehaviour
{
    //SINGLETON
    public static LevelEditorManager Instance;
    
    //UI ELEMENTS
    public TMP_InputField scrollSpeedUI;
    public TextMeshProUGUI numberOfNotesUI;
    public Toggle waitForSecondsToggleUI;
    public TMP_InputField waitForSecondsInputUI;
    public Button deleteAllNotesButtonUI;
    public Button saveChartButtonUI;
    public TMP_InputField timerInputUI;
    public Button playButtonUI;
    public TextMeshProUGUI playButtonTextUI;
    public Button restartTimerButtonUI;
    public TextMeshProUGUI hitIndicatorUI;
    public Transform hitLine;

    //BUTTON COLORS
    private Color redButton = new Color(0.8823529f, 0, 0, 1);
    private Color yellowButton = new Color(1f, 0.7843137f, 0, 1);
    private Color greenButton = new Color(0, 0.8823529f, 0, 1);
    private Color blueButton = new Color(0, 0.5115622f, 8823529f, 1);
    //TIME VARIABLES
    private float secondsAndMiliseconds;
    private int minutes;
    private float currentTime;
    public bool isPlaying;
    //AUDIO VARIABLES
    public AudioSource audioSource;
    //LEVEL EDITOR VARIABLES
    private IEnumerator coroutine;
    [SerializeField]
    private float scrollSpeed = 1;
    [SerializeField]
    private float noteAlpha = 0.25f;
    public GameObject circleInput;
    public GameObject squareInput;
    public GameObject triangleInput;
    public GameObject diamondInput;
    //FALLING INPUT VARIABLES
    public Transform inputChart;
    private List<GameObject> circleList = new List<GameObject>();
    private List<GameObject> squareList = new List<GameObject>();
    private List<GameObject> triangleList = new List<GameObject>();
    private List<GameObject> diamondList = new List<GameObject>();
    
    public SpriteRenderer[] LastNotes;
    //FUNCTIONS
    //NOTE CREATION
    public void CreateCircle()
    {
        GameObject obj = Instantiate(circleInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = audioSource.time;
        obj.transform.localPosition = new Vector2(0, fic.inputTime);
        circleList.Add(obj);
    }
    public void CreateSquare()
    {
        GameObject obj = Instantiate(squareInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = audioSource.time;
        obj.transform.localPosition = new Vector2(2, fic.inputTime);
        circleList.Add(obj);
    }
    public void CreateTriangle()
    {
        GameObject obj = Instantiate(triangleInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = audioSource.time;
        obj.transform.localPosition = new Vector2(4, fic.inputTime);
        circleList.Add(obj);
    }
    public void CreateDiamond()
    {
        GameObject obj = Instantiate(diamondInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = audioSource.time;
        obj.transform.localPosition = new Vector2(6, fic.inputTime);
        circleList.Add(obj);
    }

    //TIMER FUNCTION
    private void CalculateTime()
    {
        currentTime = audioSource.time;
        secondsAndMiliseconds = currentTime % 60;
        minutes = Mathf.FloorToInt((float)currentTime / 60f);
        if (secondsAndMiliseconds >= 60)
        {
            secondsAndMiliseconds -= 60;
            minutes += 1;
        }
        timerInputUI.text = minutes.ToString("00") + ":" + secondsAndMiliseconds.ToString("00.000");
    }
    //UI FUNCTIONS
    public void ScrollSpeedUIChange()
    {
        scrollSpeed = float.Parse(scrollSpeedUI.text);
        audioSource.pitch = scrollSpeed;
    }
    //PLAY/PAUSE BUTTON
    public void PlayButtonToggle()
    {
        Image playButtonImage = playButtonUI.GetComponent<Image>();
        //PLAY
        if(playButtonImage.color == greenButton)
        {
            StartCoroutine(coroutine);
        }
        //PAUSE
        else if(playButtonImage.color == redButton)
        {
            //PAUSE MUSIC FUNCTION GOES HERE
            audioSource.Stop();
            ResetCoroutine();
            playButtonTextUI.text = "PLAY";
            playButtonImage.color = greenButton;
        }
        //COUNTING DOWN
        else
        {
            ResetCoroutine();
            playButtonTextUI.text = "PLAY";
            playButtonImage.color = greenButton;
        }
    }

    //RESTART TIMER BUTTON
    public void RestartTimer()
    {
        timerInputUI.text = "00:00.000";
    }

    //COUNTDOWN COROUTINE
    private IEnumerator Countdown()
    {
        int seconds = int.Parse(waitForSecondsInputUI.text);
        playButtonUI.GetComponent<Image>().color = yellowButton;
        while (seconds > 0)
        {
            playButtonTextUI.text = "STARTING IN " + seconds.ToString();
            yield return new WaitForSeconds(1);
            --seconds;
        }
        if(seconds == 0)
        {
            playButtonTextUI.text = "PAUSE";
            playButtonUI.GetComponent<Image>().color = redButton;
            //audioSource.Play();
            isPlaying = true;
        }
    }
    //RESET COROUTINE
    private void ResetCoroutine()
    {
        StopCoroutine(coroutine);
        coroutine = Countdown();
        isPlaying = false;
    }
    void Awake()
    {
        /*/circleList.Add(LastNotes[0]);
        squareList.Add(LastNotes[1]);
        triangleList.Add(LastNotes[2]);
        diamondList.Add(LastNotes[3]);*/
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        coroutine = Countdown();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            CalculateTime();
        }
    }
}