using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class GemRotator : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            //Rotate the gem
            transform.Rotate(new Vector3(0, 0, 135) * Time.deltaTime);
        }
    }
}