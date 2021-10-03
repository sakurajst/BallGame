using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend : Ball
{
    GameManager gm;
    GameObject playerBall;
    private bool hasCollided = false;
    private static Stack<GameObject> friendPool = new Stack<GameObject>();
    public static List<Friend> redBalls = new List<Friend>();

    public Vector3 factor1, factor2, factor3, factor4, factor5, factor6, factor7, factor8;
    [SerializeField] Vector3 total;
    [SerializeField] float weight1 = 1, weight2 = 1, weight3 = 1, weight4 = 1, weight5 = 1, weight6 = 1, weight7 = 1, weight8 = 1;

    void Start()
    {
        friction = 数值调节类._红球摩檫力;
        maxSpeed = 数值调节类._红球最大速度;
        speed = 数值调节类._红球速度;
        reboundForce = 数值调节类._红球反弹力;

        playerBall = GameObject.FindGameObjectWithTag("Player");
        rb = transform.GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        playerBall = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Friend>().enabled = true;
        GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("C_Red");

        //if (GetComponent<MeshRenderer>().materials[0].name != "C_Red (Instance)")
        //{
        //    // Debug.LogErrorFormat("红球材质错误！，当前材质是{0}", GetComponent<MeshRenderer>().materials[0].name);
        //}
    }

    public static void ClearPool()
    {
        friendPool.Clear();
    }

    void Update()
    {
        total = weight1 * factor1 + weight2 * factor2 + weight3 * factor3 + weight4 * factor4 + weight5 * factor5 + weight6 * factor6 + weight7 * factor7 + weight8 * factor8;
        total = new Vector3(total.x, 0, total.z);
        Roll(total.normalized);
    }

    private void LateUpdate()
    {
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.CompareTag("SmallBlackBall"))
        {
            //红球变灰
           // EffectManager.ChangeColor(gameObject, collision, Resources.Load<Material>("C_Grey"));
            this.enabled = false;
            Invoke("Red2Grey", 1);


            hasCollided = true;
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            //红球变黑
           // EffectManager.ChangeColor(gameObject, collision, Resources.Load<Material>("C_Black"));
            this.enabled = false;
            Invoke("Red2Black", 1);


            hasCollided = true;
        }

        if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("SmallBlackBall") || collision.gameObject.CompareTag("RedBall") || collision.gameObject.CompareTag("GreyBall") || collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log(collision.gameObject.tag);
            Rebound(collision);
            // Debug.Log(rb.velocity);
            hasCollided = true;
        }

        if (collision.gameObject.layer == 10)
        {
            CollideWall(collision);
            //Debug.Log("撞墙");
            hasCollided = true;
        }
    }

    void Red2Grey()
    {
        DestroySelf();
        Hostage.GenerateSelf(transform.position);
    }

    void Red2Black()
    {
        DestroySelf();
        Enemy.GenerateSelf(transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AirWall"))
        {
            DestroySelf();
            float rangeMin = 20f;
            float rangeMax = 25f; // gm.rangeMax;
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
                //  Debug.LogFormat("空气墙后重生：射线没有射中,位置{0}", (pos + dir));
                GenerateSelf(pos + dir);
            }
        }
    }

    protected void DestroySelf()
    {
        rb.velocity = Vector3.zero;
        //Debug.Log("销毁红球");
        gm.SetBallNum("red", false);

        friendPool.Push(gameObject);
        gameObject.SetActive(false);

        redBalls.Remove(this);
    }

    public void ChangeSelf()
    {
        rb.velocity = Vector3.zero;
        gm.SetBallNum("red", false);
        friendPool.Push(gameObject);
        gameObject.SetActive(false);

        redBalls.Remove(this);
    }

    public static void GenerateSelf(Vector3 pos)
    {
        //Debug.Log("生成红球");
        GameObject.Find("Manager").GetComponent<GameManager>().SetBallNum("red", true);

        GameObject go;
        if (friendPool.Count > 0)
        {
            go = friendPool.Pop();
        
        }
        else
        {
            go = Instantiate<GameObject>((GameObject)Resources.Load("Balls/RedBall"));

        }

    
        go.GetComponent<Friend>().enabled = true;
        go.GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("C_Red");
        go.transform.parent = GameObject.Find("RedBalls").transform;
        go.transform.position = pos;



        redBalls.Add(go.GetComponent<Friend>());
        go.SetActive(true);
    }

}
