
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Android;

public class GPSManager : MonoBehaviour
{
    [SerializeField] private TMP_Text latitudeText;
    [SerializeField] private TMP_Text longitudeText;
    [SerializeField] private TMP_Text gpsStatusText;
    [SerializeField] private TMP_Text accuracyText;

    private Coroutine gpsCoroutine;

    void Start()
    {
        gpsStatusText.text = "GPS Not Connected";
        ResetCoordinates();
    }

    // Starts GPS when the user presses the Start button
    public void StartTracking()
    {
        if (gpsCoroutine == null)
        {
            gpsCoroutine = StartCoroutine(StartGPS());
        }
    }

    // Stops GPS when the user presses the Stop button
    public void StopTracking()
    {
        if (gpsCoroutine != null)
        {
            StopCoroutine(gpsCoroutine);
            gpsCoroutine = null;
        }

        Input.location.Stop();

        gpsStatusText.text = "GPS Tracking Stopped";

        ResetCoordinates();
    }

    private IEnumerator StartGPS()
    {
        // Android location permissions
        #if UNITY_ANDROID && !UNITY_EDITOR

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            gpsStatusText.text = "Waiting for location permission...";

            Permission.RequestUserPermissions(new string[]
            {
                Permission.FineLocation,
                Permission.CoarseLocation
            });
             int permissionWait = 30;
             
             while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation) && permissionWait > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                permissionWait--;
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                gpsStatusText.text = "Precise location permission required";
                gpsCoroutine = null;
                yield break;
            }
        }

        #endif

        // Checks whether location services are enabled
        if (!Input.location.isEnabledByUser)
        {
            gpsStatusText.text = "Location services unavailable";
            gpsCoroutine = null;
            yield break;
        }

        gpsStatusText.text = "Starting GPS...";

        // Starts GPS with desired accuracy and update distance
        Input.location.Start(5f, 1f);

        int maxWait = 20;

        // Waits for GPS initialization
        while (Input.location.status == LocationServiceStatus.Initializing
               && maxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWait--;
        }

        // Checks whether GPS successfully connected
        if (Input.location.status == LocationServiceStatus.Running)
        {
            gpsStatusText.text = "GPS Connected!";

            // Continuously updates location information
            while (Input.location.status == LocationServiceStatus.Running)
            {
                LocationInfo currentLocation = Input.location.lastData;

                latitudeText.text = "Latitude: " + currentLocation.latitude.ToString("F6");

                longitudeText.text = "Longitude: " + currentLocation.longitude.ToString("F6");

                accuracyText.text = "Accuracy: " + currentLocation.horizontalAccuracy.ToString("F1") + " m";

                yield return new WaitForSecondsRealtime(1);
            }

            // Handles unexpected GPS disconnection
            gpsStatusText.text = "GPS Disconnected";
        }
        else if (Input.location.status == LocationServiceStatus.Initializing)
        {
            gpsStatusText.text = "GPS Timed Out";
        }
        else
        {
            gpsStatusText.text = "GPS Connection Failed";
        }

        Input.location.Stop();
        ResetCoordinates();
        gpsCoroutine = null;
    }

    // Resets displayed location information
    private void ResetCoordinates()
    {
        latitudeText.text = "Latitude: --";
        longitudeText.text = "Longitude: --";
        accuracyText.text = "Accuracy: -- m";
    }

    // Stops tracking when the component is disabled
    private void OnDisable()
    {
        if (gpsCoroutine != null)
        {
            StopCoroutine(gpsCoroutine);
            gpsCoroutine = null;
        }

        Input.location.Stop();
    }
}
