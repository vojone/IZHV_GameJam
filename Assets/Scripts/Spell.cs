using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 7.5f;

    public float spellPower = 1.0f;

    public string type = "";

    public Vector3 maxSpellSize = new Vector3(0.3f, 0.3f, 0.3f); 

    public Vector3 minSpellSize = new Vector3(0.22f, 0.22f, 0.22f); 

    public float maxSpellDamage = 2.0f;

    public string parentTag = "";

    public Vector3 positionCorrection = new Vector3(0.0f, 0.0f, 0.0f);

    public float maxLifetime = 5.0f;


    public Vector2 shootingDirection = new Vector2(1.0f, 1.0f);

    private SpriteRenderer spriteR;

    public Sprite sprite;

    public Sprite[] particlesExplosion;

    public Sprite[] particlesTail;


    private float lifetime = 0.0f;


    public ParticleSystem ExplosionParticleSystem;

    public ParticleSystem TailParticleSystem;

    private bool destroying = false;


    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = sprite;

        foreach(var particle in particlesExplosion) {
            ExplosionParticleSystem.textureSheetAnimation.AddSprite(particle);
        }

        foreach(var particle in particlesTail) {
            TailParticleSystem.textureSheetAnimation.AddSprite(particle);
        }

        //To make spells more accurate
        transform.localPosition += positionCorrection;

        //Set spell size
        var spellSizeFactor = maxSpellSize*spellPower;
        if(spellSizeFactor.x < minSpellSize.x ||
           spellSizeFactor.y < minSpellSize.y ||
           spellSizeFactor.z < minSpellSize.z)
        {
            spellSizeFactor = minSpellSize*spellPower;
        }

        transform.localScale = spellSizeFactor;
        ExplosionParticleSystem.transform.localScale = spellSizeFactor;
        TailParticleSystem.transform.localScale = spellSizeFactor;

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
        if(destroying) {
            if(!ExplosionParticleSystem.IsAlive()) {
                Destroy(gameObject); 
            }
        }
        else {
            UpdatePosition();
    
            if(lifetime >= maxLifetime) {
                DestroySpell();
            }
        }
    }

    void UpdatePosition() {
        Vector3 movement = new Vector3(shootingDirection.x, shootingDirection.y, 0.0f);
        movement = movement * speed * Time.deltaTime;
        transform.Translate(movement);

        lifetime += Time.deltaTime;

        //Debug.Log(movement);
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
        if (!other.gameObject.CompareTag(parentTag) && 
            other.gameObject.layer == LayerMask.NameToLayer("Blocking")) 
        {
            if(other.gameObject.CompareTag("Enemy")) {
                other.gameObject.GetComponent<Enemy>().Damage(type, maxSpellDamage*spellPower);
            }

            DestroySpell();
        }
    }

    void DestroySpell()
    {
        ExplosionParticleSystem.Play();
        TailParticleSystem.Pause();

        TailParticleSystem.GetComponent<ParticleSystemRenderer>().sortingLayerID = SortingLayer.NameToID("Decoration");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        spriteR.enabled = false;
        destroying = true;
    }
}
