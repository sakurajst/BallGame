using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using UnityEngine.SocialPlatforms;

public class Enemy : Ball
{
    GameObject playerBall;
    RaycastHit hit;
    [SerializeField] int sqrDis = 36;
    [SerializeField] Vector3 direction;
    bool openCorouitine = false;

    void Start()
    {
        playerBall = GameObject.Find("PlayerBall");
        rb = transform.GetComponent<Rigidbody>();

        StartCoroutine(RandomMove());
    }

    void Update()
    {
        if (Physics.SphereCast(transform.position, 1, rb.velocity, out hit, 5, 1 << 10))
            direction += hit.normal * rb.velocity.magnitude * 0.8f + Vector3.Cross(hit.normal, Vector3.up).normalized;

        direction += Vector3.Cross(Vector3.up, rb.velocity).normalized * Random.Range(-1f, 1f);

        int index = FindRedBall();
        if (index >= 0)
        {
                if ((Friend.redBalls[index].transform.position - transform.position).sqrMagnitude < (playerBall.transform.position - transform.position).sqrMagnitude)
                    direction = (Friend.redBalls[index].transform.position - transform.position).normalized;
                else
                    direction = (playerBall.transform.position - transform.position).normalized;
                StopCoroutine(RandomMove());
                openCorouitine = false;
        }
        else
        {
                if (!openCorouitine)
                {
                    StartCoroutine(RandomMove());
                    openCorouitine = true;
                }

        }

        direction = direction.normalized;
        Roll(direction);
        
    }

    int FindRedBall()
    {
        for (int i = 0; i < Friend.redBalls.Count; i++)
        {
            if ((Friend.redBalls[i].transform.position - transform.position).sqrMagnitude < sqrDis)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator RandomMove()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            GameObject.Find("Manager").GetComponent<FindPath>().FindTarget(transform.position, pos, SetPath);
            yield return new WaitForSeconds(4);
            foreach (Vector3 v in vecs)
            {
                while (true)
                {
                    direction += (v - transform.position).normalized;
                    if ((v - transform.position).sqrMagnitude <= 1)
                        break;
                    yield return 0;
                }

                yield return 0;
            }

            yield return 0;
        }
    }

}
