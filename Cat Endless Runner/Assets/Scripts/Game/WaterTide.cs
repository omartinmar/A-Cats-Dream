using UnityEngine;

public class WaterRise : MonoBehaviour
{

    GameObject player;

    [SerializeField] float currentSpeed;
    float normalSpeed=0.008f;
    float fastSpeed=0.05f;
    public float triggerDistance = 4f;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }




    void FixedUpdate()
    {  
        //float distance = Vector3.Distance(transform.position, playerTransform.position);

        float distance = player.transform.position.y - transform.position.y ;
        Debug.Log("Distance: " + distance + "Player: " + player.transform.position.y + " || WaterTide: " + transform.position.y);
        if (distance <= triggerDistance)
        {
            currentSpeed = normalSpeed;
        }
        else
        {
            currentSpeed = fastSpeed;
        }


        transform.position = new Vector3(0,transform.position.y+currentSpeed,0);
        //transform.position = new Vector3(0,transform.position.y+0.008f,0);    
    }
    
}
