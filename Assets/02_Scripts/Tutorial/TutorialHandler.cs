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

        text[0] = "클릭을 눌러 진행.";
        text[1] = "라스트오브 가디언즈에 오신 여러분들 환영합니다.\n" +
            "라스트오브 가디언즈는 카드를 선택하는 멀리건 시스템이 있습니다.\n" +
            "총 10종류의 카드중 5장의 카드를 선택해 이번 게임에서 사용합니다.\n" +
            "멀리건은 총 3회진행됩니다.";
        text[2] = "게임을 시작하기전에\n 카드를 선택해보죠.\n\n" +
            "3장의 카드중 2장의\n 카드를 선택해주세요.";
        text[3] = "카드를 선택했습니다.\n완료를 눌러 다음 선택을 진행하거나\n 카드를 다시 선택할 수 있습니다.";
        text[4] = "첫멀리건이 끝났습니다.\n 3장의 카드중 1장을 선택해주세요.";
        text[5] = "마지막선택입니다.\n 일반카드 4장 중 2장을 골라주세요";
        text[6] = "마지막 멀리건을 종료합니다.\n 게임 안에서는 지금까지 선택한\n 5종류의 타워만 사용 가능합니다.";
        text[7] = "실제 사용하는 카드를 고릅니다.\n" +
            "카드를 사용하여 타워를 설치합니다.\n" +
            "레벨업마다 하나의 카드를 획득할수있습니다\n\n" +
            "초기 카드 두장을 선택합니다.";
        text[8] = "카드를 선택했습니다\n" +
            "재선택을 하거나 버튼을\n " +
            "눌러 게임을 진행해주세요.";
        text[9] = "웨이브가 시작되었습니다.\n" +
            "홀수 웨이브에는 왼쪽\n";
        text[10] = "짝수 웨이브에서는\n 오른쪽에서 실행됩니다.";
        text[11] = "하단의 카드를 선택하여\n 타워를 설치할수 있습니다.\n\n\n\n" +
            "카드를 선택해 드래그해보세요";
        text[12] = "드래그한 카드를 설치할 위치에 놓아주세요.\n" +
            "타워를 설치하면 해당 타워가 생성됩니다.\n\n"+
            "한번 설치한 카드는 재배치가 불가능합니다.\n"+
            "신중하게 설치해주세요";
        text[13] = "초반 멀리건 튜토리얼을 종료합니다.\n" +
            "추가 조작 및 합성법은 사진으로 소개합니다.";
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
    public int MulligunStep;

    [Header("카메라")]
    public CinemachineVirtualCamera virtualCamera;
    float currentSize;

    [Header("스킵")]
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
    ////////////////////////////////////시작//////////////////////////////////////////////
    public void GameStart()
    {
        InGameManager.Instance.MulliganPause();
        typingCoroutine = StartCoroutine(TypeText(1));
    }

    ///////////////////////////////////멀리건//////////////////////////////////////////////
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

    ////////////////////////////////////카드설렉션/////////////////////////////////////////////
    public void CardSelection()
    {
        graphicRaycaster.enabled = true;
        dialogueText.gameObject.GetComponent<RectTransform>().position = dialogPos[0].position;
        typingCoroutine = StartCoroutine(TypeText(7));
    }


    ////////////////////////////////////웨이브시작//////////////////////////////////////////////
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

    ////////////////////////////////////건설//////////////////////////////////////////////
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
    ////////////////////////////////////공통사용 메서드//////////////////////////////////////////////

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

        // 각 단계별 후속 처리
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

   //////////////////////////////////////스킵////////////////////////////////////////
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
