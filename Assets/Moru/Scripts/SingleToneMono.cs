using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleToneMono<T> : MonoBehaviour where T : SingleToneMono<T>
{
    protected static T m_instance;
    public static T Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<T>();
            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
            m_instance = this as T;

        else if (m_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}

public class SingleTone<T> where T : class, new()
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            // 만약 instance가 존재하지 않을 경우 새로 생성한다.
            if (_instance == null)
            {
                _instance = new T();

            }
            // _instance를 반환한다.
            return _instance;
        }
    }

    public void DoSomething()
    {
        Debug.Log("Singleton : DoSomething!");
    }
}