using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{

    [SerializeField] public String scene;
    public GameObject winCanvas;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            //Time.timeScale = 0f;
            SceneManager.LoadScene(scene);
        }
    }
}
