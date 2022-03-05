using System.Collections.Generic;
using UnityEngine;
using WaifuGo.Managers;
using WaifuGO.MapDisplay;

namespace WaifuGO.GameManager
{
    public class WaifuGenerator : MonoBehaviour, IEntityManager
    {
        public GameObject parentObject;

        public GameObject[] waifus;

        private TileManager tileManager;

        private float timeToWaifuAppear;
        public float maxTimeToWaifu;
        public float minTimeToWaifu;

        private float timer;

        private List<Waifu> waifusInField = new List<Waifu>();

        private void Awake()
        {
            timer = timeToWaifuAppear;

            tileManager = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileManager>();

            timeToWaifuAppear = GetNextTime();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > timeToWaifuAppear)
            {
                timer = 0;
                timeToWaifuAppear = GetNextTime();
                SpawnWaifu();
            }
        }

        private float GetNextTime()
        {
            return Random.Range(minTimeToWaifu, maxTimeToWaifu);
        }

        private void SpawnWaifu()
        {
            float newLat = tileManager.GetLatitute + Random.Range(-0.00005f, 0.00005f);
            float newLon = tileManager.GetLongitute + Random.Range(-0.00005f, 0.00005f);

            int monsterId = Random.Range(0, waifus.Length);
            Waifu waifu = Instantiate(waifus[monsterId], Vector3.zero, waifus[monsterId].transform.rotation).GetComponent<Waifu>();

            waifu.tileManager = tileManager;
            waifu.Init(newLat, newLon);
            waifusInField.Add(waifu);
        }

        public void UpdateEntitiesPosition()
        {
            if (waifusInField.Count == 0)
                return;

            Waifu[] waifuList = waifusInField.ToArray();

            for (int i = 0; i < waifuList.Length; i++)
            {
                waifuList[i].UpdatePosition();
            }
        }
    }
}
