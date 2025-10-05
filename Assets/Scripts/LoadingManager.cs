using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("Asignar en el Inspector")]
    public Slider progressBar; 
    public string nextScene = "MenuInicialScene"; 
    public float minLoadTime = 2f; 
    public float maxLoadTime = 5f; 

    void Start()
    {
        if (progressBar == null)
        {
            Debug.LogError("Debes asignar el Slider en el Inspector.");
            return;
        }    
        progressBar.value = 0;
        progressBar.minValue = 0;
        progressBar.maxValue = 100;
        StartCoroutine(FakeLoading());
    }

    IEnumerator FakeLoading()
    {
        float loadTime = Random.Range(minLoadTime, maxLoadTime);
        float elapsed = 0f;
        while (elapsed < loadTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / loadTime) * 100f;
            progressBar.value = progress;
            yield return null;
        }
        progressBar.value = 100f;
        SceneManager.LoadScene(nextScene);
    }
}
