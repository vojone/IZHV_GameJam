using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    ///<summary>
    ///Spell that will be created by this wand
    ///</summary>
    public GameObject spellPrefab;


    public float maxCharge = 10.0f;

    public float minCharge = 3.0f;

    private float currentCharge = 0.0f;

    /// <summary>
    /// Determines how fast is charging of this wand.
    /// </summary>
    public float chargingSpeed = 8.0f;

    public Vector3 spellOffset = new Vector3(0.15f, -0.15f, 0.0f);


    public ParticleSystem chargingPS;


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public float GetChargePerc() {
        return currentCharge/maxCharge;
    }

    public void Charge() {
        AddEnergy(chargingSpeed*Time.deltaTime);
    }

    public void AddEnergy(float energy) {
        currentCharge += energy;

        if(currentCharge > maxCharge) {
            currentCharge = maxCharge;
        }

        if(!chargingPS.isPlaying) {
            chargingPS.Play();
        }
    }

    public void setEnergyTo(float energy) {
        currentCharge = energy;
    }

    public void Fire(Vector3 position, Vector2 direction, int author) {
        chargingPS.Stop();

        if(currentCharge < minCharge) {
            return;
        }

        //Debug.Log(direction);

        var spell = Instantiate(spellPrefab, position + spellOffset, Quaternion.identity);

        spell.GetComponent<Spell>().shootingDirection = new Vector2(direction.x, direction.y);
        spell.GetComponent<Spell>().parentTag = "Player";
        spell.GetComponent<Spell>().spellPower = currentCharge/maxCharge;

        currentCharge = 0.0f;
    }
}
