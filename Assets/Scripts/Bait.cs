using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bait : MonoBehaviour
{
    public AudioSource baitSource;
    [SerializeField] private AudioClip goodBaitClip;
    [SerializeField] private AudioClip badBaitClip;
    [SerializeField] private AudioClip speedBaitClip;
    // Speed of the bait movement
    [SerializeField] private float moveSpeed = 1f;

    // Amount of health to increase when bait is collected (for bait type 0)
    [SerializeField] int healthUp = 10;

    // Amount of health to decrease when bait is collected (for bait type 1)
    [SerializeField] int healthDown = 5;

    // Speed multiplier for bait type 2
    [SerializeField] float speedModifier = 1.5f;

    // Duration of the speed boost effect
    [SerializeField] float speedDuration = 1.0f;

    // Time before the bait is automatically destroyed
    [SerializeField] float destroyBaitWaitTime = 5.0f;

    // Static score variable for the bait
    [SerializeField] public static int score;

    // Identifier for bait type (0: health, 1: damage, 2: speed boost)
    [SerializeField] int baitID;

    

    // Reference to the player's transform
    private Transform playerTransform;

    asynchronousLoadingManager asynchronousLoadingManager;

    GameObject player;
    Vector3 move;
    // Returns the current score
    private bool isSecondLoaded = false;
    
    void Start()
    {
        asynchronousLoadingManager = FindAnyObjectByType<asynchronousLoadingManager>();
        // Start the coroutine to destroy the bait after a delay
        StartCoroutine(DestroyBait());
       
        // Find the player object and set its transform reference
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        baitSource = GetComponent<AudioSource>(); 
        if(baitSource ==null){
            Debug.LogError("Audio Source boş");
        }
    }

    void Update()
    {
        // Handle bait movement each frame
        if (player != null && !IsWallInFront(move))
        {
            Movement();
        }

        if (Input.GetKeyDown(KeyCode.F) && !isSecondLoaded )
        {
            asynchronousLoadingManager.LoadGame(2);
            isSecondLoaded = true;
        }
        
    }

    public int getScore()
    {
        return score;
    }


   
    private bool IsWallInFront(Vector3 move)
    {
        float rayDistance = 1f; // Raycast distance
        Vector3 rayOrigin = transform.position; // Ray start
        Vector3 rayDirection = move.normalized; ; // Ray direction


        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Wall")) // if ray hits wall
            {
                return true;
            }
        }

        return false;
    }

    // Controls the movement of the bait based on its type
    void Movement()
    {
        if (baitID == 0) // Bait type 0 moves away from the player
        {
            Vector3 directionAway = (transform.position - playerTransform.position).normalized;
            move = directionAway;
            transform.position += directionAway * moveSpeed * Time.deltaTime;
        }
        else if (baitID == 1) // Bait type 1 moves towards the player
        {
            Vector3 directionTowards = (playerTransform.position - transform.position).normalized;
            move = directionTowards;
            
            transform.position += directionTowards * moveSpeed * Time.deltaTime * 3f;
            if (directionTowards.x < 0)
            {
            transform.rotation = Quaternion.Euler(180, 180, 0);
            }   
            else 
            {
            transform.rotation = Quaternion.Euler(0, 0, 0);    
            }     
        }
    }

    // Called when the bait collides with another object
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Check if the collider belongs to the player
        {
            HandlePlayerCollision(other);
            
        }
    }
    


    void HandlePlayerCollision(Collider other){
        Player player = other.transform.GetComponent<Player>();
        if (player != null)
        {
        // Perform an action based on bait type
        switch (baitID)
        {
        case 0: // Health increase
        player.baitHeal(healthUp);
        SoundManager.instance.PlaySound(goodBaitClip);

        score++;
        if (score == 10)
        {
            asynchronousLoadingManager.LoadGame(2);  // load second scene acyns
        }
        break;

        case 1: // Health decrease
        player.baitDamage(healthDown);
        SoundManager.instance.PlaySound(badBaitClip);
        break;

        case 2: // Speed boost
        player.ActivateSpeedBoost(speedModifier, speedDuration);
        SoundManager.instance.PlaySound(speedBaitClip);
        break;
        }
        }

            // Destroy the bait after collision
            Destroy(this.gameObject, 0.2f);
    }




    // Coroutine to destroy the bait after a certain time
    IEnumerator DestroyBait()
    {
        if(baitID == 1)
        yield return new WaitForSeconds(destroyBaitWaitTime+1.5f);
        else
        yield return new WaitForSeconds(destroyBaitWaitTime);
        Destroy(gameObject);
    }
}
