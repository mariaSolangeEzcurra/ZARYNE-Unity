using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioScena : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("Cargando escena: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadPreviousScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int previousIndex = currentIndex - 1;

        if (previousIndex >= 0)
        {
            Debug.Log("Regresando a escena anterior: " + previousIndex);
            SceneManager.LoadScene(previousIndex);
        }
        else
        {
            Debug.Log("No hay escena anterior en Build Settings");
        }
    }
}
