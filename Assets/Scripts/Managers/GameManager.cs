using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager Instance;
    //UI ELEMENTS
    public TextMeshProUGUI timerUI;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI hitIndicator;
    //TIME VARIABLES
    private int seconds;
    private int minutes;
    private double startTime;
    private double currentTime;
    private bool started;
    //INPUT VARIABLES
    private List<GameObject> fallingInputList;
    
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
