using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public enum State {  MainMenu, InGame, Win, Lose, Tutorial};
    public State currentState;
    public int currentLevel;
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


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

            ChangeState(State.MainMenu);

        currentLevel = PlayerPrefs.GetInt("level", 0);
        UIManager.Instance.UpdateLevelText(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && currentState == State.MainMenu)
        {
            StartLevel();
        }
    }

    public void LoadLevel()
    {

    }

    public void StartLevel()
    {
        ChangeState(State.InGame);
    }
    public void WinLevel()
    {
        if (currentState != State.Win)
        {
            currentLevel++;
            PlayerPrefs.SetInt("level", currentLevel);
            ChangeState(State.Win);
        }
    }

    public void LoseLevel()
    {
        ChangeState(State.Lose);
    }
    public void ChangeState(State state)
    {
        currentState = state;
        UIManager.Instance.UpdateState(state);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }
}
