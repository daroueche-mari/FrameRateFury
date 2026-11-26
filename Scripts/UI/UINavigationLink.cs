using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UINavigationLink : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject PreGamePanel;
    public GameObject ScoreBoard;
    public GameObject LoadSaveBoard;
    public GameObject PausePanel;
    public GameObject EndGamePanel;

    public GameObject GamePanel;
    public TextMeshProUGUI IndicatorTxt;
    public TextMeshProUGUI TitleTxt;
    public Button PlayBtnMenu;
    public Button LoadBtnMenu;
    public Button ScoreBtnMenu;
    public Button QuitBtnMenu;
    public Button CloseScoreBtn;
    public Button SaveOneBtn;
    public Button SaveTwoBtn;
    public Button CloseLoadBoardBtn;
    public Button ResumeBtn;
    public Button RestartBtn;
    public Button SaveQuitBtn;
    public Button RetryBtn;
    public Button SaveQuitEndBtn;
    public Button PauseBtn;
    public Button ClosePauseBtn;
    public GameObject PreGameManager;
    public GameObject GManager;
    public GameObject UIGManager;
  

    //----------------------------------------------------------------------------

    // State Main Menu 
    public void PlayBtnAction()
    {
        if (!PreGamePanel.activeInHierarchy && MainMenuPanel.activeInHierarchy)
        {
            MainMenuPanel.SetActive(false);
            PreGameManager.SetActive(false);
            PreGameManager.SetActive(true);
            PreGamePanel.SetActive(true);
        }
        
    }
    public void LoadBtnAction()
    {
        if(!LoadSaveBoard.activeInHierarchy && MainMenuPanel.activeInHierarchy)
        {
            LoadSaveBoard.SetActive(true);
        }
        
    }
    public void CloseLoadGameBtnAction()
    {
        if (LoadSaveBoard.activeInHierarchy)
        {
            LoadSaveBoard.SetActive(false);
        }
    }
    
    public void ScoreBtnAction()
    {
        if (!ScoreBoard.activeInHierarchy)
        {
            ScoreBoard.SetActive(true);
        }
    }

    public void CloseScoreBtnAction()
    {
        if (ScoreBoard.activeInHierarchy)
        {
            ScoreBoard.SetActive(false);
        }
    }

    public void QuitBtnAction()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
            }
    public void LoadSaveSlot01()
    {
        // Chargement slot01 + logic of loading of save
    }

    public void LoadSaveSlot02()
    {
        // Chargement slot02 + logic of loading of save
    }


    //----------------------------------------------------------------------------

    // State In Game 
    public void ResumeGame()
    {
        if (GamePanel.activeInHierarchy && PausePanel.activeInHierarchy)
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void RestartGame()
    {
        if (GamePanel.activeInHierarchy && PausePanel.activeInHierarchy)
        {
            PausePanel.SetActive(false);
            GamePanel.SetActive(false);
            GManager.SetActive(false);
            PreGameManager.SetActive(false);
            PreGameManager.SetActive(true);
            PreGamePanel.SetActive(true);
        }
    }

    public void SaveQuitGame()
    {
        if (GamePanel.activeInHierarchy && PausePanel.activeInHierarchy)
        {
            // SaveGame();
            PausePanel.SetActive(false);
            GamePanel.SetActive(false);
            GManager.SetActive(false);
            UIGManager.SetActive(false);
            MainMenuPanel.SetActive(true);
        }
    }



    public void OpenPauseMenu()
    {
        if (!PausePanel.activeInHierarchy)
        {
            Time.timeScale = 0f;
            PausePanel.SetActive(true);
        }
    }

    public void ClosePauseMenu()
    {
        if (PausePanel.activeInHierarchy)
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //----------------------------------------------------------------------------
    // State End Game
    public void RetryGame()
    {
        if (EndGamePanel.activeInHierarchy)
        {
            EndGamePanel.SetActive(false);
            GamePanel.SetActive(false);
            PreGameManager.SetActive(false);
            PreGameManager.SetActive(true);
            PreGamePanel.SetActive(true);
        }
    }

    public void SaveQuitEnd()
    {
        // SaveGame();
        EndGamePanel.SetActive(false);
        UIGManager.SetActive(false);
        GManager .SetActive(false);
        MainMenuPanel.SetActive(true);
    }


}
