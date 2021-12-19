using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    public GameObject spellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(Vector3 position, Vector2 direction, int author) {
        //Debug.Log(direction);

        var spell = Instantiate(spellPrefab, position, Quaternion.identity);

        spell.GetComponent<Spell>().shootingDirection = new Vector2(direction.x, direction.y);
        spell.GetComponent<Spell>().parentTag = "Player";
    }
}
