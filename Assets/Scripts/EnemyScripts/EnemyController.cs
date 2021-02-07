using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private EnemyType enemyType = EnemyType.Idle;

    [Header("Impact Setup")]
    [SerializeField] private float impactForce = 0f;
    [SerializeField] private Rigidbody chestRb = null;

    [Header("Component Reference")]
    [SerializeField] private NavMeshAgent aiAgent = null;
    [SerializeField] private Animator enemyAnimator = null;
    [SerializeField] private GameObject ragdoll = null;
    [SerializeField] private CapsuleCollider capsuleCollider = null;
    [SerializeField] private EyeSightHandler eyeSightHandler = null;

    [Header("Enemy Patrol Setup")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Hook Setup")]
    [SerializeField] private GameObject hookPrefab = null;
    [SerializeField] private Transform hookSpawnPoint = null;

    private Transform spawnedHookRef = null;
    private Transform targetPoint = null;
    #endregion

    #region Delegates
    private delegate void EnemyAIMechanism();
    private EnemyAIMechanism enemyAIMechanism;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        switch (enemyType)
        {
            case EnemyType.Idle:
                enemyAIMechanism += StandingAIMechanism;
                break;
            case EnemyType.Walk:
                ChangeTargetDestination();

                enemyAnimator.SetBool("Walk", true);
                EnemyCharacterStatus = EnemyStatus.Walking;

                enemyAIMechanism += WalkingAIMechanism;
                break;
        }
        
    }

    private void Update()
    {
        enemyAIMechanism();
    }
    #endregion

    #region Core Functions
    private void WalkingAIMechanism()
    {
        if (EnemyCharacterStatus == EnemyStatus.Walking)
        {
            if (aiAgent.remainingDistance <= aiAgent.stoppingDistance)
            {
                ChangeTargetDestination();
            }

            EnemyRotation();

            if (eyeSightHandler.PlayerCaught && EnemyCharacterStatus != EnemyStatus.Throw)
            {
                Vector3 rotDirection = PlayerCharacterController.Instance.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(rotDirection);
                EnemyCharacterStatus = EnemyStatus.Throw;
                enemyAnimator.SetBool("Walk", false);
                enemyAnimator.SetTrigger("Throw");

                aiAgent.isStopped = true;
            }
        }
    }

    private void StandingAIMechanism()
    {
        if (eyeSightHandler.PlayerCaught && EnemyCharacterStatus != EnemyStatus.Throw)
        {
            Vector3 rotDirection = PlayerCharacterController.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(rotDirection);
            EnemyCharacterStatus = EnemyStatus.Throw;
            enemyAnimator.SetBool("Walk", false);
            enemyAnimator.SetTrigger("Throw");
        }
    }

    private void ChangeTargetDestination()
    {
        targetPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        aiAgent.SetDestination(targetPoint.position);
    }

    private void EnemyRotation()
    {
        Vector3 rotDirection = (targetPoint.position - transform.position).normalized;
        rotDirection = new Vector3(rotDirection.x, 0, rotDirection.z);
        transform.rotation = Quaternion.LookRotation(rotDirection);
    }

    private void EnemyMovement()
    {

    }
    #endregion

    #region Public Functions
    public void ThrowHook()
    {
        if (spawnedHookRef == null)
        {
            spawnedHookRef = Instantiate(hookPrefab, hookSpawnPoint.position, transform.rotation).transform;
            spawnedHookRef.GetComponent<HookHandler>().HookOwnerCharacter = HookOwner.Enemy;
            spawnedHookRef.GetComponent<HookHandler>().OwnerTransform = transform;
        }
    }

    public void EnableRagdoll(bool value)
    {
        ragdoll.SetActive(value);
        enemyAnimator.enabled = !value;
        capsuleCollider.enabled = !value;
        eyeSightHandler.gameObject.SetActive(false);
      
        this.enabled = false;
    }

    public void ApplyImpactForce(Vector3 impactDirection)
    {
        chestRb.AddForce(impactDirection * impactForce, ForceMode.Impulse);
    }

    public void PauseEnemyAnimator(bool value)
    {
        if (value)
        {
            enemyAnimator.speed = 0;
        }
        else
        {
            enemyAnimator.speed = 1;
        }
    }

    public Vector3 GetHookSpawnPointPosition()
    {
        return hookSpawnPoint.position;
    }

    public void Attack()
    {
        enemyAnimator.SetTrigger("Punch");
    }
    #endregion

    #region Getter And Setter
    public EnemyStatus EnemyCharacterStatus { get; set; }

    public Transform PlayerCaughtTransform { get; set; }

    public Transform SpawnedHookRef { get => spawnedHookRef; set => spawnedHookRef = value; }
    #endregion
}
