using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HookJoystickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Properties
    public static HookJoystickHandler Instance = null;
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

        HookJoystickStatus = HookJoystickStatus.Inactive;
    }
    #endregion

    #region Getter And Setter
    public HookJoystickStatus HookJoystickStatus { get; set; }
    #endregion

    #region Interface Functions
    public void OnPointerDown(PointerEventData eventData)
    {
        HookJoystickStatus = HookJoystickStatus.Active;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HookJoystickStatus = HookJoystickStatus.Inactive;

        LevelUIManager.Instance.EnableHookJoystick(false);
    }
    #endregion
}
