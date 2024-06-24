using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] GameObject body;
    Vector3 hipMoveDirection;

    public float stepDistance;
    public float stepHeight;
    public float stepSpeed;

    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldPosition;

    [SerializeField] Transform footOrigin;

    float lerp = 0;
    public bool onStep;
    bool onDefault;
    bool firstMove;
    [SerializeField] float initialOffset;

    Vector3 hipLastPos;

    // Start is called before the first frame update
    void Start()
    {

        currentPosition = newPosition = oldPosition = footOrigin.position - Vector3.up * 0.6f;

        hipLastPos = body.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = currentPosition;

        MoveLeg();

        HipMoveDirection();

        if (hipMoveDirection != Vector3.zero)
        {
            if (!firstMove)
            {
                FirstMove();
            }
            else
            {
                
            }
        }      
        
    }

  
    
    void FirstMove()
    {
        Vector3 targetPoint = footOrigin.position + hipMoveDirection * initialOffset;
        Ray ray = new Ray(targetPoint + Vector3.up * 10, Vector3.down);
        Physics.Raycast(ray, out RaycastHit hit, 15);
        currentPosition = newPosition = oldPosition = hit.point + Vector3.up * 0.42f;

        firstMove = true;

        

    }

void MoveLeg()
    {
        float distanceToOrigin = Vector3.Distance(new Vector3(newPosition.x, 0f, newPosition.z), new Vector3(footOrigin.position.x, 0f, footOrigin.position.z));

        if (distanceToOrigin > stepDistance/2 && !onStep && body.GetComponent<ApexBot_Controller>().LegsOnGround())
        {
            onStep = true;
            Vector3 targetPoint = footOrigin.position + (hipMoveDirection * stepDistance/2f);

            Ray ray = new Ray(targetPoint + Vector3.up * 5, Vector3.down);
            

            if(Physics.Raycast(ray, out RaycastHit hit, 50))
            {
                newPosition = hit.point + Vector3.up * 0.42f;
                Debug.DrawRay(newPosition + Vector3.up * 10, Vector3.down * 15, Color.red, 10);
            }

            lerp = 0;
        }

        if(lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            Debug.DrawRay(tempPosition, Vector3.down * 0.1f, Color.red, 10);

            lerp += Time.deltaTime * stepSpeed;
        }
        else
        {
            oldPosition = newPosition;
            onStep = false;
        }
    }

    void HipMoveDirection()
    {

        hipMoveDirection = body.transform.position - hipLastPos;
        hipMoveDirection.y = 0f;
        hipMoveDirection = hipMoveDirection.normalized;

        hipLastPos = body.transform.position;

    }

    private void OnDrawGizmos()
    {
        if (Vector3.Distance(newPosition, footOrigin.position) > stepDistance/2)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(footOrigin.position, 0.2f);
       
    }


}


        


