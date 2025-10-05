using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
public class BotonPanel : MonoBehaviour
{
    [Header("Panel que se abrirÃ¡/cerrarÃ¡")]
    public GameObject panelUI;

    [Header("BotÃ³n de cierre dentro del panel (opcional)")]
    public Button botonCerrar;

    void Start()
    {
        if (panelUI != null)
            panelUI.SetActive(false); 
=======

public class BotonPanel : MonoBehaviour
{
    [Header("Panel que se abrirá/cerrará")]
    public GameObject panelUI;


    [Header("Botón de cierre dentro del panel (opcional)")]
    public Button botonCerrar;


    void Start()
    {
        if (panelUI != null)
            panelUI.SetActive(false); // Aseguramos que empiece oculto

>>>>>>> Geraldine

        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarPanel);
        }
    }

<<<<<<< HEAD
=======

>>>>>>> Geraldine
    public void AbrirPanel()
    {
        if (panelUI != null)
            panelUI.SetActive(true);
    }

<<<<<<< HEAD
=======

>>>>>>> Geraldine
    public void CerrarPanel()
    {
        if (panelUI != null)
            panelUI.SetActive(false);
    }
}
