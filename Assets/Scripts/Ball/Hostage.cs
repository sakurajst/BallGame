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
    GameManager gm;
    RaycastHit hit;
    private bool hasCollided = true;
    private static Stack<GameObject> hostagePool = new Stack<GameObject>();
    public static List<Hostage> greyBalls = new List<Hostage>();
    static int GreyMaxNum = 20;

    [SerializeField] Vector3 direction;

    FindPath findPath = null;

    void Start()
    {
        friction = 数值调节类._灰球摩檫力;
        maxSpeed = 数值调节类._灰球最大速度;
        speed = 数值调节类._灰球速度;
        reboundForce = 数值调节类._灰球反弹力;

        gm = GameObject.Find("Manager").GetComponent<GameManager>();
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

    public static void ClearPool()
    {
        hostagePool.Clear();
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

    private void LateUpdate()
    {
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.CompareTag("RedBall") ||collision.gameObject.CompareTag("Player"))
        {
            //灰球变红
            //EffectManager.ChangeColor(gameObject, collision, Resources.Load<Material>("C_Red"));
            this.enabled = false;
            Invoke("Grey2Red", 1);
            hasCollided = true;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            StopCoroutine(RandomMove());
            StartCoroutine(RandomMove());
        }
        if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("SmallBlackBall") || collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("GreyBall") || collision.gameObject.CompareTag("Player"))
        {
            Rebound(collision);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
            CollideWall(collision);
            //Debug.Log("撞墙");
            hasCollided = true;
        }
    }

    void Grey2Red()
    {
        Friend.GenerateSelf(transform.position);
        DestroySelf();
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AirWall"))
        {
            DestroySelf();
            float rangeMin = 20f;
            float rangeMax = 25f;  // gm.rangeMax;
            RaycastHit hit;
            Vector3 pos = other.transform.position;
            
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(rangeMin, rangeMax);
            pos += dir.normalized * 20;
            if (Physics.Raycast(pos, dir, out hit, 50, 1 << 10))
            {
                // Debug.LogFormat("空气墙后重生：射线射中,位置{0}", hit.point - dir.normalized * 0.5f);
                GenerateSelf(hit.point - dir.normalized * 0.5f);
            }
            else
            {
                // Debug.LogFormat("空气墙后重生：射线没有射中,位置{0}", (pos + dir));
                GenerateSelf(pos + dir);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedColorRange"))
        {
            Debug.Log("结束时小球全变红");
            // EffectManager.ChangeColorWhenOver(gameObject, other, Resources.Load<Material>("C_Red"));
            this.enabled = false;
            Invoke("Grey2Red", 1.5f);
        }
    }

    public void DestroySelf()
    {
        rb.velocity = Vector3.zero;
        // Debug.Log("销毁灰球");
        // 灰球数量减一
        gm.SetBallNum("grey", false);

        hostagePool.Push(gameObject);
        gameObject.SetActive(false);

        greyBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        if (GreyMaxNum <= greyBalls.Count)
            return;

        //Debug.Log("生成灰球");
        GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("grey", true);
        GameObject go;
        if (hostagePool.Count > 0)
        {
            go = hostagePool.Pop();
            
        }
        else
        {
            go = Instantiate<GameObject>((GameObject)Resources.Load("Balls/GreyBall"));

        }

        go.transform.parent = GameObject.Find("GreyBalls").transform;
        go.transform.position = pos;
        go.GetComponent<Hostage>().enabled = true;
        go.GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("C_Grey");

        greyBalls.Add(go.GetComponent<Hostage>());
        go.SetActive(true);
    }


    private void OnEnable()
    {

        GetComponent<Hostage>().enabled = true;
        GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("C_Grey");


        //if (GetComponent<MeshRenderer>().materials[0].name != "C_Grey (Instance)")
        //{
        //    //  Debug.LogErrorFormat("灰球材质错误！，当前材质是{0}", GetComponent<MeshRenderer>().materials[0].name);
        //}

    }
}
