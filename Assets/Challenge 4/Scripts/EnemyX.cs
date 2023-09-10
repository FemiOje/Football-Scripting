using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody enemyRb;
    private GameObject playerGoal;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveTowardPlayerGoal();
    }

    private void OnCollisionEnter(Collision other)
    {
        string otherGameObject = other.gameObject.name;

        // If enemy collides with either goal, destroy it
        if (otherGameObject.Equals("Player Goal") || otherGameObject.Equals("Enemy Goal"))
        {
            Destroy(gameObject);
        }
    }

    private void MoveTowardPlayerGoal()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
    }

}
