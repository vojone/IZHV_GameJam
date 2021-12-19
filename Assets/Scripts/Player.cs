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

    /// <summary>
    /// The number that divides player speed when spell is charging (CANNOT be null).
    /// </summary>
    public float chargingSpellHandicap = 3;

    /// <summary>
    /// The direction of player.
    /// </summary>
    public bool headingRight = true;

    public GameObject wand;

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

    private bool chargingSpell;

    private bool takingDmg;

    private float defaultSpeed;

    private Animator animator;

    PlayerInput controls;
    void Awake()
    {
        controls = new PlayerInput();

        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Charge.performed += ctx => chargingSpell = true;
        controls.Player.Charge.canceled += ctx => Fire();
    }
 
    private void OnEnable() {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void OnCharge() {
        //Charging spell penalization
        speed = speed / chargingSpellHandicap;
    }

    private void Fire() {
        //Position of cursor in screen
        var cursorPos = Mouse.current.position.ReadValue();

        //Position in world space
        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, 10));

        var position = transform.position;
        var spellDirection = new Vector2(worldPos.x-position.x, worldPos.y-position.y);

        //Normalize direction of spell
        spellDirection = spellDirection.normalized;

        wand.GetComponent<Wand>().Fire(position, spellDirection, gameObject.transform.GetInstanceID());

        //Charging spell penalization removal
        speed *= chargingSpellHandicap;
        chargingSpell = false;
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
        UpdateCursor();
        Animate();
    }

    void UpdatePosition() {
        Vector3 movement = new Vector3(move.x, move.y, 0.0f).normalized * speed * Time.deltaTime;
        
        transform.Translate(movement);

        movementSpeed = movement.magnitude;
    }

    void UpdateCharacter() {
    }

    void UpdateCursor() {
        if(chargingSpell) {
            Cursor.SetCursor(cursorSpellActive, Vector2.zero, CursorMode.Auto);
        } 
        else {
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        }
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
