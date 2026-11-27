using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RouteLine : MonoBehaviour
{
    [SerializeField] float heightAboveMap = 0.1f;

    LineRenderer line;
    Transform start;
    Transform destination;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;      
        line.enabled = false;        
    }

    public void ShowRoute(Transform from, Transform to)
    {
        start = from;
        destination = to;
        line.enabled = true;
    }

    public void HideRoute()
    {
        start = null;
        destination = null;
        line.enabled = false;
    }

    void Update()
    {
        if (start == null || destination == null) return;

        line.SetPosition(0, OnMap(start.position));
        line.SetPosition(1, OnMap(destination.position));
    }

    Vector3 OnMap(Vector3 position)
    {
        position.y = heightAboveMap;   
        return position;
    }
}