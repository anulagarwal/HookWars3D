using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    //Added
    private bool objectCaught = false;
    private Transform enemyCaughtTransform = null;

    //Added
    private Transform objectCaughtTransform = null;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        enemyCaught = false;
        playerCaught = false;
        ForceStop = false;
    }

    private void Update()
    {
        if (!Hit && !ForceStop)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * velocity, Space.Self);
        }

        if (HookOwnerCharacter == HookOwner.Player)
        {
            if (enemyCaught || objectCaught)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * velocity, Space.Self);
            }
           
            else if (ForceStop)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * velocity, Space.Self);
            }

            ropeRenderer.SetPosition(0, PlayerCharacterController.Instance.GetHookSpawnPointPosition());
            ropeRenderer.SetPosition(1, transform.position);

            if (Vector3.Distance(transform.position, PlayerCharacterController.Instance.transform.position) <= 0.9f && PlayerCharacterController.Instance.PlayerCharacterStatus == PlayerStatus.Riding)
            {
                if (Hit)
                {
                  
                    PlayerCharacterController.Instance.ResetPlayer();

                    Destroy(gameObject);
                }
            }
            else if(Vector3.Distance(transform.position, PlayerCharacterController.Instance.transform.position) <= 1.5f && ForceStop)
            {

                //Added here
                if (objectCaught)
                {
                    objectCaughtTransform.parent = null;
                    objectCaughtTransform.GetComponent<BoxCollider>().enabled = false;
                    objectCaught = false;
                }
                PlayerCharacterController.Instance.ResetPlayer();
                Destroy(gameObject);
            }
        }
        else if(HookOwnerCharacter == HookOwner.Enemy)
        {
            if (playerCaught)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * velocity, Space.Self);

                //print(Vector3.Distance(transform.position, OwnerTransform.position));
                if (Vector3.Distance(transform.position, OwnerTransform.position) <= 1.2f)
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
            else if (ForceStop)
            {
                transform.Translate(-Vector3.forward * Time.deltaTime * velocity, Space.Self);

                if (Vector3.Distance(transform.position, OwnerTransform.position) <= 0.65f)
                {
                    OwnerTransform.GetComponent<EnemyController>().PauseEnemyAnimator(false);
                    OwnerTransform.GetComponent<EnemyController>().SpawnedHookRef = null;
                    OwnerTransform.GetComponent<EnemyController>().EnemyCharacterStatus = EnemyStatus.Walking;
                    Destroy(gameObject);
                }
            }

            ropeRenderer.SetPosition(0, OwnerTransform.GetComponent<EnemyController>().GetHookSpawnPointPosition());
            ropeRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (HookOwnerCharacter == HookOwner.Player)
            {
                PlayerCharacterController.Instance.PlayerCharacterStatus = PlayerStatus.Riding;
                // this.enabled = false;
                Hit = true;
            }
            else if(HookOwnerCharacter == HookOwner.Enemy)
            {
                ForceStop = true;
            }
        }
        else if(other.gameObject.tag == "Enemy" && HookOwnerCharacter == HookOwner.Player)
        {
            Hit = true;
            enemyCaught = true;
            enemyCaughtTransform = other.gameObject.transform;
            enemyCaughtTransform.GetComponent<NavMeshAgent>().isStopped = true;
            enemyCaughtTransform.GetComponent<EnemyController>().enabled = false;
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

        //Added here
        else if(other.gameObject.tag=="Hookable" && HookOwnerCharacter == HookOwner.Player)
        {
            Hit = true;

            other.gameObject.transform.parent = transform;
            objectCaught = true;
            objectCaughtTransform = other.gameObject.transform;
        }
    }
    #endregion

    #region Getter And Setter
    public HookOwner HookOwnerCharacter { get; set; }

    public Transform OwnerTransform { get; set; }

    public bool ForceStop { get; set; }
    #endregion
}
