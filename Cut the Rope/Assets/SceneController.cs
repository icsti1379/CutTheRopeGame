using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneController
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance;
    
        string level1 = "Level1";
        string level2 = "Level2";
        string level3 = "Level3";
        string level4 = "Level4";
        string level5 = "Level5";
        string test = "Test";

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                SceneManager.LoadScene(level1);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                SceneManager.LoadScene(level2);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SceneManager.LoadScene(level3);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(level4);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                SceneManager.LoadScene(level5);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene(test);
            }
        }
    }
}