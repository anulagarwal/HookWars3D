using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    #region Properties
    public static LevelUIManager Instance = null;

    [Header("Gameplay UI Panel Setup")]
    [SerializeField] private GameObject hookJoystick = null;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    #region Gameplay UI Panel Functions
    public void EnableHookJoystick(bool value)
    {
        hookJoystick.SetActive(value);
    }
    #endregion
}
