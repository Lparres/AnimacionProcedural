using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Mouse Sensitivity")]
    [SerializeField] float sensX;
    [SerializeField] float sensY;


    [Header("Clamping")]
    [SerializeField] float minY;
    [SerializeField] float maxY;


    [Header("Spectator")]
    [SerializeField] float spectatorMoveSpeed;
    [SerializeField] bool isSpectator;


    private float rotX;
    private float rotY;

    private Vector3 currentRot;
    private Vector3 currentPos;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void LateUpdate()
    {
        //Mouse movements
        rotX += Input.GetAxis("Mouse X") * sensX;
        rotY += Input.GetAxis("Mouse Y") * sensY;

        //Clamp vertical rotation
        rotY = Mathf.Clamp(rotY, minY, maxY);

        if (isSpectator)
        {
            //Rotate camera
            currentRot = Vector3.SmoothDamp(currentRot, new Vector3(-rotY, rotX, 0), ref velocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRot);

            //Movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float y = Input.GetAxis("Jump"); ;


            Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
            transform.position += dir * spectatorMoveSpeed * Time.deltaTime; 
        }
    }
}
