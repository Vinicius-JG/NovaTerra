﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBehaviour : MonoBehaviour
{

    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    private void Update()
    {

    }


}
