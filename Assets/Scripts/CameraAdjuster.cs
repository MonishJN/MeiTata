using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAdjuster : MonoBehaviour {
    [SerializeField] private int baseCameraSize; 
    private Camera cam;

    void Awake() {
        cam = GetComponent<Camera>();
        AdjustCamera();
    }

    
    void AdjustCamera() {
        float screenAspect = (float)Screen.width / Screen.height;
        float referenceAspect = 16f / 9f;

        // Scale based on inspector-set baseCameraSize
       if (screenAspect < referenceAspect) {
            cam.orthographicSize = baseCameraSize * (referenceAspect / screenAspect);
        } else {
            cam.orthographicSize = baseCameraSize;
        }
    }

}
