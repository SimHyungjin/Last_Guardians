using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cinemachine;

public class TutoriaDialogue
{
    public string[] text;
    public void init()
    {
        text = new string[20];

        text[0] = "Ŭ���� ���� ����.";
        text[1] = "��Ʈ���� ������ ���� �����е� ȯ���մϴ�.\n" +
            "��Ʈ���� �������� ī�带 �����ϴ� �ָ��� �ý����� �ֽ��ϴ�.\n" +
            "�� 10������ ī���� 5���� ī�带 ������ �̹� ���ӿ��� ����մϴ�.\n" +
            "�ָ����� �� 3ȸ����˴ϴ�.";
        text[2] = "������ �����ϱ�����\n ī�带 �����غ���.\n\n" +
            "3���� ī���� 2����\n ī�带 �������ּ���.";
        text[3] = "ī�带 �����߽��ϴ�.\n�ϷḦ ���� ���� ������ �����ϰų�\n ī�带 �ٽ� ������ �� �ֽ��ϴ�.";
        text[4] = "ù�ָ����� �������ϴ�.\n 3���� ī���� 1���� �������ּ���.";
        text[5] = "�����������Դϴ�.\n �Ϲ�ī�� 4�� �� 2���� ����ּ���";
        text[6] = "������ �ָ����� �����մϴ�.\n ���� �ȿ����� ���ݱ��� ������\n 5������ Ÿ���� ��� �����մϴ�.";
        text[7] = "���� ����ϴ� ī�带 ���ϴ�.\n" +
            "ī�带 ����Ͽ� Ÿ���� ��ġ�մϴ�.\n" +
            "���������� �ϳ��� ī�带 ȹ���Ҽ��ֽ��ϴ�\n\n" +
            "�ʱ� ī�� ������ �����մϴ�.";
        text[8] = "ī�带 �����߽��ϴ�\n" +
            "�缱���� �ϰų� ��ư��\n " +
            "���� ������ �������ּ���.";
        text[9] = "���̺갡 ���۵Ǿ����ϴ�.\n" +
            "Ȧ�� ���̺꿡�� ����\n";
        text[10] = "¦�� ���̺꿡����\n �����ʿ��� ����˴ϴ�.";
        text[11] = "�ϴ��� ī�带 �����Ͽ�\n Ÿ���� ��ġ�Ҽ� �ֽ��ϴ�.\n\n\n\n" +
            "ī�带 ������ �巡���غ�����";
        text[12] = "�巡���� ī�带 ��ġ�� ��ġ�� �����ּ���.\n" +
            "Ÿ���� ��ġ�ϸ� �ش� Ÿ���� �����˴ϴ�.\n\n"+
            "�ѹ� ��ġ�� ī��� ���ġ�� �Ұ����մϴ�.\n"+
            "�����ϰ� ��ġ���ּ���";
        text[13] = "�ʹ� �ָ��� Ʃ�丮���� �����մϴ�.\n" +
            "�߰� ���� �� �ռ����� �������� �Ұ��մϴ�.";
    }
}

public enum TutorialState
{
    isWaiting,
    isExplaining,
    Roading
}
public enum TutorialStep
{
    None,
    TutorialStart,
    GameStart,
    MulliganStart,
    MulliganSelect,
    MulliganEnd,
    CardSelect,
    WaveStart,
    WaveStepTwo,
    DescriptionCard,
    Construct,
    TutorialEnd
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
    public int MulligunStep;

    [Header("ī�޶�")]
    public CinemachineVirtualCamera virtualCamera;
    float currentSize;

    [Header("��ŵ")]
    public GameObject skipPopup;
    public Button skipPopupButton;
    public Button skipButton;
    public Button cancleButton;

    public Image TutorialPannel;

