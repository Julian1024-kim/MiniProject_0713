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

    [Header("텍스트")]
    [SerializeField] TextMeshProUGUI rewardCoinText;
    [SerializeField] TextMeshProUGUI storeCoinText;

    [Header("스테이지 선택")]
    [SerializeField] WorldData worldData;
    [SerializeField] GameObject stageButtonPrefab;
    [SerializeField] Transform buttonContainer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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

            UpdateStoreCoinUI();
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
}
