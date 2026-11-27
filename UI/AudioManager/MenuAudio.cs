using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject MainMenuPanel;


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
        if(MainMenuPanel.activeInHierarchy && !audioSource.isPlaying)
        {
            audioSource.Play();
        } else if(!MainMenuPanel.activeInHierarchy && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
