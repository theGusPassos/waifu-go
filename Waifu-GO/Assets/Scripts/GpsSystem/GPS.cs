using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WaifuGO.GpsSystem
{
    public class GPS : MonoBehaviour
    {
        public static GPS Instance { get; set; }

        public static bool gpsConnected = false;

        public GameObject   errorMessage;

        //private float longitute;
        //private float latitute;

        private int timeoutSeconds = 30;
        private int resetTime = 7;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine("StartLocationService");
        }

        public float GetLatitute()
        {
            return Input.location.lastData.latitude;
        }

        public float GetLongitute()
        {
            return Input.location.lastData.longitude;
        }

        private void Update()
        {
        }

        private IEnumerator StartLocationService()
        {
            try
            {
                Input.location.Start(1f, 0.1f);

                while (Input.location.status == LocationServiceStatus.Initializing &&
                        timeoutSeconds > 0)
                {
                    yield return new WaitForSeconds(1);
                    timeoutSeconds--;
                }

                if (FoundError())
                    yield break;

                errorMessage.SetActive(false);
                gpsConnected = true;
                yield break;
            }
            finally
            {
            }
        }

        private bool FoundError()
        {
            if (!Input.location.isEnabledByUser)
            {
                ShowErrorMessage("O dispositivo não está permitindo a conexão com o GPS");
                return true;
            }

            if (timeoutSeconds <= 0)
            {
                ShowErrorMessage("Timeout ao se conectar com o sistema de GPS");
                return true;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                ShowErrorMessage("Não foi possível se conctar ao sistema de GPS");
                return true;
            }

            return false;
        }

        private void ShowErrorMessage(string message)
        {
            //StartCoroutine("ResumeAfterError");

            errorMessage.SetActive(true);
            errorMessage.GetComponentInChildren<Text>().text = message;
        }

        private IEnumerator ResumeAfterError()
        {
            while (resetTime > 0)
            {
                resetTime--;
                yield return new WaitForSeconds(1);
            }

            SceneManager.LoadScene("MainMenu");
        }
    }
}