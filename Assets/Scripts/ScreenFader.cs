using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using Unity.Cinemachine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] CinemachineFollow transposer;

    Vector3 originalDamping;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        originalDamping = transposer.TrackerSettings.PositionDamping;
    }

    async Task Fade(float targetTransparency)
    {
        float start = canvasGroup.alpha, t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparency, t / fadeDuration);
            await Task.Yield();
        }
        canvasGroup.alpha = targetTransparency;
    }

    public async Task FadeOut()
    {
        await Fade(1f);
        SetDamping(Vector3.zero);
    }

    public async Task FadeIn()
    {
        await Fade(0f);
        SetDamping(originalDamping);
    }

    void SetDamping(Vector3 d)
    {
        if (!transposer) return;
        transposer.TrackerSettings.PositionDamping = d;
    }
}
