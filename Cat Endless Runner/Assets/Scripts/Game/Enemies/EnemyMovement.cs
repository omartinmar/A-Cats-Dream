using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{

    public float speed =2f;

    //private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D boxCollider;
    public float chaseDistance = 2.5f;
    private GameObject player;
    private Rigidbody2D body;
    [SerializeField] float rangeOffset=1f;
     [SerializeField] Collider2D colliderRay;
    private PlayerCollision playerCollisionScript;
     private EnemyDamage enemyDamageScript;
    [SerializeField] float detectionRange=0.1f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        playerCollisionScript = FindFirstObjectByType<PlayerCollision>();
        enemyDamageScript = FindFirstObjectByType<EnemyDamage>();  
        //capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }



    void FixedUpdate()
    {
    
    if (playerCollisionScript.playerIsDead || enemyDamageScript.isDead) return;

     Vector2 origin = new Vector2(
        boxCollider.bounds.center.x + Math.Sign(transform.localScale.x) * rangeOffset,
        boxCollider.bounds.min.y + 0.05f);

    RaycastHit2D raycast = Physics2D.Raycast(origin, Vector2.down, detectionRange, LayerMask.GetMask("Ground"));


    //FOLLOW PLAYER AND CHECK IF ENEMY IS GROUNDED
    if (Vector2.Distance(transform.position, player.transform.position) <= chaseDistance
        && raycast.collider != null)
    {
        

        float directionX = player.transform.position.x - transform.position.x;

        body.linearVelocity = new Vector2(Mathf.Sign(directionX) * speed, body.linearVelocity.y);

        //FLIP TO CHASE PLAYER
        if (Math.Sign(transform.localScale.x) != Mathf.Sign(directionX))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        
    }

    else{
        //PATROL MODE
        if (raycast.collider != null)
        {
            body.linearVelocity = new Vector2(
                Mathf.Sign(transform.localScale.x) * speed,
                body.linearVelocity.y
            );
        }
        //FLIP WHEN REACH END
        else{
            transform.localScale = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z);
        }
    }


}



    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 offset = new Vector2(

        boxCollider.bounds.center.x + Mathf.Sign(transform.localScale.x) * rangeOffset,
        boxCollider.bounds.center.y);

        Gizmos.DrawSphere(offset, 0.05f);
        Gizmos.DrawLine(offset, offset + Vector2.down * detectionRange);
    }
        

        
}