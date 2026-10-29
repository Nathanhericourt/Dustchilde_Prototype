using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string speakerName = "NPC";

    [TextArea(2, 4)]
    [SerializeField] private string[] dialogueLines;

    [Header("Repeat Visit")]
    [TextArea(2, 4)]
    [SerializeField] private string[] repeatDialogueLines;

    [Header("Face-to-Face")]
    [SerializeField] private float turnDuration = 0.8f;
    [SerializeField] private float npcForwardOffset = 0f; // adjust if model doesn't face its own +Z

    [Tooltip("Optional. A point on the NPC's head. The player's camera tilts to look at it. Leave empty to look straight ahead.")]
    [SerializeField] private Transform lookTarget;

    [Header("On Conversation Finished")]
    [Tooltip("If ticked, the event below only runs the first time this conversation finishes. Repeat chats won't run it again.")]
    [SerializeField] private bool eventOnlyOnFirstConversation = true;
    [Tooltip("Runs every time this NPC's dialogue finishes (initial or repeat). Drag in a DoorHinge > Unlock() to gate a door behind this conversation.")]
    [SerializeField] private UnityEvent onDialogueComplete;

    private bool hasSpokenBefore;
    private bool hasFiredEvent;
    private bool isTurning;
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null) playerTransform = playerMovement.transform;
    }

    public void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive)
        {
            DialogueManager.Instance.DisplayNextLine();
            return;
        }

        if (isTurning || playerTransform == null) return;

        StartCoroutine(FaceToFaceThenTalk());
    }

    private IEnumerator FaceToFaceThenTalk()
    {
        isTurning = true;
        DialogueManager.Instance.LockInteraction();

        // Preserve each object's original tilt (X/Z) - only change yaw (Y)
        Vector3 playerStartEuler = playerTransform.eulerAngles;
        Vector3 npcStartEuler = transform.eulerAngles;

        Vector3 toNpc = transform.position - playerTransform.position;
        toNpc.y = 0f;
        float playerTargetYaw = toNpc.sqrMagnitude > 0.001f
            ? Quaternion.LookRotation(toNpc).eulerAngles.y
            : playerStartEuler.y;

        Vector3 toPlayer = -toNpc;
        float npcTargetYaw = toPlayer.sqrMagnitude > 0.001f
            ? Quaternion.LookRotation(toPlayer).eulerAngles.y + npcForwardOffset
            : npcStartEuler.y;

        Quaternion playerStartRot = playerTransform.rotation;
        Quaternion npcStartRot = transform.rotation;
        Quaternion playerTargetRot = Quaternion.Euler(playerStartEuler.x, playerTargetYaw, playerStartEuler.z);
        Quaternion npcTargetRot = Quaternion.Euler(npcStartEuler.x, npcTargetYaw, npcStartEuler.z);

        float startPitch = playerMovement != null ? playerMovement.CameraPitch : 0f;
        float targetPitch = 0f;
        if (lookTarget != null && Camera.main != null)
        {
            Vector3 toTarget = lookTarget.position - Camera.main.transform.position;
            float flatDistance = new Vector2(toTarget.x, toTarget.z).magnitude;
            targetPitch = -Mathf.Atan2(toTarget.y, flatDistance) * Mathf.Rad2Deg;
        }

        float elapsed = 0f;
        while (elapsed < turnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / turnDuration;

            playerTransform.rotation = Quaternion.Slerp(playerStartRot, playerTargetRot, t);
            transform.rotation = Quaternion.Slerp(npcStartRot, npcTargetRot, t);

            if (playerMovement != null)
                playerMovement.CameraPitch = Mathf.Lerp(startPitch, targetPitch, t);

            yield return null;
        }

        playerTransform.rotation = playerTargetRot;
        transform.rotation = npcTargetRot;
        if (playerMovement != null) playerMovement.CameraPitch = targetPitch;

        isTurning = false;

        string[] linesToUse = (hasSpokenBefore && repeatDialogueLines.Length > 0)
            ? repeatDialogueLines
            : dialogueLines;

        hasSpokenBefore = true;

        DialogueManager.Instance.StartDialogue(speakerName, linesToUse, OnConversationFinished);

    }

    private void OnConversationFinished()
    {
        if (eventOnlyOnFirstConversation && hasFiredEvent) return;

        hasFiredEvent = true;
        onDialogueComplete.Invoke();
    }

    public string GetInteractPrompt()
    {
        return $"Press to talk to {speakerName}";
    }
}