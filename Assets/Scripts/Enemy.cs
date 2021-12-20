using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float HP = 10.0f;

    public float speed = 1.0f;

    public float visionRadius = 3.0f; 

    public float defTimeToWake = 1.0f;

    public bool moveEnable = true;

    private bool hasTarget = false;

    private bool attacking = false;

    private float movementSpeed = 1.0f;

    private Vector2 movementDirection = Vector2.zero;

    private float timeToWake = 0.0f;

    private GameObject attackTarget; 

    // Start is called before the first frame update
    void Start()
    {
        attackTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        CheckForPlayer();

        if(attackTarget != null && hasTarget) {
            movementDirection = GetMoveVector();
        }
        else {
            movementDirection = Vector2.zero;
        }


        Move();
        Animate();
    }


    Vector2 GetMoveVector() {
        return attackTarget.transform.position - transform.position;
    }

    void Move() {
        Vector3 movement = new Vector3(movementDirection.x, movementDirection.y, 0.0f).normalized * speed * Time.deltaTime;
        
        if(movement.magnitude > 0.0f) {
            //Check if character can move in movement direction
            Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;

            var hit1 = Physics2D.BoxCast(bounds.center, bounds.size*1.1f, 0.0f, movement, movement.magnitude*1.1f);
            var hit2 = Physics2D.BoxCast(bounds.center, bounds.size*1.1f, 0.0f, new Vector3(movement.x*1.3f, movement.y, 0), movement.magnitude*1.1f);
            var hit3 = Physics2D.BoxCast(bounds.center, bounds.size*1.1f, 0.0f, new Vector3(movement.x, movement.y*1.3f, 0), movement.magnitude*1.1f);
            if(hit1.collider == null && hit2.collider == null && hit3.collider == null) {
                if(moveEnable) {
                    transform.Translate(movement);
                }
            }
            else {
                movement = Vector3.zero;
            }
        }

        movementSpeed = movement.magnitude;
    }


    void CheckForPlayer() {
        float distance;
        attackTarget = GameManager.Instance.NearestPlayer(transform.position, out distance);

        if(attackTarget != null) {
            Debug.Log("Player found");
            if(distance < visionRadius) {
                Debug.Log("Player is waking up me");

                timeToWake -= Time.deltaTime;

                if(timeToWake < 0.0f) {
                    Debug.Log("I am going to kill him");
                    hasTarget = true;
                    timeToWake = defTimeToWake;
                }
            }
            else {
                Debug.Log("Distance is too big");

                timeToWake = defTimeToWake;
            }
        }
        else {
            hasTarget = true;
            timeToWake = defTimeToWake;
        }
    }   

    void Animate() {

    }
}
