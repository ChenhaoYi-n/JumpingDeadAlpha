using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleMove : MonoBehaviour
{
    public float rotatespeed;
    public GameObject player;
    //float z = 0f;
    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {   
        rotatespeed = 100.0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            /*
            z += Time.deltaTime * rotatespeed;
            if (z > 360.0f)
            {
                z = 0.0f;
            }
            transform.localRotation = Quaternion.Euler(0f, 0f, z);
            */
            transform.Rotate(new Vector3(0f,0f, -1 * rotatespeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            /*
            z += Time.deltaTime * rotatespeed;
            if (z > 360.0f)
            {
                z = 0.0f;
            }
            transform.localRotation = Quaternion.Euler(0f, 0f, z);
            */
            transform.Rotate(new Vector3(0f, 0f, rotatespeed * Time.deltaTime));
        }

       
        transform.position = player.transform.position;
    }
}
