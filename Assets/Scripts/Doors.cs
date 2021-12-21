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

    public bool defaultState = true;

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
        bool onlyone = true;

        if((InteractsNOR.Length == 0 && !wasNANDchosen) ||
           (InteractsNAND.Length == 0 && wasNANDchosen)) {
            return defaultState;
        }

        if(wasNANDchosen) {
            for(int i = 0; i < InteractsNAND.Length; i++) {
                if(i == 0) {
                    result = InteractsNAND[i].GetComponent<InteractiveElement>().GetState();
                }
                else {
                    result = Utils.AND(result, InteractsNAND[i].GetComponent<InteractiveElement>().GetState());
                    onlyone = false;
                }
            }
        }
        else {
            for(int i = 0; i < InteractsNOR.Length; i++) {
                if(i == 0) {
                    result = InteractsNOR[i].GetComponent<InteractiveElement>().GetState();
                }
                else {
                    result = Utils.OR(result, InteractsNOR[i].GetComponent<InteractiveElement>().GetState());
                    onlyone = false;
                }
            }
        }

        if(!onlyone)
            result = !result;

        return result;
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
