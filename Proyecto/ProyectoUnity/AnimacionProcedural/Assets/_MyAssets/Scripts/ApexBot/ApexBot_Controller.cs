using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApexBot_Controller : MonoBehaviour
{
    [Range(-30.0f, 30.0f)]
    public float speed;
    [SerializeField] float stepDistance;
    [SerializeField] float stepHeight;

    bool inLimitV;
    bool inLimitH;
    [SerializeField] float[] limiteVertical = new float[2];
    [SerializeField] float[] limiteHorizontal = new float[2];   

    [SerializeField] GameObject[] IKLegs;
    [SerializeField] GameObject head;

    [SerializeField] float hipHeight;

    float currentAverage;
    [SerializeField] float smoothTime = 0.01F;
    private float fvelocity = 0f;
    private Vector3 vvelocity = Vector3.zero;
    Vector3 currentRot;


    [Header("Breathing")]
    [SerializeField] float breathingFreq;
    [SerializeField] float breathingAmp;

    [SerializeField] Slider speedSlider;


    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).transform.position -= new Vector3(0f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        speed = speedSlider.value;

        Move();
        YAxisMove();
        BodyRotation();
        SetLegsSpeed();
        //Breathing();
    }




    void Move()
    {
        Vector3 moveVector = Vector3.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        if ((y > 0 && transform.position.z < limiteVertical[1]) || (y < 0 && transform.position.z > limiteVertical[0]))
        {
            moveVector += Vector3.forward * y;
        }

        if ((x > 0 && transform.position.x < limiteHorizontal[1]) || (x < 0 && transform.position.x > limiteHorizontal[0]))
        {
            moveVector += Vector3.right * x;
        }

        Vector3.Normalize(moveVector);

        transform.Translate(moveVector * speed * Time.deltaTime);


        /*
        if (Input.GetKey(KeyCode.Q))
        {
            print("a");
            transform.Rotate(Vector3.up * speed * 20 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-Vector3.up * speed * 20 * Time.deltaTime);
        }
        */
    }

    void YAxisMove()
    {
        float averageFootHeight = 0;
        for (int i = 0; i < IKLegs.Length; i++)
        {
            averageFootHeight += IKLegs[i].transform.position.y;
        }
        averageFootHeight /= 4;

        currentAverage = Mathf.SmoothDamp(currentAverage, averageFootHeight, ref fvelocity, smoothTime);
        averageFootHeight = currentAverage;

        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * breathingFreq) * breathingAmp + averageFootHeight, transform.position.z);
    }

    void BodyRotation()
    {
        Vector3 vector1 = (IKLegs[3].transform.position - IKLegs[0].transform.position).normalized;
        Vector3 vector2 = (IKLegs[1].transform.position - IKLegs[2].transform.position).normalized;

        Vector3 crossProduct = Vector3.Cross(vector1, vector2).normalized;

        currentRot = Vector3.SmoothDamp(currentRot, crossProduct, ref vvelocity, smoothTime);
        transform.up = currentRot;  

    }

    void SetLegsSpeed()
    {
        float speed = GetComponent<ApexBot_Controller>().speed;
        foreach (GameObject leg in IKLegs)
        {
            leg.GetComponent<IKFootSolver>().stepSpeed = speed * 2;
        }

    }

    public bool LegsOnGround()
    {
        foreach (GameObject leg in IKLegs)
        {
            if(leg.GetComponent<IKFootSolver>().onStep)
            {
                
                return false;
            }
        }
        return true;
    }



    void Breathing()
    {
        transform.position += new Vector3(transform.position.x, Mathf.Sin(Time.time * breathingFreq) * breathingAmp, transform.position.z);
    }

   

}
