using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    public Animator animator;
    private bool isSpeedBoostActive = false;    
    [SerializeField] float dashDistance = 25;
    [SerializeField] float dashCooldown = 5f; // dash cooldown
    [SerializeField] float dashDuration = 0.2f; // Dash time
    [SerializeField] float healthMax = 100;
    [SerializeField] float healthDecreaseWaitTime = 0.5f; // Longer wait time means slower decrease speed
    [SerializeField] Rigidbody playerRigidbody;
    private UIManager uiManager;

    public float currentHealth;
    private float originalSpeed;
    private bool isDashing = false; // Dash controller
    private bool dashCooldownActive = false; // Dash waiting control
    private float movementMagnitude;

    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();

        currentHealth = healthMax;

        originalSpeed = moveSpeed; 
        
        StartCoroutine(DecreaseHealth());
    }

    void Update()
    {
        
        CalculateMovement();
        PlayerDeath();
        animator.SetFloat("Speed", moveSpeed);
    }

    private void AttemptDash(Vector3 move)
    {
        if (!isDashing && !dashCooldownActive && move != Vector3.zero != IsWallInFront(move, dashDistance/4))
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
        Vector3 movedirection = new Vector3(horizontalInput, 0, verticalInput);
        float movementMagnitude = movedirection.magnitude;
        animator.SetFloat("Key", movementMagnitude);
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Kullan�c� dash yapmak isterse
        {
            AttemptDash(move);
        }

        // Dash s�ras�nda hareket kontrol�
        if (!isDashing)
        {
            Move(move);
        }

        if (horizontalInput < 0)
        {
        transform.rotation = Quaternion.Euler(180, 180, 0);
        }
        else if (horizontalInput > 0)
        {

        transform.rotation = Quaternion.Euler(0, 0, 0);
        } 
    }

    private void Move(Vector3 move)
    {
        
        if (!isDashing && !IsWallInFront(move,1))
        {
            Vector3 targetVelocity = move * moveSpeed;
            playerRigidbody.velocity = new Vector3(targetVelocity.x, playerRigidbody.velocity.y, targetVelocity.z );
        }
        else
        {
            // if wall velocity = 0
            playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
        }
    }


    private bool IsWallInFront(Vector3 move, float distance)
    {
        Vector3 rayOrigin = transform.position; // Ray'in başlangıç noktası
        Vector3 rayDirection = move.normalized; // Ray'in yönü (normalize edilmiş)

        // Varsayılan olarak tam uzunlukta bir kırmızı ray çiz (duvara çarpmazsa)
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * distance, Color.red);

        // Raycast işlemi
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, distance))
        {
            // Çarpma varsa ray'i çarpma noktasına kadar yeşil çiz
            Debug.DrawLine(rayOrigin, hit.point, Color.green);

            // Eğer çarpılan obje "Wall" etiketi taşıyorsa
            if (hit.collider.CompareTag("Wall"))
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

    if (currentHealth > 0)
    {
        animator.SetTrigger("Hurt");
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
            uiManager.GameOver();
            Destroy(gameObject);
        }
    }
}
