using TMPro;
using UnityEngine;

public class UIGameManagement : MonoBehaviour
{
    public TextMeshProUGUI ValueFps;
    public TextMeshProUGUI ValueWave;
    public TextMeshProUGUI ValueScore;
    public TextMeshProUGUI ValueEnemy;
    public TextMeshProUGUI PlayerNameTxt;

    private void OnEnable()
    {
        ValueWave.text = "VAGUE : " + 1.ToString();
        ValueScore.text = "SCORE : " + 0.ToString();
        ValueEnemy.text = "ENEMY : " + 0.ToString();
        PlayerNameTxt.text = MyCharacter.mylistChar[0].Name;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledDeltaTime > 0f)
        {
            int fps = (int)(1.0f / Time.unscaledDeltaTime);
            ValueFps.text = "FPS : " + fps.ToString();
        }
    }
}
