using UnityEngine;
using System.Collections;

public class HairDryer : MonoBehaviour, IInteractable
{
    public bool IsBeingUsed { get; private set; }
    public string HairDryerID { get; private set; }
    public Animator HairDryerAnimator;
    [SerializeField] private GameObject player;

    [SerializeField] private float energyCost = 20f;
    [SerializeField] private bool requiresElectricity = true;
    [SerializeField] private bool countsAsTask = true;
    [SerializeField] private TaskManager taskManager;

    public bool CanInteract()
    {
        return !IsBeingUsed;
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

        StartCoroutine(DryHairCoroutine());
    }

    private IEnumerator DryHairCoroutine()
    {
        IsBeingUsed = true;
        player.SetActive(false);
        HairDryerAnimator.Play("HairDryerAnimation", 0, 0f);

        yield return null;

        AnimatorStateInfo stateInfo = HairDryerAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        player.SetActive(true);
        IsBeingUsed = false;
    }
}
