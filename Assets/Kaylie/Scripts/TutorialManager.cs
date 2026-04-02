using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{



    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

  

}
