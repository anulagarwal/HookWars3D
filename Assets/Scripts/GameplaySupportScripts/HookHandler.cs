using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private float velocity = 0f;
    [SerializeField] private bool Hit = false;

    [Header("Componenet Reference")]
    [SerializeField] private LineRenderer ropeRenderer = null;

    private bool enemyCaught = false;
    private bool playerCaught = false;
    private Transform enemyCaughtTransform = null;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        enemyCaught = false;
        playerCaught = false;
    }

    private void Update()
    {
        if (!Hit)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * velocity, Space.Self);
        }

        if (HookOwnerCharacter == HookOwner.Player)
        {
            if (enemyCaught)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * velocity, Space.Self);
            }

            ropeRenderer.SetPosition(0, PlayerCharacterController.Instance.GetHookSpawnPointPosition());
            ropeRenderer.SetPosition(1, transform.position);

            if (Vector3.Distance(transform.position, PlayerCharacterController.Instance.transform.position) <= 1.5f && PlayerCharacterController.Instance.PlayerCharacterStatus == PlayerStatus.Riding)
            {
                PlayerCharacterController.Instance.ResetPlayer();
                if (Hit)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if(HookOwnerCharacter == HookOwner.Enemy)
        {
            if (playerCaught)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * velocity, Space.Self);
            }

            ropeRenderer.SetPosition(0, OwnerTransform.GetComponent<EnemyController>().GetHookSpawnPointPosition());
            ropeRenderer.SetPosition(1, transform.position);

            if (Vector3.Distance(transform.position, OwnerTransform.position) <= 1.5f)
            {
                PlayerCharacterController.Instance.ResetPlayer();
                PlayerCharacterController.Instance.transform.parent = null;
                OwnerTransform.GetComponent<EnemyController>().PauseEnemyAnimator(false);
                OwnerTransform.GetComponent<EnemyController>().Attack();
                if (Hit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            PlayerCharacterController.Instance.PlayerCharacterStatus = PlayerStatus.Riding;
            // this.enabled = false;
            Hit = true;
        }
        else if(other.gameObject.tag == "Enemy" && HookOwnerCharacter == HookOwner.Player)
        {
            Hit = true;
            enemyCaught = true;
            enemyCaughtTransform = other.gameObject.transform;
            enemyCaughtTransform.parent = transform;
        }
        else if(other.gameObject.tag == "Player" && HookOwnerCharacter == HookOwner.Enemy)
        {
            Hit = true;
            playerCaught = true;
            OwnerTransform.GetComponent<EnemyController>().PlayerCaughtTransform = other.gameObject.transform;
            PlayerCharacterController.Instance.transform.parent = transform;
            PlayerCharacterController.Instance.PlayerCharacterStatus = PlayerStatus.CaughtByEnemy;
        }
    }
    #endregion

    #region Getter And Setter
    public HookOwner HookOwnerCharacter { get; set; }

    public Transform OwnerTransform { get; set; }
    #endregion
}
