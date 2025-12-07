using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    private EnemyMovement enemyMovementScript;
    public bool isDead;


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyMovementScript = FindFirstObjectByType<EnemyMovement>(); 
    }
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Claws"))
        {
            StartCoroutine(PerformDeath());
            
        }    
    }

    private IEnumerator PerformDeath()
    {
        isDead=true;
        body.bodyType = RigidbodyType2D.Static;
        body.linearVelocity = Vector2.zero;

        animator.Play("Death");
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
        
        
    }
}

