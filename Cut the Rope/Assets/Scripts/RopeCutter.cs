using UnityEngine;

namespace Player
{
    public class RopeCutter : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            //If left mouse button is clicked cast out a ray to cut the rope
            if (Input.GetMouseButton(0))
            {
                //Spawn a raycast at the mouse position as a point
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                //If raycast hit link destroy game object
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Link")
                    {
                        Destroy(hit.collider.gameObject);
                        Destroy(hit.transform.parent.gameObject, 1.15f);
                    }
                }
            }
        }
    }
}