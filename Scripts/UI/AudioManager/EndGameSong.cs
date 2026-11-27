using UnityEngine;

public class EndGameSong : MonoBehaviour
{
    public GameObject EndGamePanel;
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
        if(EndGamePanel.activeInHierarchy && !audioSource.isPlaying)
        {
            audioSource.Play();

        } else if(!EndGamePanel.activeInHierarchy && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
