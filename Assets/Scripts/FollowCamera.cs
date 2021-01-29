using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class FollowCamera : MonoBehaviour
    {
        public float smoothing;
        public float rotationSmoothing;

        public Transform player;

        void Start()
        {
            
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, smoothing);
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, rotationSmoothing);
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        }
    }
}