using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Vector3 timeCheckPoint = new Vector3(2.0f, 1.0f, 1.0f);

    /// <summary>
    /// Number of hits that can player get before he dies.
    /// </summary>
    public float HP = 10.0f;

    /// <summary>
    /// Speed of player movement.
    /// </summary>
    public float speed = 2;

    public float timeLoopLength = 60.0f;

    public float disappearAnimLength = 1.5f;

    public float appearAnimLength = 1.5f;

    private float remainingTime;

    /// <summary>
    /// Bounce when player collides with something.
    /// </summary>
    public float bounce = 8;

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

    public string cursorWandChargingPath;

    private Texture2D[] cursorWandCharging;

    private int currentCursorInd;

    public Texture2D cursorInfo;

    /// <summary>
    /// Main RigidBody of the player model.
    /// </summary>
    private Rigidbody2D rigidBody;

    private Vector2 move;

    private Vector3 lastMoveDirection;

    private Vector2 look;

    private float movementSpeed = 1.0f;

    private bool chargingSpell = false;

    private bool takingDmg = false;

    private bool bounced = false;

    private Animator animator;

    public bool moveEnable = true;

    public bool disappearing = false;

    public bool appearing = false;

    private Vector3 originalScale;

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

    void Start()
    {
        animator = GetComponent<Animator>();
        takingDmg = false;

        lastMoveDirection = Vector3.zero;

        cursorWandCharging = Resources.LoadAll<Texture2D>(cursorWandChargingPath);
        if(cursorWandCharging.Length == 0) {
            Debug.Log("Cannot load wand cursor textures!");
        }

        remainingTime = timeLoopLength;

        originalScale = transform.localScale;

        currentCursorInd = 0;
    }

    void Update() {

    }
 
    void FixedUpdate() 
    {
        UpdatePosition();
        UpdateCharacter();
        UpdateCursor();
        CheckRemainingTime();
        Animate();
    }


    void CheckRemainingTime() {
        if(remainingTime <= 0.0f) {
            moveEnable = false;
            disappearing = true;

            if(remainingTime < -disappearAnimLength) {
                Vector3 newPosition = timeCheckPoint;
                transform.position = newPosition;
                GameManager.Instance.FastCamMove();

                disappearing = false;
                appearing = true;

                if(remainingTime < -(disappearAnimLength + appearAnimLength)) {

                    remainingTime = timeLoopLength;
                    moveEnable = true;
                    disappearing = false;
                    appearing = false;
                }
            }
        }
        else {
            disappearing = false;
            appearing = false;
        }
    }

    void UpdatePosition() {
        Vector3 movement = new Vector3(move.x, move.y, 0.0f).normalized * speed * Time.deltaTime;
        
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

        lastMoveDirection = movement;

        movementSpeed = movement.magnitude;
    }

    void UpdateCharacter() {
        remainingTime -= Time.deltaTime;

        if(chargingSpell) {
            wand.GetComponent<Wand>().Charge();
        }
        else {
            wand.GetComponent<Wand>().setEnergyTo(0.0f);
        }

    }

    void UpdateCursor() {
        if(chargingSpell) {
            float chargePerc = wand.GetComponent<Wand>().GetChargePerc();
            currentCursorInd = Mathf.FloorToInt(chargePerc*(cursorWandCharging.Length - 1));
            
            if(currentCursorInd >= cursorWandCharging.Length) { //Just for safety
                currentCursorInd = 0;
            }

            Cursor.SetCursor(cursorWandCharging[currentCursorInd], Vector2.zero, CursorMode.Auto);
        } 
        else {
            currentCursorInd = 0;
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
        animator.SetBool("Appearing", appearing);
        animator.SetBool("Disappearing", disappearing);
		animator.SetBool("TakingDmg", takingDmg);
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Blocking"))
        {   
            //Little bounce when BoxCast is not enough
            transform.Translate(-lastMoveDirection*bounce);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Blocking") && !bounced)
        {   
            //Little bounce when BoxCast is not enough
            transform.Translate(-lastMoveDirection*bounce);
            bounced = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        bounced = false;
    }
}
