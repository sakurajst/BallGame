using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using System.Threading;

public class Boss : Ball
{
    [SerializeField] GameObject playerBall;
    private bool hasCollided = false;
    [SerializeField] Vector3 direction;
    [SerializeField] float sqrShadowRange;

    RaycastHit hit;
    int nowHP,sumHP;


    void Start()
    {
        friction = 数值调节类._Boss摩檫力;
        maxSpeed = 数值调节类._Boss最大速度;
        speed = 数值调节类._Boss速度;
        reboundForce = 数值调节类._Boss反弹力;

        sumHP = 数值调节类._Boss生命;
        nowHP = sumHP;
        rb = transform.GetComponent<Rigidbody>();

        sqrShadowRange = transform.GetChild(0).transform.localScale.x * transform.GetChild(0).transform.localScale.x;
        //transform.GetComponent<SphereCollider>().radius = transform.GetChild(0).transform.localScale.x * 0.5f;

        StartCoroutine(FindPlayer());

    }


    void Update()
    {
        if (Physics.SphereCast(transform.position, 1, rb.velocity, out hit, 5, 1 << 10))
            direction = (playerBall.transform.position - transform.position).normalized + hit.normal * rb.velocity.magnitude * 0.1f + Vector3.Cross(hit.normal, Vector3.up).normalized;
        else
            direction = (playerBall.transform.position - transform.position).normalized;
        direction += Vector3.Cross(Vector3.up, rb.velocity).normalized * Random.Range(-1f, 1f);
        Roll(direction.normalized);

    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            yield return new WaitForSeconds(数值调节类._Boss跳起间隔_秒);

            transform.GetChild(0).gameObject.SetActive(false);
            Vector3 dir = new Vector3((playerBall.transform.position - transform.position).x, 0, (playerBall.transform.position - transform.position).z).normalized;
            for (int i = 0; i < 数值调节类._Boss跳起时每帧一次上移帧数; i++)
            {
                transform.position += new Vector3(0, Mathf.Max(0, 1f - i * 0.01f), 0);
                yield return 0;
            }

            transform.position = new Vector3(playerBall.transform.position.x, 30, playerBall.transform.position.z) - dir * 3;

            yield return new WaitForSeconds(0.5f);
            rb.velocity += dir * 15;
            yield return new WaitForSeconds(1f);
            transform.GetChild(0).gameObject.SetActive(true);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        // 遇见第三阶段合体主角且体积大于自己
        if (collision.gameObject.CompareTag("Player") && collision.transform.localScale.x >= transform.localScale.x && collision.gameObject.GetComponent<Player>().union)
        {
            // 体积减小，范围减小，炸出黑球
            nowHP--;
            transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, 2.2f, (float)nowHP / sumHP);

            transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1) * Mathf.Lerp(1f, 5f, (float)nowHP / sumHP);
            transform.GetComponent<SphereCollider>().radius = transform.GetChild(0).transform.localScale.x * 0.5f;
            sqrShadowRange = transform.GetChild(0).transform.localScale.x * transform.GetChild(0).transform.localScale.x;

            // 炸出黑球在范围边缘
            int a = 2;
            for (int i = 0; i < a; i++)
            {
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                // EffectManager.GenerateTrailBlack(transform, transform.position + dir * transform.GetChild(0).transform.localScale.x * 1.5f, 5, 1);
                Enemy.GenerateSelf(transform.position + dir * transform.GetChild(0).transform.localScale.x * 1.5f);
            }

            if (nowHP <= 0)
            {
                GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("black", false);

                Destroy(GameObject.Find("BossRings"));
                //EffectManager.ChangeColor(gameObject, collision, Resources.Load<Material>("C_Red"));
                this.enabled = false;
                Invoke("Boss2Red", 1);
                GameObject.Find("Manager").GetComponent<GameManager>().PlayerWin();

                hasCollided = true;


            }
            hasCollided = true;
            StartCoroutine(AllowCollide());
        }
    }

    void Boss2Red()
    {
        gameObject.SetActive(false);
        Friend.GenerateSelf(transform.position);
        Destroy(gameObject);
    }

    IEnumerator AllowCollide()
    {
        yield return new WaitForSeconds(1);
        hasCollided = false;
    }
}
