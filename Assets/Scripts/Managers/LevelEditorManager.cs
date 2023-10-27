using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelEditorManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager Instance;
    
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
    private double musicStart;
    private double currentTime;
    public bool isPlaying;
    //AUDIO VARIABLES
    public AudioSource audioSource;
    //LEVEL EDITOR VARIABLES
    private IEnumerator coroutine;
    [SerializeField]
    private float scrollSpeed = 1;
    [SerializeField]
    private float missedAlpha = 0.25f;

    //FALLING INPUT VARIABLES
    public Transform inputChart;
    private List<SpriteRenderer> circleList = new List<SpriteRenderer>();
    private List<SpriteRenderer> squareList = new List<SpriteRenderer>();
    private List<SpriteRenderer> triangleList = new List<SpriteRenderer>();
    private List<SpriteRenderer> diamondList = new List<SpriteRenderer>();
    
    public SpriteRenderer[] LastNotes;
    //FUNCTIONS
    //TIMER FUNCTION
    void CalculateTime()
    {
        secondsAndMiliseconds = audioSource.time % 60;
        minutes = Mathf.FloorToInt((float)audioSource.time / 60f);
        if (secondsAndMiliseconds >= 60)
        {
            secondsAndMiliseconds -= 60;
            minutes += 1;
        }
        timerInputUI.text = minutes.ToString("00") + ":" + secondsAndMiliseconds.ToString("00.000");
    }
    //BUTTON FUNCTIONS
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
        }
        //COUNTING DOWN
        else
        {
            StopCoroutine(coroutine);
            playButtonImage.color = greenButton;
        }
    }
    //COUNTDOWN COROUTINE
    private IEnumerator Countdown()
    {
        int seconds = int.Parse(waitForSecondsInputUI.text);
        playButtonUI.GetComponent<Image>().color = yellowButton;
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            --seconds;
            playButtonTextUI.text = "Starting in " + seconds.ToString();
            if(seconds == 0)
            {
                playButtonTextUI.text = "Pause";
                playButtonUI.GetComponent<Image>().color = redButton;
                musicStart = audioSource.time;
                audioSource.Play();
                isPlaying = true;
            }
        }
    }
    void Awake()
    {
        circleList.Add(LastNotes[0]);
        squareList.Add(LastNotes[1]);
        triangleList.Add(LastNotes[2]);
        diamondList.Add(LastNotes[3]);
    }
    // Start is called before the first frame update
    void Start()
    {
        coroutine = Countdown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
