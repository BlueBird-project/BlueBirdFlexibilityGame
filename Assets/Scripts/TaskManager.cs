using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("Day")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private int tasksRequiredPerDay = 2;

    [Header("Task Toggles")]
    [SerializeField] private Toggle[] taskToggles;

    private int completedTasks;

    public int CompletedTasks => completedTasks;

    public bool CompleteTask()
    {
        if (completedTasks >= tasksRequiredPerDay)
            return false;

        foreach (Toggle toggle in taskToggles)
        {
            if (!toggle.isOn)
            {
                toggle.isOn = true;
                completedTasks++;

                Debug.Log(
                    $"Task completed: {completedTasks}/{tasksRequiredPerDay}"
                );

                if (IsDayComplete)
                {
                    GeneralScript.Instance.CompleteDay();
                }

                return true;
            }
        }

        return false;
    }

    public bool IsDayComplete => completedTasks >= tasksRequiredPerDay;
}