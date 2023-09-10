using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
//using UnityEngine.Windows;

public class player_Inputs : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public Vector3 v3;
    public float scaleNumber = 1.0f;//= 100%
    public Vector3 scalev3;
    private int direction;
    public Transform tr;

    // Start is called once before the first frame update when this object containing this script first exist in the game scene
    void Start()
    {
        
    }

    // Update is called once per frame/ every frame per sec; fps -> internal CPU timer
    void Update()//this is a built-in while loop already
    {
        //to move the object
        HardCodedMoveObject();//calling the method
        //AxisInput();
        //Debug.Log(Time.deltaTime);
    }

    void HardCodedMoveObject()//self delcared method/function
    {
        if (Input.GetKey(KeyCode.E))//hard-coded scripting of input keys// as events to listen for
        {//
            transform.Translate(Vector3.up * playerSpeed * Time.deltaTime);
            //Vector3.up - y axis; Vector3.forward - z-axis; Vector3.right - x-axis}
        }
        else if (Input.GetKey(KeyCode.W))//hard-coded scripting of input keys// as events to listen for
        {//
            transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
            //Vector3.up - y axis; Vector3.forward - z-axis; Vector3.right - x-axis}
        }
        else if (Input.GetKey(KeyCode.S))//hard-coded scripting of input keys// as events to listen for
        {//
            transform.Translate(Vector3.back * playerSpeed * Time.deltaTime);
            //Vector3.up - y axis; Vector3.forward - z-axis; Vector3.right - x-axis}
        }
        else if (Input.GetKey(KeyCode.D))//hard-coded scripting of input keys// as events to listen for
        {//
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
            //Vector3.up - y axis; Vector3.forward - z-axis; Vector3.right - x-axis}
        }
        else if (Input.GetKey(KeyCode.A))//hard-coded scripting of input keys// as events to listen for
        {//
            transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
            //Vector3.up - y axis; Vector3.forward - z-axis; Vector3.right - x-axis}
        }

        if(Input.GetMouseButtonUp(0))//(1), (2)
        {
            Debug.Log("mouse 0  is clicked");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("mouse 1  is clicked");
        }
        else if (Input.GetMouseButton(2))
        {
            Debug.Log("mouse 2  is clicked");
        }
    }
    
    void AxisInput()
    {
        float axisIntRaw = Input.GetAxisRaw("Outlier"); //only accepted rounded numbers [only -1, 0, or +1]
        float axisInt = Input.GetAxis("Outlier");// has a decimal range from -1 to 0 to +1

        if (axisInt != 0.0f)//is a floating value of ranged -1 to +1
        {
            //transform.Translate(Vector3.right * axisInt * Time.deltaTime * playerSpeed);
            //move by a number of values
            //Debug.Log("is moving" + axisInt);
            transform.position = v3;
            //teleport or set position to target Vector3
            
            
        }
        /* else if (axisInt < 0.0f)
        {
            transform.Translate(Vector3.right * axisInt * Time.deltaTime * playerSpeed);
            Debug.Log("move negative");
        } */

        if(Input.GetKey(KeyCode.O))
       {
            RotateThis();//rotate by values
       }
       if(Input.GetKeyUp(KeyCode.P))
       {
            ScaleThis(scaleNumber);
       }
       if(Input.GetKeyUp(KeyCode.I))
       {
            ScaleThis(-scaleNumber);
       }



    }

    void RotateThis()
    {
        //transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f) * playerSpeed);
        transform.Rotate(Vector3.back * playerSpeed);
        Debug.Log("is turning");
        //transform.localEulerAngles = new Vector3(1.0f, 0.0f, 0.0f) * playerSpeed;
        //setting a fixed angle by vector3 values
        
    }

    void ScaleThis(float scaleValue)
    {
        transform.localScale += scalev3 * scaleValue;//scaleValue is multiplier %
    }
}
