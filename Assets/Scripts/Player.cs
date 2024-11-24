using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    private bool isSpeedBoostActive = false;    
    [SerializeField] int dashDistance = 25;
    [SerializeField] float dashCooldown = 5f; // dash cooldown
    [SerializeField] float dashDuration = 0.2f; // Dash time
    [SerializeField] float healthMax = 100;
    [SerializeField] float healthDecreaseWaitTime = 0.5f; // Longer wait time means slower decrease speed
    [SerializeField] Rigidbody playerRigidbody;

    public float currentHealth;
    private float originalSpeed;
    private bool isDashing = false; // Dash controller
    private bool dashCooldownActive = false; // Dash waiting control

    private void Start()
    {
        currentHealth = healthMax;

        originalSpeed = moveSpeed; 
        
        StartCoroutine(DecreaseHealth());
    }

    void Update()
    {
        CalculateMovement();
        PlayerDeath();
    }

    private void AttemptDash(Vector3 move)
    {
        if (!isDashing && !dashCooldownActive && move != Vector3.zero != IsWallInFront(move))
        {
            StartCoroutine(Dash(move));
        }
    }

    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        
        Vector3 dashVelocity = direction * dashDistance; // apply dash move
        playerRigidbody.velocity = new Vector3(dashVelocity.x, playerRigidbody.velocity.y, dashVelocity.z);

        currentHealth -= 2; // using dash decreases health

        // wait for dash time
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // start dash cooldown
        dashCooldownActive = true;
        yield return new WaitForSeconds(dashCooldown);
        dashCooldownActive = false;
    }




    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift)) // Kullan�c� dash yapmak isterse
        {
            AttemptDash(move);
        }

        // Dash s�ras�nda hareket kontrol�
        if (!isDashing)
        {
            Move(move);
        }
    }

    private void Move(Vector3 move)
    {
        
        if (!isDashing && !IsWallInFront(move))
        {
            Vector3 targetVelocity = move * moveSpeed;
            playerRigidbody.velocity = new Vector3(targetVelocity.x, playerRigidbody.velocity.y, targetVelocity.z);
        }
        else
        {
            // if wall velocity = 0
            playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
        }
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

    IEnumerator DecreaseHealth()
    {
        while (currentHealth > 0)
        {
            currentHealth--;
            yield return new WaitForSeconds(healthDecreaseWaitTime); // Decreases health every "healthDecreaseWaitTime" time
        }
    }
    
    public void baitHeal(float healthValue)
    { 
        currentHealth += healthValue; // Increase currentHealth by healthValue
        
        if(currentHealth > healthMax) // If increased currentHealth exceeds max value, set currentHealth to healthMax
            currentHealth = healthMax;
    }

    // Reduces the player's current health by a specified value
public void baitDamage(float healthValue)
{   
    // Ensure current health does not go below 0
    if (currentHealth != 0)
    {
        currentHealth -= healthValue; // Decrease currentHealth by healthValue
    }
}

// Activates a temporary speed boost for the player
public void ActivateSpeedBoost(float speedModifier, float speedDuration)
{
    // Start the speed boost coroutine if it's not already active
    if (!isSpeedBoostActive) 
    {
        StartCoroutine(SpeedBoostCoroutine(speedModifier, speedDuration));
    }
}

// Coroutine to handle the speed boost effect
private IEnumerator SpeedBoostCoroutine(float speedModifier, float speedDuration)
{
    isSpeedBoostActive = true;               // Set the speed boost as active
    moveSpeed *= speedModifier;             // Increase player's movement speed

    yield return new WaitForSeconds(speedDuration); // Wait for the duration of the speed boost

    moveSpeed = originalSpeed;              // Reset the movement speed to the original value
    isSpeedBoostActive = false;             // Mark the speed boost as inactive
}



    private void PlayerDeath()
    {
        if (currentHealth <= 0)
        {
            FindObjectOfType<Spawn_Manager>().StopSpawning();
            Destroy(gameObject);
        }
    }
}
