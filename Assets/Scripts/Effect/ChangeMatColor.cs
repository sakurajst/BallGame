﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class ChangeMatColor : MonoBehaviour
    {
        Material ChangeColorMaterial
        {
            get
            {
                if (!changeColorMaterial) changeColorMaterial = Resources.Load<Material>("C_Change");
                return changeColorMaterial;
            }
        }
        Material changeColorMaterial;

        new Renderer renderer = null;
        public float time = 1;
        public Vector3 contact = Vector3.zero;
        public Material target = null;
        Vector3 startPos = Vector3.zero;
        float startTime = 0;

        void Start()
        {
            startPos = transform.position;
            renderer = GetComponent<Renderer>();
            Material former = renderer.sharedMaterial;
            renderer.material = ChangeColorMaterial;
            renderer.material.SetVector("_Contact", contact);
            renderer.material.SetFloat("_Range", 0);
            renderer.material.SetColor("_BaseColor", former.GetColor("_BaseColor"));
            renderer.material.SetColor("_EmissionColor", former.GetColor("_EmissionColor"));
            renderer.material.SetColor("_BaseE", target.GetColor("_BaseColor"));
            renderer.material.SetColor("_EmissionE", target.GetColor("_EmissionColor"));
            startTime = Time.time;
        }

        void Update()
        {
            renderer.material.SetVector("_Contact", contact + transform.position - startPos);
            renderer.material.SetFloat("_Range", (Time.time - startTime) / time);
            if (Time.time - startTime > time)
            {

                Destroy(this);
            }
        }
    }
}

