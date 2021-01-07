using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    #region Properties
    public static PlayerCharacterController Instance = null;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 0f;

    [Header("Component Reference")]
    [SerializeField] private CharacterController cc = null;
    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private Transform directionIndicator = null;
    [SerializeField] private MeshRenderer directionIndicatorMeshRenderer = null;

    [Header("Hook Throw Mech. Setup")]
    [SerializeField] private GameObject hookPrefab = null;
    [SerializeField] private Transform hookSpawnPoint = null;

    private VariableJoystick hookJoystick = null;
    private Vector3 joystickDirection = Vector3.zero;
    private Transform spawnedHookRef = null;
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
    #endregion

    #region Getter And Setter
    public PlayerStatus PlayerCharacterStatus { get; set; }
    #endregion

    #region Core Functions
    private void HookDirectionIndication()
    {
        joystickDirection = new Vector3(hookJoystick.Horizontal, 0, hookJoystick.Vertical).normalized;

        if (joystickDirection != Vector3.zero)
        {
            directionIndicator.rotation = Quaternion.LookRotation(joystickDirection);
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
            playerAnimator.SetTrigger("Throw");

            transform.rotation = directionIndicator.rotation;
            directionIndicator.rotation = Quaternion.identity; 
        }
    }

    private void HookRideMechanism()
    {
        Vector3 direction = (spawnedHookRef.position - transform.position).normalized;
        cc.Move(new Vector3(direction.x, 0f, direction.z) * Time.deltaTime * moveSpeed);
    }
    #endregion

    #region Public Functions
    public void ThrowHook()
    {
        spawnedHookRef = Instantiate(hookPrefab, hookSpawnPoint.position, transform.rotation).transform;
    }

    public void ResetPlayer()
    {
        PlayerCharacterStatus = PlayerStatus.Idle;
        PausePlayerAniamtor(false);
        EnableDirectionIndicatorMeshRenderer(true);
        LevelUIManager.Instance.EnableHookJoystick(true);
    }

    public void PausePlayerAniamtor(bool value)
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
    #endregion

    #region Private Functions
    private void EnableDirectionIndicatorMeshRenderer(bool value)
    {
        directionIndicatorMeshRenderer.enabled = value;
    }
    #endregion
}
