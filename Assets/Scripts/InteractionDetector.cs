
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange;

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private TMP_Text costText;

    [Header("Visuals")]
    [SerializeField] private Color normalCostColor = Color.white;
    [SerializeField] private Color peakHourCostColor = Color.red;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        interactionIcon.SetActive(false);

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (PeakHourDecisionUI.Instance != null &&
            PeakHourDecisionUI.Instance.IsOpen)
            return;

        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            TryInteractAtScreenPosition(
                touch.position.ReadValue()
            );
        }
    }

    private void LateUpdate()
    {
        RefreshInteractable();

        if (interactableInRange != null)
            UpdateInteractionCost();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (PeakHourDecisionUI.Instance != null &&
            PeakHourDecisionUI.Instance.IsOpen)
            return;

        if (!context.performed)
            return;

        if (context.control.device is Keyboard)
        {
            interactableInRange?.Interact();
            return;
        }

        if (context.control.device is Mouse &&
            Mouse.current != null)
        {
            TryInteractAtScreenPosition(
                Mouse.current.position.ReadValue()
            );
            return;
        }

        if (context.control.device is Touchscreen &&
            Touchscreen.current != null)
        {
            TryInteractAtScreenPosition(
                Touchscreen.current.primaryTouch.position.ReadValue()
            );
        }
    }

    public void RefreshInteractable()
    {
        if (interactableInRange == null)
            return;

        if (!interactableInRange.CanInteract())
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }

    private void TryInteractAtScreenPosition(Vector2 screenPosition)
    {
        if (mainCamera == null)
            return;

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(screenPosition);

        Collider2D[] hits =
            Physics2D.OverlapPointAll(worldPosition);

        foreach (Collider2D hit in hits)
        {
            IInteractable interactable =
                hit.GetComponentInParent<IInteractable>();

            if (interactable == null ||
                !interactable.CanInteract())
                continue;

            interactable.Interact();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable =
            collision.GetComponentInParent<IInteractable>();

        if (interactable == null ||
            !interactable.CanInteract())
            return;

        interactableInRange = interactable;
        interactionIcon.SetActive(true);

        UpdateInteractionCost();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable =
            collision.GetComponentInParent<IInteractable>();

        if (interactable == null ||
            interactable != interactableInRange)
            return;

        interactableInRange = null;
        interactionIcon.SetActive(false);
    }

    private void UpdateInteractionCost()
    {
        if (interactableInRange == null ||
            costText == null ||
            EnergyManager.Instance == null)
            return;

        bool requiresElectricity =
            interactableInRange.RequiresElectricity;

        float baseCost =
            interactableInRange.EnergyCost;

        float finalCost =
            EnergyManager.Instance.GetTaskCost(
                baseCost,
                requiresElectricity
            );

        costText.text = Mathf.RoundToInt(finalCost).ToString();

        bool peakCostApplies =
            requiresElectricity &&
            EnergyManager.Instance.IsPeakHour;

        costText.color = peakCostApplies
            ? peakHourCostColor
            : normalCostColor;
    }
}
