using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public String scene;
    [SerializeField] public PlayableDirector playableDirector;
    private double timer;

    


    void Start()
    {
        timer = playableDirector.playableAsset.duration;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0)
        {
          SceneManager.LoadScene(scene);  
        }
        
        
    }
}
