using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有小球的父类
/// </summary>
public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float friction, f, maxSpeed;
    public float speed = 1;
    public List<Vector3> vecs;
    [SerializeField] float reboundForce = 0.3f;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPath(List<Vector3> path)
    {
        vecs = path;
        Debug.Log(vecs.Count);
    }

    public void Roll(Vector3 direction)
    {
        rb.freezeRotation = false;
        //没达到最大速度，增加
        if ((rb.velocity + direction * speed).magnitude < maxSpeed) 
            rb.velocity += direction * speed;
        //当前速度要考虑摩擦力
        if (rb.velocity.sqrMagnitude > 0)
        {
            rb.velocity -= rb.velocity.normalized * friction;
            rb.velocity -= rb.velocity.sqrMagnitude * rb.velocity.normalized * f;

            if (rb.velocity.sqrMagnitude <= 0.25f * friction * friction)
            {
                rb.velocity = Vector3.zero;

            }
            if (rb.velocity == Vector3.zero)
            {
                rb.freezeRotation = true;
            }
            else
            {
                rb.freezeRotation = false;
            }
        }
    }

    protected void Rebound(Collision co)
    {
        Vector3 vself = rb.velocity, vother = co.collider.GetComponent<Rigidbody>().velocity, normal = co.contacts[0].normal;
        rb.velocity += normal * reboundForce * Vector3.Project(vother, normal).magnitude;

        if (co.gameObject.tag == "Boss" && gameObject.name == "PlayerBall")
        {
            rb.velocity += normal * reboundForce * Vector3.Project(vother, normal).magnitude * 10;
        }
    }

    protected void CollideWall(Collision co)
    {
        Vector3 vself = co.relativeVelocity, normal = co.contacts[0].normal;
        rb.velocity += normal * reboundForce * Vector3.Project(vself, -normal).magnitude * 0.3f;
    }

}
