using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ATAT : MonoBehaviour
{
    [SerializeField] Text speed;
    [SerializeField] Text stepSpeed;
    [SerializeField] Text stepHeight;

    [SerializeField] ATAT_Brain ATAT_Reference;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexts();
    }

    void UpdateTexts()
    {
        speed.text = (Mathf.Round(ATAT_Reference.speed * 10) / 10).ToString();
        stepSpeed.text = (Mathf.Round(ATAT_Reference.stepSpeed * 10) / 10).ToString();
        stepHeight.text = (Mathf.Round(ATAT_Reference.stepHeight * 10) / 10).ToString();
    }
}
