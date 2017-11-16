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
        }
    }
}