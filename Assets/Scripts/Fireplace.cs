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

        TurnFireOn();
    }

    public void TurnFireOn()
    {
        SetFireOn(true);
    }

    public void SetFireOn(bool turnedOn)
    {
        if (IsOn = turnedOn)
        {
            FireAnimator.Play("FireAnimation");
        }
    }
}
