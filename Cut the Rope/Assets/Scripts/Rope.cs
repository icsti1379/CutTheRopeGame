using UnityEngine;

namespace Player
{
    public class Rope : MonoBehaviour
    {
        //Reference to the hook
        public Rigidbody2D hook;

        //Reference to the link prefab
        public GameObject linkPrefab;

        //Reference to the ball (weight)
        public Weight weigth;

        //Amount of links
        public int links = 7;

        /// <summary>
        /// Call generate rope method
        /// </summary>
        void Start()
        {
            GenerateRope();
        }

        /// <summary>
        /// Method for generating ropes
        /// </summary>
        void GenerateRope()
        {
            Rigidbody2D previousRB = hook;

            for (int i = 0; i < links; i++)
            {
                //Reference prefabs to GameObjects, HingeJoints
                GameObject link = Instantiate(linkPrefab, transform);
                HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
                joint.connectedBody = previousRB;

                if (i < links - 1)
                {
                    previousRB = link.GetComponent<Rigidbody2D>();
                }
                else
                {
                    weigth.ConnectRopeEnd(link.GetComponent<Rigidbody2D>());
                }
            }
        }
    }
}