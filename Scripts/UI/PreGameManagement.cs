using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreGameManagement : MonoBehaviour
{
    public GameObject FilterBlack;
    public TextMeshProUGUI ReadyTxt;
    public GameObject PreGamePanel;
    public GameObject GamePanel;
    public GameObject PreGameManager;
    public GameObject GManager;
    public GameObject UIGManager;

    void OnEnable()
    {
        FilterBlack.SetActive(true);
        ReadyTxt.gameObject.SetActive(true);
    }

    void Update()
    {
        ReadyToStartGame();
    }

    public void ReadyToStartGame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame && PreGamePanel.activeInHierarchy)
        {
            FilterBlack.SetActive(false);
            ReadyTxt.gameObject.SetActive(false);
            PreGamePanel.SetActive(false);
            PreGameManager.SetActive(false);
            GManager.SetActive(true);
            UIGManager.SetActive(true);
            GamePanel.SetActive(true);
        }
    }
}