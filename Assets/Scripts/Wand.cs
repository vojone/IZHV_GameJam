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

    private float currentCharge = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getChargePerc() {
        return currentCharge/maxCharge;
    }

    public void charge(float energy) {
        currentCharge += energy;

        if(currentCharge > maxCharge) {
            currentCharge = maxCharge;
        }
    }

    public void fire(Vector3 position, Vector2 direction, int author) {
        //Debug.Log(direction);

        var spell = Instantiate(spellPrefab, position, Quaternion.identity);

        spell.GetComponent<Spell>().shootingDirection = new Vector2(direction.x, direction.y);
        spell.GetComponent<Spell>().parentTag = "Player";

        currentCharge = 0.0f;
    }
}
