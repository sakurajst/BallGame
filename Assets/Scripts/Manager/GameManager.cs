using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int redNum = 1, blackNum, greyNum, sumNum;
    private static bool second = false, third = false;

    [SerializeField] int newGreyNum = 20;
    [SerializeField] int newBlackNum = 10;
    [SerializeField] GameObject winWindow;
    [SerializeField] GameObject lostWindow;
    [SerializeField] GameObject holo;
    [SerializeField] GameObject unionButtonArrow;
    public float rangeMin = -25;
    public float rangeMax = 25;
    [SerializeField] int angle1, angle2, angle3;
    [SerializeField] float greyBallR, blackBallR;
    Player player ;
    Coroutine coroutine;
    [SerializeField] GameObject boss;

    InputManager inputManager;
    UIManager ui;


    private void Awake()
    {
        Friend.redBalls.Clear();
        Hostage.greyBalls.Clear();
        Enemy.blackBalls.Clear();

        Friend.ClearPool();
        Hostage.ClearPool();
        Enemy.ClearPool();

    }

    void Start()
    {
        angle1 = 100;
        angle2 = 80;
        angle3 = 70;

        second = false;
        third = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        InitializeBalls();
    }


    public void DivideRedBalls(int changeNum)
    {
        //EffectManager.BreakPower();

        StopCoroutine(coroutine);
        player.transform.localScale = new Vector3(1, 1, 1);

        List<Friend> reds = Friend.redBalls;
        RaycastHit hit;
        int redsCount = reds.Count;
        List<GameObject> changeBalls = new List<GameObject>();
        for (int i = 0; i < redsCount; i++)
        {

            if (!reds[i].gameObject.activeSelf)
            {
                // UnityEngine.Debug.LogFormat("i={0},是隐藏的,循环总数{1}", i,redsCount);
                //Vector3 pos = new Vector3(Random.Range(rangeMin, rangeMax), 0.5f, Random.Range(rangeMin, rangeMax));
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                if (Physics.Raycast(player.transform.position, dir, out hit, 10, 1 << 10))
                {
                    reds[i].transform.position = hit.point - dir.normalized * 0.5f;
                }
                else
                {
                    reds[i].transform.position = player.transform.position + dir * 8;
                }

                if (changeNum > 0)
                {
                    changeBalls.Add(reds[i].gameObject);
                    changeNum--;
                    UnityEngine.Debug.Log("changeNum--;");
                }
                else
                {
                   // EffectManager.GenerateTrail(player.transform, reds[i].transform, 5, 1);
                    StartCoroutine(waitRedAppear(reds[i].gameObject, 1));
                }
            }
        }

        for (int i = 0; i < changeBalls.Count; i++)
        {
            changeBalls[i].GetComponent<Friend>().ChangeSelf();
            //EffectManager.GenerateTrailBlack(player.transform, changeBalls[i].transform, 5, 1);
            StartCoroutine(waitGeneraterEnmey(changeBalls[i].transform.position, 1));
        }

        //UnityEngine.Debug.Log("解除合体,主角" + player.transform.localScale);
        player.GetComponent<Player>().union = false;
    }

    IEnumerator waitRedAppear(GameObject redBall, float t)
    {
        yield return new WaitForSeconds(t);
        redBall.SetActive(true);
    }

    IEnumerator waitGeneraterEnmey(Vector3 pos, float t)
    {
        yield return new WaitForSeconds(t);
        Enemy.GenerateSelf(pos);
    }

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

    public void SetBallNum(string color, bool add)
    {
        if (add)
        {
            switch (color)
            {
                case "red":
                    redNum++;
                    break;
                case "black":
                    blackNum++;
                    break;
                case "grey":
                    greyNum++;
                    break;
                default:
                    UnityEngine.Debug.LogErrorFormat("修改小球个数的字符串有误，，{0}", color);
                    break;
            }
        }
        else
        {
            switch (color)
            {
                case "red":
                    redNum--;
                    break;
                case "black":
                    blackNum--;
                    break;
                case "grey":
                    greyNum--;
                    break;
                default:
                    UnityEngine.Debug.LogErrorFormat("修改小球个数的字符串有误，，{0}", color);
                    break;
            }
        }
        sumNum = redNum + blackNum + greyNum;

        if (sumNum > newGreyNum + newBlackNum)
        {
            if ((float)redNum / sumNum < 0.3)
            {
                player.state = 1;
                //inputManager.maxAngle = angle1;
            }
            else if ((float)redNum / sumNum >= 0.3 && (float)redNum / sumNum <= 0.6)
            {
                player.state = 2;
                //inputManager.maxAngle = angle2;
                if (!second)
                {
                    //UnityEngine.Debug.LogFormat("第二阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                    second = true;
                    blackNum++;
                    GenerateBoss();
                    player.state = 2;

                    unionButtonArrow.SetActive(true);

                }
            }
            else if (third && (float)redNum / sumNum > 0.6)
            {
                player.state = 3;
                //inputManager.maxAngle = angle3;
            }

            if (!second)
            {
                //UnityEngine.Debug.LogFormat("sum{0},{1},{2},{3}",sumNum, redNum, greyNum, blackNum);
                float per = (float)redNum / sumNum / 0.3f;

            }
            else
            {
                float per = Mathf.Max(0, ((float)redNum / sumNum - 0.3f) / 0.6f);

                // PostProcessing.SetSaturation(Mathf.Lerp(-15f,20f, per));
            }
        }


    }


    void GenerateBoss()
    {
        float rangeMin = 3;
        float rangeMax = 5;
        RaycastHit hit;
        Vector3 pos = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
        Vector3 dir = new Vector3((Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax), 0, (Random.Range(-1, 1) + 0.5f) * 2 * Random.Range(rangeMin, rangeMax));
        if (Physics.Raycast(pos, dir, out hit, 3, 1 << 10))
        {
                boss.SetActive(true);
                boss.transform.position = hit.point - dir.normalized * 1.5f;
        }
        else
        {
                boss.SetActive(true);
                boss.transform.position = pos + dir;
        }
    }

    private void checkState()
    {
        if ((float)redNum / sumNum > 0.6)
        {
            if (!third)
            {
                // UnityEngine.Debug.LogFormat("第三阶段,红{0},灰{1},黑{2}，比例{3}", redNum, greyNum, blackNum, (float)redNum / sumNum);
                third = true;

                player.state = 3;
                inputManager.maxAngle = angle3;
                //ui.ChangeUI(redNum, greyNum, blackNum, player.state);
                //切bgm
                //StartCoroutine(Music2TO3());
            }
        }
    }

    IEnumerator BeingUnion()
    {
        float sumTime = 数值调节类._基础合体时间 + Friend.redBalls.Count * 数值调节类._每红球加合体时间;
 


        for (float t = Time.time; Time.time - t < sumTime;)
        {
            //UnityEngine.Debug.LogFormat("合体中...此时sumTime={0},Time.time={1}, t = {2}",sumTime,Time.time,t);
            yield return 0;
        }
        // UnityEngine.Debug.Log("准备执行解除合体函数");


        DivideRedBalls(0);
    }

    public void PlayerWin()
    {

        Invoke("winWindowAppear", 5);
    }

    void winWindowAppear()
    {
        winWindow.SetActive(true);
    }

    public void PlayerLost()
    { 
        Time.timeScale = 0;
        lostWindow.SetActive(true);
    }
}
