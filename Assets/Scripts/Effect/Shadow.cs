﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] public Material[] materials;
    [SerializeField] public float size;

    // Update is called once per frame
    void Update()
    {
        SetMaterials();
    }

    void SetMaterials()
    {
        foreach(Material mat in materials)
        {
            mat.SetVector("_Center", transform.position);
            mat.SetFloat("_Range", transform.localScale.x * size);
        }
    }

    private void OnDisable()
    {
        foreach (Material mat in materials)
        {
            mat.SetVector("_Center", new Vector3(0,100,0));
            mat.SetFloat("_Range", 0);
        }
    }
}
