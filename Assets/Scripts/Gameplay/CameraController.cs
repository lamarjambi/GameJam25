using UnityEngine;

public class CameraController : MonoBehaviour
{
    // credit tutorial: https://youtu.be/H7pjj1K91HE?si=DLhjA9as8rwV4ydf
    [SerializeField] private float panSpeed = 1f;
    
    private Vector3 lastMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        // while right-click, pan camera
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * panSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}