using UnityEngine;

public class InGameSong : MonoBehaviour
{
    public GameObject GamePanel;
    public AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        StartMusic();
    }

    public void StartMusic()
    {
        if(GamePanel.activeInHierarchy && !audioSource.isPlaying)
        {
            audioSource.Play();

        } else if(!GamePanel.activeInHierarchy && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
