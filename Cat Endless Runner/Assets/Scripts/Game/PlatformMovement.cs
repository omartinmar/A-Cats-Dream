using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    public float speed = 4f;

    [SerializeField] public Transform pointA;

    [SerializeField] public Transform pointB;
    private Transform currentTarget;
    private Vector3 initialPosition;

    private bool onCollision;

    void Awake()
    {
        initialPosition=gameObject.transform.position;
    }
    void Start()
    {
        currentTarget = pointB;
        
    }

    void Update()
    {
        //if (onCollision)
        //{
            transform.position = Vector3.MoveTowards(transform.position,
            currentTarget.position,
            speed * Time.fixedDeltaTime);


            if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
            {
                if (currentTarget == pointB) currentTarget = pointA;
                else currentTarget = pointB;
            }
        //}

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
            onCollision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
            onCollision = false;
        }
    }

    public void ResetPlatform()
    {
        onCollision = false; 
        transform.position = initialPosition;
        currentTarget = pointB; 

    }

}
