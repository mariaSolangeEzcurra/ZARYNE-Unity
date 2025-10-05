using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
<<<<<<< HEAD
            Debug.LogWarning("El nombre de la escena estÃ¡ vacÃ­o");
=======
            Debug.LogWarning("El nombre de la escena está vacío");
>>>>>>> Geraldine
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> Geraldine
