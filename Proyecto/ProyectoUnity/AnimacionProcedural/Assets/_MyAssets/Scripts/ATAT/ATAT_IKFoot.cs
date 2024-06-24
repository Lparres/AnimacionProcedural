using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATAT_IKFoot : MonoBehaviour
{
    public float stepHeight;
    public float stepSpeed;

    //float stepDistance = 12;

    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldPosition;

    public bool canMoveLeg;

    [SerializeField] Transform footBone;
    [SerializeField] Transform targetOrigin;

    float lerp = 1;

    float footRotationSpeed = 6;


    private void Awake()
    {
        currentPosition = newPosition = oldPosition = transform.position;
    }
    void Start()
    {
        SetTargetPoint();
    }



    void Update()
    {
        transform.position = currentPosition;
        MoveLeg();
        FixFootRotation();
    }

    void MoveLeg()
    {
        if (canMoveLeg)
        {
            lerp = 0;
            canMoveLeg = false;
            newPosition = SetTargetPoint();

        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            Debug.DrawRay(tempPosition, Vector3.down * 0.1f, Color.red, 50);

            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    Vector3 SetTargetPoint()
    {

        Ray ray = new Ray(targetOrigin.position, Vector3.down);
        Debug.DrawRay(targetOrigin.position, Vector3.down * 10f, Color.red, 50);

        if (Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            return hit.point + Vector3.up * 2.3f;
        }
        else return Vector3.zero;


    }

    void FixFootRotation()
    {
        Ray ray = new Ray(currentPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * footRotationSpeed);
        }
    }
}
