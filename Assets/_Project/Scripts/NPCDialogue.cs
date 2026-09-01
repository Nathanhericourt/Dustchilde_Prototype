using UnityEngine;
using System.Collections;

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

    private bool hasSpokenBefore;
    private bool isTurning;
    private Transform playerTransform;

    private void Awake()
    {
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null) playerTransform = player.transform;
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
        DialogueManager.Instance.LockInteraction(); // freeze player during the turn itself

        Quaternion playerStartRot = playerTransform.rotation;
        Quaternion npcStartRot = transform.rotation;

        Vector3 toNpc = transform.position - playerTransform.position;
        toNpc.y = 0f;
        Quaternion playerTargetRot = toNpc.sqrMagnitude > 0.001f ? Quaternion.LookRotation(toNpc) : playerStartRot;

        Vector3 toPlayer = -toNpc;
        Quaternion npcTargetRot = toPlayer.sqrMagnitude > 0.001f ? Quaternion.LookRotation(toPlayer) : npcStartRot;

        float elapsed = 0f;
        while (elapsed < turnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / turnDuration;
            playerTransform.rotation = Quaternion.Slerp(playerStartRot, playerTargetRot, t);
            transform.rotation = Quaternion.Slerp(npcStartRot, npcTargetRot, t);
            yield return null;
        }

        playerTransform.rotation = playerTargetRot;
        transform.rotation = npcTargetRot;
        isTurning = false;

        string[] linesToUse = (hasSpokenBefore && repeatDialogueLines.Length > 0)
            ? repeatDialogueLines
            : dialogueLines;

        hasSpokenBefore = true;

        DialogueManager.Instance.StartDialogue(speakerName, linesToUse);
    }

    public string GetInteractPrompt()
    {
        return $"Press to talk to {speakerName}";
    }
}