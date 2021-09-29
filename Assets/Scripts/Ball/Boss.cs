using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using System.Threading;
//using System.Diagnostics;

public class Boss : Ball
{
    [SerializeField] GameObject playerBall;
    private bool hasCollided = false;
    [SerializeField] Vector3 direction;
    [SerializeField] int bossHP = 3;
    [SerializeField] int count;
    [SerializeField] float dropGap;
    [SerializeField] float preTime;
    [SerializeField] float dropRange;
    [SerializeField] float sqrShadowRange;
    [SerializeField] float jumpTimeGap = 30;

    RaycastHit hit;
    int sumHP;


    void Start()
    {

        sumHP = bossHP;
        rb = transform.GetComponent<Rigidbody>();

        //sqrShadowRange = transform.GetChild(0).transform.localScale.x * transform.GetChild(0).transform.localScale.x;
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
            yield return new WaitForSeconds(jumpTimeGap);

            //transform.GetChild(0).gameObject.SetActive(false);
            Vector3 dir = new Vector3((playerBall.transform.position - transform.position).x, 0, (playerBall.transform.position - transform.position).z).normalized;
            for (int i = 0; i < count; i++)
            {
                transform.position += new Vector3(0, Mathf.Max(0, 1f - i * 0.01f), 0);
                yield return 0;
            }

            transform.position = new Vector3(playerBall.transform.position.x, 30, playerBall.transform.position.z) - dir * 3;

            yield return new WaitForSeconds(0.5f);
            rb.velocity += dir * 15;
            yield return new WaitForSeconds(1f);
            //transform.GetChild(0).gameObject.SetActive(true);

        }

    }
}
