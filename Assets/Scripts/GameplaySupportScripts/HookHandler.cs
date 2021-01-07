using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private float velocity = 0f;

    [Header("Componenet Reference")]
    [SerializeField] private LineRenderer ropeRenderer = null;
    #endregion

    #region MonoBehaviour Functions
    private void Update()
    {
        ropeRenderer.SetPosition(0, PlayerCharacterController.Instance.GetHookSpawnPointPosition());
        ropeRenderer.SetPosition(1, transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime * velocity, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            PlayerCharacterController.Instance.PlayerCharacterStatus = PlayerStatus.Riding;
            this.enabled = false;
        }
        else if (other.gameObject.tag == "Player" && PlayerCharacterController.Instance.PlayerCharacterStatus == PlayerStatus.Riding)
        {
            PlayerCharacterController.Instance.ResetPlayer();
        }
    }
    #endregion
}
