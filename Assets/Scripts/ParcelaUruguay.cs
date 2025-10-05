using UnityEngine;

public class ParcelaUruguay : MonoBehaviour
{
    public FormularioUruguay formularioUruguay; 

    private void OnMouseDown()
    {
        if (formularioUruguay != null)
        {
            formularioUruguay.MostrarFormulario();
        }
        else
        {
            Debug.LogError("No se asignó FormularioUruguay al ObjetoInteractivo");
        }
    }
}
