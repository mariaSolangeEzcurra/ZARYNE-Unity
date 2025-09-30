using UnityEngine;

public class PrefabClickable : MonoBehaviour
{
    [SerializeField] private InfoWindowController infoWindow;
    private string[] infoPages = {
        "Página 1: Descripción del objeto",
        "Página 2: Detalles adicionales",
        "Página 3: Consejos o stats"
    };

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // click izquierdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) // si clickeamos este prefab
                {
                    infoWindow.OpenWindow(infoPages);
                }
            }
        }
    }
}
