using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveElement : MonoBehaviour
{
    public Sprite activeSprite;

    public Sprite defaultSprite;

    public bool isButton = false;

    public float timeToForgetState = 2.0f;

    public bool state = false;

    public bool isEnabled = true;

    private float remainingTime = 0.0f;

    private SpriteRenderer spriteR;
    // Start is called before the first frame update
    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();

        spriteR.sprite = defaultSprite;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
    }

    void FixedUpdate() {
        if(isButton) {
            if(remainingTime > 0.0f) {
                remainingTime -= Time.deltaTime;
            }
            else {
                AutoToggle();
            }
        }
    }

    void AutoToggle() {
        remainingTime = 0.0f;
        state = false;
        isEnabled = true;
    }

    void UpdateSprite() {
        if(state) {
            spriteR.sprite = activeSprite;
        }
        else {
            spriteR.sprite = defaultSprite;
        }
    }

    public bool GetState() {
        return state;
    }

    public bool IsEnabled() {
        return isEnabled;
    }

    public bool Toggle() {
        if(isEnabled) {
            if(isButton) {
                remainingTime = timeToForgetState;
                state = true;
                isEnabled = false;
            }
            else {
                state = state ? false : true;
            }

            return true;
        }
        else {
            return false;
        }
    }
}
