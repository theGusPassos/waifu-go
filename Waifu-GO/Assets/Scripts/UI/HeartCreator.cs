using UnityEngine;

namespace WaifuGO.UI
{
    public class HeartCreator : MonoBehaviour
    {
        public GameObject parent;

        public GameObject heart;

        public float    timeToAppear;
        private float   timer;

        public float maxX;
        public float minX;

        private void Awake()
        {
            timer = timeToAppear;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > timeToAppear)
            {
                timer = 0;
                CreateHeart();
            }
        }

        private void CreateHeart()
        {
            GameObject h = Instantiate(heart, parent.transform);

            float scale = Random.Range(0.4f, 1.4f);
            h.transform.localScale = new Vector3(scale, scale, 1);

            h.transform.localPosition = new Vector3(
                Random.Range(minX, maxX),
                -800,
                transform.position.z
                );
        }
    }
}
