
using System.Collections;
using UnityEngine;

public class PollutionFade : MonoBehaviour
{
    [Header("Pollution")]
    [SerializeField] private GameObject pollutionGroup;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1.5f;

    [SerializeField] private float delayBeforeFade = 1f;

    private SpriteRenderer[] renderers;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (pollutionGroup == null)
        {
            Debug.LogWarning("Pollution group is not assigned.");
            return;
        }

        renderers = pollutionGroup
            .GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }
    }

    public void ShowPollution()
    {
        if (pollutionGroup == null || renderers == null)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        pollutionGroup.SetActive(true);

        foreach (SpriteRenderer sr in renderers)
        {
            sr.gameObject.SetActive(true);

            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }

        fadeCoroutine = StartCoroutine(FadeIn());
    }


    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            foreach (SpriteRenderer sr in renderers)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            yield return null;
        }

        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }

        fadeCoroutine = null;
    }

}
