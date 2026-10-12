using UnityEngine;

public class WashingMachine : MonoBehaviour, IInteractable
{
    public bool IsOn { get; private set; }
    public string WashingMachineID { get; private set; }
    public Animator WashingMachineAnimator;

    [SerializeField] private float energyCost = 20f;
    [SerializeField] private bool requiresElectricity = true;
    [SerializeField] private bool countsAsTask = true;
    [SerializeField] private TaskManager taskManager;
    void Start()
    {

    }

    void Update()
    {

    }

    public bool CanInteract()
    {
        return !IsOn;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        if (requiresElectricity &&
        !EnergyManager.Instance.CanPerformEnergyTask())
        {
            return;
        }

        if (!EnergyManager.Instance.TryConsume(energyCost))
        {
            Debug.Log("Not enough energy!");
            return;
        }

        if (countsAsTask)
        {
            bool taskCompleted = taskManager.CompleteTask();

            if (taskCompleted && requiresElectricity)
            {
                EnergyManager.Instance.SetPeakHour(true);
            }
        }

        TurnOnWashingmachine();
    }

    public void TurnOnWashingmachine()
    {
        SetMachineOn(true);
    }

    public void SetMachineOn(bool turnedOn)
    {
        if (IsOn = turnedOn)
        {
            WashingMachineAnimator.Play("WashingMachineAnimation");
        }
    }
}
