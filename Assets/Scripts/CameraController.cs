using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    ///<summary>
    ///Player, which will be followed by the camera
    ///</summary>
    public GameObject player;

    ///<summary>
    ///Can be used for simple adjusting camera
    ///</summary>
    public Vector3 additionalOffset = Vector3.zero;

    public float adjustSpeed = 0.025f;

    public float fastModeAdjustSpeed = 1.0f;

    private float currentSpeed;

    public Vector3 tolerance = new Vector2(0.12f, 0.12f);

    public Vector3 innerTolerance = new Vector2(0.08f, 0.08f);

    private Vector3 centeredPosition;

    //Difference between "ideal" and current position of camera
    private Vector3 diff;

    private Vector3 movement = Vector3.zero;

    private Vector3 offset;

    private bool adjustingOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get starting offset
        offset = transform.position - player.transform.position;

        centeredPosition = GetCenteredPosition();

        currentSpeed = adjustSpeed;
    }

    void LateUpdate()
    {
        centeredPosition = GetCenteredPosition();
        diff = GetDiff();

        if(Mathf.Abs(diff.x) > tolerance.x ||
           Mathf.Abs(diff.y) > tolerance.y) {
            //Player is not in bigger rectangle -> camera should be adjusted
            adjustingOn = true;
            movement = GetAdjustingVector();
        }
        else if(Mathf.Abs(diff.x) < innerTolerance.x &&
                Mathf.Abs(diff.y) < innerTolerance.y) {
            //Camera points to small rectangle
            adjustingOn = false;
        }
        
        if(adjustingOn) {
            Adjust();
        }

        //transform.position = player.transform.position + offset + additionalOffset;
    }

    Vector3 GetCenteredPosition() {
        return player.transform.position + offset + additionalOffset;
    }

    Vector3 GetDiff() {
        return transform.position - centeredPosition;
    }

    Vector3 GetAdjustingVector() {
        Vector3 v = centeredPosition - transform.position;
        v.z = 0.0f; //We are only in 2D Space

        return v.normalized;
    }

    public void setFastMode(bool turnOn) {
        if(turnOn) {
            currentSpeed = fastModeAdjustSpeed;
        }
        else {
            currentSpeed = adjustSpeed;
        }
    }

    public void Adjust(bool transitioned = true) {
        if(transitioned) { //Smooth slide to players position
            float interpolCoef = diff.magnitude > innerTolerance.x*2 || diff.magnitude > innerTolerance.y*2 ? 1.0f : diff.magnitude;

            transform.position += movement*currentSpeed*interpolCoef;
        }
        else { //Immediate jump to players position
            transform.position = centeredPosition;
        }
    }
}
