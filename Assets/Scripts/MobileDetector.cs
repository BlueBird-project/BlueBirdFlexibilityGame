using System.Runtime.InteropServices;
using UnityEngine;

public class MobileDetector : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    [DllImport("__Internal")]
    private static extern bool IsLandscape();
#endif

    public GameObject joysticks;
    public GameObject rotateDevicePanel;

    private bool lastLandscapeState;

    void Start()
    {
        if (IsRunningOnMobile())
        {
            joysticks.SetActive(true);

            lastLandscapeState = IsDeviceLandscape();
            UpdateOrientation(lastLandscapeState);
        }
        else
        {
            joysticks.SetActive(false);

            if (rotateDevicePanel != null)
                rotateDevicePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsRunningOnMobile())
            return;

        bool currentLandscapeState = IsDeviceLandscape();

        if (currentLandscapeState != lastLandscapeState)
        {
            lastLandscapeState = currentLandscapeState;
            UpdateOrientation(currentLandscapeState);
        }
    }

    void UpdateOrientation(bool landscape)
    {
        if (rotateDevicePanel != null)
        {
            rotateDevicePanel.SetActive(!landscape);
        }

        if (joysticks != null)
        {
            joysticks.SetActive(landscape);
        }
    }

    bool IsRunningOnMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobile();
#else
        return false;
#endif
    }

    bool IsDeviceLandscape()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsLandscape();
#else
        return Screen.width > Screen.height;
#endif
    }
}