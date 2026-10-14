
public interface IInteractable
{
    bool CanInteract();
    void Interact();

    float EnergyCost { get; }
    bool RequiresElectricity { get; }

    bool IsCompleted { get; }
    void MarkCompleted();
}
