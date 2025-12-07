using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Timer timerScript;
    public float respawnTimer;

    private GameManager gameManager;


    void Start()
    {
        timerScript = FindFirstObjectByType<Timer>();
        gameManager = FindFirstObjectByType<GameManager>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {  
            gameManager.respawnPoint = transform.position;
            gameManager.respawnTime= timerScript.timePassed;
            gameObject.SetActive(false);
        }
        
    }
}
