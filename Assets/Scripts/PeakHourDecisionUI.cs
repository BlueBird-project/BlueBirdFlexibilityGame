
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PeakHourDecisionUI : MonoBehaviour
{
    public static PeakHourDecisionUI Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Button doItAnywayButton;
    [SerializeField] private Button doItLaterButton;

    public bool IsOpen => panel != null && panel.activeSelf;

    private Action onConfirm;
    private Action onCancel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        panel.SetActive(false);

        doItAnywayButton.onClick.AddListener(Confirm);
        doItLaterButton.onClick.AddListener(Cancel);
    }

    public void Show(float baseCost, Action confirm, Action cancel)
    {
        onConfirm = confirm;
        onCancel = cancel;

        float finalCost = EnergyManager.Instance.GetTaskCost(
            baseCost, true);

        messageText.text =  "It's peak hour. The grid is under strain. " + $"This task will cost {finalCost:0} energy instead of " + $"{baseCost:0}. " + "Do you want to do it anyway?";

        panel.SetActive(true);
    }

    private void Confirm()
    {
        panel.SetActive(false);

        Action callback = onConfirm;
        ClearCallbacks();

        callback?.Invoke();
    }

    private void Cancel()
    {
        panel.SetActive(false);

        Action callback = onCancel;
        ClearCallbacks();

        callback?.Invoke();
    }

    private void ClearCallbacks()
    {
        onConfirm = null;
        onCancel = null;
    }
}
