using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseMove : MonoBehaviour {

    public float moveSpeed = 5.0f;
    private float LimitX;
    private float LimitY;

    void Start()

    {

    }

    

    void Update()

    {



        float mouseX = Input.GetAxis("Mouse X") * moveSpeed ;

        float mouseY = Input.GetAxis("Mouse Y") * moveSpeed ;

        

        // transform.localRotation = transform.localRotation * Quaternion.Euler(-mouseY, 0, 0);


        LimitX = transform.localEulerAngles.y;
        LimitX = Mathf.Clamp(LimitX,240,300);
        
        Debug.Log(LimitX);


        if (LimitX>=240 && LimitX <=300)
        { 
             if (LimitX <=240|| LimitX >=300)
             {
                LimitX = Mathf.Clamp(LimitX,240,300);
                transform.localEulerAngles = new Vector3(0, LimitX, 0);
             }
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, mouseX, 0);

        }
    


    }

}