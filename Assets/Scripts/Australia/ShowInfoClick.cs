using UnityEngine;
using TMPro;
using UnityEngine.UI;   // <-- ESTA LÍNEA ES LA QUE FALTABA

public class ShowInfoClick : MonoBehaviour
{
    public GameObject infoPanel;     // Arrastra el Panel del Canvas aquí
    public TMP_Text infoText1;       // Primer texto (columna 1)
    public TMP_Text infoText2;       // Primer texto (columna 1)
    public TMP_Text infoText3;
    public Image imagen;       // Tercer texto (columna 3)


    private bool isVisible = false;

    void Update()
    {
        // Si el panel está visible y se hace clic izquierdo en cualquier parte
        if (isVisible && Input.GetMouseButtonDown(0))
        {
            // Si el clic no fue sobre el objeto mismo, ocultamos
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) 
            {
                ClosePanel();
            }
            else if (hit.transform != transform) 
            {
                ClosePanel();
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isVisible) // solo abrir si no está visible
        {
            isVisible = true;
            infoPanel.SetActive(true);

        
        }
    }

    void ClosePanel()
    {
        isVisible = false;
        infoPanel.SetActive(false);
    }
}
