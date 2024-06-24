using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATAT_Brain : MonoBehaviour
{

    [SerializeField] Transform[] arrayLegs;

    [SerializeField] Transform neckBone;
    public float smoothTime = 2F;
    private Vector3 velocity = Vector3.zero;
    private float fvelocity = 0f;
    Vector3 currentNeckRot;
    float currentAverage;
    bool neckRotSwitch;

    int index = 0;

    public float stepSpeed;
    public float stepHeight;

    public float speed;

    void Start()
    {

        transform.GetChild(0).GetChild(0).transform.position -= new Vector3(0f, 2f, 0f);
        StartCoroutine(LegsCycle());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        YAxisMove();
        NeckCycle();

        ChangeStats();

        UpdateFootValues();
    }

    IEnumerator LegsCycle()
    {
        while (true)
        {
            print(index);
            arrayLegs[index].GetComponent<ATAT_IKFoot>().canMoveLeg = true;
            index++;
            if (index == arrayLegs.Length) index = 0;
            yield return new WaitForSeconds(12 / speed / 4);
        }
    }

    void NeckCycle()
    {
        if (currentNeckRot.z > -25f && !neckRotSwitch)
        {
            currentNeckRot = Vector3.SmoothDamp(currentNeckRot, new Vector3(-10f, 0, -30f), ref velocity, smoothTime, 10f);
            neckBone.localEulerAngles = currentNeckRot;
        }
        else
        {
            neckRotSwitch = true;
            if (currentNeckRot.z < 25f && neckRotSwitch)
            {
                currentNeckRot = Vector3.SmoothDamp(currentNeckRot, new Vector3(-10f, 0, 30f), ref velocity, smoothTime, 10f);
                neckBone.localEulerAngles = currentNeckRot;
            }
            else
            {
                neckRotSwitch = false;
            }
        }

    }

    void YAxisMove()
    {
        float averageFootHeight = 0;
        foreach (Transform leg in arrayLegs)
        {
            averageFootHeight += leg.transform.position.y;
        }
        averageFootHeight /= 4;

        currentAverage = Mathf.SmoothDamp(currentAverage, averageFootHeight, ref fvelocity, 0.5f);
        averageFootHeight = currentAverage;

        transform.position = new Vector3(transform.position.x, averageFootHeight - 2.3f, transform.position.z);
    }

    void ChangeStats()
    {
        if (Input.GetKeyDown(KeyCode.X) && speed > 1)
        {
            speed -= 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.C) && speed < 6)
        {
            speed += 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            stepSpeed -= 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            stepSpeed += 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.N) && stepHeight > 0)
        {
            stepHeight -= 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.M) && stepHeight < 6)
        {
            stepHeight += 0.2f;
        }

        if (stepSpeed < speed / 5) stepSpeed = speed / 5;
        if (stepSpeed > speed) stepSpeed = speed;

    }

    void UpdateFootValues()
    {
        foreach (Transform leg in arrayLegs)
        {
            leg.GetComponent<ATAT_IKFoot>().stepHeight = stepHeight;
            leg.GetComponent<ATAT_IKFoot>().stepSpeed = stepSpeed;
        }


    }
}