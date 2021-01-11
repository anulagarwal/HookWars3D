using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEventsHandler : MonoBehaviour
{
    #region Properties
    [Header("Component Reference")]
    [SerializeField] private SkinnedMeshRenderer hookSkinnedMeshRenderer = null;
    [SerializeField] private EnemyController enemyController = null;
    #endregion

    #region MonoBehaviour Functions
    #endregion

    #region Anim Event Functions
    private void AnimEvent_ThrowHook()
    {
        EnableHookSkinnedMeshRenderer(false);
        enemyController.PauseEnemyAnimator(true);
        enemyController.ThrowHook();
    }

    private void AnimEvent_Punch()
    {
        EnableHookSkinnedMeshRenderer(true);
        enemyController.PauseEnemyAnimator(true);
        enemyController.PlayerCaughtTransform.GetComponent<PlayerCharacterController>().EnableRagdoll(true);
        enemyController.PlayerCaughtTransform.GetComponent<PlayerCharacterController>().ApplyImpactForce((enemyController.PlayerCaughtTransform.position - enemyController.transform.position).normalized);
    }
    #endregion

    #region Private Functions
    private void EnableHookSkinnedMeshRenderer(bool value)
    {
        hookSkinnedMeshRenderer.enabled = value;
    }
    #endregion
}
