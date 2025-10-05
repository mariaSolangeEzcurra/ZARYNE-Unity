using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("El nombre de la escena est� vac�o");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}