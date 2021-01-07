using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventHandler : MonoBehaviour
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
        PlayerCharacterController.Instance.PausePlayerAniamtor(true);
        PlayerCharacterController.Instance.ThrowHook();
    }
    #endregion

    #region Private Functions
    private void EnableHookSkinnedMeshRenderer(bool value)
    {
        hookSkinnedMeshRenderer.enabled = value;
    }
    #endregion
}
