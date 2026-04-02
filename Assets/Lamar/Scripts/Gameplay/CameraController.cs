using UnityEngine;

public class CameraController : MonoBehaviour
{
    // tutorial tutorial: https://youtu.be/H7pjj1K91HE?si=DLhjA9as8rwV4ydf
    [SerializeField] private float panSpeed = 1f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float pinchZoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private Transform backgroundParent;

    private Vector3 lastMousePosition;
    private Camera cam;
    private Bounds backgroundBounds;

    void Start()
    {
        cam = GetComponent<Camera>();
        backgroundBounds = CalculateBackgroundBounds();
    }

    void Update()
    {
        if (GameManager.IsLocked) return;
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

        // scroll wheel zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
            ApplyZoom(-scroll * zoomSpeed);

        // pinch to zoom
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevDistance = Vector2.Distance(t0Prev, t1Prev);
            float currDistance = Vector2.Distance(t0.position, t1.position);

            ApplyZoom((prevDistance - currDistance) * pinchZoomSpeed);
        }

        ClampCameraPosition();
    }

    void ApplyZoom(float delta)
    {
        if (cam.orthographic)
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + delta, minZoom, maxZoom);
        else
            transform.Translate(0, 0, -delta, Space.Self);
    }

    Bounds CalculateBackgroundBounds()
    {
        SpriteRenderer[] renderers = backgroundParent.GetComponentsInChildren<SpriteRenderer>();
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);
        return bounds;
    }

    void ClampCameraPosition()
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        float minX = backgroundBounds.min.x + halfWidth;
        float maxX = backgroundBounds.max.x - halfWidth;
        float minY = backgroundBounds.min.y + halfHeight;
        float maxY = backgroundBounds.max.y - halfHeight;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}