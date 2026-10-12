using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RequestFullscreen();
#endif

    public void StartGame()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        RequestFullscreen();
#endif

        SceneManager.LoadScene(1);
    }
}