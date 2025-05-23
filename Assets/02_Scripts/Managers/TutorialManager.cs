using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    //[SerializeField] private TutorialStep[] steps;
    private int currentIndex = 0;
    public TutorialHandler tutorialHandeler; 
    private void Awake()
    {
        Instance = this;
        tutorialHandeler = GetComponent<TutorialHandler>();
    }
    public void ChangeStep(TutorialStep tutorialStep)
    {
        tutorialHandeler.ChangeStep(tutorialStep);
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
