using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("패널들")]
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject SoundPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject ClearPanel;
    [SerializeField] GameObject StorePanel;
    [SerializeField] GameObject PreparePanel;
    [SerializeField] Canvas UICanvas;
    [SerializeField] GameObject dim;
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject StageSelectPanel;

    GameObject optionPanel;

    [Header("일반텍스트")]
    [SerializeField] TextMeshProUGUI rewardCoinText;
    [SerializeField] TextMeshProUGUI storeCoinText;

    [Header("업그레이드텍스트")]
    [SerializeField] TextMeshProUGUI atkUpgradeText;
    [SerializeField] TextMeshProUGUI prodUpgradeText;

    [Header("스테이지 선택")]
    [SerializeField] WorldData worldData;
    [SerializeField] GameObject stageButtonPrefab;
    [SerializeField] Transform buttonContainer;

    [Header("스피드")]
    public TextMeshProUGUI speedBtnText;
    private bool isFastSpeed = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        GenerateStageButtons();
        OpenMainMenu();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (optionPanel == null || !optionPanel.activeSelf)
            {
                OpenOptionPanel();
            }
        }
    }
    public void GameSpeed()
    {
        if (Time.timeScale == 0 && !isFastSpeed) return;

        isFastSpeed = !isFastSpeed;

        if( isFastSpeed)
        {
            Time.timeScale = 2;
            speedBtnText.text = "X2";
        }
        else
        {
            Time.timeScale = 1;
            speedBtnText.text = "X1";
        }

    }
    public void GenerateStageButtons() // 버튼 여러개
    {
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        for (int i = 0; i < worldData.stages.Count; i++)
        {
            int index = i;
            GameObject btnObj = Instantiate(stageButtonPrefab, buttonContainer);

            var btnText = btnObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (btnText != null) btnText.text = $"Stage {index + 1}";

            btnObj.GetComponent<Button>().onClick.AddListener(() => { OnStageButtonClicked(index); }); // 그냥쓰면 버튼생기면 바로실행됨
        }
    }
    void OnStageButtonClicked(int index)
    {
            CloseAllPanels();
        StageManager.instance.LoadAndStartStage(worldData, index);
    }
    public void OpenMainMenu()
    {
        CloseAllPanels();
        MainMenuPanel.SetActive(true);
    }
    public void OpenSelectStage()
    {
        CloseAllPanels();
        StageSelectPanel.SetActive(true);
    }
    public void OpenClearPanel(int rewardAmount)
    {
        if (ClearPanel != null)
        {
            ClearPanel.SetActive(true);
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
            ClearPanel.transform.SetAsLastSibling();
            Time.timeScale = 0f;
        }

        if(rewardCoinText != null)
        {
            rewardCoinText.text = "+0 Coin GET!!";
            int displayValue = 0;

            DOTween.To(() => displayValue, x => displayValue = x, rewardAmount, 1.5f)
            .OnUpdate(() => { rewardCoinText.text = "+" + displayValue.ToString(); }).SetUpdate(true).SetEase(Ease.OutQuad);
        }
    }

    public void OpenStorePanel()
    {
        if (StorePanel != null)
        {
            StorePanel.SetActive(true);
            dim.SetActive(true);

            dim.transform.SetAsLastSibling();
            StorePanel.transform.SetAsLastSibling();

            UpdateStoreUI();
            Time.timeScale = 0f;
        }
    }
    public void UpdateStoreCoinUI()
    {
        if(storeCoinText != null && CoinManager.instance !=null)
        {
            storeCoinText.text = CoinManager.instance.GetTotalCoins().ToString("N0");// 콤마넣기
        }
    }
    public void UpdateStoreUI()
    {
        UpdateStoreCoinUI();

        if(UpgradeManager.instance != null)
        {
            if (atkUpgradeText != null)
            {
                int atkLv = UpgradeManager.instance.atkLevel;
                int atkCost = UpgradeManager.instance.GetAtkUpgradeCost();
                atkUpgradeText.text = $"Attack (Lv.{atkLv})\n<color=#FFD700>{atkCost:N0} Coin</color>";
                Debug.Log("가격 갱신됨: " + atkCost);
            }
            if (prodUpgradeText != null)
            {
                int prodLv = UpgradeManager.instance.prodLevel;
                int prodCost = UpgradeManager.instance.GetProdUpgradeCost();
                prodUpgradeText.text = $"Prod (Lv.{prodLv})\n<color=#FFD700>{prodCost:N0} Coin</color>";
            }
        }
    }
    public void OnClickUpgradeAtk()
    {
        if(UpgradeManager.instance != null)
        {
            UpgradeManager.instance.UpgradeAtk();
            UpdateStoreUI();
        }
    }
    public void OnClickUpgradeProd()
    {
        if (UpgradeManager.instance != null)
        {
            UpgradeManager.instance.UpgradeProd();
            UpdateStoreUI();
        }
    }

    public void CloseStorePanel()
    {
        if (StorePanel != null)
        {
            StorePanel.SetActive(false);
            dim.SetActive(false);
        }
    }

    public void OpenOptionPanel()
    {
        if (optionPanel == null)
        {
            optionPanel = Instantiate(PausePanel, UICanvas.transform);
            optionPanel.SetActive(true);
        }
        else
        {
            optionPanel.SetActive(true);
        }

        dim.SetActive(true);
        dim.transform.SetAsLastSibling();
        optionPanel.transform.SetAsLastSibling();

        Time.timeScale = 0f;
    }

    public void CloseOptionPanel()
    {
        if (optionPanel != null) optionPanel.SetActive(false);
        dim.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSoundPanel()
    {
        SoundPanel.SetActive(true);
        SoundPanel.transform.SetAsLastSibling();
    }

    public void OpenGameOverPanel()
    {
        if (GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
            GameOverPanel.transform.SetAsLastSibling();
        }
    }
  
    public void CloseAllPanels()
    {
        if (ClearPanel != null) ClearPanel.SetActive(false);
        if (StorePanel != null) StorePanel.SetActive(false);
        if (PreparePanel != null) PreparePanel.SetActive(false);
        if (StageSelectPanel != null) StageSelectPanel.SetActive(false);
        if (MainMenuPanel !=null) MainMenuPanel.SetActive(false);
    }

    public void OpenPreparePanel()
    {
        PreparePanel.SetActive(true);
        ClearPanel.SetActive(false);
        StorePanel.SetActive(false);
        dim.SetActive(false);
    }

    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("모든데이터 초기화");

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
