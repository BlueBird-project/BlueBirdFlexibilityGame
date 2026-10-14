
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sadMoveSpeed = 1.5f;

    [Header("Sprite Libraries")]
    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private SpriteLibraryAsset playerLibrary;
    [SerializeField] private SpriteLibraryAsset sadPlayerLibrary;

    [Header("Pollution")]
    [SerializeField] private PollutionFade pollutionFade;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    private bool isSad;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (spriteLibrary == null)
            spriteLibrary = GetComponent<SpriteLibrary>();

        SetSadMode(false);
    }

    void Update()
    {
        if (PauseController.IsGamePaused)
        {
            if (rb.linearVelocity != Vector2.zero)
            {
                rb.linearVelocity = Vector2.zero;
                StopMovementAnimations();
            }

            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
        animator.SetBool(
            "isWalking",
            rb.linearVelocity.magnitude > 0
        );
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            StopMovementAnimations();
        }
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    void StopMovementAnimations()
    {
        animator.SetBool("isWalking", false);
        animator.SetFloat("LastInputX", moveInput.x);
        animator.SetFloat("LastInputY", moveInput.y);
    }

    public void SetSadMode(bool sad)
    {
        isSad = sad;

        moveSpeed = isSad ? sadMoveSpeed : 3f;

        if (spriteLibrary != null)
        {
            spriteLibrary.spriteLibraryAsset =
                isSad ? sadPlayerLibrary : playerLibrary;
        }

        if (isSad && pollutionFade != null)
        {
            pollutionFade.ShowPollution();
        }
    }
}
