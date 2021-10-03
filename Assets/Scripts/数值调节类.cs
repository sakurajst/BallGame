using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 数值调节类 : MonoBehaviour
{
    #region 主角红球数值
    [Header("主角红球属性")]
    [SerializeField] float 主角红球摩檫力;
    [SerializeField] float 主角红球速度;
    [SerializeField] float 主角红球最大速度;
    [SerializeField] float 主角红球反弹力;
    [SerializeField] int 遇Boss打破合体散落数;
    [SerializeField] float 基础合体时间;
    [SerializeField] float 每红球加合体时间;
    [Space()]
    public static float _主角红球摩檫力;
    public static float _主角红球速度;
    public static float _主角红球最大速度;
    public static float _主角红球反弹力;
    public static int _遇Boss打破合体散落数;
    public static float _基础合体时间;
    public static float _每红球加合体时间;
    #endregion

    #region 红球数值
    [Header("红球属性")]
    [SerializeField] float 红球摩檫力;
    [SerializeField] float 红球速度;
    [SerializeField] float 红球最大速度;
    [SerializeField] float 红球反弹力;
    [SerializeField] float 集群权重更新周期;
    [Space()]
    public static float _红球摩檫力;
    public static float _红球速度;
    public static float _红球最大速度;
    public static float _红球反弹力;
    public static float _集群权重更新周期;
    #endregion

    #region 灰球数值
    [Header("灰球属性")]
    [SerializeField] float 灰球摩檫力;
    [SerializeField] float 灰球速度;
    [SerializeField] float 灰球最大速度;
    [SerializeField] float 灰球反弹力;
    [Space()]
    public static float _灰球摩檫力;
    public static float _灰球速度;
    public static float _灰球最大速度;
    public static float _灰球反弹力;
    #endregion

    #region 黑球数值
    [Header("黑球属性")]
    [SerializeField] float 黑球摩檫力;
    [SerializeField] float 黑球速度;
    [SerializeField] float 黑球最大速度;
    [SerializeField] float 黑球反弹力;
    [SerializeField] float 黑球找红球视野平方;
    [Space()]
    public static float _黑球摩檫力;
    public static float _黑球速度;
    public static float _黑球最大速度;
    public static float _黑球反弹力;
    public static float _黑球找红球视野平方;
    #endregion

    #region Boss数值
    [Header("Boss属性")]
    [SerializeField] float Boss摩檫力;
    [SerializeField] float Boss速度;
    [SerializeField] float Boss最大速度;
    [SerializeField] float Boss反弹力;
    [SerializeField] int Boss生命;
    [SerializeField] int Boss跳起时每帧一次上移帧数;
    [SerializeField] int Boss跳起间隔_秒;
    [Space()]
    public static float _Boss摩檫力;
    public static float _Boss速度;
    public static float _Boss最大速度;
    public static float _Boss反弹力;
    public static int _Boss生命;
    public static int _Boss跳起时每帧一次上移帧数;
    public static int _Boss跳起间隔_秒;
    #endregion

    private void Awake()
    {
        初始化();
        
    }

    void 初始化()
    {
        _主角红球摩檫力 = 主角红球摩檫力;
        _主角红球速度 = 主角红球速度;
        _主角红球最大速度 = 主角红球最大速度;
        _主角红球反弹力 = 主角红球反弹力;
        _遇Boss打破合体散落数 = 遇Boss打破合体散落数;
        _基础合体时间= 基础合体时间;
        _每红球加合体时间= 每红球加合体时间;


    _红球摩檫力 = 红球摩檫力;
        _红球速度 = 红球速度;
        _红球最大速度 = 红球最大速度;
        _红球反弹力 = 红球反弹力;
        _集群权重更新周期= 集群权重更新周期;


        _灰球摩檫力 = 灰球摩檫力;
        _灰球速度 = 灰球速度;
        _灰球最大速度 = 灰球最大速度;
        _灰球反弹力 = 灰球反弹力;


        _黑球摩檫力 = 黑球摩檫力;
        _黑球速度 = 黑球速度;
        _黑球最大速度 = 黑球最大速度;
        _黑球反弹力 = 黑球反弹力;
        _黑球找红球视野平方= 黑球找红球视野平方;

        _Boss摩檫力 = Boss摩檫力;
        _Boss速度 = Boss速度;
        _Boss最大速度 = Boss最大速度;
        _Boss反弹力 = Boss反弹力;
        _Boss生命 = Boss生命;
        _Boss跳起时每帧一次上移帧数 = Boss跳起时每帧一次上移帧数;
        _Boss跳起间隔_秒 = Boss跳起间隔_秒;
    }

}
