using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class MapController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] float MinZoom = 1.5f;
    [SerializeField] float MaxZoom = 10f;
    [SerializeField] float PinchZoomSpeed = 0.01f;
    [SerializeField] float ScrollZoomSpeed = 0.005f;

    [Header("Map Edges")]
    [SerializeField] Vector2 MapMin = new Vector2(-10f, -10f);
    [SerializeField] Vector2 MapMax = new Vector2(10f, 10f);

    [Header("Dont Move Map")]
    [SerializeField] GameObject infoPanel;

    Camera Camera;
    bool IsPinching;
    float LastPinchDistanse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (infoPanel != null && infoPanel.activeSelf) return;
        var touchscreen = Touchscreen.current;
        if (touchscreen != null && touchscreen.touches[1].isInProgress)
        {
            HandlePinch(touchscreen);
        }
        else
        {
            IsPinching = false;
            var pointer = Pointer.current;
            if (pointer != null && pointer.press.isPressed)
                Pan(pointer.delta.ReadValue());
            if (Mouse.current != null)
                Zoom(Mouse.current.scroll.ReadValue().y * ScrollZoomSpeed);
        }

        KeepInsideMap();
    
    }

    void HandlePinch(Touchscreen touchscreen)
    {
        Vector2 finger1 = touchscreen.touches[0].position.ReadValue();
        Vector2 finger2 = touchscreen.touches[1].position.ReadValue();
        float distance = Vector2.Distance(finger1, finger2);

        if (!IsPinching)
        {
            IsPinching = true;             
        }
        else
        {
            float change = distance - LastPinchDistanse;   
            Zoom(change * PinchZoomSpeed);
        }

        LastPinchDistanse = distance;
    }
    void Zoom(float amount)
    {
        Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize - amount, MinZoom, MaxZoom);
    }
    void Pan(Vector2 screenDelta)
    {
        float unitsPerPixel = Camera.orthographicSize * 2f / Screen.height;
        Vector3 move = new Vector3(-screenDelta.x, 0f, -screenDelta.y) * unitsPerPixel;
        transform.position += move;
    }
    void KeepInsideMap()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, MapMin.x, MapMax.x);
        pos.z = Mathf.Clamp(pos.z, MapMin.y, MapMax.y);
        transform.position = pos;
    }
}
