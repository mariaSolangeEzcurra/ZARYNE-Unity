using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CartillaInicio : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject cartillaPanel;     // Panel de la cartilla
    public TMP_InputField inputFieldTMP; // Input TMP para mostrar el texto
    public Image imagenUI;               // Objeto UI de tipo Image en el panel


    [Header("Configuración")]
    [TextArea] public string mensaje;    // Texto que se escribirá
    public float velocidadTexto = 0.05f; // Tiempo entre letras
    public float tiempoTexto = 2f;       // Tiempo de espera tras terminar el texto
    public Sprite[] imagenes;            // Lista de imágenes a mostrar en secuencia
    public float tiempoImagen = 2f;      // Tiempo que dura cada imagen


    void Start()
    {
        cartillaPanel.SetActive(true);
        imagenUI.gameObject.SetActive(false); // Ocultar la imagen al inicio
        StartCoroutine(SecuenciaCartilla());
    }


    IEnumerator SecuenciaCartilla()
    {
        imagenUI.gameObject.SetActive(true);
        int indiceImagen = 0;
        float tiempoAcumulado = 0f;
        inputFieldTMP.text = "";
        foreach (char letra in mensaje)
        {
            inputFieldTMP.text += letra;
            // Cambiar imagen según tiempo
            tiempoAcumulado += velocidadTexto;
            if (tiempoAcumulado >= tiempoImagen)
            {
                indiceImagen = (indiceImagen + 1) % imagenes.Length;
                imagenUI.sprite = imagenes[indiceImagen];
                tiempoAcumulado = 0f;
            }
            yield return new WaitForSeconds(velocidadTexto);
        }


        yield return new WaitForSeconds(tiempoTexto);
        cartillaPanel.SetActive(false);
    }


}

