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

    public Texture2D cursorDefault;

    public Texture2D cursorSpellActive;

    public Texture2D cursorInfo;

    /// <summary>
    /// Main RigidBody of the player model.
    /// </summary>
    private Rigidbody2D rigidBody;

    private Vector2 move;

    private Vector2 look;

    private float movementSpeed;

    private bool takingDmg;

    private Animator animator;

    PlayerInput controls;
    void Awake()
    {
        controls = new PlayerInput();

        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }
 
    private void OnEnable() {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnCharge() {
        Cursor.SetCursor(cursorSpellActive, Vector2.zero, CursorMode.Auto);

        //Charging spell penalization
        speed = speed / 2;
    }

    private void onFire() {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);

        var cursorPos = Mouse.current.position.ReadValue();
        var spellDirection = new Vector3(cursorPos.x, cursorPos.y, 0.0f);

        //Normalize direction of spell
        spellDirection = spellDirection - transform.position;
        spellDirection = spellDirection.normalized;

        //Charging spell penalization removal
        speed *= 2;
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

        movementSpeed = movement;
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

        animator.SetFloat("Speed", movementSpeed);
		animator.SetBool("TakingDmg", takingDmg);
    }
}
