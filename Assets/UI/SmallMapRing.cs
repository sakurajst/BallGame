using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMapRing : MonoBehaviour
{
    private const double V = 0.8;
    [SerializeField] float mapRotateSpeed = (float)V;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, mapRotateSpeed);
    }
}
