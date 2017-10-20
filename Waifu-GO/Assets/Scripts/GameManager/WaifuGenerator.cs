using UnityEngine;

namespace WaifuGO.GameManager
{
    public class WaifuGenerator : MonoBehaviour
    {
        public GameObject parentObject;

        public GameObject[] waifus;

        public float timeToWaifuAppear;
        private float timer;

        public float maxX;
        public float minX;
        public float maxY;
        public float minY;

        private void Awake()
        {
            timer = timeToWaifuAppear;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > timeToWaifuAppear)
            {
                timer = 0;
                GameObject waifu = Instantiate(waifus[Random.Range(0, waifus.Length)], parentObject.transform);
                waifu.transform.localPosition = GetRandomPos();
            }
        }

        private Vector2 GetRandomPos()
        {
            return new Vector2(
                Random.Range(minX, maxX), Random.Range(minY, maxY)
                );
        }
    }
}
