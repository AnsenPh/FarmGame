using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  
public class BaseButton : Button 
{
    public float ScaleRatio = 1.1f;
    public float Duration = 0.1f;

    Action<int> m_Callback = null;
    int m_CustomerData = 0;

    public void SetClickCallback(Action<int> _Callback, int _CustomerData = 0)
    {
        m_Callback = _Callback;
        m_CustomerData = _CustomerData;
    }

    protected override void OnDestroy()
    {
        transform.DOKill(true);
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();
        BindButtonCallback();
    }

    private void BindButtonCallback()
    {
        onClick.AddListener(OnBtn);
    }

    private void OnBtn()
    {
        if(m_Callback!=null)
        {
            m_Callback(m_CustomerData);
        }
        transform.DOKill(true);
        Sequence Seq = DOTween.Sequence();
        Seq.Append(transform.DOScale(ScaleRatio, Duration));
        Seq.Append(transform.DOScale(1.0f, Duration));
    }

    public void SetTitle(string _Title)
    {
        gameObject.transform.Find("Title").GetComponent<Text>().text = _Title;
    }

    public string GetTitle()
    {
        return gameObject.transform.Find("Title").GetComponent<Text>().text;
    }

    public int GetCustmerData()
    {
        return m_CustomerData;
    }
}
