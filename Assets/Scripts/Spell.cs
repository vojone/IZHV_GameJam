using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 7.5f;

    public Vector3 spellSizeFactor = new Vector3(0.20f, 0.20f, 0.20f); 

    public float spell_dmg = 1;

    public string parentTag = "";

    public Vector3 positionCorrection = new Vector3(0.0f, 0.0f, 0.0f);

    public float maxLifetime = 5.0f;

    public float changingInterval = 0.15f;

    public Vector2 shootingDirection = new Vector2(1.0f, 1.0f);

    private SpriteRenderer spriteR;

    public string spellPath = "Fireball";

    private Sprite[] sprites;

    private float lastChangeBefore = 0.0f;

    private float lifetime = 0.0f;

    private int currentSpriteIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();

        sprites = Resources.LoadAll<Sprite>(spellPath);
        if(sprites.Length == 0) {
            Debug.Log("Cannot load spell animation sprites!");
        }

        //To make spells more accurate
        transform.localPosition += positionCorrection;

        //Set spell size
        transform.localScale = spellSizeFactor;

        //Heading to direction of shooting (and correction of movement direction)
        shootingDirection = SetRotation(shootingDirection);
    }


    Vector2 SetRotation(Vector2 targetDirection) {
        var angle = Vector2.SignedAngle(Vector2.right, targetDirection);

        transform.Rotate(0, 0, angle);

        //Movement direction correction
        return Utils.RotateVector(targetDirection, -angle);
    }

    void FixedUpdate()
    {
        UpdatePosition();
        UpdateSprite();

        if(lifetime >= maxLifetime) {
            DestroySpell();
        }
    }

    void UpdatePosition() {
        Vector3 movement = new Vector3(shootingDirection.x, shootingDirection.y, 0.0f);
        movement = movement * speed * Time.deltaTime;
        transform.Translate(movement);

        lifetime += Time.deltaTime;

        //Debug.Log(movement);
    }

    void UpdateSprite() {
        if(lastChangeBefore > changingInterval) {
            //Only if resources were loaded
            if(sprites.Length > 0) {
                spriteR.sprite = (Sprite)sprites[currentSpriteIndex];
            }

            currentSpriteIndex++;
            if(currentSpriteIndex >= sprites.Length) {
                currentSpriteIndex = 0;
            }

            lastChangeBefore -= changingInterval;
        }

        lastChangeBefore += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(parentTag);
        //Debug.Log("Collides with: " + other.gameObject);

        //Making parent object immune against spell
        if (!other.gameObject.CompareTag(parentTag)) 
        {
            DestroySpell();
        }
    }

    void DestroySpell()
    { 
        Destroy(gameObject); 
    }
}
