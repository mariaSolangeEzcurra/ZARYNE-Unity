using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FormularioCultivo : MonoBehaviour
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

    [Header("Visual Object (Cube in Scene)")]
    public GameObject objetoVisual;

    [Header("Day/Night Simulation")]
    public Light directionalLight;

    [Header("Extra objects that appear by crop")]
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
        string frecuenciaStr = GetDropdownTextSafe(frecuenciaDropdown, "N/A");
        string fertilizante = GetDropdownTextSafe(fertilizanteDropdown, "N/A");
        string cantidadFert = GetDropdownTextSafe(cantidadDropdown, "N/A");
        string duracionStr = GetDropdownTextSafe(duracionDropdown, "N/A");

        string mensaje = "Feedback – Murray River, Australia\n\n";
        mensaje += $"Crop: {cultivo}\n";
        mensaje += $"Irrigation type: {metodoRiego}\n";
        mensaje += $"Irrigation frequency: {frecuenciaStr}\n";
        mensaje += $"Fertilizer type: {fertilizante}\n";
        mensaje += $"Fertilizer dosage: {cantidadFert}\n";
        mensaje += $"Cycle duration: {duracionStr}\n\n";

        bool aprobado = true;

        // --- CORN (MAIZE) ---
        if (cultivo == "Corn" || cultivo == "Maize")
        {
            if (metodoRiego == "Flood irrigation")
            {
                mensaje += "Error: Corn does not tolerate excess water; flooding plus rainfall causes rot.\n";
                mensaje += "Correction: Use sprinkler or drip irrigation, with high frequency (3 times per week).\n\n";
                aprobado = false;
            }

            if (cantidadFert == "Low")
            {
                mensaje += "Error: Corn requires sufficient nitrogen for growth.\n";
                mensaje += "Correction: Chemical fertilizer, medium dosage (controlled N application).\n\n";
                aprobado = false;
            }

            if (duracionStr == "Long (6–7 months)")
            {
                mensaje += "Error: Corn matures faster than that.\n";
                mensaje += "Correction: Short cycle (~4 months).\n\n";
                aprobado = false;
            }
        }

        // --- WHEAT ---
        else if (cultivo == "Wheat")
        {
            if (metodoRiego == "Flood irrigation")
            {
                mensaje += "Error: Flooding is inefficient and risks waterlogging.\n";
                mensaje += "Correction: Drip irrigation preferred; sprinkler is acceptable if managed carefully, with medium frequency (1–2 times/week).\n\n";
                aprobado = false;
            }

            if (cantidadFert == "High")
            {
                mensaje += "Error: Excess nitrogen + phosphorus can harm yield and soil balance.\n";
                mensaje += "Correction: Medium dosage (balanced N + P).\n\n";
                aprobado = false;
            }

            if (duracionStr == "Short (4 months)")
            {
                mensaje += "Partial error: Wheat can mature in ~5 months depending on conditions; too short may reduce yield.\n";
                mensaje += "Correction: Medium cycle (~5 months).\n\n";
                aprobado = false;
            }
        }

        // --- GRAPES ---
        else if (cultivo == "Grapes")
        {
            if (metodoRiego == "Sprinkler irrigation" || metodoRiego == "Flood irrigation")
            {
                mensaje += "Error: Grapes are very sensitive to excess water; fruit quality decreases.\n";
                mensaje += "Correction: Drip irrigation only, with low frequency (precise, minimal water).\n\n";
                aprobado = false;
            }

            if (cantidadFert == "High")
            {
                mensaje += "Error: Excess nutrients reduce fruit quality.\n";
                mensaje += "Correction: Low or medium dosage, minimal nitrogen, optional potassium/organic matter.\n\n";
                aprobado = false;
            }

            if (duracionStr == "Medium (5 months)")
            {
                mensaje += "Partial error: Grapes take ~6–7 months to harvest; they are perennial.\n";
                mensaje += "Correction: Long cycle (6–7 months until harvest, perennial crop).\n\n";
                aprobado = false;
            }
        }

        mensaje += "Evaluation:\n";
        mensaje += aprobado
            ? "Approved: Management aligns with Murray River standards (≥4 correct decisions)."
            : "Failed: Critical mistakes in irrigation or fertilization choices.";

        if (resultadoTexto != null) resultadoTexto.text = mensaje;
        if (panelResultados != null) panelResultados.SetActive(true);
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

    private int ParseDropdownIntSafe(TMP_Dropdown dd, int fallback)
    {
        string txt = GetDropdownTextSafe(dd, fallback.ToString());
        int val;
        if (int.TryParse(txt, out val)) return val;
        return fallback;
    }
}
