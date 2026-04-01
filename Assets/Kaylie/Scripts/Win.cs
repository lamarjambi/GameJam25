using UnityEngine;
using UnityEngine.SceneManagement;
public class Win : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip menuMusic;

    void Start()
    {
        if (audioSource != null && menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
