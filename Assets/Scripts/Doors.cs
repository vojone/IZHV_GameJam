using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    ///<summary>
    /// Primary logical function to process states of connected interactive elements (buttons, levers)
    ///</summary>
    public GameObject[] InteractsNAND;

    ///<summary>
    /// Secondary logical function to process states of connected interactive elements
    ///</summary>
    public GameObject[] InteractsNOR;

    public Sprite openedDoors;

    public Sprite closedDoors;

    public bool opened = false;

    public bool wasNANDchosen = true;

    public bool force = false;

    private SpriteRenderer spriteR;

    private BoxCollider2D doorCollider;

    void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = closedDoors;

        doorCollider = GetComponent<BoxCollider2D>();

        if(InteractsNAND.Length > 0) {
            wasNANDchosen = true;
        }
        else if(InteractsNOR.Length > 0) {
            wasNANDchosen = false;
        }
        else {
            ForceClose();
            Debug.Log("No logical inputs were connected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        opened = GetInteractsState();
        UpdateState();
    }

    bool GetInteractsState() {
        bool result = true;
        bool first = true;

        if(wasNANDchosen) {
            foreach(var input in InteractsNAND) {
                if(first) {
                    result = input.GetComponent<InteractiveElement>().GetState();
                }
                else {
                    result = NAND(result, input.GetComponent<InteractiveElement>().GetState());
                }
            }
        }
        else {
            foreach(var input in InteractsNOR) {
                if(first) {
                    result = input.GetComponent<InteractiveElement>().GetState();
                }
                else {
                    result = NOR(result, input.GetComponent<InteractiveElement>().GetState());
                }
            }
        }

        return result;
    }

    bool NAND(bool a, bool b) {
        return !a && !b;
    }

    bool NOR(bool a, bool b) {
        return !a || !b;
    }

    void UpdateState() {
        if(!force) {
            if(opened) {
                spriteR.sprite = openedDoors;
                doorCollider.enabled = false;
            }
            else {
                spriteR.sprite = closedDoors;
                doorCollider.enabled = true;
            }
        }
    }

    void ForceOpen() {
        spriteR.sprite = openedDoors;
        doorCollider.enabled = false;
        force = true;
    }

    void ForceClose() {
        spriteR.sprite = closedDoors;
        doorCollider.enabled = true;
        force = false;
    }
}
