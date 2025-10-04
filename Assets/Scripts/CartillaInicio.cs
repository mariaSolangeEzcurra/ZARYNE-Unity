using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartillaInicio : MonoBehaviour
{
    [Header("Referencias")]
    public TMP_InputField inputTextoUI;   // Campo TMP_InputField
    public Image[] imagenes;              // Lista de imágenes (una por sección)

    [Header("Configuración")]
    [TextArea(5, 10)] public string textoOriginal; // Texto con secciones separadas por doble salto de línea
    public float velocidad = 0.05f;               // Tiempo entre letras
    public float pausaEntreSecciones = 1.5f;      // Tiempo entre secciones

    private string[] secciones;        // Secciones de texto divididas por "\n\n"

    void OnEnable()
    {
        // Dividir el texto en secciones por doble salto de línea
        secciones = textoOriginal.Split(new string[] { "\n\n" }, System.StringSplitOptions.None);

        // Apagar texto e imágenes al inicio
        if (inputTextoUI != null) inputTextoUI.gameObject.SetActive(false);

        if (imagenes != null)
        {
            foreach (var img in imagenes)
                if (img != null) img.gameObject.SetActive(false);
        }

        StopAllCoroutines();
        StartCoroutine(MostrarSecuencia());
    }

    IEnumerator MostrarSecuencia()
    {
        for (int index = 0; index < secciones.Length; index++)
        {
            string parte = secciones[index];

            // Activar campo de texto
            if (!inputTextoUI.gameObject.activeSelf)
                inputTextoUI.gameObject.SetActive(true);

            inputTextoUI.text = ""; // Limpia el campo antes de escribir

            // Manejar imágenes
            if (imagenes != null && imagenes.Length > 0)
            {
                foreach (var img in imagenes)
                    if (img != null) img.gameObject.SetActive(false);

                if (index < imagenes.Length && imagenes[index] != null)
                    imagenes[index].gameObject.SetActive(true);
            }

            // Efecto de escritura letra por letra
            for (int i = 0; i < parte.Length; i++)
            {
                inputTextoUI.text += parte[i];
                yield return new WaitForSeconds(velocidad);
            }

            yield return new WaitForSeconds(pausaEntreSecciones);
        }

        // Al finalizar: apagar imágenes y texto
        if (imagenes != null && imagenes.Length > 0)
        {
            foreach (var img in imagenes)
                if (img != null) img.gameObject.SetActive(false);
        }

        if (inputTextoUI != null)
            inputTextoUI.gameObject.SetActive(false);
    }
}
