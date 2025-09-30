using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textoUI;    
    public float velocidad = 0.05f;
    public float pausaEntreTextos = 1.0f;
    public Image[] imagenes;
    public GameObject botonContinuar;
    private string textoCompleto;

    void Start()
    {       
        textoCompleto = textoUI.text;
        textoUI.text = "";
        textoUI.enableWordWrapping = true;
        textoUI.overflowMode = TextOverflowModes.Overflow;
        if (botonContinuar != null)
            botonContinuar.SetActive(false);
        StartCoroutine(MostrarTextosSecuenciales());
    }
    IEnumerator MostrarTextosSecuenciales()
    {
        string[] partes = textoCompleto.Split('\n');
        for (int index = 0; index < partes.Length; index++)
        {
            string parte = partes[index];
            textoUI.text = "";
            if (imagenes != null && imagenes.Length > 0)
            {
                foreach (var img in imagenes)
                    if (img != null) img.gameObject.SetActive(false);

                if (index < imagenes.Length && imagenes[index] != null)
                    imagenes[index].gameObject.SetActive(true);
            }
            for (int i = 0; i < parte.Length; i++)
            {
                textoUI.text += parte[i];
                yield return new WaitForSeconds(velocidad);
            }

            yield return new WaitForSeconds(pausaEntreTextos);
        }
        if (botonContinuar != null)
            botonContinuar.SetActive(true);
    }
}

