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
        playerBall = GameObject.Find("PlayerBall");
        rb = transform.GetComponent<Rigidbody>();
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    public static void ClaerPool()
    {
        friendPool.Clear();
    }

    void Update()
    {
        total = weight1 * factor1 + weight2 * factor2 + weight3 * factor3 + weight4 * factor4 + weight5 * factor5 + weight6 * factor6 + weight7 * factor7 + weight8 * factor8;
        total = new Vector3(total.x, 0, total.z);
        Roll(total.normalized);
    }

}
