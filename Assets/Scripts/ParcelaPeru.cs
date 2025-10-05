using UnityEngine;

public class ParcelaPeru : MonoBehaviour
{
    public FormularioCultivo formularioCultivo; 

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
