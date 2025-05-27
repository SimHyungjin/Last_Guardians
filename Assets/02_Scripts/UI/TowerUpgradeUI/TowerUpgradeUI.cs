using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [Header("데이터")]
    public TowerUpgrade towerUpgrade;
    [SerializeField] private TextMeshProUGUI currentMasteryPoint;

    [Header("설명창")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI descriptionTitleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("업그레이드 버튼")]
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<TextMeshProUGUI> level;
    [SerializeField] private Image fillBox;

    [Header("리셋 버튼")]
    bool isResetPanelActive = false;
    [SerializeField] private Button resetEntryButton;
    [SerializeField] private Image resetPannel;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button cancleButton;

    private float buttonHoldTime = 0f;
    private bool isButtonHeld = false;
    private int currentButtonIndex = -1;

    private void Awake()
    {
        currentMasteryPoint.text = towerUpgrade.towerUpgradeData.currentMasteryPoint.ToString();
        resetEntryButton.onClick.AddListener(OnResetEntryButton);
        cancleButton.onClick.AddListener(OnResetCancelButton);
        resetButton.onClick.AddListener(OnResetButton);
        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            int index = i;
            TowerUpgradeButtonHandler handler = button.GetComponent<TowerUpgradeButtonHandler>();
            if (handler != null)
            {
                handler.OnButtonHeld += HandleButtonHeld;
            }
        }
        UpdateButtons();
    }
    private void OnEnable()
    {
        isResetPanelActive = false;
        resetPannel.gameObject.SetActive(false);
        UpdateButtons();
    }
    private void Update()
    {
        if (!isResetPanelActive)
        {
            if (isButtonHeld)
            {
                buttonHoldTime += Time.deltaTime;
                if (buttonHoldTime >= 0.5f && currentButtonIndex != -1)
                {
                    if (towerUpgrade.CanUpgrade((TowerUpgradeType)currentButtonIndex))
                    {
                        float duration = 1f;
                        fillBox.gameObject.SetActive(true);
                        fillBox.transform.position = buttons[currentButtonIndex].transform.position;
                        fillBox.fillAmount += (Time.deltaTime / duration);
                        if (fillBox.fillAmount >= 1f)
                        {
                            towerUpgrade.Upgrade((TowerUpgradeType)currentButtonIndex);
                            UpgradeCompleate();
                        }
                    }
                }
            }
        }
    }

    private void UpdateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            int index = i;
            TowerUpgradeType type = (TowerUpgradeType)i;
            if (IsTier1(type))
            {
                SetButtonActive(button, true);
            }
            else if (towerUpgrade.towerUpgradeData.SumOfLevelUP >= 10 && IsTier2(type))
            {
                SetButtonActive(button, true);
            }
            else if (towerUpgrade.towerUpgradeData.SumOfLevelUP >= 20 && IsTier3(type))
            {
                SetButtonActive(button, true);
            }
            else
            {
                SetButtonInactive(button);
            }
            UpdateButtonColor(button, i);
            currentMasteryPoint.text = towerUpgrade.towerUpgradeData.currentMasteryPoint.ToString();
        }
    }
    private void AllInActive()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            button.interactable = false;

        }
    }

    private void HandleButtonHeld(Button button, bool isHeld)
    {

        if (isHeld)
        {
            isButtonHeld = true;
            currentButtonIndex = buttons.IndexOf(button);
            ShowDescription(currentButtonIndex);

        }
        else
        {
            isButtonHeld = false;
            buttonHoldTime = 0f;
            currentButtonIndex = -1;
            fillBox.fillAmount = 0f;
            descriptionPanel.SetActive(false);
        }
    }

    private void UpgradeCompleate()
    {
        isButtonHeld = false;
        UpdateButtons();
        fillBox.fillAmount = 0f;
        descriptionPanel.SetActive(false);
    }

    private bool IsTier1(TowerUpgradeType type)
    {
        return type == TowerUpgradeType.AttackPower ||
               type == TowerUpgradeType.AttackSpeed ||
               type == TowerUpgradeType.AttackRange ||
               type == TowerUpgradeType.EffectValue ||
               type == TowerUpgradeType.EffectDuration ||
               type == TowerUpgradeType.EffectRange;
    }
    private bool IsTier2(TowerUpgradeType type)
    {
        return type == TowerUpgradeType.Penetration ||
               type == TowerUpgradeType.ContinuousAttack ||
               type == TowerUpgradeType.Catalysis ||
               type == TowerUpgradeType.EffectTransfer;
    }

    private bool IsTier3(TowerUpgradeType type)
    {
        return type == TowerUpgradeType.CombetMastery ||
               type == TowerUpgradeType.MultipleAttack ||
               type == TowerUpgradeType.BossSlayer ||
               type == TowerUpgradeType.Emergencyresponse;
    }

    private void SetButtonActive(Button button, bool isActive)
    {
        button.interactable = isActive;
        if (button.GetComponent<Image>() != null)
        {
            button.GetComponent<Image>().color = Color.gray;
        }
    }

    private void SetButtonInactive(Button button)
    {
        button.interactable = false;
        if (button.GetComponent<Image>() != null)
        {
            button.GetComponent<Image>().color = Color.black;
        }
    }

    private void UpdateButtonColor(Button button, int index)
    {
        if (towerUpgrade.towerUpgradeData.currentLevel[index] >= 1)
        {
            button.GetComponent<Image>().color = Color.white;
            level[index].gameObject.SetActive(true);
            String levelText = "Lv.  " + towerUpgrade.towerUpgradeData.currentLevel[index].ToString();
            level[index].text = levelText;
        }
        else
        {
            //button.GetComponent<Image>().color = Color.gray;
            level[index].gameObject.SetActive(false);
        }
    }

    public void ShowDescription(int index)
    {
        TowerUpgradeType type = (TowerUpgradeType)index;
        descriptionTitleText.text = type.ToString();
        descriptionText.text = towerUpgrade.towerUpgradeData.description[index];

        descriptionPanel.transform.SetParent(buttons[index].transform);
        descriptionPanel.transform.localPosition = new Vector3(0, 150, 0);
        descriptionPanel.SetActive(true);
    }


    public void OnResetEntryButton()
    {
        isResetPanelActive = true;
        resetPannel.gameObject.SetActive(true);
        AllInActive();
    }

    public void OnResetCancelButton()
    {
        isResetPanelActive = false;
        resetPannel.gameObject.SetActive(false);
        UpdateButtons();
    }

    public void OnResetButton()
    {
        isResetPanelActive = false;
        resetPannel.gameObject.SetActive(false);
        towerUpgrade.Reset();
        UpdateButtons();
    }
}

