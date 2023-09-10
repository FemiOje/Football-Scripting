using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    [SerializeField] ParticleSystem turboParticle;
    private Rigidbody playerRb;
    private float normalSpeed = 500f;
    private float turboSpeed = 1000f;
    private GameObject focalPoint;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        HandleParticles();
    }

    private void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");

        HandleMovement(verticalInput);

        SetPowerupIndicatorPosition();
        SetTurboParticlePosition();
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine("PowerupCooldown");
        }
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                BlastEnemyWithPowerupForce(enemyRigidbody, awayFromPlayer);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                BlastEnemyWithNormalForce(enemyRigidbody, awayFromPlayer);
            }
        }
    }



    void AddNormalForce(float verticalInput)
    {
        playerRb.AddForce(normalSpeed * Time.deltaTime * verticalInput * focalPoint.transform.forward);
    }

    void AddTurboForce(float verticalInput)
    {
        playerRb.AddForce(turboSpeed * Time.deltaTime * verticalInput * focalPoint.transform.forward);
    }

    void SetPowerupIndicatorPosition()
    {
        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    void SetTurboParticlePosition()
    {
        turboParticle.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    void HandleParticles()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            turboParticle.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            turboParticle.Stop();
        }
    }

    void HandleMovement(float verticalInput)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AddTurboForce(verticalInput);
        }
        else
        {
            AddNormalForce(verticalInput);
        }
    }

    void BlastEnemyWithNormalForce(Rigidbody enemyRigidbody, Vector3 awayFromPlayer)
    {
        enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
    }

    void BlastEnemyWithPowerupForce(Rigidbody enemyRigidbody, Vector3 awayFromPlayer)
    {
        enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

}
