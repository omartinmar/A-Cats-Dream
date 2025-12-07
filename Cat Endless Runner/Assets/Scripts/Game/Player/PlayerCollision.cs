using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;

    private Animator animator;
    private PlayerMovement playerMovementScript;
    private Rigidbody2D body;
    private float hurtTimer = 0;
    private float hurtCooldown = 1.5f;
    private float resetHitsTimer = 5f;

    public bool playerIsDead;
    public int maxHits = 3;
    public int hits;
    public bool isHurt;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerMovementScript = FindFirstObjectByType<PlayerMovement>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        hits = 0;
    }
    private void Update()
    {
        hurtTimer += Time.fixedDeltaTime;
        if (hurtTimer >= resetHitsTimer) hits = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerIsDead)
        {
       

            if (collision.CompareTag("Traps")) Die();


            if (collision.CompareTag("WaterTide"))
            {
                GameOverCall();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!playerIsDead)
        {
            if (collision.collider.CompareTag("Enemies"))
            {
                if (hurtTimer >= hurtCooldown)
                {
                    hurtTimer = 0;
                    hits += 1;
                    StartCoroutine(Hurt(collision));
                    if (hits >= maxHits) Die();
                }
            }
        }

    }

    private IEnumerator Hurt(Collision2D collision)
    {
        playerMovementScript.isHurt = true;

        animator.Play("Hurt");
        Vector2 direction = (body.transform.position - collision.transform.position).normalized;
        body.linearVelocity = Vector2.zero;
        body.linearVelocity = new Vector2(Mathf.Sign(direction.x) * 6f, 8f);

        yield return new WaitForSeconds(0.5f);

        playerMovementScript.isHurt = false;

    }

    public void GameOverCall()
    {
        gameManager.GameOver();
    }

    public void Die()
    {
        body.linearVelocity = Vector2.zero;
        playerMovementScript.enabled = false;
        playerIsDead = true;
        animator.Play("Death");
    }

}
