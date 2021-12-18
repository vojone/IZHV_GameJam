using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Number of hits that can player get before he dies.
    /// </summary>
    public int HP = 10;

    /// <summary>
    /// Speed of player movement.
    /// </summary>
    public float speed = 2;

    public bool headingRight = true;

    /// <summary>
    /// Main RigidBody of the player model.
    /// </summary>
    private Rigidbody2D rigidBody;

    private Vector2 move;

    private float movementSpeed;

    private bool takingDmg;

    private Animator animator;

    PlayerInput controls;
    void Awake()
    {
        controls = new PlayerInput();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
    }
 
    private void OnEnable() {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        takingDmg = false;
    }

    void Update() {

    }
 
    void FixedUpdate() 
    {
        UpdatePosition();
        UpdateCharacter();
        Animate();
    }

    void UpdatePosition() {
        Vector3 movement = new Vector3(move.x, move.y, 0.0f).normalized * speed * Time.deltaTime;
        
        transform.Translate(movement);

        movementSpeed = move.magnitude;
    }

    void UpdateCharacter() {

    }

    void Animate() {
        //Heading update
		if(move.x > 0) { 
			headingRight = true;
		}
		else if(move.x < 0) {
			headingRight = false;
		}


		if(headingRight) {
		    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
		else {
			transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
		}

        Debug.Log(movementSpeed);
        animator.SetFloat("Speed", movementSpeed);
		animator.SetBool("TakingDmg", takingDmg);
    }
}
