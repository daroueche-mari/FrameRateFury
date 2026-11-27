using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreGameManagement : MonoBehaviour
{
    public GameObject FilterBlack;
    public GameObject PreGamePanel;
    public GameObject GamePanel;
    public GameObject PreGameManager;
    public GameObject GManager;
    public GameObject UIGManager;
    public GameObject MyPlayer;
    public GameObject myJoy;

    void OnEnable()
    {
        FilterBlack.SetActive(true);
    }

    public void ReadyToStartGame()
    {
        if (PreGamePanel.activeInHierarchy && !GamePanel.activeInHierarchy)
        {
            FilterBlack.SetActive(false);
            PreGamePanel.SetActive(false);
            PreGameManager.SetActive(false);
            GManager.SetActive(true);
            UIGManager.SetActive(true);
            GamePanel.SetActive(true);
            MyPlayer.SetActive(true);
            myJoy.SetActive(true);
        }
    }
}