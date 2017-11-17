using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Player
{
    public class Weight : MonoBehaviour
    {
        #region GameUI
        //Score counter
        private int score;
        //Score counter text
        public Text countText;

        //Physics texts
        public Text energyText;
        public Text speedText;
        public Text directionText;

        #endregion

        #region variables
        //Set the y axes of connected anchor as distance from chain end
        public float distanceFromChainEnd = 0.6f;
        //QuestItem collected
        public static int questCompleted;
        #endregion

        #region PhysicsFormules
        public float Mass = 1;
        public float Radius = 0.5f;
        public float Energy;
        public float Speed;
        public Vector3 Direction;

        //UI
        public float pulseEnergy;
        public Vector3 pulseVector;
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
            rb.mass = 8;
            float mass = rb.mass;

            float inertia = rb.inertia;
            Vector2 velocity = rb.velocity;
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

        #region Update
        void Update()
        {
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            
            Speed = Mathf.Abs(rigid.velocity.x) + Mathf.Abs(rigid.velocity.y);

            Energy = Custom.Physics.GetEnergy(Speed, Mass);

            Direction = rigid.velocity.normalized;

            speedText.text = "Speed: " + Speed.ToString();
            PhysicsUI();
        }
        #endregion

        #region OnTriggerEnter
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
        #endregion

        #region PhysicsCalculating
        private void OnCollisionEnter2D(Collision2D collision)
        {

            Custom.Physics.Pulse pulse = Custom.Physics.CalculatePulse(this, collision.contacts[0]);

            pulseVector = new Vector3(0,0,0) + pulse.Direction;
            pulseEnergy = pulse.Energy;
            //Debug.LogWarning("Pulse Energy: " + pulse.Energy);
            PhysicsUI();
        }
        #endregion

        #region GameUI
        /// <summary>
        /// Set the text of the physics on UI
        /// </summary>
        public void PhysicsUI()
        {
            energyText.text = "Kinetic energy: " + pulseEnergy.ToString();
            directionText.text = "Direction: " + pulseVector.ToString();
        }

        /// <summary>
        /// Set the text of counter on the UI
        /// </summary>
        public void SetCountText()
        {
            countText.text = "Count: " + score.ToString();
        }
        #endregion
    }
}