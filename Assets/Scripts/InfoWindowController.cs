using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoWindowController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private string[] infoPages;
    private int currentIndex = 0;

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseWindow);
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        gameObject.SetActive(false);
    }

    public void OpenWindow(string[] pages)
    {
        infoPages = pages;
        currentIndex = 0;
        UpdatePage();
        gameObject.SetActive(true);
    }

    private void UpdatePage()
    {
        if (infoPages != null && infoPages.Length > 0)
            infoText.text = infoPages[currentIndex];
    }

    private void NextPage()
    {
        if (infoPages == null || infoPages.Length == 0) return;
        currentIndex = (currentIndex + 1) % infoPages.Length;
        UpdatePage();
    }

    private void PrevPage()
    {
        if (infoPages == null || infoPages.Length == 0) return;
        currentIndex = (currentIndex - 1 + infoPages.Length) % infoPages.Length;
        UpdatePage();
    }

    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
