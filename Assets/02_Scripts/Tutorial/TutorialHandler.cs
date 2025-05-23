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

        text[0] = "Ŭ���� ���� ����.";
        text[1] = "��Ʈ���� ������ ���� �����е� ȯ���մϴ�.";
        text[2] = "�� ������ Ÿ�����潺 �����Դϴ�.";
        text[3] = "��Ʈ���� �������� ī�带 �����ϴ� �ָ��� �ý����� �ֽ��ϴ�.";
        text[4] = "�� 10������ ī���� 5���� ī�带 ������ �̹� ���ӿ��� ����մϴ�.";
        text[5] = "6 ������ �Ӽ�ī���� 3�� 4���� �Ϲ�ī���� 2���� �����ϰԵ˴ϴ�.";
        text[6] = "�ָ����� �� 3ȸ����˴ϴ�.";
        text[7] = "������ �����ϱ�����\n ī�带 �����غ���.";
        text[8] = "";
        text[9] = "3���� ī���� 2����\n ī�带 �������ּ���.";
        text[10] = "3���� ī���� 1���� ī�带 �������ּ���.";
        text[11] = "4���� ī���� 2���� ī�带 �������ּ���.";
        text[12] = "�ָ����� �������ϴ�.";
        text[13] = "���� ���� ����� ī�带 �����մϴ�.";
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
    [Header("���̶���Ʈ��")]
    public Canvas tutorialCanvas;
    private GraphicRaycaster graphicRaycaster;
    [SerializeField] private Material cutoutBaseMaterial;
    private Material cutout;
    public GameObject cutoutObject;
    private Tweener currentTween;

    [Header("���� Ÿ����")]
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    private bool isTyping = false;
    private string fullText;
    TutoriaDialogue tutoriaDialogue = new TutoriaDialogue();
    private Coroutine typingCoroutine;
    [SerializeField] public RectTransform[] dialogPos;

    [Header("����")]
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
                Debug.Log("�����");
                break;
            case TutorialState.isExplaining:
                Debug.Log("������");
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
                Debug.Log("���� ����");
                GameStart();
                break;
            case TutorialStep.MulliganStart:
                Debug.Log("�ָ��� ����");
                MulliganStart();
                break;
            case TutorialStep.MulliganEnd:
                Debug.Log("�ָ��� ��");
                break;

            case TutorialStep.TowerSelect:
                Debug.Log("Ÿ�� ����");
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
