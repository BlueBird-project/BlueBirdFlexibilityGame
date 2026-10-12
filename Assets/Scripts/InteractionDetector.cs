using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;

    public GameObject interactionIcon;

    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        interactionIcon.SetActive(false);

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            Vector2 position = touch.position.ReadValue();

            TryInteractAtScreenPosition(position);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (context.control.device is Keyboard)
        {
            interactableInRange?.Interact();

            return;
        }

        if (context.control.device is Mouse)
        {
            Vector2 screenPosition =
                Mouse.current.position.ReadValue();

            TryInteractAtScreenPosition(screenPosition);

            return;
        }

        if (context.control.device is Touchscreen)
        {
            Vector2 screenPosition =
                Touchscreen.current.primaryTouch.position.ReadValue();

            TryInteractAtScreenPosition(screenPosition);

            return;
        }
    }

    private void TryInteractAtScreenPosition(Vector2 screenPosition)
    {
        if (mainCamera == null)
            return;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPosition);

        if (hits.Length == 0)
        {
            return;
        }


        foreach (Collider2D hit in hits)
        {
            IInteractable interactable =
                hit.GetComponentInParent<IInteractable>();

            if (interactable == null)
                continue;

            interactable.Interact();

            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(
                out IInteractable interactable) &&
            interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(
                out IInteractable interactable) &&
            interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}