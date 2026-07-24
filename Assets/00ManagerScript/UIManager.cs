using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("패널들")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject soundPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject clearPanel;
    [SerializeField] GameObject storePanel;
    [SerializeField] GameObject preparePanel;
    [SerializeField] Canvas UICanvas;
    [SerializeField] GameObject dim;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject stageSelectPanel;

    [Header("일반텍스트")]
    [SerializeField] TextMeshProUGUI rewardCoinText;
    [SerializeField] TextMeshProUGUI storeCoinText;
    [SerializeField] TextMeshProUGUI sunText;
    [SerializeField] TextMeshProUGUI countdownText;

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
        UpdateSunUI();
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
                OpenPausePanel();
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
        mainMenuPanel.SetActive(true);
    }
    public void OpenSelectStage()
    {
        CloseAllPanels();
        stageSelectPanel.SetActive(true);
    }
    public void OpenClearPanel(int rewardAmount)
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
            clearPanel.transform.SetAsLastSibling();
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
        if (storePanel != null)
        {
            storePanel.SetActive(true);
            dim.SetActive(true);

            dim.transform.SetAsLastSibling();
            storePanel.transform.SetAsLastSibling();

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
    private void UpdateSunUI()
    {
        if (sunText != null && SunManager.instance != null)
        {
            sunText.text = SunManager.instance.currentSun.ToString();
        }
    }
    public void UpdateCountdownText(string message)
    {
        if (countdownText != null)
        {
            countdownText.text = message;

            countdownText.transform.localScale = Vector3.one * 1.5f;
            countdownText.transform.DOScale(1f, 0.5f).SetUpdate(true);
        }
    }
    public void SetActiveCountdown(bool active)
    {
        countdownText.gameObject.SetActive(active);
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
    public void OnClickRetryButton()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Time.timeScale = 1;

        StageManager.instance.RestartCurrentStage();
    }

    public void CloseStorePanel()
    {
        if (storePanel != null)
        {
            storePanel.SetActive(false);
            dim.SetActive(false);
        }
    }

    public void OpenPausePanel()
    {

        pausePanel.SetActive(true);
        dim.SetActive(true);
        dim.transform.SetAsLastSibling();
        pausePanel.transform.SetAsLastSibling();

        Time.timeScale = 0f;
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
        dim.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSoundPanel()
    {
        soundPanel.SetActive(true);
        soundPanel.transform.SetAsLastSibling();
    }

    public void OpenGameOverPanel()
    {
            gameOverPanel.SetActive(true);
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
            gameOverPanel.transform.SetAsLastSibling();
        
    }
  
    public void CloseAllPanels()
    {
        if (clearPanel != null) clearPanel.SetActive(false);
        if (storePanel != null) storePanel.SetActive(false);
        if (preparePanel != null) preparePanel.SetActive(false);
        if (stageSelectPanel != null) stageSelectPanel.SetActive(false);
        if (pausePanel !=null) pausePanel.SetActive(false);
        if (mainMenuPanel !=null) mainMenuPanel.SetActive(false);
        if (dim!= null) dim.SetActive(false);
    }

    public void OpenPreparePanel()
    {
        CloseAllPanels();
        preparePanel.SetActive(true);
    }

    public void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("모든데이터 초기화");

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
