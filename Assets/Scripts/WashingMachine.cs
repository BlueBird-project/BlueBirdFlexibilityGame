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

    public float EnergyCost => energyCost;
    public bool RequiresElectricity => requiresElectricity;
    public bool IsCompleted { get; private set; }

    void Start()
    {

    }

    void Update()
    {

    }

    public bool CanInteract()
    {
        return !IsOn && !IsCompleted;
    }

    public void MarkCompleted()
    {
        IsCompleted = true;
    }


    public void Interact()
    {
        if (!CanInteract())
            return;

        if (countsAsTask &&
            (taskManager == null || !taskManager.CanCompleteTask()))
            return;

        EnergyManager.Instance.RequestEnergyTask(
            energyCost,
            requiresElectricity,
            () =>
            {
                if (countsAsTask)
                {
                    bool completed = taskManager.CompleteTask();

                    if (!completed)
                        return;
                }

                EnergyManager.Instance.OnEnergyTaskCompleted(requiresElectricity);

                MarkCompleted();
                TurnOnWashingmachine();
            }
        );
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
