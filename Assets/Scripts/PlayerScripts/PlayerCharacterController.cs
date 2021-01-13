using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    #region Properties
    public static PlayerCharacterController Instance = null;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float ridingSpeed = 0f;
    [SerializeField] private float hookRange = 0f;

    [Header("Component Reference")]
    [SerializeField] private CharacterController cc = null;
    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private Transform directionIndicator = null;
    [SerializeField] private MeshRenderer directionIndicatorMeshRenderer = null;

    [Header("Hook Throw Mech. Setup")]
    [SerializeField] private GameObject hookPrefab = null;
    [SerializeField] private Transform hookSpawnPoint = null;

    [Header("Ragdoll Setup")]
    [SerializeField] private float impactForce = 0f;
    [SerializeField] private GameObject ragdoll = null;
    [SerializeField] private Rigidbody chestRb = null;

    private VariableJoystick hookJoystick = null;
    private Vector3 joystickDirection = Vector3.zero;
    private Transform spawnedHookRef = null;
    private Transform enemyCaughtTransform = null;
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

    private void Start()
    {
        PlayerCharacterStatus = PlayerStatus.Idle;
        hookJoystick = FindObjectOfType<VariableJoystick>();
    }

    private void Update()
    {
        if (PlayerCharacterStatus != PlayerStatus.CaughtByEnemy)
        {
            if (PlayerCharacterStatus != PlayerStatus.Riding && PlayerCharacterStatus != PlayerStatus.Throw)
            {
                HookDirectionIndication();
                HookThrowMechanism();
            }
            else if (PlayerCharacterStatus == PlayerStatus.Riding)
            {
                HookRideMechanism();
            }
        }

        if (spawnedHookRef)
        {
            if(Vector3.Distance(transform.position,spawnedHookRef.position) >= hookRange)
            {
                spawnedHookRef.GetComponent<HookHandler>().ForceStop = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && PlayerCharacterStatus != PlayerStatus.Aiming)
        {
            ResetPlayer();

            if (enemyCaughtTransform == null)
            {
                playerAnimator.SetTrigger("Punch");
            }

            if (spawnedHookRef)
            {
                enemyCaughtTransform = other.gameObject.transform;
                enemyCaughtTransform.parent = null;

                Destroy(spawnedHookRef.gameObject);
               // spawnedHookRef.GetComponent<HookHandler>().DamageEnemy();
            }
        }
    }
    #endregion

    #region Getter And Setter
    public PlayerStatus PlayerCharacterStatus { get; set; }

    public float HookRange { get => hookRange; }
    #endregion

    #region Core Functions
    private void HookDirectionIndication()
    {
        joystickDirection = new Vector3(hookJoystick.Horizontal, 0, hookJoystick.Vertical).normalized;

        //PlayerMovement();
        if (joystickDirection != Vector3.zero)
        {
            //EnableDirectionIndicatorMeshRenderer(true);
            directionIndicator.rotation = Quaternion.LookRotation(joystickDirection);
            transform.rotation = Quaternion.LookRotation(joystickDirection);
        }
        else
        {
            //EnableDirectionIndicatorMeshRenderer(false);
        }
    }

    private void HookThrowMechanism()
    {
        if (HookJoystickHandler.Instance.HookJoystickStatus == HookJoystickStatus.Active)
        {
            PlayerCharacterStatus = PlayerStatus.Aiming;
        }
        else if (HookJoystickHandler.Instance.HookJoystickStatus == HookJoystickStatus.Inactive && PlayerCharacterStatus == PlayerStatus.Aiming)
        {
            EnableDirectionIndicatorMeshRenderer(false);
            PlayerCharacterStatus = PlayerStatus.Throw;

            if (PlayerCharacterStatus != PlayerStatus.Aiming)
            {
                playerAnimator.SetBool("Run", false);
            }

            playerAnimator.SetTrigger("Throw");

            transform.rotation = directionIndicator.rotation;
            directionIndicator.rotation = Quaternion.identity; 
        }
    }

    private void HookRideMechanism()
    {
        if (spawnedHookRef)
        {
            Vector3 direction = (spawnedHookRef.position - transform.position).normalized;
            cc.Move(new Vector3(direction.x, 0f, direction.z) * Time.deltaTime * ridingSpeed);
        }
    }

    private void PlayerMovement()
    {
        cc.Move(joystickDirection * Time.deltaTime * moveSpeed);

        if (PlayerCharacterStatus == PlayerStatus.Aiming)
        {
            playerAnimator.SetBool("Run", true);
        }
    }
    #endregion

    #region Public Functions
    public void ThrowHook()
    {
        spawnedHookRef = Instantiate(hookPrefab, hookSpawnPoint.position, transform.rotation).transform;
        spawnedHookRef.GetComponent<HookHandler>().HookOwnerCharacter = HookOwner.Player;
        spawnedHookRef.GetComponent<HookHandler>().OwnerTransform = transform;
    }

    public void ResetPlayer()
    {
        PlayerCharacterStatus = PlayerStatus.Idle;
        PausePlayerAnimator(false);
        EnableDirectionIndicatorMeshRenderer(true);
        LevelUIManager.Instance.EnableHookJoystick(true);
    }

    public void PausePlayerAnimator(bool value)
    {
        if (value)
        {
            playerAnimator.speed = 0;
        }
        else
        {
            playerAnimator.speed = 1;
        }
    }

    public Vector3 GetHookSpawnPointPosition()
    {
        return hookSpawnPoint.position;
    }

    public void Attack()
    {
        playerAnimator.SetTrigger("Punch");
    }

    public void KillEnemyCaught()
    {
        if (enemyCaughtTransform)
        {
            enemyCaughtTransform.GetComponent<EnemyController>().EnableRagdoll(true);
            enemyCaughtTransform.GetComponent<EnemyController>().ApplyImpactForce((enemyCaughtTransform.position - transform.position).normalized);
        }
    }

    public void EnableRagdoll(bool value)
    {
        playerAnimator.enabled = !value;
        ragdoll.SetActive(value);
    }

    public void ApplyImpactForce(Vector3 impactDirection)
    {
        chestRb.AddForce(impactDirection * impactForce, ForceMode.Impulse);
    }
    #endregion

    #region Private Functions
    private void EnableDirectionIndicatorMeshRenderer(bool value)
    {
        directionIndicatorMeshRenderer.enabled = value;
    }
    #endregion

    #region Gizmos Functions
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hookRange);
    }
    #endregion
}
