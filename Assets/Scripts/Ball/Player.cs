using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主角
/// </summary>
public class Player : Ball
{
    GameManager gm ;
    bool hasCollided = false;
    public int state = 1; //123对应小中大
    public bool union = false;

    void Start()
    {
        friction = 数值调节类._主角红球摩檫力;
        maxSpeed = 数值调节类._主角红球最大速度;
        speed = 数值调节类._主角红球速度;
        reboundForce = 数值调节类._主角红球反弹力;

        state = 1;
        rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        //if (hasCollided) 
        //    return;


        if (union)
        {
            hasCollided = true;
            StartCoroutine(AllowPlayerCollide(2));
            if (collision.gameObject.CompareTag("Boss"))
            {
                Rebound(collision);
                // 体积小于boss
                if (transform.localScale.x < collision.transform.localScale.x)
                {
                    gm.DivideRedBalls(数值调节类._遇Boss打破合体散落数);
                }
                // 体积大于boss
                else
                {
                    gm.DivideRedBalls(0);
                }
            }
            // 遇到范围内小黑球
            else if (collision.gameObject.CompareTag("SmallBlackBall") && collision.transform.GetComponent<Enemy>().fast)
            {
                gm.DivideRedBalls(1);
            }

            StartCoroutine(AllowPlayerCollide(2));
        }
        // 主角一阶段碰到大黑，小黑均死亡
        else if (state == 1)
        {
            if (collision.gameObject.CompareTag("SmallBlackBall") || collision.gameObject.CompareTag("Boss"))
            {
                // 主角变灰
                DestroySelf();
            }
        }

        if (collision.gameObject.CompareTag("Boss")|| collision.gameObject.CompareTag("SmallBlackBall") || collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("GreyBall"))
        {
            Rebound(collision);
            hasCollided = true;
        }



        if (collision.gameObject.layer == 10)
        {
            CollideWall(collision);
            hasCollided = true;
            StartCoroutine(AllowPlayerCollide(0.2f));
        }
    }

    IEnumerator AllowPlayerCollide(float t)
    {
        yield return new WaitForSeconds(t);
        hasCollided = false;
    }

    protected void Hurt()
    {
        if (union)
        {
            gm.DivideRedBalls(1);
        }
    }

    protected void DestroySelf()
    {
        GetComponent<MeshRenderer>().materials[0].EnableKeyword("_Emission");
        GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.black);
        GetComponent<MeshRenderer>().materials[0].color = Color.grey;
        // 游戏失败
        gm.PlayerLost();
    }

}
