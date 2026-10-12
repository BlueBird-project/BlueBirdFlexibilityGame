using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TMP_Text energyText;

    private void Update()
    {
        energyText.text = Mathf.RoundToInt(EnergyManager.Instance.DailyFuel).ToString();
    }
}