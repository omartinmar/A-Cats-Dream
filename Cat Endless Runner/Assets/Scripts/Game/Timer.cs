using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public static Timer instance; //todo



   public float timePassed;
   float highscoreTime;
  
   [SerializeField] public TextMeshProUGUI timeText;
   [SerializeField] public TextMeshProUGUI highScoreText;
   public PlayerCollision playerCollision;

    void Awake()
    {
        if (instance == null)
        {
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {   
       
        playerCollision = FindFirstObjectByType<PlayerCollision>(); 

        //DELETE DATA
        //PlayerPrefs.DeleteKey("highscore");
        //PlayerPrefs.Save();

        float aux = PlayerPrefs.GetFloat("highscore",0);
        if (aux < 1)
        {
          highscoreTime = float.MaxValue;  
        }
        else
        {
           highscoreTime = PlayerPrefs.GetFloat("highscore"); 

           //Set Highscore values
            int min = Mathf.FloorToInt(highscoreTime/60);
            int sec = Mathf.FloorToInt(highscoreTime%60);
            //Display on text
            highScoreText.text = string.Format("{0:00}:{1:00}", min, sec);
        } 
    }


    void Update()
    {
        if (!playerCollision.playerIsDead)
        {
           timePassed += Time.deltaTime;
           
        }
        
        int min = Mathf.FloorToInt(timePassed/60);
        int sec = Mathf.FloorToInt(timePassed%60);
        
        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
        

        if (timePassed < highscoreTime)
        {
            PlayerPrefs.SetFloat("highscore",timePassed);
              
        }


        //Time for win scene
        Timer.instance.timePassed=timePassed;
        
    }
}
