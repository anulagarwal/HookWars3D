using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventsHandler : MonoBehaviour
{
    #region Properties
    [Header("Component Reference")]
    [SerializeField] private SkinnedMeshRenderer hookSkinnedMeshRenderer = null;
    #endregion

    #region MonoBehaviour Functions
    #endregion

    #region Anim Event Functions
    private void AnimEvent_ThrowHook()
    {
        EnableHookSkinnedMeshRenderer(false);
        PlayerCharacterController.Instance.PausePlayerAnimator(true);
        PlayerCharacterController.Instance.ThrowHook();
    }

    private void AnimEvent_Punch()
    {
        PlayerCharacterController.Instance.KillEnemyCaught();
    }
    #endregion

    #region Private Functions
    private void EnableHookSkinnedMeshRenderer(bool value)
    {
        hookSkinnedMeshRenderer.enabled = value;
    }
    #endregion
}
