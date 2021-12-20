using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float HP = 10.0f;

    public float speed = 1.0f;

    private float movementSpeed = 1.0f;

    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        CheckForPlayer();
    }

    void CheckForPlayer() {
        float distance;
        GameManager.Instance.NearestPlayer(transform.position, out distance);
    }


    void Animate() {

    }
}
