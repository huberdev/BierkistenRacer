using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class Movement : MonoBehaviour
    {
        public float speed;
        public float turnSpeed;
        public float gravity;
        public GameObject respawnText;
        public EngineSounds engineSounds;
        public Text speedText;

        // wheels
        public GameObject wheel1;
        public GameObject wheel2;
        public GameObject wheel3;
        public GameObject wheel4;

        // steering wheel
        public GameObject steeringWheel;

        private Rigidbody rigidBody;
        private float respawnTime = 0f;
        private float respawnTimeOnSide = 0f;
        private bool respawnAllowed = false;
        private float currentSpeed = 0f;

        private Vector3 startPosition;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            startPosition = transform.position;

            engineSounds.PlayStartSound();
            engineSounds.PlayLoopSound();
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

            // get speed
            speedText.text = GetCurrentSpeed().ToString() + " km/h";

            // set engine loop sound
            engineSounds.SetVolumeLoopSound(GetCurrentSpeed()/100f);

            // animate wheels
            AnimateWheels();
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

                    engineSounds.PlayStartSound();
                    engineSounds.PlayLoopSound();

                    // reset transform
                    transform.rotation = Quaternion.identity;
                    transform.position = startPosition;
            }

            // animate steering wheel
            AnimateSteeringWheel();
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
            if (!respawnAllowed) {
                engineSounds.PlayStopSound();
                engineSounds.StopLoopSound();
                respawnAllowed = true;
                respawnText.SetActive(true);
                // Debug.Log("Allow Respawn");
            }
        }

        public float GetCurrentSpeed() {
            currentSpeed = Mathf.RoundToInt(rigidBody.velocity.magnitude);

            // check if car is driving forward or backward
            var localVel = transform.InverseTransformDirection(rigidBody.velocity);
            // Debug.Log("localVel: " + localVel.z);

            if (currentSpeed >= 0f) {

                // car is driving backwards
                if (localVel.z < 0)
                    currentSpeed = currentSpeed * -1f;

                return currentSpeed;
            } else {
                return 0f;
            }
        }

        public void AnimateWheels() {
            wheel1.transform.RotateAround(wheel1.transform.position, transform.right, Time.deltaTime * GetCurrentSpeed() * 40f);
            wheel2.transform.RotateAround(wheel2.transform.position, transform.right, Time.deltaTime * GetCurrentSpeed() * 40f);
            wheel3.transform.RotateAround(wheel3.transform.position, transform.right, Time.deltaTime * GetCurrentSpeed() * 40f);
            wheel4.transform.RotateAround(wheel4.transform.position, transform.right, Time.deltaTime * GetCurrentSpeed() * 40f);
        }

        public void AnimateSteeringWheel() {

            // get angular velocity
            Vector3 localangularvelocity = transform.InverseTransformDirection(rigidBody.angularVelocity);
            // Debug.Log("localangularvelocity: " + localangularvelocity.y);

            steeringWheel.transform.Rotate(new Vector3(0f, (localangularvelocity.y/4f), 0f));
            // steeringWheel.transform.RotateAround(steeringWheel.transform.position, transform.up, Time.deltaTime * localangularvelocity.y * 150f);
        }
    }
}
