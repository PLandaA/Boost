using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionHandler : MonoBehaviour{
    [SerializeField] private float levelLoadDelay = 1f;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip crash;
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private ParticleSystem successParticles;

    private AudioSource _audioSource;

    private bool isTransitioning;
    private bool collisionDisable;

    private void Start(){
        _audioSource = GetComponent<AudioSource>();
        
    }

    private void Update(){
        RespondToDebugKeys();
    }

    void RespondToDebugKeys(){
        if (Input.GetKeyDown(KeyCode.L)){
            LoadNextLevel();
            
        } else if (Input.GetKeyDown(KeyCode.C)){
            collisionDisable = !collisionDisable; //toggle collision
        }
        
    }

    private void OnCollisionEnter(Collision other){
        if (isTransitioning || collisionDisable) return;
        switch (other.gameObject.tag){ 
            case "Friendly":
                break; 
            case "Finish":
                StartSuccesSequence();
                break; 
            case "Fuel" :
                break; 
            default:
                StartCrashSequence();
                break;
            
        }
    }

    private void StartSuccesSequence(){
        
        DisableMovement(success);
        successParticles.Play();
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    void StartCrashSequence(){
        DisableMovement(crash);
        crashParticles.Play();
        Invoke(nameof(ReloadLevel), levelLoadDelay);
    }

    private void DisableMovement(AudioClip audioClip){
        GetComponent<Movement>().enabled = false;
        isTransitioning = true;
        _audioSource.PlayOneShot(audioClip);
        //_audioSource.Stop();
    }

    void LoadNextLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
}