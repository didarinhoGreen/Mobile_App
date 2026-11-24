using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Android;

public class GPSManager : MonoBehaviour
{
    [SerializeField] private TMP_Text latitudeText;
    [SerializeField] private TMP_Text longitudeText;
    [SerializeField] private TMP_Text gpsStatusText;

    void Start()
    {
        StartCoroutine(StartGPS());
    }

    private IEnumerator StartGPS()
    {
        // This section only runs on Android builds
        // Checks and requests location permissions
#if UNITY_ANDROID && !UNITY_EDITOR

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermissions(new string[]
            {
                Permission.FineLocation,
                Permission.CoarseLocation
            });

            int permissionWait = 30;

            // Waits for the user to grant permission
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)
                   && permissionWait > 0)
            {
                yield return new WaitForSeconds(1);
                permissionWait--;
            }

            // Stops if precise location permission is not granted
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                gpsStatusText.text = "Precise location permission required";
                yield break;
            }
        }

#endif

        // Checks whether location services are enabled
        if (!Input.location.isEnabledByUser)
        {
            gpsStatusText.text = "Location services unavailable";
            yield break;
        }

        gpsStatusText.text = "Starting GPS...";

        // Starts the GPS service
        Input.location.Start();

        int maxWait = 20;

        // Waits for the GPS service to initialize
        while (Input.location.status == LocationServiceStatus.Initializing
               && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Checks whether the GPS successfully connected
        if (Input.location.status == LocationServiceStatus.Running)
        {
            gpsStatusText.text = "GPS Connected!";
            while (Input.location.status == LocationServiceStatus.Running)
            {
                latitudeText.text = "Latitude: " + Input.location.lastData.latitude;
                longitudeText.text = "Longitude: " + Input.location.lastData.longitude;
                
                yield return new WaitForSeconds(1);
            }
            gpsStatusText.text = "GPS Disconnected";
            Input.location.Stop();
        }

        else if (Input.location.status == LocationServiceStatus.Initializing)
        {
            gpsStatusText.text = "GPS Timed Out";
            Input.location.Stop();
            yield break;
        }
        else
        {
            gpsStatusText.text = "GPS Connection Failed";
            Input.location.Stop();
            yield break;
        }
    }
}