using System.Collections;
using UnityEngine;

namespace WaifuGO.GpsSystem
{
    public class GPS : MonoBehaviour
    {
        public static GPS Instance { get; set; }

        public static bool gpsConnected = false;

        public float longitute;
        public float latitute;

        private int timeoutSeconds = 30;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine("StartLocationService");
        }

        private void Update()
        {
            if (gpsConnected)
            {
                print("is enabled: " + Input.location.isEnabledByUser);

                print("longitute: " + Input.location.lastData.longitude);
                print("latitute: " + Input.location.lastData.latitude);
            }
        }

        private IEnumerator StartLocationService()
        {
            Input.location.Start(1f, 0.1f);

            while (
                Input.location.status == LocationServiceStatus.Initializing &&
                timeoutSeconds > 0)
            {
                yield return new WaitForSeconds(1);
                timeoutSeconds--;
            }
            
            if (timeoutSeconds <= 0)
            {
                HandleTimeOut();
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                HandleFailedStatus();
                yield break;
            }
            else
            {
                gpsConnected = true;
                yield break;
            }
        }

        private void HandleTimeOut()
        {
            print("time out");
        }

        private void HandleFailedStatus()
        {
            print("unable to determine device location");
        }
    }
}