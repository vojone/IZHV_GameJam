using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    ///<summary>
    ///Player, which will be followed by the camera
    ///</summary>
    public GameObject player = null;

    ///<summary>
    ///Can be used for simple adjusting camera
    ///</summary>
    public Vector3 additionalOffset = Vector3.zero;

    public float adjustSpeed = 0.02f;

    public float fastModeAdjustSpeed = 0.1f;

    private float currentSpeed;

    public Vector3 tolerance = new Vector2(0.4f, 0.4f);

    public Vector3 innerTolerance = new Vector2(0.02f, 0.02f);

    private Vector3 centeredPosition;

    //Difference between "ideal" and current position of camera
    private Vector3 diff;

    private Vector3 movement = Vector3.zero;

    private Vector3 offset;

    private bool adjustingOn = false;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = adjustSpeed;
    }

    public void SetPlayer(GameObject playerToBeFollowed) {
        player = playerToBeFollowed;

        recordOffset();

        //Center camera to player
        offset.x = 0.0f;
        offset.y = 0.0f;

        centeredPosition = GetCenteredPosition();
    }

    public void recordOffset() {
        //Get starting offset
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if(player != null) {
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
            float interpolCoef = diff.magnitude*currentSpeed;
            
            if(diff.magnitude*currentSpeed > innerTolerance.x*2 || 
               diff.magnitude*currentSpeed > innerTolerance.y*2) {
                interpolCoef = 1.0f;
            }

            Vector3 translation = movement*interpolCoef;

            // Debug.Log(diff);

            if(Mathf.Abs(translation.x) > Mathf.Abs(diff.x)) {
                translation.x = centeredPosition.x - transform.position.x;
            }

            if(Mathf.Abs(translation.y) > Mathf.Abs(diff.y)) {
                translation.y = centeredPosition.y - transform.position.y;
            }
        
            transform.position += translation;
        }
        else { //Immediate jump to players position
            transform.position = centeredPosition;
        }
    }
}
