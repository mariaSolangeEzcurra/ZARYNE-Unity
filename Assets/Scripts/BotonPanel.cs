using UnityEngine;
using UnityEngine.UI;

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

        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarPanel);
        }
    }

    public void AbrirPanel()
    {
        if (panelUI != null)
            panelUI.SetActive(true);
    }

    public void CerrarPanel()
    {
        if (panelUI != null)
            panelUI.SetActive(false);
    }
}
