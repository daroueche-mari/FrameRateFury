using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.InputSystem;


public class PreGameManagement : MonoBehaviour
{
    public GameObject FilterBlack;
    public TextMeshProUGUI ReadyTxt;
    public GameObject PreGamePanel;
    public GameObject GamePanel;
    public GameObject PreGameManager;

    
    void OnEnable()
    {
        FilterBlack.SetActive(true);
        ReadyTxt.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ReadyToStartGame();

    }

    public void ReadyToStartGame()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame && PreGamePanel.activeInHierarchy)
        {
            // add logic launch the game
            FilterBlack.SetActive(false);
            ReadyTxt.gameObject.SetActive(false);
            PreGamePanel.SetActive(false);
            PreGameManager.SetActive(false);
            GamePanel.SetActive(true);
        }
    }





}
