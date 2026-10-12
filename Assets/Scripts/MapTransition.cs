using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPosition;
    CinemachineConfiner2D confiner;

    enum Direction { Up, Down, Left, Right, Teleport }

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FadeTransition(collision.gameObject);

            //MapController_Manual.Instance?.HighlightArea(mapBoundary.name);
            //MapController_Dynamic.Instance?.UpdateCurrentArea(mapBoundary.name);
        }
    }

    async void FadeTransition(GameObject player)
    {
        PauseController.SetPause(true);

        await ScreenFader.Instance.FadeOut();   

        confiner.BoundingShape2D = mapBoundary;
        UpdatePlayerPosition(player);

        await ScreenFader.Instance.FadeIn();

        PauseController.SetPause(false);
    }

    void UpdatePlayerPosition(GameObject player)
    {
        if(direction == Direction.Teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

        Vector3 additivePos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                additivePos.y += 2f;
                break;
            case Direction.Down:    
                additivePos.y += -2f;
                break;
            case Direction.Left:
                additivePos.x += -2f;
                break;
            case Direction.Right:   
                additivePos.x += 2f;
                break;
        }

        player.transform.position = additivePos;
    }
}

