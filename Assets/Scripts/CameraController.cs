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

    public float step = 0.1f;

    public Vector3 tolerance = new Vector2(3.0f, 3.0f);

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset + additionalOffset;
    }

    void Adjust() {
        transform.position += (player.transform.position + offset + additionalOffset)*step;
    }
}
