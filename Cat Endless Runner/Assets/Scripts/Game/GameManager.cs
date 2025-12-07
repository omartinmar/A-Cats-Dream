using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverCanvas;
    public GameObject winCanvas;
    public GameObject pauseCanvas;
    public GameObject optionsCanvas;

    [SerializeField] public GameObject player;
    [SerializeField] public GameObject waterTide;

    private bool isPaused;
    public Vector3 respawnPoint;
    public float respawnTime;
    private Animator playerAnimator;
    private Rigidbody2D playerBody;
    public PlayerMovement playerMovementScript;
    public PlatformMovement platformMovementScript;

     public string highScoreText;
     private PlayerCollision playerCollisionScript;
     private Timer  timerScript;
     

    void Awake()
    {
        timerScript = FindFirstObjectByType<Timer>();
        platformMovementScript = FindFirstObjectByType<PlatformMovement>();
        playerAnimator = player.GetComponent<Animator>();
        playerBody = player.GetComponent<Rigidbody2D>();
        playerCollisionScript = FindFirstObjectByType<PlayerCollision>(); 
        
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)  && !gameOverCanvas.activeInHierarchy)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Respawn()
    {
        //Platforms
        //platformMovementScript.ResetPlatform();

        //Player
        playerCollisionScript.playerIsDead=false;
        playerCollisionScript.hits=0;
        player.SetActive(true);
        playerBody.bodyType = RigidbodyType2D.Dynamic;
        playerAnimator.Play("Idle");
        

        //Store time
       timerScript.timePassed = respawnTime;

        //Player respawn checkpoint
        player.transform.position = respawnPoint;
            
        //WaterTide respawn below respawn point
        waterTide.transform.position =  new Vector3(
            waterTide.transform.position.x,
            respawnPoint.y-10f,
            waterTide.transform.position.z);
      

        
        gameOverCanvas.SetActive(false);
        player.SetActive(true);
        playerMovementScript.enabled = true;
        
        Time.timeScale = 1f;
        
    }

    public void Pause()
    {
        pauseCanvas.SetActive(true);
        isPaused=true;
        Time.timeScale = 0f;
        
    }
    public void Resume()
    {
        pauseCanvas.SetActive(false);
        isPaused=false;
        Time.timeScale = 1f; 
    }
    
    public void GameOver()
    {
        //Enable GameOver Canvas
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
