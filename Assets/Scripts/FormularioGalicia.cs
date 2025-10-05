using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormularioGalicia : MonoBehaviour
{
    [Header("Referencias UI - Formulario")]
    public TMP_Dropdown cultivoDropdown;
    public TMP_Dropdown riegoDropdown;
    public TMP_Dropdown frecuenciaDropdown;
    public TMP_Dropdown fertilizanteDropdown;
    public TMP_Dropdown cantidadDropdown;
    public TMP_Dropdown duracionDropdown;
    public Button guardarBtn;
    public Button closeFormBtn;
    public GameObject panelFormulario;

    [Header("Referencias UI - Resultados")]
    public GameObject panelResultados;
    public TextMeshProUGUI resultadoTexto;
    public Button cerrarBtn;

    [Header("Result Images")]
    public Image resultadoImagen;
    public Sprite imagenAprobado;
    public Sprite imagenFallado;

    [Header("Objeto visual (cubo en escena)")]
    public GameObject objetoVisual;

    [Header("Simulación Día/Noche")]
    public Light directionalLight;

    [Header("Extra objects that appear by crop")]
    public List<GameObject> objetosExtra; // 👈 ahora es una lista

    private bool isSimulando = false;
    private Canvas canvasPrincipal;

    private void Start()
    {
        canvasPrincipal = GetComponentInParent<Canvas>();
        if (canvasPrincipal == null)
            canvasPrincipal = FindObjectOfType<Canvas>();

        if (canvasPrincipal != null)
            canvasPrincipal.gameObject.SetActive(true);

        if (panelFormulario != null) panelFormulario.SetActive(false);
        if (panelResultados != null) panelResultados.SetActive(false);

        if (objetoVisual != null) objetoVisual.SetActive(true);
        // Desactivar todos los objetos extra al inicio
        foreach (var obj in objetosExtra)
        {
            if (obj != null) obj.SetActive(false);
        }

        if (guardarBtn != null)
        {
            guardarBtn.onClick.RemoveAllListeners();
            guardarBtn.onClick.AddListener(OnGuardarPressed);
        }

        if (cerrarBtn != null)
        {
            cerrarBtn.onClick.RemoveAllListeners();
            cerrarBtn.onClick.AddListener(OnCerrarResultados);
        }

        if (closeFormBtn != null) // 👈 configurar el nuevo botón
        {
            closeFormBtn.onClick.RemoveAllListeners();
            closeFormBtn.onClick.AddListener(OnCerrarFormulario);
        }
    }

    public void MostrarFormulario()
    {
        if (canvasPrincipal != null && !canvasPrincipal.gameObject.activeSelf)
            canvasPrincipal.gameObject.SetActive(true);

        if (panelFormulario != null) panelFormulario.SetActive(true);
        if (panelResultados != null) panelResultados.SetActive(false);

        Debug.Log("Galicia form displayed from cube interaction");
    }

    private void OnGuardarPressed()
    {
        if (isSimulando) return;

        if (panelFormulario != null) panelFormulario.SetActive(false);

        if (guardarBtn != null) guardarBtn.interactable = false;

        StartCoroutine(SimularDiasAntesResultados());
    }

    private IEnumerator SimularDiasAntesResultados()
    {
        isSimulando = true;

        float duracion = 6f;
        float rotacionTotal = 1080f;
        float rotacionPorSegundo = rotacionTotal / duracion;
        float tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {
            float delta = Time.deltaTime;
            if (directionalLight != null)
                directionalLight.transform.Rotate(Vector3.right, rotacionPorSegundo * delta);
            tiempoPasado += delta;
            yield return null;
        }
        MostrarResultados();
        // Activar el objeto según el cultivo elegido
        string cultivoSeleccionado = GetDropdownTextSafe(cultivoDropdown, "N/A");

        foreach (var obj in objetosExtra)
        {
            if (obj != null)
            {
                // El nombre del objeto debe coincidir con el texto del dropdown
                obj.SetActive(obj.name == cultivoSeleccionado);
            }
        }
    }

    public void MostrarResultados()
    {
        if (canvasPrincipal != null && !canvasPrincipal.gameObject.activeSelf)
            canvasPrincipal.gameObject.SetActive(true);

        string cultivo = GetDropdownTextSafe(cultivoDropdown, "N/A");
        string metodoRiego = GetDropdownTextSafe(riegoDropdown, "N/A");
        string frecuenciaStr = GetDropdownTextSafe(frecuenciaDropdown, "N/A");
        string fertilizante = GetDropdownTextSafe(fertilizanteDropdown, "N/A");
        string cantidadFert = GetDropdownTextSafe(cantidadDropdown, "N/A");
        string duracionStr = GetDropdownTextSafe(duracionDropdown, "N/A");

        string mensaje = "Galicia Evaluation\n\n";
        mensaje += $"Crop: {cultivo}\n";
        mensaje += $"Irrigation type: {metodoRiego}\n";
        mensaje += $"Irrigation frequency: {frecuenciaStr}\n";
        mensaje += $"Fertilizer type: {fertilizante}\n";
        mensaje += $"Fertilizer dosage: {cantidadFert}\n";
        mensaje += $"Crop duration: {duracionStr}\n\n";

        bool aprobado = true;

        // --- Crop validations for Galicia ---
        if (cultivo == "Potato")
        {
            if (metodoRiego != "Sprinkler irrigation")
            {
                mensaje += "Error: Potatoes grow best with sprinkler irrigation for even soil moisture.\n";
                aprobado = false;
            }
            if (frecuenciaStr != "Medium (1–2 times per week)")
            {
                mensaje += "Error: Potatoes need medium irrigation frequency (every 5–7 days).\n";
                aprobado = false;
            }
            if (fertilizante != "Chemical" && fertilizante != "Mixed")
            {
                mensaje += "Error: Potatoes require chemical or mixed fertilizers (N + K balance).\n";
                aprobado = false;
            }
            if (cantidadFert != "Medium (balanced)")
            {
                mensaje += "Error: Medium fertilizer dosage is recommended for potatoes.\n";
                aprobado = false;
            }
            if (duracionStr != "Medium (4–6 months)")
            {
                mensaje += "Error: Potato crops have a medium cycle of about 4–5 months.\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Maize (corn)")
        {
            if (metodoRiego != "Sprinkler irrigation")
            {
                mensaje += "Error: Corn grows best with sprinkler irrigation for higher yields.\n";
                aprobado = false;
            }
            if (frecuenciaStr != "Medium (1–2 times per week)")
            {
                mensaje += "Partial error: Corn in Galicia can rely on rainfall, but moderate irrigation improves yield.\n";
                aprobado = false;
            }
            if (fertilizante != "Chemical")
            {
                mensaje += "Error: Corn requires chemical fertilizers rich in nitrogen (NPK).\n";
                aprobado = false;
            }
            if (cantidadFert != "Medium (balanced)")
            {
                mensaje += "Error: Corn needs medium to high fertilizer dosage for optimal growth.\n";
                aprobado = false;
            }
            if (duracionStr != "Medium (4–6 months)")
            {
                mensaje += "Error: Corn requires a medium cycle (5–6 months) to reach maturity.\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Vineyard (grapes)")
        {
            if (metodoRiego != "Drip irrigation")
            {
                mensaje += "Error: Vineyards require drip irrigation to avoid fungal diseases.\n";
                aprobado = false;
            }
            if (frecuenciaStr != "Low (every 10–12 days)")
            {
                mensaje += "Error: Vineyards require low irrigation frequency (every 10–14 days).\n";
                aprobado = false;
            }
            if (fertilizante != "Mixed (combination)")
            {
                mensaje += "Error: Vineyards should use mixed fertilizers (organic + potassium, magnesium).\n";
                aprobado = false;
            }
            if (cantidadFert != "Medium (balanced)")
            {
                mensaje += "Error: Medium fertilizer dosage is ideal for vineyards.\n";
                aprobado = false;
            }
            if (duracionStr != "Long (8–12 months)")
            {
                mensaje += "Error: Vineyards have a long cycle of 8–9 months per harvest.\n";
                aprobado = false;
            }
        }

        if (aprobado)
            mensaje += "\nResult: Approved";
        else
            mensaje += "\nResult: Failed";

        if (resultadoTexto != null) resultadoTexto.text = mensaje;
        if (resultadoImagen != null)
        {
            if (aprobado && imagenAprobado != null)
                resultadoImagen.sprite = imagenAprobado;
            else if (!aprobado && imagenFallado != null)
                resultadoImagen.sprite = imagenFallado;
        }
        if (panelResultados != null) panelResultados.SetActive(true);

        Debug.Log("Galicia results displayed");
    }

    private void OnCerrarResultados()
    {
        if (panelResultados != null) panelResultados.SetActive(false);
    }

    private void OnCerrarFormulario()
    {
        if (panelFormulario != null) panelFormulario.SetActive(false);
        Debug.Log("Formulario cerrado por el usuario");
    }

    private string GetDropdownTextSafe(TMP_Dropdown dd, string defaultValue)
    {
        if (dd == null || dd.options == null || dd.options.Count == 0) return defaultValue;
        int idx = Mathf.Clamp(dd.value, 0, dd.options.Count - 1);
        return dd.options[idx].text;
    }
}
