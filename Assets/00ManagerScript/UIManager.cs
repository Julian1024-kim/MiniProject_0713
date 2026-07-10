using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject SoundPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject ClearPanel;
    [SerializeField] GameObject StorePanel;
    [SerializeField] Canvas UICanvas;
    [SerializeField] GameObject dim;

    GameObject optionPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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
    public void OpenClearPanel()
    {
        if (ClearPanel != null)
        {
            ClearPanel.SetActive(true);
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
            ClearPanel.transform.SetAsLastSibling();
            Time.timeScale = 0f;
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

            Time.timeScale = 0f;
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
    public void OpenStoreFromClear()
    {
        ClearPanel.SetActive(false); // 결과창 끄고
        OpenStorePanel();            // 상점창 열기
    }
    public void CloseAllPanels()
    {
        if (ClearPanel != null) ClearPanel.SetActive(false);
        if (StorePanel != null) StorePanel.SetActive(false);
        dim.SetActive(false);
    }
}
