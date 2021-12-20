using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCheckpoint : MonoBehaviour
{
    public bool isEnabled = true;

    public bool isMain = false;

    public bool isActive = false;

    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);


    public Sprite mainSprite;

    public Sprite disabledSprite;

    public Sprite enabledSprite;

    public Sprite disabledSpriteActive;

    public Sprite enabledSpriteActive;

    ///<summary>
    /// Primary logical function to process states of connected interactive elements (buttons, levers)
    ///</summary>
    public GameObject[] InteractsNAND;

    ///<summary>
    /// Secondary logical function to process states of connected interactive elements
    ///</summary>
    public GameObject[] InteractsNOR;
    
    public bool wasNANDchosen = true;

    private SpriteRenderer spriteR;
    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = disabledSprite;

        if(InteractsNAND.Length > 0) {
            wasNANDchosen = true;
        }
        else if(InteractsNOR.Length > 0) {
            wasNANDchosen = false;
        }
        else {
            isEnabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isEnabled = GetInteractsState();

        if(isMain) {
            spriteR.sprite = mainSprite;
        }
        else if(isActive) {
            if(isEnabled) {
                spriteR.sprite = enabledSpriteActive;
            }
            else {
                spriteR.sprite = disabledSpriteActive;
            }
        }
        else {
            if(isEnabled) {
                spriteR.sprite = enabledSprite;
            }
            else {
                spriteR.sprite = disabledSprite;
            }
        }
    }


    bool GetInteractsState() {
        if(isMain) {
            return true;
        }

        bool result = true;
        bool first = true;

        if(wasNANDchosen) {
            foreach(var input in InteractsNAND) {
                if(first) {
                    result = input.GetComponent<InteractiveElement>().GetState();
                }
                else {
                    result = Utils.NAND(result, input.GetComponent<InteractiveElement>().GetState());
                }
            }
        }
        else {
            foreach(var input in InteractsNOR) {
                if(first) {
                    result = input.GetComponent<InteractiveElement>().GetState();
                }
                else {
                    result = Utils.NOR(result, input.GetComponent<InteractiveElement>().GetState());
                }
            }
        }

        return result;
    }

    public void setActive(bool newstate) {
        isActive = newstate;
    }

    public bool IsEnabled() {
        return isEnabled;
    }

    public Vector3 GetCheckpoint() {
        Bounds bounds = gameObject.GetComponent<Collider2D>().bounds;
        return bounds.center + offset;
    }
}
