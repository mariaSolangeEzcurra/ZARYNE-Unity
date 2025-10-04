using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FormularioCultivo : MonoBehaviour
{
    [Header("Referencias UI - Formulario")]
    public TMP_Dropdown cultivoDropdown;
    public TMP_Dropdown riegoDropdown;
    public TMP_Dropdown frecuenciaDropdown;
    public TMP_Dropdown fertilizanteDropdown;
    public TMP_Dropdown cantidadDropdown;
    public TMP_Dropdown duracionDropdown;
    public Button guardarBtn;
    public GameObject panelFormulario;

    [Header("Referencias UI - Resultados")]
    public GameObject panelResultados;
    public TextMeshProUGUI resultadoTexto;
    public Button cerrarBtn;

    [Header("Objeto visual (cubo en escena)")]
    public GameObject objetoVisual;

    [Header("Simulación Día/Noche")]
    public Light directionalLight;

    [Header("Objetos extra que aparecen según cultivo")]
    public List<GameObject> objetosExtra; // lista de objetos extra (ej: Cubo, Cápsula, Esfera)

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

        // Ocultar todos los objetos extra al inicio
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
    }

    public void MostrarFormulario()
    {
        if (canvasPrincipal != null && !canvasPrincipal.gameObject.activeSelf)
            canvasPrincipal.gameObject.SetActive(true);

        if (panelFormulario != null) panelFormulario.SetActive(true);
        if (panelResultados != null) panelResultados.SetActive(false);

        Debug.Log("Formulario mostrado desde el cubo");
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

        // Mostrar solo el objeto que coincide con el cultivo seleccionado
        string cultivoSeleccionado = GetDropdownTextSafe(cultivoDropdown, "N/D");

        foreach (var obj in objetosExtra)
        {
            if (obj != null)
            {
                // Activar solo el que coincida con el nombre
                obj.SetActive(obj.name == cultivoSeleccionado);
                if (obj.activeSelf)
                    Debug.Log($"Objeto extra activado: {obj.name}");
            }
        }

        isSimulando = false;
        if (guardarBtn != null) guardarBtn.interactable = true;
    }

    public void MostrarResultados()
    {
        if (canvasPrincipal != null && !canvasPrincipal.gameObject.activeSelf)
            canvasPrincipal.gameObject.SetActive(true);

        string cultivo = GetDropdownTextSafe(cultivoDropdown, "N/D");
        string metodoRiego = GetDropdownTextSafe(riegoDropdown, "N/D");
        string frecuenciaStr = GetDropdownTextSafe(frecuenciaDropdown, "N/D");
        string fertilizante = GetDropdownTextSafe(fertilizanteDropdown, "N/D");
        string cantidadFert = GetDropdownTextSafe(cantidadDropdown, "N/D");
        string duracionStr = GetDropdownTextSafe(duracionDropdown, "N/D");

        string mensaje = "Evaluación del Valle de Majes\n\n";
        mensaje += $"Cultivo: {cultivo}\n";
        mensaje += $"Método de riego: {metodoRiego}\n";
        mensaje += $"Frecuencia de riego: {frecuenciaStr}\n";
        mensaje += $"Fertilizante: {fertilizante}\n";
        mensaje += $"Cantidad de fertilizante: {cantidadFert}\n";
        mensaje += $"Duración del ciclo: {duracionStr}\n\n";

        bool aprobado = true;

        if (cultivo == "Arroz")
        {
            if (metodoRiego != "Riego por gravedad")
            {
                mensaje += "Error: El arroz necesita riego por gravedad (inundación).\n";
                aprobado = false;
            }
            if (frecuenciaStr != "3 veces por semana")
            {
                mensaje += "Error: El arroz requiere riegos frecuentes.\n";
                aprobado = false;
            }
            if (fertilizante == "Orgánico")
            {
                mensaje += "Error: El arroz necesita fertilizante químico o mixto.\n";
                aprobado = false;
            }
            if (cantidadFert == "Baja")
            {
                mensaje += "Error: El arroz requiere fertilización media como mínimo.\n";
                aprobado = false;
            }
            if (duracionStr != "4–6 meses")
            {
                mensaje += "Error: El arroz se cultiva en ciclos de 4–6 meses.\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Maíz chala")
        {
            if (metodoRiego == "Riego por gravedad")
            {
                mensaje += "Error parcial: El maíz no requiere inundación, se recomienda aspersión o goteo.\n";
                aprobado = false;
            }
            if (frecuenciaStr != "1–2 veces por semana")
            {
                mensaje += "Error: El maíz requiere riego de 1–2 veces por semana.\n";
                aprobado = false;
            }
            if (fertilizante == "Orgánico")
            {
                mensaje += "Error: El maíz necesita fertilizante químico o mixto.\n";
                aprobado = false;
            }
            if (cantidadFert == "Baja")
            {
                mensaje += "Error: El maíz necesita fertilización media.\n";
                aprobado = false;
            }
            if (duracionStr != "4–6 meses")
            {
                mensaje += "Error: El maíz tiene un ciclo de 4–6 meses.\n";
                aprobado = false;
            }
        }
        else if (cultivo == "Alfalfa")
        {
            if (metodoRiego == "Riego por gravedad")
            {
                mensaje += "Error: La alfalfa se riega mejor con aspersión o goteo.\n";
                aprobado = false;
            }
            if (frecuenciaStr != "1–2 veces por semana")
            {
                mensaje += "Error: La alfalfa requiere riego de 1–2 veces por semana.\n";
                aprobado = false;
            }
            if (fertilizante == "Químico")
            {
                mensaje += "Error parcial: La alfalfa fija nitrógeno, se recomienda orgánico o mixto.\n";
            }
            if (cantidadFert == "Alta")
            {
                mensaje += "Error: La alfalfa no requiere dosis altas, se recomienda baja o media.\n";
                aprobado = false;
            }
            if (duracionStr != "8–12 meses")
            {
                mensaje += "Error: La alfalfa es perenne y dura 8–12 meses.\n";
                aprobado = false;
            }
        }

        if (aprobado)
            mensaje += "\nResultado: Aprobado";
        else
            mensaje += "\nResultado: Desaprobado";

        if (resultadoTexto != null) resultadoTexto.text = mensaje;
        if (panelResultados != null) panelResultados.SetActive(true);

        Debug.Log("Resultados mostrados");
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
