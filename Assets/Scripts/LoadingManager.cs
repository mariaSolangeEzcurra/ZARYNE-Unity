using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // Arrastra el Slider en el Inspector
    public string nextScene = "MenuInicialScene"; // Escena destino
    public float minLoadTime = 3f; // Tiempo mínimo de carga simulada
    public float maxLoadTime = 5f; // Tiempo máximo de carga simulada

    void Start()
    {
        if (progressBar == null)
        {
            Debug.LogError("Debes asignar el Slider en el Inspector.");
            return;
        }
        StartCoroutine(FakeLoading());
    }

    IEnumerator FakeLoading()
    {       
        float loadTime = Random.Range(minLoadTime, maxLoadTime);
        float elapsed = 0f;
        while (elapsed < loadTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / loadTime);
            progressBar.value = progress;
            yield return null;
        }
        // Cuando la barra llega al 100%, carga la escena
        SceneManager.LoadScene(nextScene);
    }
}
