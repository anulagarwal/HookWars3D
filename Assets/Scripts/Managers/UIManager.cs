using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject InGame;
    public GameObject Win;
    public GameObject Lose;
    public GameObject awesomeText;
    public Text mainLevel;
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }


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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLevelText(int level)
    {
        mainLevel.text = "LEVEL " + level;
    }

    public void SpawnAwesomeText(string s , Vector3 pos)
    {
//        GameObject g = Instantiate(awesomeText, pos, Quaternion.identity);
//        g.GetComponent<AwesomeText>().SetText(s);
    }
    public void UpdateState(GameManager.State state)
    {

        switch (state)
        {
            case GameManager.State.InGame:
                MainMenu.SetActive(false);
                InGame.SetActive(true);
                Win.SetActive(false);
                Lose.SetActive(false);

                break;

            case GameManager.State.MainMenu:

                MainMenu.SetActive(true);
                InGame.SetActive(false);
                Win.SetActive(false);
                Lose.SetActive(false);

                break;

            case GameManager.State.Win:
                MainMenu.SetActive(false);
                InGame.SetActive(false);
                Win.SetActive(true);
                Lose.SetActive(false);
                break;

            case GameManager.State.Lose:
                MainMenu.SetActive(false);
                InGame.SetActive(false);
                Win.SetActive(false);
                Lose.SetActive(true);
                break;

        }
    }
}
