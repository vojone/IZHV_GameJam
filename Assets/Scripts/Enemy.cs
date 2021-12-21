using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float HP = 10.0f;

    public float speed = 1.0f;

    public string dmgType = "Bite";

    public float attackCooldown = 1.0f;

    public float damage = 1.0f;

    public float visionRadius = 4.0f; 

    public float defTimeToWake = 3.0f;

    public bool moveEnable = true;

    private bool hasTarget = false;

    private bool attacking = false;

    private float movementSpeed = 1.0f;

    private Vector2 movementDirection = Vector2.zero;

    private float timeToWake = 0.0f;

    private GameObject attackTarget; 

    public float TimeToLostTarget = 5.0f;

    private Animator animator;

    private float curTimeToLostTarget;

    private float currentAttackCooldown;

    public GameObject stateShow;

    private SpriteRenderer stateShowR;

    public Sprite confusedSprite;

    public Sprite warningSprite;

    public ParticleSystem deadExplosionPS;

    private bool takingDmg = false;

    private bool dying = false;

    public float timeToDie = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        attackTarget = null;

        currentAttackCooldown = attackCooldown;

        animator = gameObject.GetComponent<Animator>();

        stateShowR = stateShow.GetComponent<SpriteRenderer>();
        stateShowR.sprite = null;

        curTimeToLostTarget = TimeToLostTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        if(dying) {
            timeToDie -= Time.deltaTime;

            if(timeToDie <= 0.0f) {
                Destroy(gameObject);
            }
        }
        else {
            CheckForPlayer(true);

            if(attackTarget != null && hasTarget) {
                movementDirection = GetMoveVector();
            }
            else {
                movementDirection = Vector2.zero;
            }

            Move();
            Animate();
            UpdateCharacter();
        }
    }


    Vector2 GetMoveVector() {
        return attackTarget.transform.position - transform.position;
    }

    void Move() {
        Vector3 movement = new Vector3(movementDirection.x, movementDirection.y, 0.0f).normalized * speed * Time.deltaTime;
    
        if(movement.magnitude > 0.0f) {
            //Check if character can move in movement direction
            Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;

            var hit = Physics2D.BoxCast(bounds.center, bounds.size*1.05f, 0.0f, movement, movement.magnitude);
            if(hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Blocking")) {
                if(moveEnable) {
                    transform.Translate(movement);
                }
            }
            else {
                if(hit.collider.gameObject.tag == "Player") {
                    Attack(hit.collider.gameObject);
                }

                movement = Vector3.zero;
            }
        }

        movementSpeed = movement.magnitude;
    }


    public void Attack(GameObject target) {
        attacking = true;

        if(currentAttackCooldown <= 0 && target != null) {
            target.GetComponent<Player>().Damage(dmgType, damage);
            currentAttackCooldown = attackCooldown;
        }
    }


    public void Damage(string type, float damagePower, bool fromPlayer = true) {
        if(fromPlayer) {
            CheckForPlayer(false);
        }

        takingDmg = true;

        HP -= damagePower;

        if(HP <= 0.0f) {
            HP = 0.0f;
        }
    }

    void UpdateCharacter() {
        currentAttackCooldown -= Time.deltaTime;

        curTimeToLostTarget -= Time.deltaTime;


        attacking = false;
        takingDmg = false;

        if(HP == 0.0f) {
            deadExplosionPS.Play();

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            stateShowR.enabled = false;

            dying = true;
        }
    }


    void CheckForPlayer(bool distanceLimited) {
        float distance;
        attackTarget = GameManager.Instance.NearestPlayer(transform.position, out distance);

        if(attackTarget != null) {
            //Debug.Log("Player found");
            if(distance < visionRadius || !distanceLimited) {
                //Debug.Log("Player is waking up me");

                timeToWake -= Time.deltaTime;

                if(timeToWake < 0.0f || !distanceLimited) {
                    //Debug.Log("I am going to kill player");
                    hasTarget = true;
                    timeToWake = defTimeToWake;
                    stateShowR.sprite = null;
                }
                else {
                    stateShowR.sprite = warningSprite;
                }
            }
            else {
                //Debug.Log("Distance is too big");
    
                if(curTimeToLostTarget <= 0.0f) {
                    hasTarget = false;
                    timeToWake = defTimeToWake;
                    curTimeToLostTarget = TimeToLostTarget;
                    stateShowR.sprite = null;
                }
                else {
                    stateShowR.sprite = confusedSprite;
                }
            }
        }
        else {
            hasTarget = false;
            timeToWake = defTimeToWake;
            curTimeToLostTarget = TimeToLostTarget;
            stateShowR.sprite = null;
        }
    } 

    void Animate() {
        animator.SetFloat("Speed", movementSpeed);
        animator.SetBool("Attacking", attacking);
        animator.SetBool("TakingDmg", takingDmg);
    }
}
