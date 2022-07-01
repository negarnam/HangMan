using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{
    [Header("Scene to load")]
    public string sceneName;
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


}
