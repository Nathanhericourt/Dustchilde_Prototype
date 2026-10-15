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

    [Header("Objective Steps")]
    [SerializeField] private ObjectiveStep[] steps;

    [Header("Completion")]
    [SerializeField] private string allCompleteText = "All objectives complete.";

    private int currentStepIndex;
    private int currentProgress;
    private bool allComplete;

    public bool AllComplete => allComplete;
    public int CurrentStepIndex => currentStepIndex;
    private int[] storedProgress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        storedProgress = new int[steps.Length];
    }

    private void Start()
    {
        currentStepIndex = 0;
        currentProgress = 0;

        ShowCurrentStep();
    }

    // Call this each time the player does something that counts (picks up an item, etc.)
    public void AddProgress()
    {
        if (allComplete || steps == null || steps.Length == 0) 
            return;

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
        if (allComplete || steps == null || steps.Length == 0) 
            return;

        ObjectiveStep finishedStep = steps[currentStepIndex];

        // Move on to the next step first, then run the finished step's events
        currentStepIndex++;
        currentProgress = (currentStepIndex < steps.Length) ? storedProgress[currentStepIndex] : 0;

        finishedStep.onStepComplete?.Invoke();

        // If the new step was already done in advance, finish it straight away
        if (currentStepIndex < steps.Length && steps[currentStepIndex].progressNeeded > 0 &&
            currentProgress >= steps[currentStepIndex].progressNeeded)
        {
            CompleteCurrentStep();
        }

        if (currentStepIndex >= steps.Length)
        {
            allComplete = true;
            if (objectiveText != null) objectiveText.text = allCompleteText;
        }
        else
        {
            ShowCurrentStep();
        }

    }

    public void CompleteStepIfCurrent(int stepIndex)
    {
        if (stepIndex == currentStepIndex) CompleteCurrentStep();
    }

    public void AddProgressIfCurrent(int stepIndex)
    {
        if (stepIndex == currentStepIndex) AddProgress();
    }

    public bool IsCurrentStep(int index)
    {
        return !allComplete && currentStepIndex == index;
    }

    private void ShowCurrentStep()
    {
        if (objectiveText == null || steps == null || steps.Length == 0) return;

        ObjectiveStep step = steps[currentStepIndex];

        if (step.progressNeeded > 0)
        {
            objectiveText.text = $"{step.description} ({currentProgress}/{step.progressNeeded})";
        }
        else
        {
            objectiveText.text = step.description;
        }
    }

    public void AddProgressToStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= steps.Length) return;

        if (stepIndex == currentStepIndex)
        {
            AddProgress();
        }
        else if (stepIndex > currentStepIndex)
        {
            storedProgress[stepIndex]++;   // saved until that step starts
        }
    }

}
