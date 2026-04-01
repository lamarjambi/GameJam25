using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Timing")]
    public float waitBeforeFade = .2f;
    public float fadeDuration = 4f;

    [Header("Scene")]
    public string sceneToLoad = "GameScene";

    [Header("Fade UI")]
    public Image fadeImage; // black panel

    [Header("Audio")]
    public AudioClip jumpScareClip;
    public float volume = 1f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        // IMPORTANT: start with NO black overlay
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        // play sound immediately
        if (jumpScareClip != null)
        {
            audioSource.PlayOneShot(jumpScareClip, volume);
        }

        // wait before fade
        yield return new WaitForSeconds(waitBeforeFade);

        // fade to black
        yield return StartCoroutine(FadeToBlack());

        // load scene
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FadeToBlack()
    {
        if (fadeImage == null)
        {
            yield break;
        }

        float timer = 0f;
        Color c = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }
}