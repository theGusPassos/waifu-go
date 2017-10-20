using UnityEngine;

namespace WaifuGO.Utility
{
    public class ObjectToUp : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.position += new Vector3(0, speed * Time.deltaTime);
        }
    }
}
