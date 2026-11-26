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
        ValueWave.text = 15.ToString();
        ValueScore.text = 15.ToString();
        ValueEnemy.text = 15.ToString();
        PlayerNameTxt.text = "test"; // PlayerName class
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int fps = (int)(1.0f / Time.deltaTime);
        ValueFps.text = fps.ToString();
    }
}
