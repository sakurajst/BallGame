using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主角
/// </summary>
public class Player : Ball
{
    GameManager gm = null;


    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        //gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    protected void DestroySelf()
    {
        GetComponent<MeshRenderer>().materials[0].color = Color.grey;

        //游戏失败，调用GameManger的函数
        gm.PlayerLost();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
