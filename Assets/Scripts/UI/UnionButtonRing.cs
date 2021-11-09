using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionButtonRing : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1;

    void Update()
    {
        transform.Rotate(0,0, rotateSpeed);
    }
}
