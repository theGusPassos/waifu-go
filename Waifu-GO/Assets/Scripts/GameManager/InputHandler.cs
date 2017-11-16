using UnityEngine;

namespace WaifuGO.GameManager
{
    public class InputHandler : MonoBehaviour
    {
        Vector3 touchPosWorld;

        Ray ray;
        RaycastHit hit;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                int layer_mask = LayerMask.GetMask("waifus");

                if (Physics.Raycast(ray, out hit, 100000, layer_mask))
                {
                    hit.collider.GetComponent<Waifu>().TalkTo();
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

                

                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                if (hitInformation.collider != null)
                {
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    Debug.Log("Touched " + touchedObject.transform.name);
                }
            }
        }
    }
}
