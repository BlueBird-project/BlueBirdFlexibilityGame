using UnityEngine;

public class Fireplace : MonoBehaviour , IInteractable
{
    public bool IsOn { get; private set; }
    public string FireID { get; private set; }
    public Animator FireAnimator;

    [SerializeField] private float energyCost = 0f;
    [SerializeField] private bool requiresElectricity = false;
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

                TurnFireOn();
            }
        );
    }


    public void TurnFireOn()
    {
        SetFireOn(true);
    }

    public void SetFireOn(bool turnedOn)
    {
        IsOn = turnedOn;

        if (turnedOn && FireAnimator != null)
            FireAnimator.Play("FireAnimation", 0, 0f);
    }
}
