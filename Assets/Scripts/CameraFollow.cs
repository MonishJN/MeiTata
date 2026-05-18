using UnityEngine;
public class CameraFollow : MonoBehaviour{
    private Transform playerTransform;

    public float targetWidth = 1920f;   // your design resolution width
    public float targetHeight = 1080f;  // your design resolution height

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        AdjustCamera(cam, targetWidth, targetHeight);
    }

    void LateUpdate()
    {
    // Find player if reference is lost (like after a scene load)
        if (playerTransform == null)
        {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
        }

        if (playerTransform != null)
         {
        // Smoothly follow or snap to player (Z must stay at -10)
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
        }
    }
    void AdjustCamera(Camera cam, float targetWidth, float targetHeight)
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = targetWidth / targetHeight;

        if (screenAspect >= targetAspect)
        {
            // Wider screen → fit height
            cam.orthographicSize = targetHeight / 2f;
        }
        else
        {
            // Taller screen → fit width
            float differenceInAspect = targetAspect / screenAspect;
            cam.orthographicSize = (targetHeight / 2f) * differenceInAspect;
        }
    }

}
