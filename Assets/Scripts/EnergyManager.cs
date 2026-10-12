using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Header("Energy")]
    [SerializeField] private float dailyFuel = 60f;
    [SerializeField] private float battery = 0f;
    [SerializeField] private float renewableEnergy = 0f;
    [SerializeField] private float fossilFuel = 100f;
    [SerializeField] private TMP_Text energyText;

    [Header("Time")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] private float currentHour = 0f;

    [Header("Peak")]
    [SerializeField] private bool isPeakHour;
    [SerializeField] private Image sliderFill;

    public float DailyFuel => dailyFuel;
    public float Battery => battery;
    public float RenewableEnergy => renewableEnergy;
    public float FossilFuel => fossilFuel;
    public bool IsPeakHour => isPeakHour;
    public float CurrentHour => currentHour;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdatePeakVisual();
        UpdateEnergyVisual();
    }

    private void UpdatePeakVisual()
    {
        if (sliderFill == null)
            return;

        if (isPeakHour)
        {
            ColorUtility.TryParseHtmlString("#B62B1E", out Color red);
            sliderFill.color = red;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#749B3C", out Color green);
            sliderFill.color = green;
        }
    }

    private void UpdateEnergyVisual()
    {
        if (energyText == null)
            return;

        energyText.text = Mathf.RoundToInt(dailyFuel).ToString();

        if (dailyFuel < 30f)
        {
            ColorUtility.TryParseHtmlString("#B62B1E", out Color red);
            energyText.color = red;
        }
        else if (dailyFuel < 60f)
        {
            energyText.color = new Color(1f, 0.55f, 0f); // Orange
        }
        else
        {
            ColorUtility.TryParseHtmlString("#749B3C", out Color green);
            energyText.color = green;
        }
    }

    public bool CanConsume(float amount)
    {
        return dailyFuel >= amount;
    }

    public bool TryConsume(float amount)
    {
        if (!CanConsume(amount))
            return false;

        dailyFuel -= amount;
        UpdateEnergyVisual();

        return true;
    }

    public bool CanPerformEnergyTask()
    {
        if (isPeakHour)
        {
            Debug.Log(
                "It's a peak hour. The grid is under strain. " +
                "You need to wait for off-peak, or you need to connect a new energy source."
            );

            return false;
        }

        return true;
    }

    public void SetPeakHour(bool peak)
    {
        isPeakHour = peak;

        if (sliderFill != null)
        {
            sliderFill.color = isPeakHour
                ? Color.red
                : Color.green;
        }
    }

    public void AddRenewableEnergy(float amount)
    {
        renewableEnergy += amount;
    }

    public void AddBattery(float amount)
    {
        battery += amount;
    }
}