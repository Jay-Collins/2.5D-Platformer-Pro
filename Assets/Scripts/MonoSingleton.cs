using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    public static T instance
    {
        get
        {
            if (!_instance)
                Debug.Log(typeof(T)+" is NULL");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this as T;
        
        Init();
    }

    public void Init(){}
}
