using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutoriaDialogue
{
    public string[] text;
    public void init()
    {
        text = new string[20];

        text[0] = "클릭을 눌러 진행.";
        text[1] = "라스트오브 가디언즈에 오신 여러분들 환영합니다.";
        text[2] = "이 게임은 타워디펜스 게임입니다.";
        text[3] = "라스트오브 가디언즈는 카드를 선택하는 멀리건 시스템이 있습니다.";
        text[4] = "총 10종류의 카드중 5장의 카드를 선택해 이번 게임에서 사용합니다.";
        text[5] = "6 종류의 속성카드중 3장 4장의 일반카드중 2장은 선택하게됩니다.";
        text[6] = "멀리건은 총 3회진행됩니다.";
        text[7] = "게임을 시작하기전에\n 카드를 선택해보죠.";
        text[8] = "";
        text[9] = "3장의 카드중 2장의\n 카드를 선택해주세요.";
        text[10] = "3장의 카드중 1장의 카드를 선택해주세요.";
        text[11] = "4장의 카드중 2장의 카드를 선택해주세요.";
        text[12] = "멀리건이 끝났습니다.";
        text[13] = "이제 실제 사용할 카드를 선택합니다.";
    }
}

public enum TutorialState
{
    isWaiting,
    isExplaining,
}
public enum TutorialStep
{
    GameStart,
    MulliganStart,
    MulliganEnd,
    TowerSelect
}

public class TutorialHandler : MonoBehaviour
{
    [Header("하이라이트원")]
    public Canvas tutorialCanvas;
    private GraphicRaycaster graphicRaycaster;
    [SerializeField] private Material cutoutBaseMaterial;
    private Material cutout;
    public GameObject cutoutObject;
    private Tweener currentTween;

    [Header("설명 타이핑")]
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    private bool isTyping = false;
    private string fullText;
    TutoriaDialogue tutoriaDialogue = new TutoriaDialogue();
    private Coroutine typingCoroutine;
    [SerializeField] public RectTransform[] dialogPos;

    [Header("상태")]
    public TutorialState tutorialState;
    public TutorialStep tutorialStep;
    private void Awake()
    {
        cutout= new Material(cutoutBaseMaterial);
        cutoutObject.GetComponent<Image>().material = cutout;
        cutoutObject.SetActive(true);
        graphicRaycaster= tutorialCanvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = true;
        tutoriaDialogue.init();
    }

    void Update()
    {
        if (tutorialState == TutorialState.isExplaining)
        {
        }
        if (tutorialState == TutorialState.isWaiting)
        {
            if (Input.GetMouseButtonDown(0)&&tutorialStep==TutorialStep.GameStart)
            {                
                dialogueText.text = tutoriaDialogue.text[0];
                dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[0].position;
                dialogueText.gameObject.SetActive(false);
                ChangeStep(TutorialStep.MulliganStart);
            }
        }
    }

    public void ChangeState(TutorialState tutorialState)
    {

        this.tutorialState = tutorialState;
        switch (tutorialState)
        {
            case TutorialState.isWaiting:
                Debug.Log("대기중");
                break;
            case TutorialState.isExplaining:
                Debug.Log("설명중");
                break;
        }
    }

    public void ChangeStep(TutorialStep tutorialStep)
    {
        ChangeState(TutorialState.isExplaining);
        this.tutorialStep = tutorialStep;
        switch (tutorialStep)
        {
            case TutorialStep.GameStart:                
                Debug.Log("게임 시작");
                GameStart();
                break;
            case TutorialStep.MulliganStart:
                Debug.Log("멀리건 시작");
                MulliganStart();
                break;
            case TutorialStep.MulliganEnd:
                Debug.Log("멀리건 끝");
                break;

            case TutorialStep.TowerSelect:
                Debug.Log("타워 선택");
                break;
        }
    }

    public void GameStart()
    {
        Debug.Log("Game Start");
        InGameManager.Instance.MulliganPause();
        typingCoroutine = StartCoroutine(TypeText(1, 6));
    }
    public void MulliganStart()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        cutout.SetColor("_Color", new Color(0.15f, 0.15f, 0.15f, 0.7f));
        Setcutoff(0.5f, 0.5f, 0.25f);
        dialogueText.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(7, 9));
    }

    public void Setcutoff(float x, float y, float size)
    {
        graphicRaycaster.enabled = true;
        cutoutObject.SetActive(true);
        cutout.SetVector("_Center", new Vector4(x, y, 0f, 0f));
        cutout.SetFloat("_Radius", 0.5f);

        currentTween?.Kill();

        currentTween = DOTween.To(
            () => cutout.GetFloat("_Radius"),
            r => cutout.SetFloat("_Radius", r),
            size,
            0.5f // duration
        ).SetEase(Ease.InCubic);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

    }


    private IEnumerator TypeText(int start, int end)
    {
        yield return new WaitForSeconds(0.3f);
        isTyping = true;
        fullText = "";

        for (int i = start; i <= end; i++)
        {
            fullText += tutoriaDialogue.text[i]+"\n";
        }
        Debug.Log(fullText);
        string currentText = "";

        yield return new WaitUntil(() => !Input.GetMouseButton(0));

        for (int i = 0; i < fullText.Length; i++)
        {
            if (Input.GetMouseButtonDown(0))
            {
                dialogueText.text = fullText;
                break;
            }

            currentText += fullText[i];
            dialogueText.text = currentText;

            yield return new WaitForSeconds(typingSpeed);
        }

        if (tutorialStep == TutorialStep.GameStart)
        {
            dialogueText.text += "\n"+tutoriaDialogue.text[0];
            ChangeState(TutorialState.isWaiting);
        }

        isTyping = false;
    }
}
