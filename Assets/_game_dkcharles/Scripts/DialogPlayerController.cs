using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

/// <summary>
/// Disables player movement during Yarn Spinner dialogue.
/// Attach to the Player GameObject or a GameManager.
/// Wire OnDialogueStart and OnDialogueComplete from the DialogueRunner.
/// </summary>
public class DialoguePlayerController : MonoBehaviour
{
    [Header("Player References")]
    [Tooltip("The PlayerInput component on the player")]
    [SerializeField] private PlayerInput playerInput;
    
    [Tooltip("The ThirdPersonController component on the player")]
    [SerializeField] private ThirdPersonController thirdPersonController;
    
    [Tooltip("The StarterAssetsInputs component on the player")]
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;

    [Header("Settings")]
    [Tooltip("Should camera look be disabled during dialogue?")]
    [SerializeField] private bool disableCameraLookDuringDialogue = true;
    
    [Tooltip("Should the player be frozen in place during dialogue?")]
    [SerializeField] private bool freezePlayerPosition = false;

    // Cached values for restoring state
    private Vector3 frozenPosition;
    private bool wasMovementDisabled = false;

    private void Start()
    {
        // Auto-find references if not assigned
        if (playerInput == null || thirdPersonController == null || starterAssetsInputs == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (playerInput == null)
                    playerInput = player.GetComponent<PlayerInput>();
                if (thirdPersonController == null)
                    thirdPersonController = player.GetComponent<ThirdPersonController>();
                if (starterAssetsInputs == null)
                    starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
            }
        }
    }

    /// <summary>
    /// Call this from DialogueRunner.onDialogueStart UnityEvent
    /// Disables player input to prevent movement during dialogue
    /// </summary>
    public void DisablePlayerMovement()
    {
        if (wasMovementDisabled) return; // Prevent double-disable
        
        wasMovementDisabled = true;

        // Store position if freezing
        if (freezePlayerPosition && thirdPersonController != null)
        {
            frozenPosition = thirdPersonController.transform.position;
        }

        // Clear any current input to stop ongoing actions
        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.move = Vector2.zero;
            starterAssetsInputs.jump = false;
            starterAssetsInputs.sprint = false;
            
            if (disableCameraLookDuringDialogue)
            {
                starterAssetsInputs.look = Vector2.zero;
            }
        }

        // Disable the player input component
        // This stops all input processing to ensure player stays still during dialogue
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

        Debug.Log("Player movement disabled for dialogue");
    }

    /// <summary>
    /// Call this from DialogueRunner.onDialogueComplete UnityEvent
    /// Re-enables player input after dialogue ends
    /// </summary>
    public void EnablePlayerMovement()
    {
        if (!wasMovementDisabled) return; // Prevent enable without prior disable
        
        wasMovementDisabled = false;

        // Re-enable the player input to restore normal controls
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }

        Debug.Log("Player movement enabled after dialogue");
    }

    private void LateUpdate()
    {
        // If freeze position is enabled, continuously reset player position during dialogue
        // This prevents any residual physics or animation from moving the player
        if (freezePlayerPosition && wasMovementDisabled && thirdPersonController != null)
        {
            thirdPersonController.transform.position = frozenPosition;
        }
    }
}
