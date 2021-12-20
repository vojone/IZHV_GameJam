using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Rotation of vector by deg angle
    //Rotation transformation (2D rotation matrix) is used for it
    public static Vector2 RotateVector(Vector2 vector, float deg) {
        float sin = Mathf.Sin(deg * Mathf.Deg2Rad);
        float cos = Mathf.Cos(deg * Mathf.Deg2Rad);

        float x = vector.x;
        float y = vector.y;
         
        Vector2 result = new Vector2(cos*x - sin*y, sin*x + cos*y);
        return result;
    }

    public static bool CoinFlip() {
        return Random.value >= 0.5 ? true : false;
    }
}
