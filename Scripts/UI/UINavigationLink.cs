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
    public GameObject PauseBox;
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
    public GameObject PreGameManager;
    public GameObject GManager;
    public GameObject UIGManager;
    public GameObject NewGameBox;
    public Button ValidNewGameBtnOne;
    public Button ValidNewGameBtnTwo;
    public GameObject BgSpace;
    public Button BackFromPreGameBtn;
    public TMP_InputField InputNewGameOne;
    public TMP_InputField InputNewGameTwo;
    public TextMeshProUGUI NamePlayerInGame;
    public GameObject SaveManager;

    //----------------------------------------------------------------------------

    // Main Menu 
    public void PlayBtnAction()
    {
        if (!NewGameBox.activeInHierarchy && MainMenuPanel.activeInHierarchy)
        {
            NewGameBox.SetActive(true);
        }
    }

    //----------------------------------------------------------------------------

    public void CloseNewGameBoxBtnAction()
    {
        if (NewGameBox.activeInHierarchy)
        {
            NewGameBox.SetActive(false);
        }
    }

    public void LaunchNewGameOneAction()
    
    {
        if(InputNewGameOne.text != null)
        {
            MyCharacter.mylistChar[0].Name = InputNewGameOne.text;
        }
        NewGameBox.SetActive(false);
        MainMenuPanel.SetActive(false);
        PreGameManager.SetActive(false);
        PreGameManager.SetActive(true);
        BgSpace.SetActive(true);
        PreGamePanel.SetActive(true);
    }

    public void LaunchNewGameTwoAction()
    {
        if (InputNewGameTwo.text != null)
        {
            MyCharacter.mylistChar[1].Name = InputNewGameTwo.text;
        }
        NewGameBox.SetActive(false);
        MainMenuPanel.SetActive(false);
        PreGameManager.SetActive(false);
        PreGameManager.SetActive(true);
        BgSpace.SetActive(true);
        PreGamePanel.SetActive(true);
    }

    //----------------------------------------------------------------------------

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

    public void LoadSaveSlot01()
    {
        SaveManager.SetActive(false);
        SaveManager.SetActive(true);
        SaveManagement.LoadGame("1");
        LoadSaveBoard.SetActive(false);
        MainMenuPanel.SetActive(false);
        PreGameManager.SetActive(false);
        PreGameManager.SetActive(true);
        BgSpace.SetActive(true);
        PreGamePanel.SetActive(true);

    }

    public void LoadSaveSlot02()
    {
        SaveManager.SetActive(false);
        SaveManager.SetActive(true);
        SaveManagement.LoadGame("2");
        LoadSaveBoard.SetActive(false);
        MainMenuPanel.SetActive(false);
        PreGameManager.SetActive(false);
        PreGameManager.SetActive(true);
        BgSpace.SetActive(true);
        PreGamePanel.SetActive(true);
    }


    //----------------------------------------------------------------------------

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
    //----------------------------------------------------------------------------

    public void QuitBtnAction()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
            }
 

    //----------------------------------------------------------------------------


    // In PreGame


    public void BackFromPreGameActionBtn()
    {
        PreGameManager.SetActive(false);
        BgSpace.SetActive(false);
        PreGamePanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    //----------------------------------------------------------------------------


    // In Game & Pause Game
 

    // Pause Game
    public void ResumeGame()
    {
        if (GamePanel.activeInHierarchy && PauseBox.activeInHierarchy)
        {
            PauseBox.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void RestartGame()
    {
        if (GamePanel.activeInHierarchy && PauseBox.activeInHierarchy)
        {
            PauseBox.SetActive(false);
            GamePanel.SetActive(false);
            GManager.SetActive(false);
            PreGameManager.SetActive(false);
            PreGameManager.SetActive(true);
            PreGamePanel.SetActive(true);
        }
    }

    public void SaveQuitGame()
    {
        if (GamePanel.activeInHierarchy && PauseBox.activeInHierarchy)
        {
            SaveManager.SetActive(false);
            SaveManager.SetActive(true);
            if(NamePlayerInGame.text == MyCharacter.mylistChar[0].Name)
            {
                SaveManagement.SaveGame("1");
            } else if(NamePlayerInGame.text == MyCharacter.mylistChar[1].Name)
            {
                SaveManagement.SaveGame("2");
            }
            PauseBox.SetActive(false);
            GamePanel.SetActive(false);
            GManager.SetActive(false);
            UIGManager.SetActive(false);
            MainMenuPanel.SetActive(true);
        }
    }

    //----------------------------------------------------------------------------

    public void OpenPauseMenu()
    {
        if (!PauseBox.activeInHierarchy)
        {
            Time.timeScale = 0f;
            PauseBox.SetActive(true);
        }
    }

    public void ClosePauseMenu()
    {
        if (PauseBox.activeInHierarchy)
        {
            PauseBox.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //----------------------------------------------------------------------------
    // End Game
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
        SaveManager.SetActive(false);
        SaveManager.SetActive(true);
        if (NamePlayerInGame.text == MyCharacter.mylistChar[0].Name)
        {
            SaveManagement.SaveGame("1");
        }
        else if (NamePlayerInGame.text == MyCharacter.mylistChar[1].Name)
        {
            SaveManagement.SaveGame("2");
        }
        EndGamePanel.SetActive(false);
        UIGManager.SetActive(false);
        GManager .SetActive(false);
        MainMenuPanel.SetActive(true);
    }


}
