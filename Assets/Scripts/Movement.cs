using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Movement : MonoBehaviour
    {
        public float speed;
        public float turnSpeed;
        public float gravity;
        public GameObject respawnText;

        private Rigidbody rigidBody;
        private float respawnTime = 0f;
        private float respawnTimeOnSide = 0f;
        private bool respawnAllowed = false;

        private Vector3 startPosition;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            startPosition = transform.position;
        }

        void FixedUpdate()
        {
            // movement only if car is on ground
            if (Physics.Raycast(transform.position, -transform.up, 2f)) {

                // Debug.Log("On Ground");

                if (Input.GetKey(KeyCode.W)) {
                    rigidBody.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * speed * 10);
                } else if (Input.GetKey(KeyCode.S)) {
                    rigidBody.AddRelativeForce(new Vector3(Vector3.forward.x, 0, Vector3.forward.z) * -speed * 10);
                }

                Vector3 localVelocity = transform.InverseTransformDirection(rigidBody.velocity);
                localVelocity.x = 0;
                rigidBody.velocity = transform.TransformDirection(localVelocity);

                if (Input.GetKey(KeyCode.A)) {
                    rigidBody.AddTorque(-Vector3.up * turnSpeed * 10);
                } else if (Input.GetKey(KeyCode.D)) {
                    rigidBody.AddTorque(Vector3.up * turnSpeed * 10);
                }

                // gravity
                rigidBody.AddForce(Vector3.down * gravity * 10);
            } else {

                // Debug.Log("In Air");

                // higher gravity when in air
                rigidBody.AddForce(Vector3.down * gravity * 30);
            }
        }

        private void Update() {

            // check if vehicle is upside down
            CheckVehicleUpsideDown();

            // check if vehicle is on side
            CheckVehicleOnSide();

            // Respawn
            if (Input.GetKey(KeyCode.Space) && respawnAllowed) {
                    Debug.Log("Respawn");
                    respawnText.SetActive(false);
                    respawnAllowed = false;
                    respawnTime = 0f;
                    respawnTimeOnSide = 0f;

                    // reset transform
                    transform.rotation = Quaternion.identity;
                    transform.position = startPosition;
            }
        }

        private void CheckVehicleUpsideDown() {
            // is upside down
            if (transform.up.y < 0f && !respawnAllowed) {
                // Debug.Log("CheckVehicleUpsideDown: " + respawnTime + "/" + Time.deltaTime);
                respawnTime -= Time.deltaTime;
                if (respawnTime < -3f) {
                    AllowRespawn();
                }
            } else {
                respawnTime = 0f;
            }
        }

        private void CheckVehicleOnSide() {
            // if raycast to ground is failing
            if (!Physics.Raycast(transform.position, -transform.up, 2f)) {
                // Debug.Log("CheckVehicleOnSide: " + respawnTimeOnSide + "/" + Time.deltaTime);
                respawnTimeOnSide -= Time.deltaTime;
                if (respawnTimeOnSide < -3f) {
                    AllowRespawn();
                }
            } else {
                respawnTimeOnSide = 0f;
            }
        }

        private void AllowRespawn() {
            respawnAllowed = true;
            respawnText.SetActive(true);
            // Debug.Log("Allow Respawn");
        }
    }
}
