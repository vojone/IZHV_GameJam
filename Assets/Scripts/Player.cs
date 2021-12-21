using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject timeCheckPoint;

    public GameObject mainTimeCheckpoint;

    /// <summary>
    /// Number of hits that can player get before he dies.
    /// </summary>
    public float HP;

    public float totalHP = 10.0f;

    /// <summary>
    /// Speed of player movement.
    /// </summary>
    public float speed = 2;

    public float timeLoopLength = 60.0f;

    public float disappearAnimLength = 1.5f;

    public float appearAnimLength = 1.5f;

    public float remainingTime;

    /// <summary>
    /// Bounce when player collides with something.
    /// </summary>
    public float bounce = 8.0f;

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

    public ParticleSystem BloodParticleSystem;

    public bool moveEnable = true;

    public bool dmgEnable = true;

    public bool disappearing = false;

    public bool appearing = false;

    private Vector3 originalScale;

    PlayerInput controls;

    private List<GameObject> objectsToInteract = new List<GameObject>();

    void Awake()
    {
        controls = new PlayerInput();

        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Charge.performed += ctx => chargingSpell = true;
        controls.Player.Charge.canceled += ctx => Fire();
    }

    private void OnPause() {
        GameManager.Instance.TogglePause();
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

    private void OnInteract() {
        if(objectsToInteract.Count > 0) {
            var currentObjectToInteract = objectsToInteract[objectsToInteract.Count - 1];
            if(currentObjectToInteract.CompareTag("InteractiveElement")) {
                InteractiveElement el = currentObjectToInteract.GetComponent<InteractiveElement>();

                if(el.IsEnabled()) {
                    el.Toggle();
                }
            }

            if(currentObjectToInteract.CompareTag("TimeCheckpoint")) {
                TimeCheckpoint el = currentObjectToInteract.GetComponent<TimeCheckpoint>();

                if(timeCheckPoint != null) {
                    timeCheckPoint.GetComponent<TimeCheckpoint>().setActive(false);
                }

                if(el.IsEnabled()) {
                    timeCheckPoint = currentObjectToInteract;
                    timeCheckPoint.GetComponent<TimeCheckpoint>().setActive(true);
                }
            }
        }
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

        if(timeCheckPoint != null) {
            timeCheckPoint.GetComponent<TimeCheckpoint>().setActive(true);
        }

        remainingTime = timeLoopLength;

        originalScale = transform.localScale;

        currentCursorInd = 0;

        HP = totalHP;
    }

    void Update() {

    }
 
    void FixedUpdate() 
    {
        UpdatePosition();
        UpdateCursor();
        CheckRemainingTime();
        Animate();

        UpdateCharacter();
    }

    public void Damage(string type, float damagePower) {
        takingDmg = true;

        if(BloodParticleSystem != null) {
            BloodParticleSystem.Play();
        }

        HP -= damagePower;

        if(HP < 0.0f) {
            HP = 0.0f;
        }
    }


    void CheckRemainingTime() {
        if(remainingTime <= 0.0f) {
            moveEnable = false;
            dmgEnable = false;
            disappearing = true;

            if(remainingTime < -disappearAnimLength) {
                MoveToCheckPoint();

                disappearing = false;
                appearing = true;

                if(remainingTime < -(disappearAnimLength + appearAnimLength)) {

                    remainingTime = timeLoopLength;
                    moveEnable = true;
                    dmgEnable = true;
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


    void MoveToCheckPoint() {
        Vector3 newPosition;

        if(timeCheckPoint != null && timeCheckPoint.GetComponent<TimeCheckpoint>().IsEnabled()) {
            newPosition = timeCheckPoint.GetComponent<TimeCheckpoint>().GetCheckpoint();
        }
        else {
            if(mainTimeCheckpoint == null) {
                return;
            }

            newPosition = mainTimeCheckpoint.GetComponent<TimeCheckpoint>().GetCheckpoint();
        }

        transform.position = newPosition;
        GameManager.Instance.FastCamMove();
    }

    void UpdatePosition() {
        Vector3 movement = new Vector3(move.x, move.y, 0.0f).normalized * speed * Time.deltaTime;
        
        if(movement.magnitude > 0.0f) {
            //Check if character can move in movement direction
            Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
            var hit = Physics2D.BoxCast(bounds.center, bounds.size*1.05f, 0.0f, movement, movement.magnitude*1.05f);
            if(hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Blocking")) {
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
        takingDmg = false;
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

        if(other.gameObject.layer == LayerMask.NameToLayer("Interactive")) {
            objectsToInteract.Add(other.gameObject);
            //Debug.Log("Listening for interaction");
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

        if(other.gameObject.layer == LayerMask.NameToLayer("Interactive") && 
           objectsToInteract.Count > 0) {

            var toRemove = objectsToInteract.Find(obj => obj.GetInstanceID() == other.gameObject.GetInstanceID());
            objectsToInteract.Remove(toRemove);
        }
    }
}
