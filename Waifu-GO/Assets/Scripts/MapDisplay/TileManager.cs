using System;
using System.Collections;
using UnityEngine;
using WaifuGo.Managers;
using WaifuGo.SideProject;
using WaifuGO.GameManager;
using WaifuGO.GpsSystem;

namespace WaifuGO.MapDisplay
{
    public class TileManager : MonoBehaviour
    {
        private static bool showMap = false;

        [SerializeField]
        private MapSettings mapSettings;
        [SerializeField]
        private Transform   character;

        private GameObject  mapTile;

        private float       longitute;
        private float       latitute;
        public float        longituteStandard = -73.7638f;
        public float        latituteStandard  = 42.6564f;

        // Hack
        public MonsterManager monsterManger;
        public WaifuGenerator waifuManager;

        private IEntityManager iMonsterManager;

        public float GetLatitute
        {
            get
            {
                return latitute;
            }
        }

        public float GetLongitute
        {
            get
            {
                return longitute;
            }
        }

        private void Start()
        {
            if (waifuManager != null)
                iMonsterManager = waifuManager;
            else
                iMonsterManager = monsterManger;

            StartMap();

            StartCoroutine(LoadTiles(mapSettings.zoom));
        }

        private void StartMap()
        {
            mapTile = GameObject.CreatePrimitive(PrimitiveType.Plane);
            mapTile.name = "World Map";
            mapTile.transform.localScale = Vector3.one * mapSettings.scale;

            iMonsterManager.UpdateEntitiesPosition();

            mapTile.GetComponent<Renderer>().material = mapSettings.material;
            mapTile.transform.parent = transform;

            showMap = true; 
        }

        private void Update()
        {
            //print("gps: " + latitute);
            //print("gps: " + longitute);

            character.position = Vector3.Lerp(
                character.position, 
                new Vector3(0, 0.25f, 0),
                2f * Time.deltaTime
                );

            if (GPS.gpsConnected)
            {
                latitute  = GPS.Instance.GetLatitute();
                longitute = GPS.Instance.GetLongitute();
            }
            else
            {
                latitute  = latituteStandard;
                longitute = longituteStandard;
            }
        }

        private IEnumerator LoadTiles(int zoom)
        {
            while (showMap)
            {
                WWW mapApiWebpage = new WWW(GetApiCurrentUrl());
                yield return mapApiWebpage;

                mapTile.GetComponent<Renderer>().material.mainTexture = mapApiWebpage.texture;

                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// Accessing the web api with the current longitute and latitute
        /// </summary>
        private string GetApiCurrentUrl()
        {
            return String.Format(
                "https://api.mapbox.com/v4/mapbox.{5}/{0},{1},{2}/{3}x{3}@2x.png?access_token={4}",
                longitute,
                latitute,
                mapSettings.zoom,
                mapSettings.size,
                mapSettings.apiToken,
                mapSettings.style
                );
        }
    }
    
    [Serializable]
    public class MapSettings
    {
        public Material material;
        public int      zoom = 18;
        public int      size = 640;
        public float    scale = 1f;
        public string   apiToken;
        public string   style = "pencil";
    }
}
