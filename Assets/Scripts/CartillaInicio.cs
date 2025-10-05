using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CartillaInicio : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject cartillaPanel;     
    public TMP_InputField inputFieldTMP; 
    public Image imagenUI;               

    [Header("Configuración")]
    [TextArea] public string mensaje;    
    public float velocidadTexto = 0.05f; 
    public float tiempoTexto = 2f;      
    public Sprite[] imagenes;            
    public float tiempoImagen = 2f;      
    
    void Start()
    {
        cartillaPanel.SetActive(true);
        imagenUI.gameObject.SetActive(false); 
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
