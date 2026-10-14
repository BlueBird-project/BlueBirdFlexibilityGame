using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;

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


    public void RequestEnergyTask(
        float baseCost,
        bool requiresElectricity,
        Action onSuccess)
    {
        float finalCost = GetTaskCost(baseCost, requiresElectricity);

        Debug.Log(
            $"Energy task requested. Base cost: {baseCost}, " +
            $"Peak Hour: {isPeakHour}, Final cost: {finalCost}, " +
            $"Available energy: {dailyFuel}"
        );

        if (requiresElectricity && isPeakHour)
        {
            if (!CanConsume(finalCost))
            {
                Debug.Log(
                    $"Not enough energy. Required: {finalCost}, " +
                    $"available: {dailyFuel}"
                );
                return;
            }

            PeakHourDecisionUI.Instance.Show(
                baseCost,
                () =>
                {
                    float confirmedCost =
                        GetTaskCost(baseCost, requiresElectricity);

                    if (!CanConsume(confirmedCost))
                    {
                        Debug.Log(
                            $"Not enough energy. Required: {confirmedCost}, " +
                            $"available: {dailyFuel}"
                        );
                        return;
                    }

                    if (TryConsume(confirmedCost))
                    {
                        if (requiresElectricity && isPeakHour)
                        {
                            if (playerMovement != null)
                            {
                                playerMovement.SetSadMode(true);
                            }
                            else
                            {
                                Debug.LogWarning(
                                    "PlayerMovement is not assigned in EnergyManager."
                                );
                            }
                        }

                        onSuccess?.Invoke();
                    }
                },
                () =>
                {
                    Debug.Log("Task postponed by the player.");
                }
            );

            return;
        }

        if (!CanConsume(finalCost))
        {
            Debug.Log(
                $"Not enough energy. Required: {finalCost}, " +
                $"available: {dailyFuel}"
            );
            return;
        }

        if (TryConsume(finalCost))
        {
            onSuccess?.Invoke();
        }
    }

    public void OnEnergyTaskCompleted(bool requiresElectricity)
    {
        if (requiresElectricity && !isPeakHour)
        {
            Debug.Log("First electrical task completed. Peak Hour activated.");
            SetPeakHour(true);
        }
    }

    public bool TryConsumeTask(float finalCost)
    {
        if (!CanConsume(finalCost))
            return false;

        dailyFuel -= finalCost;
        UpdateEnergyVisual();

        Debug.Log(
            $"Energy consumed: {finalCost}. Remaining: {dailyFuel}"
        );

        return true;
    }


    public bool CanPerformEnergyTask()
    {
        return !isPeakHour;
    }

    public float GetTaskCost(float baseCost, bool requiresElectricity)
    {
        if (requiresElectricity && isPeakHour)
            return baseCost * 2f;

        return baseCost;
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