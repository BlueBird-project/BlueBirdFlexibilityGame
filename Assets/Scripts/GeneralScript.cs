using UnityEngine;

public class GeneralScript : MonoBehaviour
{
    public static GeneralScript Instance;

    public int CurrentDay { get; private set; } = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void CompleteDay()
    {
        Debug.Log($"Day {CurrentDay} completed!");

        // Más adelante:
        // CurrentDay++;
        // ResetEnergy();
        // ResetTasks();
        // LoadNextDay();
    }
}