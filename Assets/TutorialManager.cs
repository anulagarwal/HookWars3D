using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    public bool IsTutorialOn;

    public List<GameObject> hintPanels;
    public int currentIndex = 0;

    private static TutorialManager _instance;

    public static TutorialManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       if( PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            IsTutorialOn = true;
            GameManager.Instance.ChangeState(GameManager.State.Tutorial);

        }
        else
        {
            IsTutorialOn = false;
            GameManager.Instance.ChangeState(GameManager.State.MainMenu);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentIndex == 0)
            {
                ChangeTutorial();
            }

            else if (currentIndex == 1)
            {
                ChangeTutorial();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(currentIndex == 2)
            {
                ChangeTutorial();
            }
        }
    }
    public void EndTutorial()
    {
        ChangeTutorial();
        StartCoroutine(End(5));
    }

    IEnumerator End(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetInt("tutorial", 1);
    }
   
    public void ChangeTutorial()
    {
        if (currentIndex < hintPanels.Count - 1)
        {
            hintPanels[currentIndex].SetActive(false);
            currentIndex++;
            hintPanels[currentIndex].SetActive(true);
        }
    }
}
