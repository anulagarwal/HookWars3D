using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject InGame;
    public GameObject Win;
    public GameObject Lose;

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
