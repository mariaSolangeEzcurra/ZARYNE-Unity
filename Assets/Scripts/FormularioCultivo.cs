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
        // Simplemente desactiva el panel del formulario
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

        string mensaje = "Feedback – Majes Valley\n\n";
        mensaje += $"Crop: {cultivo}\n";
        mensaje += $"Irrigation method: {metodoRiego}\n";
        mensaje += $"Irrigation frequency: {frecuenciaStr}\n";
        mensaje += $"Fertilizer type: {fertilizante}\n";
        mensaje += $"Fertilizer amount: {cantidadFert}\n";
        mensaje += $"Cycle duration: {duracionStr}\n\n";

        bool aprobado = true;

        if (cultivo == "Arroz" || cultivo == "Rice")
        {
            if (metodoRiego != "Riego por gravedad" && metodoRiego != "Flood irrigation")
            {
                mensaje += "Error: Rice traditionally requires flooding to maintain a water layer.\n";
                mensaje += "Correction: Use flood irrigation with high frequency (continuous water layer).\n\n";
                aprobado = false;
            }

            if (cantidadFert == "Baja" || cantidadFert == "Low")
            {
                mensaje += "Error: Rice needs controlled nitrogen and phosphorus for proper growth.\n";
                mensaje += "Correction: Apply chemical fertilizer, medium dosage (100–150 kg/ha N + P).\n\n";
                aprobado = false;
            }

            if (duracionStr == "2–3 months")
            {
                mensaje += "Error: Rice takes longer to complete its growth and ripening.\n";
                mensaje += "Correction: Medium cycle (4–5 months).\n\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Maíz chala" || cultivo == "Forage Maize")
        {
            if (metodoRiego == "Riego por gravedad" || metodoRiego == "Flood irrigation")
            {
                mensaje += "Partial error: Maize does not need flooding; excess water may cause fungal diseases.\n";
                mensaje += "Correction: Use sprinkler or drip irrigation with medium frequency (every 3–4 days).\n\n";
                aprobado = false;
            }

            if (cantidadFert == "Baja" || cantidadFert == "Low")
            {
                mensaje += "Error: Forage maize requires high nitrogen levels for biomass production.\n";
                mensaje += "Correction: Chemical fertilizer, medium dosage (150–200 kg/ha N + P).\n\n";
                aprobado = false;
            }

            if (duracionStr == "2–3 months")
            {
                mensaje += "Error: Maize chala takes longer to develop enough forage volume.\n";
                mensaje += "Correction: Medium cycle (4–5 months).\n\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Alfalfa")
        {
            if (metodoRiego == "Riego por gravedad" || metodoRiego == "Flood irrigation")
            {
                mensaje += "Partial error: Alfalfa tolerates flooding but it is less efficient and increases root damage risk.\n";
                mensaje += "Correction: Sprinkler or drip irrigation, medium frequency (1–2 times per week).\n\n";
                aprobado = false;
            }

            if (fertilizante == "Químico" || fertilizante == "Chemical")
            {
                mensaje += "Error: Alfalfa fixes nitrogen naturally; pure chemical fertilizer is unnecessary.\n";
                mensaje += "Correction: Use mixed fertilization (P + K + organic matter), medium dosage.\n\n";
                aprobado = false;
            }

            if (duracionStr == "2–3 months")
            {
                mensaje += "Error: Alfalfa is perennial and produces for several years.\n";
                mensaje += "Correction: Long cycle (perennial, multiple cuts per year).\n\n";
                aprobado = false;
            }
        }

        mensaje += "Evaluation:\n";
        mensaje += aprobado ? "Approved: Crop management is consistent with Majes Valley standards." :
                              "Failed: Some critical decisions (irrigation, fertilization, or cycle) are incorrect.";

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
