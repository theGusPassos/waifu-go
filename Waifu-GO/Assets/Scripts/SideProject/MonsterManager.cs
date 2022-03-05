using System;
using System.Collections.Generic;
using UnityEngine;
using WaifuGo.Managers;
using WaifuGO.MapDisplay;

namespace WaifuGo.SideProject
{
    public class MonsterManager : MonoBehaviour, IEntityManager
    {
        private TileManager tileManager;

        [SerializeField]
        private float waitSpawnTime;
        [SerializeField]
        private float minIntervalTime;
        [SerializeField]
        private float maxIntervaltime;

        public GameObject[] allMonsters;

        private List<Monster> monsters = new List<Monster>();

        private void Start()
        {
            tileManager = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileManager>();
        }

        private void Update()
        {
            if (waitSpawnTime < Time.time)
            {
                //tempo for maior que 5, um novo tempo limite.. 
                //...é defino e ocorre Spawm de inimigo	
                waitSpawnTime = Time.time + UnityEngine.Random.Range(minIntervalTime, maxIntervaltime);
                SpawnMonster();
            }
        }

        private void SpawnMonster()
        {
            float newLat = tileManager.GetLatitute + UnityEngine.Random.Range(-0.00005f, 0.00005f);
            float newLon = tileManager.GetLongitute + UnityEngine.Random.Range(-0.00005f, 0.00005f);

            int monsterId = UnityEngine.Random.Range(0, allMonsters.Length);
            Monster monster = Instantiate(allMonsters[monsterId], Vector3.zero, Quaternion.identity).GetComponent<Monster>();

            monster.tileManager = tileManager;
            monster.Init(newLat, newLon);
            monsters.Add(monster);
        }

        public void UpdateEntitiesPosition()
        {
            if (monsters.Count == 0)
                return;

            Monster[] monster = monsters.ToArray();

            for (int i = 0; i < monster.Length; i++)
            {
                monster[i].UpdatePosition();
            }
        }
    }
}
