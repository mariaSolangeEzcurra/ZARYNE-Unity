using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FormularioUruguay : MonoBehaviour
{
    [Header("UI References - Form")]
    public TMP_Dropdown cultivoDropdown;
    public TMP_Dropdown riegoDropdown;
    public TMP_Dropdown frecuenciaDropdown;
    public TMP_Dropdown fertilizanteDropdown;
    public TMP_Dropdown cantidadDropdown;
    public TMP_Dropdown duracionDropdown;
    public Button guardarBtn;
    public GameObject panelFormulario;
    public Button cerrarFormularioBtn;

    [Header("UI References - Results")]
    public GameObject panelResultados;
    public TextMeshProUGUI resultadoTexto;
    public Button cerrarBtn;

    [Header("Result Images")]
    public Image resultadoImagen;
    public Sprite imagenAprobado;
    public Sprite imagenFallado;

    [Header("Visual Object (Scene Element)")]
    public GameObject objetoVisual;

    [Header("Day/Night Simulation")]
    public Light directionalLight;

    [Header("Extra objects by crop")]
    public List<GameObject> objetosExtra;

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

        if (cerrarFormularioBtn != null)
        {
            cerrarFormularioBtn.onClick.RemoveAllListeners();
            cerrarFormularioBtn.onClick.AddListener(OnCerrarFormulario);
        }
    }

    public void MostrarFormulario()
    {
        if (canvasPrincipal != null && !canvasPrincipal.gameObject.activeSelf)
            canvasPrincipal.gameObject.SetActive(true);

        if (panelFormulario != null) panelFormulario.SetActive(true);
        if (panelResultados != null) panelResultados.SetActive(false);
    }

    private void OnCerrarFormulario()
    {
        if (panelFormulario != null)
        {
            panelFormulario.SetActive(false);
            Debug.Log("Formulario cerrado.");
        }
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

        string cultivoSeleccionado = GetDropdownTextSafe(cultivoDropdown, "N/A");
        foreach (var obj in objetosExtra)
        {
            if (obj != null)
            {
                obj.SetActive(obj.name == cultivoSeleccionado);
            }
        }

        isSimulando = false;
        if (guardarBtn != null) guardarBtn.interactable = true;
    }

    public void MostrarResultados()
    {
        if (canvasPrincipal != null && !canvasPrincipal.gameObject.activeSelf)
            canvasPrincipal.gameObject.SetActive(true);

        string cultivo = GetDropdownTextSafe(cultivoDropdown, "N/A");
        string metodoRiego = GetDropdownTextSafe(riegoDropdown, "N/A");
        string frecuencia = GetDropdownTextSafe(frecuenciaDropdown, "N/A");
        string fertilizante = GetDropdownTextSafe(fertilizanteDropdown, "N/A");
        string cantidad = GetDropdownTextSafe(cantidadDropdown, "N/A");
        string duracion = GetDropdownTextSafe(duracionDropdown, "N/A");

        string mensaje = "Feedback – Uruguay (Rincón del Colorado, Canelones)\n\n";
        mensaje += $"Crop: {cultivo}\n";
        mensaje += $"Irrigation type: {metodoRiego}\n";
        mensaje += $"Irrigation frequency: {frecuencia}\n";
        mensaje += $"Fertilizer type: {fertilizante}\n";
        mensaje += $"Fertilizer dosage: {cantidad}\n";
        mensaje += $"Cycle duration: {duracion}\n\n";

        bool aprobado = true;

        if (cultivo == "Corn" || cultivo == "Maíz")
        {
            if (metodoRiego.Contains("Flood") || metodoRiego.Contains("Minimal"))
            {
                mensaje += "Error: Corn needs regular water, especially during flowering.\n";
                mensaje += "Correction: Use sprinkler or drip irrigation 💧 with medium–high frequency for stable yields.\n\n";
                aprobado = false;
            }

            if (fertilizante == "Organic")
            {
                mensaje += "Partial error: Organic alone may not supply enough nitrogen for high-demand corn.\n";
                mensaje += "Correction: Use chemical or mixed fertilizer with controlled nitrogen.\n\n";
                aprobado = false;
            }

            if (duracion == "Long (6–7 months)" || duracion.Contains("Long"))
            {
                mensaje += "Error: Corn matures in about 4 months in this region.\n";
                mensaje += "Correction: Medium cycle (~4 months).\n\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Grapes" || cultivo == "Uvas")
        {
            if (metodoRiego.Contains("Flood") || metodoRiego.Contains("Sprinkler"))
            {
                mensaje += "Error: Grapes are highly sensitive to excess water; fruit quality drops.\n";
                mensaje += "Correction: Drip irrigation with low frequency 💧 is optimal.\n\n";
                aprobado = false;
            }

            if (cantidad.Contains("High"))
            {
                mensaje += "Error: Excess nitrogen reduces grape quality.\n";
                mensaje += "Correction: Medium dosage, minimal nitrogen, optional organic compost.\n\n";
                aprobado = false;
            }

            if (duracion.Contains("Short"))
            {
                mensaje += "Error: Grapes require ~6–7 months to reach harvest.\n";
                mensaje += "Correction: Long cycle (~6–7 months).\n\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Tomatoes" || cultivo == "Tomates")
        {
            if (metodoRiego.Contains("Minimal"))
            {
                mensaje += "Error: Tomatoes need steady water to avoid stress and fruit cracking.\n";
                mensaje += "Correction: Sprinkler or drip irrigation 💧 at medium frequency (1–2 times/week).\n\n";
                aprobado = false;
            }

            if (fertilizante == "Chemical")
            {
                mensaje += "Partial error: Tomatoes need balanced NPK for proper fruit development.\n";
                mensaje += "Correction: Use mixed fertilizer with balanced NPK.\n\n";
                aprobado = false;
            }

            if (duracion.Contains("Long"))
            {
                mensaje += "Error: Tomatoes mature in ~3–4 months.\n";
                mensaje += "Correction: Short–medium cycle (~3–4 months).\n\n";
                aprobado = false;
            }
        }

        mensaje += "Evaluation:\n";
        mensaje += aprobado
            ? "Approved: Crop management is consistent with Rincón del Colorado standards."
            : "Failed: Some critical decisions (irrigation, fertilization, or cycle) are incorrect.";

        if (resultadoTexto != null)
            resultadoTexto.text = mensaje;

        if (resultadoImagen != null)
        {
            resultadoImagen.sprite = aprobado ? imagenAprobado : imagenFallado;
        }
        if (panelResultados != null)
            panelResultados.SetActive(true);
    }

    private void OnCerrarResultados()
    {
        if (panelResultados != null) panelResultados.SetActive(false);
    }

    private string GetDropdownTextSafe(TMP_Dropdown dd, string defaultValue)
    {
        if (dd == null || dd.options == null || dd.options.Count == 0) return defaultValue;
        int idx = Mathf.Clamp(dd.value, 0, dd.options.Count - 1);
        return dd.options[idx].text;
    }
}
