using UnityEngine;


public class ObjetoInteractivo : MonoBehaviour
{
    public FormularioGalicia formularioCultivo; // arrastrar el Canvas (con script) al Inspector


    private void OnMouseDown()
    {
        if (formularioCultivo != null)
        {
            formularioCultivo.MostrarFormulario();
        }
        else
        {
            Debug.LogError("No se asignó FormularioCultivo al ObjetoInteractivo");
        }
    }
}

