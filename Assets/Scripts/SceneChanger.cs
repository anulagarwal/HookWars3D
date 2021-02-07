using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public int currentLevel;
    public int maximumLevels;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 0);
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            if (currentLevel >= maximumLevels)
            {
                SceneManager.LoadScene("Level " + (Random.Range(0, maximumLevels) + 1));
            }
            else
            {
                SceneManager.LoadScene("Level " + (currentLevel + 1));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