    private void Awake()
    {
        cutout= new Material(cutoutBaseMaterial);
        TutorialPannel.gameObject.SetActive(false);
        cutoutObject.GetComponent<Image>().material = cutout;
        cutoutObject.SetActive(true);
        graphicRaycaster= tutorialCanvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.enabled = true;
        MulligunStep = 0;
        tutoriaDialogue.init();

        skipPopup.SetActive(true);
        skipPopupButton.onClick.AddListener(SkipPopup);
        cancleButton.onClick.AddListener(SkipPopup);
        skipButton.onClick.AddListener(SkipTutorial);
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
                dialogueText.text = "";
                dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[0].position;
                dialogueText.gameObject.SetActive(false);
                ChangeStep(TutorialStep.MulliganStart);
            }
            if(isActiveAndEnabled && Input.GetMouseButtonDown(0) && tutorialStep == TutorialStep.TutorialEnd)
            {
                PlayerPrefs.SetInt("InGameTutorial", 1);
                tutorialState = TutorialState.Roading;
                SceneLoader.LoadSceneWithFade("GameScene", true);
            }
        }
    }

    public void ChangeState(TutorialState tutorialState)
    {

        this.tutorialState = tutorialState;
        switch (tutorialState)
        {
            case TutorialState.isWaiting:
                break;
            case TutorialState.isExplaining:
                break;
        }
    }

    public void ChangeStep(TutorialStep tutorialStep)
    {
        if (this.tutorialStep == tutorialStep) return;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        dialogueText.text = "";
        ChangeState(TutorialState.isExplaining);
        this.tutorialStep = tutorialStep;
        switch (tutorialStep)
        {
            case TutorialStep.TutorialStart:
                break;
            case TutorialStep.GameStart:
                GameStart();
                break;
            case TutorialStep.MulliganStart:
                MulliganStart();
                break;
            case TutorialStep.MulliganSelect:
                MulliganSelect();
                break;
            case TutorialStep.MulliganEnd:
                MulliganEnd();
                break;

            case TutorialStep.CardSelect:
                CardSelection();
                break;

            case TutorialStep.WaveStart:
                WaveStart();
                break;
            case TutorialStep.WaveStepTwo:
                WaveStepTwo();
                break;
            case TutorialStep.DescriptionCard:
                DescriptionCard();
                break;
            case TutorialStep.Construct:
                Construct();
                break;

        }
    }
    ////////////////////////////////////����//////////////////////////////////////////////
    public void GameStart()
    {
        InGameManager.Instance.MulliganPause();
        typingCoroutine = StartCoroutine(TypeText(1));
    }

    ///////////////////////////////////�ָ���//////////////////////////////////////////////
    public void MulliganStart()
    {
        cutout.SetColor("_Color", new Color(0.15f, 0.15f, 0.15f, 0.7f));
        Setcutoff(0.5f, 0.5f, 0.25f);
        dialogueText.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(2));
    }

    public void MulliganSelect()
    {
        if (MulligunStep==0)
        {
            dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[1].position;
            typingCoroutine = StartCoroutine(TypeText(3));
        }
        else if(MulligunStep==1)
        {
            dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[1].position;
            typingCoroutine = StartCoroutine(TypeText(3));
        }
        else if(MulligunStep==2)
        {
            graphicRaycaster.enabled = true;
            dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[1].position;
            typingCoroutine = StartCoroutine(TypeText(6));
            MulligunStep++;
        }
        else
        {
            dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[1].position;
            typingCoroutine = StartCoroutine(TypeText(8));
            MulligunStep++;
        }
    }
    public void MulliganEnd()
    {
        if (MulligunStep == 0)
        {
            MulligunStep++;
            graphicRaycaster.enabled = true;
            dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[0].position;
            typingCoroutine = StartCoroutine(TypeText(4));
        }
        else if (MulligunStep == 1)
        {
            MulligunStep++;
            graphicRaycaster.enabled = true;
            dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[0].position;
            typingCoroutine = StartCoroutine(TypeText(5));
        }
    }

    ////////////////////////////////////ī�弳����/////////////////////////////////////////////
    public void CardSelection()
    {
        graphicRaycaster.enabled = true;
        dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[0].position;
        typingCoroutine = StartCoroutine(TypeText(7));
    }


    ////////////////////////////////////���̺����//////////////////////////////////////////////
    public void WaveStart()
    {
        cutout.SetColor("_Color", new Color(0.1f, 0.1f, 0.1f, 0.7f));

        Time.timeScale = 0f;
        graphicRaycaster.enabled = true;
        TowerManager.Instance.StartInteraction(InteractionState.Pause);

        currentSize = virtualCamera.m_Lens.OrthographicSize;
        virtualCamera.m_Lens.OrthographicSize = 5f;

        Setcutoff(0.1f, 0.5f, 0.1f);

        dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[2].position;
        typingCoroutine = StartCoroutine(TypeText(9));
    }
    public void WaveStepTwo()
    {
        Setcutoff(0.9f, 0.5f, 0.1f);
        typingCoroutine = StartCoroutine(TypeText(10));
    }

    public void DescriptionCard()
    {
        Setcutoff(0.5f, 0.3f, 0.1f);
        typingCoroutine = StartCoroutine(TypeText(11));
    }

    ////////////////////////////////////�Ǽ�//////////////////////////////////////////////
    public void DragCard()
    {
        Setcutoff(0.3f, 0.5f, 0.1f);
        typingCoroutine = StartCoroutine(TypeText(12));
    }

    public void Construct()
    {
        Setcutoff(0.5f, 0.5f, 2f);
        dialogueText.text = "";
        Time.timeScale = InGameManager.Instance.TimeScale;
        virtualCamera.m_Lens.OrthographicSize = currentSize;
        Invoke("TutorialEnd",3f);
    }
    public void TutorialEnd()
    {
        ChangeStep(TutorialStep.TutorialEnd);
        foreach (var tower in TowerManager.Instance.Towers)
        {
            if (tower is AttackTower)
            {
                tower.OnDisabled();
            }
        }
        MonsterManager.Instance.StopAllCoroutines();
        TowerManager.Instance.StartInteraction(InteractionState.Pause);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        cutout.SetColor("_Color", new Color(0f, 0f, 0f, 1f));
        Setcutoff(0.5f, 0.5f, 0f);
        typingCoroutine = StartCoroutine(TypeText(13));
    }
    ////////////////////////////////////������ �޼���//////////////////////////////////////////////

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
        ).SetEase(Ease.InCubic).SetUpdate(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

    }

    private IEnumerator TypeText(int dialogNum)
    {
        float startTime = Time.realtimeSinceStartup;
        float elapsedTime = 0f;

        isTyping = true;
        fullText = "";
        fullText += tutoriaDialogue.text[dialogNum];
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

            elapsedTime = Time.realtimeSinceStartup - startTime;

            while (elapsedTime < typingSpeed)
            {
                yield return null;
                elapsedTime = Time.realtimeSinceStartup - startTime;
            }

            startTime = Time.realtimeSinceStartup;
        }

        // �� �ܰ躰 �ļ� ó��
        switch (tutorialStep)
        {
            case TutorialStep.GameStart:
                dialogueText.text += "\n" + tutoriaDialogue.text[0];
                ChangeState(TutorialState.isWaiting);
                break;
            case TutorialStep.MulliganStart:
            case TutorialStep.MulliganSelect:
            case TutorialStep.MulliganEnd:
                graphicRaycaster.enabled = false;
                break;
            case TutorialStep.CardSelect:
                graphicRaycaster.enabled = false;
                break;
            case TutorialStep.WaveStart:
                ChangeStep(TutorialStep.WaveStepTwo);
                break;
            case TutorialStep.WaveStepTwo:
                ChangeStep(TutorialStep.DescriptionCard);
                break;
            case TutorialStep.DescriptionCard:
                ChangeState(TutorialState.isWaiting);
                TowerManager.Instance.EndInteraction(InteractionState.Pause);
                DragCard();
                break;
            case TutorialStep.TutorialEnd:
                ChangeState(TutorialState.isWaiting);
                TutorialPannel.gameObject.SetActive(true);
                break;
        }

        isTyping = false;
    }

   //////////////////////////////////////��ŵ////////////////////////////////////////
   public void SkipPopup()
    {
        skipPopup.SetActive(!skipPopup.activeSelf);
        if (skipPopup.activeSelf)
        {
            skipPopupButton.enabled = false;
            cancleButton.enabled = true;
        }
        else
        {
            ChangeStep(TutorialStep.TutorialStart);
            cancleButton.enabled = false;
            skipPopupButton.enabled = true;
        }

        if (tutorialStep == TutorialStep.TutorialStart)
        { 
            ChangeStep(TutorialStep.GameStart);
            GameStart(); 
        }
    }
   
    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("InGameTutorial", 1);
        tutorialState = TutorialState.Roading;
        SceneLoader.LoadSceneWithFade("GameScene", true);
    }


    private void OnDisable()
    {
        if(typingCoroutine!=null)
        StopCoroutine(typingCoroutine);
    }
   
}
