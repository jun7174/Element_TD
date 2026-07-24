using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private Button speedButton_0;
    [SerializeField] private Button speedButton_1;
    [SerializeField] private Button speedButton_2;

    [SerializeField] private TextMeshProUGUI speedText;
    
    private void Start()
    {
        speedButton_0.onClick.AddListener(OnClickGameSpeedButton);
        speedButton_1.onClick.AddListener(OnClickGameSpeedButton1);
        speedButton_2.onClick.AddListener(OnClickGameSpeedButton2);
    }

    private void OnClickGameSpeedButton()
    {
        Time.timeScale = 0.5f;
        speedText.text = "x0.5";
    }
    private void OnClickGameSpeedButton1()
    {
        Time.timeScale = 1f;
        speedText.text = "x1";
    }
    private void OnClickGameSpeedButton2()
    {
        Time.timeScale = 2f;
        speedText.text = "x2";
    }
}
