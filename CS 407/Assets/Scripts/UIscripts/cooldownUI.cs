﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class cooldownUI : MonoBehaviour
{
    public float cooldown = 5;
    public Image image;
    private bool Cool;

    // Start is called before the first frame update
    void Start()
    {
        Cool = true;
        image.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Cool == true){
            if(Input.GetMouseButtonDown(1)){
                Cool = false;
            }
        }

        if(Cool == false){
            image.fillAmount += 1 / cooldown * Time.deltaTime;
            if(image.fillAmount >= 1){
                Cool = true;
                image.fillAmount = 0;
            }
        }
    }
}
