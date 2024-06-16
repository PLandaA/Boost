using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour{
    [SerializeField] private float mainThrust = 1000f;
    [SerializeField] private float rotationThrust = 100f;
    [SerializeField] private AudioClip mainEngine;
    
    [SerializeField] private ParticleSystem[] mainEngineParticles;
    [SerializeField] private ParticleSystem leftThrustParticles;
    [SerializeField] private ParticleSystem rightThrustParticles;

    
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    
    
    void Start(){
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust(){
        if (Input.GetKey(KeyCode.Space)){
            StartThrusting();
        }
        else{
            StopThrusting();
        }
    }

    void ProcessRotation(){
        if (Input.GetKey(KeyCode.A)){
            ApplyRotation(rotationThrust);
            if (!leftThrustParticles.isPlaying){
                leftThrustParticles.Play();
            }
        } else if (Input.GetKey(KeyCode.D)){
            ApplyRotation(-rotationThrust);
            if (!rightThrustParticles.isPlaying){
                rightThrustParticles.Play();
            }

        }
        else{
            rightThrustParticles.Stop();
            leftThrustParticles.Stop();
        }
    }
    
    private void StopThrusting(){
        _audioSource.Stop();
        foreach (ParticleSystem mainEngine in mainEngineParticles){
            mainEngine.Stop();
        }
    }

    private void StartThrusting(){
        _rigidbody.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
        if (!_audioSource.isPlaying){
            _audioSource.PlayOneShot(mainEngine);
        }

        foreach (ParticleSystem mainEngine in mainEngineParticles){
            if (!mainEngine.isPlaying){
                mainEngine.Play();
            }
        }
    }


    private void ApplyRotation(float rotationThisFrame){
        _rigidbody.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * (rotationThisFrame * Time.deltaTime));
        _rigidbody.freezeRotation = false; //unfreezing rotation so the physics system can over
    }
}