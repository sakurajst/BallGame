using Mapbox.Examples;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 灰球：俘虏类
/// 遇见主角或红球变为红色
/// 移动方式：随机移动（使用寻路AI）
/// </summary>
public class Hostage : Ball
{
    [SerializeField] Vector3 direction;

    FindPath findPath = null;
    RaycastHit hit;

    void Start()
    {
        findPath = GameObject.Find("Manager").GetComponent<FindPath>();
        rb = transform.GetComponent<Rigidbody>();
        StartCoroutine(RandomMove());
    }

    void Update()
    {
        if (Physics.SphereCast(transform.position, 1, rb.velocity, out hit, 5, 1 << 10))
            direction += hit.normal * rb.velocity.magnitude + Vector3.Cross(hit.normal, Vector3.up).normalized;

        direction += Vector3.Cross(Vector3.up, rb.velocity).normalized * Random.Range(-1f, 1f);

        direction = direction.normalized;
        Roll(direction);
    }

    IEnumerator RandomMove()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
            findPath.FindTarget(transform.position, pos, SetPath);

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
