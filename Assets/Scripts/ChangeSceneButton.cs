using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
<<<<<<< HEAD
            Debug.LogWarning("El nombre de la escena está vacío");
=======
            Debug.LogWarning("El nombre de la escena est� vac�o");
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
