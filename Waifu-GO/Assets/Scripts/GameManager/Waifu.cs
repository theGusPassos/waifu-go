using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WaifuGO.Sound;

namespace WaifuGO.GameManager
{
    public class Waifu : MonoBehaviour
    {
        public string name;
        public string location;
        
        private static string[] possibleLocations = { "beach", "roadSun", "village" };

        public void TalkTo()
        {
            SoundManager.instance.StartWaifuInteraction();

            WaifuManager.currentWaifu = name;
            WaifuManager.currentLocation = GetLocation();
            SceneManager.LoadScene("WaifuDialog");
        }

        private string GetLocation()
        {
            if (string.IsNullOrEmpty(location))
            {
                return possibleLocations[Random.Range(0, possibleLocations.Length)];
            }

            return location;
        }
    }
}
