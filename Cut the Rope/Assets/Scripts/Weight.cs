using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Weight : MonoBehaviour
    {
        #region GameUI
        //Score counter
        private int score;
        //Score counter text
        public Text countText;
        //Quest
        public static int questCompleted;
        #endregion

        #region variables
        //Set the y axes of connected anchor as distance from chain end
        public float distanceFromChainEnd = 0.6f;
        #endregion

        #region Start
        /// <summary>
        /// Start method
        /// </summary>
        public void Start()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            ForceWeight(rb);
            score = 0;
            SetCountText();
        }

        private static void ForceWeight(Rigidbody2D rb)
        {
            rb.mass = 10;
            float mass = rb.mass;

            float inertia = rb.inertia;
            Vector2 velocity = rb.velocity;



        }

        public static float GetEnergy(float speed, float mass)
        {
            return 0.5f * mass * Mathf.Pow(speed, 2);
        }

        public static float GetSpeed(float energy, float mass)
        {
            return Mathf.Sqrt(energy / (0.5f * mass));
        }

        #endregion

        #region RopeGenerator
        /// <summary>
        /// Method to connect the weight to the rope end
        /// </summary>
        public void ConnectRopeEnd(Rigidbody2D endRB)
        {
            //Add a HingeJoint2D game object
            HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();

            //Disable auto configure of connected anchor to set anchor
            joint.autoConfigureConnectedAnchor = false;

            //Connect joint to the end of the rope RigidBody
            joint.connectedBody = endRB;

            //Set both axes of the anchor to zero
            joint.anchor = Vector2.zero;

            //Set axes of connected anchor to x=0 and set the y as distance from chain end
            joint.connectedAnchor = new Vector2(0f, -distanceFromChainEnd);
        }
        #endregion

        /// <summary>
        /// Method for doing sth on trigger enter
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PickUp"))
            {
                other.gameObject.SetActive(false);
                score = score + 1;
                SetCountText();
            }

            if (score >= 3)
            {
                if (other.gameObject.CompareTag("WinCollider"))
                {
                    questCompleted = 1;
                    SetCountText();
                    Debug.Log("QuestCompleted is:" + questCompleted);
                }
            }

            else if (score <= 3 && other.gameObject.CompareTag("WinCollider"))
            {
                questCompleted = 2;
                SetCountText();
                Debug.Log("QuestCompleted is:" + questCompleted);
            }

            else if (other.gameObject.CompareTag("OutOfSceneCollider"))
            {
                questCompleted = 2;
                SetCountText();
                Debug.Log("QuestCompleted is:" + questCompleted);
            }
        }

        /// <summary>
        /// Set the text of the UI
        /// </summary>
        public void SetCountText()
        {
            countText.text = "Count: " + score.ToString();
        }
    }
}