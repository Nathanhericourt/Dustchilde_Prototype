using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    // One step of the levels objectives
    [System.Serializable]
    public class ObjectiveStep
    {
        public string description = "Do something";

        [Tooltip("How many times AddProgress() must be called (e.g. 3 = collect 3 items). " + "0 = this step only finishes when CompleteCurrentStep() is called.")]
        public int progressNeeded = 0;

        [Tooltip("Runs when this step is finished. Use it to open doors, show items, etc.")]
        public UnityEvent onStepComplete;
    }

    public static ObjectiveManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI objectiveText;

    [Header("Objective Steps (played in order)")]
    [SerializeField] private ObjectiveStep[] steps;

    [Header("When every step is done")]
    [SerializeField] private string allCompleteText = "";

    private int currentStepIndex;
    private int currentProgress;
    private bool allComplete;

    public bool AllComplete => allComplete;
    public int CurrentStepIndex => currentStepIndex;

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
        ShowCurrentStep();
    }

    // Call this each time the player does something that counts (picks up an item, etc.)
    public void AddProgress()
    {
        if (allComplete || steps.Length == 0) return;

        currentProgress++;
        ObjectiveStep step = steps[currentStepIndex];

        if (step.progressNeeded > 0 && currentProgress >= step.progressNeeded)
        {
            CompleteCurrentStep();
        }
        else
        {
            ShowCurrentStep();
        }
    }

    // Call this to finish the current step straight away (for steps with Progress Needed = 0)
    public void CompleteCurrentStep()
    {
        if (allComplete || steps.Length == 0) return;

        ObjectiveStep finishedStep = steps[currentStepIndex];

        // Move on to the next step first, then run the finished step's events
        currentStepIndex++;
        currentProgress = 0;

        if (currentStepIndex >= steps.Length)
        {
            allComplete = true;
            if (objectiveText != null) objectiveText.text = allCompleteText;
        }
        else
        {
            ShowCurrentStep();
        }

        finishedStep.onStepComplete.Invoke();
    }

    private void ShowCurrentStep()
    {
        if (objectiveText == null || steps.Length == 0) return;

        ObjectiveStep step = steps[currentStepIndex];

        if (step.progressNeeded > 0)
            objectiveText.text = $"{step.description} ({currentProgress}/{step.progressNeeded})";
        else
            objectiveText.text = step.description;
    }

}
