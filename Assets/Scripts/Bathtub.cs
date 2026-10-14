using System.Collections;
using UnityEngine;

public class Bathtub : MonoBehaviour, IInteractable
{
    public bool IsBeingUsed { get; private set; }
    public string BathID { get; private set; }

    [SerializeField] private Animator bathtubAnimator;
    [SerializeField] private GameObject player;
    [SerializeField] private float energyCost = 20f;
    [SerializeField] private bool requiresElectricity = true;
    [SerializeField] private bool countsAsTask = true;
    [SerializeField] private TaskManager taskManager;
    public float EnergyCost => energyCost;
    public bool RequiresElectricity => requiresElectricity;
    public bool IsCompleted { get; private set; }

    public bool CanInteract()
    {
        return !IsBeingUsed && !IsCompleted;
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
                StartCoroutine(BathingCoroutine());
            }
        );
    }


    private IEnumerator BathingCoroutine()
    {
        IsBeingUsed = true;
        player.SetActive(false);
        bathtubAnimator.Play("BathtubAnimation", 0, 0f);

        yield return null;

        AnimatorStateInfo stateInfo = bathtubAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);
        player.SetActive(true);
        IsBeingUsed = false;
    }
}