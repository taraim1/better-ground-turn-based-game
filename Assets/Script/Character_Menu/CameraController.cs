using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject bgPrefab; 
    public Camera bgCamera; 
    public Camera MainCamera; 
    private AudioListener mainCameraAudioListener;
    private AudioListener bgCameraAudioListener;

    void Start()
    {
        mainCameraAudioListener = MainCamera.GetComponent<AudioListener>();
        bgCameraAudioListener = bgCamera.GetComponent<AudioListener>();

        if (mainCameraAudioListener != null)
        {
            mainCameraAudioListener.enabled = true; 
        }

        if (bgCameraAudioListener != null)
        {
            bgCameraAudioListener.enabled = false; 
        }
    }
    void Update()
    {
        if (bgPrefab.activeInHierarchy)
        {
            bgCamera.gameObject.SetActive(true);
            MainCamera.gameObject.SetActive(false);
            if (mainCameraAudioListener != null)
            {
                mainCameraAudioListener.enabled = false; 
            }
            if (bgCameraAudioListener != null)
            {
                bgCameraAudioListener.enabled = true; 
            }
        }
        else
        {
            bgCamera.gameObject.SetActive(false);
            MainCamera.gameObject.SetActive(true);
            if (mainCameraAudioListener != null)
            {
                mainCameraAudioListener.enabled = true; 
                if (bgCameraAudioListener != null)
                {
                    bgCameraAudioListener.enabled = false; 
                }
            }
        }
    }
}
