using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class LevelEditorManager : MonoBehaviour
{
    //SINGLETON
    public static LevelEditorManager Instance;
    
    //UI ELEMENTS
    public TMP_InputField playbackSpeedUI;
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
    public AudioMixerGroup audioMixerGroup;
    //LEVEL EDITOR VARIABLES
    private IEnumerator coroutine;
    [SerializeField]
    private float noteSpacing = 2;
    private float yOffset;
    private float playbackSpeed = 1;
    [SerializeField]
    private float noteAlpha = 0.5f;
    private int totalNotes;
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
        fic.inputTime = currentTime;
        obj.transform.localPosition = new Vector2(0, fic.inputTime * noteSpacing);
        circleList.Add(obj);
        ChangeNumberOfNotes(true);
    }
    public void CreateSquare()
    {
        GameObject obj = Instantiate(squareInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = currentTime;
        obj.transform.localPosition = new Vector2(2, fic.inputTime * noteSpacing);
        squareList.Add(obj);
        ChangeNumberOfNotes(true);
    }
    public void CreateTriangle()
    {
        GameObject obj = Instantiate(triangleInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = currentTime;
        obj.transform.localPosition = new Vector2(4, fic.inputTime * noteSpacing);
        triangleList.Add(obj);
        ChangeNumberOfNotes(true);
    }
    public void CreateDiamond()
    {
        GameObject obj = Instantiate(diamondInput, inputChart);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        FallingInputController fic = obj.GetComponent<FallingInputController>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, noteAlpha);
        fic.inputTime = currentTime;
        obj.transform.localPosition = new Vector2(6, fic.inputTime * noteSpacing);
        diamondList.Add(obj);
        ChangeNumberOfNotes(true);
    }

    //SCROLL FUNCTION
    private void ScrollNotes()
    {
        inputChart.position = new Vector2(3, -currentTime * noteSpacing + yOffset);
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
    //DISABLE UI ELEMENTS WHEN MUSIC IS PLAYING
    private void DisableUIElements()
    {
        playbackSpeedUI.interactable = false;
        waitForSecondsToggleUI.interactable = false;
        waitForSecondsInputUI.interactable = false;
        deleteAllNotesButtonUI.interactable = false;
        saveChartButtonUI.interactable = false;
        timerInputUI.interactable = false;
        restartTimerButtonUI.interactable = false;
    }

    private void EnableUIElements()
    {
        playbackSpeedUI.interactable = true;
        waitForSecondsToggleUI.interactable = true;
        waitForSecondsInputUI.interactable = true;
        deleteAllNotesButtonUI.interactable = true;
        saveChartButtonUI.interactable = true;
        timerInputUI.interactable = true;
        restartTimerButtonUI.interactable = true;
    }

    //PLAYBACK SPEED CHANGE
    public void PlaybackSpeedUIChange()
    {
        playbackSpeed = float.Parse(playbackSpeedUI.text);
        audioSource.pitch = playbackSpeed;
        audioMixerGroup.audioMixer.SetFloat("pitch", 1/playbackSpeed);
    }

    //CHANGE NUMBER OF NOTES LABEL
    public void ChangeNumberOfNotes(bool add, bool reset = false)
    {
        if(add)
            totalNotes++;            
        else if(reset)
            totalNotes = 0;
        else
            totalNotes--;
        numberOfNotesUI.text = "Nº of notes: " + totalNotes;
    }

    //DELETE ALL NOTES BUTTON
    public void DeleteAllNotesButton()
    {
        for(int i = 0; i < inputChart.childCount; i++)
            Destroy(inputChart.GetChild(i).GameObject());
        circleList.Clear();
        squareList.Clear();
        triangleList.Clear();
        diamondList.Clear();
        ChangeNumberOfNotes(false, true);
    }
    
    //SAVE CHART BUTTON
    public void SaveChartButton()
    {
        circleList.Sort((c1, c2) => c1.GetComponent<FallingInputController>().inputTime.CompareTo(c2.GetComponent<FallingInputController>().inputTime));
        squareList.Sort((c1, c2) => c1.GetComponent<FallingInputController>().inputTime.CompareTo(c2.GetComponent<FallingInputController>().inputTime));
        triangleList.Sort((c1, c2) => c1.GetComponent<FallingInputController>().inputTime.CompareTo(c2.GetComponent<FallingInputController>().inputTime));
        diamondList.Sort((c1, c2) => c1.GetComponent<FallingInputController>().inputTime.CompareTo(c2.GetComponent<FallingInputController>().inputTime));
        SaveLoadManager.inputTypeCountList.Clear();
        SaveLoadManager.inputTimeList.Clear();
        for(int i = 0; i < 4; i++)
            SaveLoadManager.inputTypeCountList.Add(0);
        foreach(GameObject circle in circleList)
        {
            SaveLoadManager.inputTypeCountList[0]++;
            SaveLoadManager.inputTimeList.Add(circle.GetComponent<FallingInputController>().inputTime);
        }
            
        foreach(GameObject square in squareList)
        {
            SaveLoadManager.inputTypeCountList[1]++;
            SaveLoadManager.inputTimeList.Add(square.GetComponent<FallingInputController>().inputTime);
        }
            
        foreach(GameObject triangle in triangleList)
        {
            SaveLoadManager.inputTypeCountList[2]++;
            SaveLoadManager.inputTimeList.Add(triangle.GetComponent<FallingInputController>().inputTime);
        }
            
        foreach(GameObject diamond in diamondList)
        {
            SaveLoadManager.inputTypeCountList[3]++;
            SaveLoadManager.inputTimeList.Add(diamond.GetComponent<FallingInputController>().inputTime);
        }
        SaveLoadManager.SaveChart(audioSource.clip.name);
    }

    //PLAY/PAUSE BUTTON
    public void PlayButtonToggle()
    {
        Image playButtonImage = playButtonUI.GetComponent<Image>();
        //PLAY
        if(playButtonImage.color == greenButton)
        {
            StartCoroutine(coroutine);
            DisableUIElements();
        }
        //PAUSE
        else if(playButtonImage.color == redButton)
        {
            audioSource.Pause();
            ResetCoroutine();
            playButtonTextUI.text = "PLAY";
            playButtonImage.color = greenButton;
            EnableUIElements();
        }
        //COUNTING DOWN
        else
        {
            ResetCoroutine();
            playButtonTextUI.text = "PLAY";
            playButtonImage.color = greenButton;
            EnableUIElements();
        }
    }

    //WHEN TIMER CHANGES
    public void OnTimerChange()
    {
        print("Entered On Timer Change");
        int minutes = int.Parse(timerInputUI.text.Split(":")[0]);
        currentTime = 0;
        for(int i = 0; i < minutes; i++)
            currentTime += 60;
        currentTime += float.Parse(timerInputUI.text.Split(":")[1]);
        audioSource.time = currentTime;
        ScrollNotes();
    }

    //RESTART TIMER BUTTON
    public void RestartTimer()
    {
        timerInputUI.text = "00:00,000";
        currentTime = 0;
        audioSource.time = currentTime;
        ScrollNotes();
    }

    //COUNTDOWN COROUTINE
    private IEnumerator Countdown()
    {
        int seconds = int.Parse(waitForSecondsInputUI.text);
        if(!waitForSecondsToggleUI.isOn)
            seconds = 0;
        playButtonUI.GetComponent<Image>().color = yellowButton;
        while (seconds > 0)
        {
            playButtonTextUI.text = "STARTING IN " + seconds.ToString();
            yield return new WaitForSeconds(1);
            --seconds;
        }
        playButtonTextUI.text = "PAUSE";
        playButtonUI.GetComponent<Image>().color = redButton;
        audioSource.Play();
        isPlaying = true;
    }

    //RESET COROUTINE
    private void ResetCoroutine()
    {
        StopCoroutine(coroutine);
        coroutine = Countdown();
        isPlaying = false;
    }

    //UNITY FUNCTIONS
    void Awake()
    {
        /*circleList.Add(LastNotes[0]);
        squareList.Add(LastNotes[1]);
        triangleList.Add(LastNotes[2]);
        diamondList.Add(LastNotes[3]);*/
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        audioSource = GetComponent<AudioSource>();
        yOffset = inputChart.transform.position.y;
    }

    void Start()
    {
        coroutine = Countdown();
    }

    void Update()
    {
        if(isPlaying)
        {
            CalculateTime();
            ScrollNotes();
        }
    }
}