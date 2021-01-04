﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum State {  MainMenu, InGame, Win, Lose};
    public State currentState;

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
        ChangeState(State.Win);
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
}