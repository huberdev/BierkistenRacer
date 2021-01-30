using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class EngineSounds : MonoBehaviour {
        public AudioSource engineStartSound;
        public AudioSource engineStopSound;
        public AudioSource engineLoopSound;

        public void PlayStartSound() {
            engineStartSound.Play();
        }

        public void PlayStopSound() {
            engineStopSound.Play();
        }

        public void PlayLoopSound() {
            if (!engineLoopSound.isPlaying) {
                Debug.Log("Start Loop sound because it is not running");
                engineLoopSound.Play();
            }
        }

        public void StopLoopSound() {
            if (engineLoopSound.isPlaying) {
                Debug.Log("Stop Loop sound because it is running");
                engineLoopSound.Stop();
            }
        }

        public void SetVolumeLoopSound(float volume) {
            if (engineLoopSound.isPlaying) {

                // minimum volume
                if (volume <= 0.15f)
                    volume = 0.15f;

                engineLoopSound.volume = volume;
            }
        }
    }
}
