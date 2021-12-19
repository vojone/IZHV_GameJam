using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public string path = "Fireball";

    public float speed = 5.0f;

    public float maxLifetime = 5.0f;

    public float changingInterval = 0.15f;

    public Vector2 shootingDirection = new Vector2(1.0f, 1.0f);

    private SpriteRenderer spriteR;
    private Object[] sprites;

    private float lastChangeBefore = 0.0f;

    private float lifetime = 0.0f;

    private int currentSpriteIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll(path, typeof(Sprite));

        //To make spells more accurate
        //transform.localPosition = new Vector3(0.5f, 0, 0);

        if(sprites.Length == 0) {
            Debug.Log("Cannot load spell sprites!");
        }

        transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        shootingDirection =  setRotation(shootingDirection);
    }


    Vector2 setRotation(Vector2 targetDirection) {
        var angle = Vector2.SignedAngle(Vector2.right, targetDirection);

        transform.Rotate(0, 0, angle);

        //Movement direction correction
        return rotateVector(targetDirection, -angle);
    }
 
    Vector2 rotateVector(Vector2 vector, float deg) {
        float sin = Mathf.Sin(deg*Mathf.Deg2Rad);
        float cos = Mathf.Cos(deg*Mathf.Deg2Rad);

        float x = vector.x;
        float y = vector.y;
         
        Vector2 result = new Vector2(cos*x-sin*y, sin*x+cos*y);
        return result;
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

    private void OnTriggerEnter(Collider other)
    { 
        DestroySpell(); 
    }

    void DestroySpell()
    { 
        Destroy(gameObject); 
    }
}
