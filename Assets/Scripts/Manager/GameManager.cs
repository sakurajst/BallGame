using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int newGreyNum = 20;
    [SerializeField] int newBlackNum = 10;
    public float rangeMin = -25;
    public float rangeMax = 25;
    [SerializeField] float greyBallR, blackBallR;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerLost()
    {

    }

    /// <summary>
    /// 初始化：随机生成一批灰球和黑球
    /// TODO:生成的位置随机性不够
    /// </summary>
    private void InitializeBalls()
    {
        //生成一堆灰球黑球

        RaycastHit hit;
        int range = 5;
        for (int i = 0; i < newGreyNum; i++)
        {

            Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(rangeMin + range, rangeMax - range);
            pos = new Vector3(pos.x, 0.5f, pos.z);
            Vector3 dir = new Vector3(pos.x, 0, pos.z).normalized * Random.Range(rangeMax - range, rangeMax);
            if (Physics.Raycast(pos, dir, out hit, 40, 1 << 10))
            {
                Hostage.GenerateSelf(hit.point - dir.normalized * greyBallR);
            }
            else
            {
                Hostage.GenerateSelf(pos + dir);
            }

        }
        for (int i = 0; i < newBlackNum; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(rangeMin + range, rangeMax - range);
            pos = new Vector3(pos.x, 0.5f, pos.z);
            Vector3 dir = new Vector3(pos.x, 0, pos.z).normalized * Random.Range(rangeMax - range, rangeMax);
            if (Physics.Raycast(pos, dir, out hit, 40, 1 << 10))
            {
                Enemy.GenerateSelf(hit.point - dir.normalized * blackBallR);
            }
            else
            {
                Enemy.GenerateSelf(pos + dir);

            }
        }
    }
}
