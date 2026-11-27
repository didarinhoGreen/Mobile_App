using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class MapButtons : MonoBehaviour
{
    [SerializeField] GameObject InfoPanel;
    [SerializeField] Image BuildingImage;
    [SerializeField] TMP_Text TitleText;
    [SerializeField] float TapMovement = 20f;
    [SerializeField] Transform User;
    [SerializeField] RouteLine RouteLine;

    Camera Camera;
    PlaceOFInterest currentPlace;
    Vector2 position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {

        var pointer = Pointer.current;
        if (pointer == null) return;
        if (pointer.press.wasPressedThisFrame) position = pointer.position.ReadValue();

        if (pointer.press.wasReleasedThisFrame)
        {
            Vector2 releasePosition = pointer.position.ReadValue();
            if (Vector2.Distance(position, releasePosition) > TapMovement) return;
            if (InfoPanel.activeSelf) return;

            TryTap(releasePosition);
        }
    }
    void TryTap(Vector2 screenPosition)
    {
       
            Ray ray = Camera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceOFInterest place = hit.collider.GetComponent<PlaceOFInterest>();
                if (place != null)
                    Open(place);
            }
      
    }
    void Open(PlaceOFInterest place)
        {
           currentPlace = place;
            TitleText.text = place.PlaceName;
            BuildingImage.sprite = place.picture;
            InfoPanel.SetActive(true);
        }
    public void SetDestination()
    {
        if (currentPlace == null) return;

        RouteLine.ShowRoute(User, currentPlace.transform);
        InfoPanel.SetActive(false);
    }
    }