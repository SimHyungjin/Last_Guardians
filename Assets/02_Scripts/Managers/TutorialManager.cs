using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    //[SerializeField] private TutorialStep[] steps;
    private int currentIndex = 0;
    public TutorialHandler tutorialHandler; 
    private void Awake()
    {
        Instance = this;
        tutorialHandler = GetComponent<TutorialHandler>();
    }
    public void ChangeStep(TutorialStep tutorialStep)
    {
        tutorialHandler.ChangeStep(tutorialStep);
    }

    private void OnDestroy()
    {
        tutorialHandler = null;
        Instance = null;
    }
    //public void TryTrigger(string name)
    //{

    //    if (currentIndex >= steps.Length)
    //        return;

    //    var step = steps[currentIndex];
    //    step.TryTrigger(name);

    //    if (step.triggerName == name)
    //    {
    //        currentIndex++;
    //    }
    //}
}
