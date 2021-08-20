using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//David promises to discuss this 'alien' code
public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => _instance;
    protected static T _instance = null;
    protected virtual void Awake()
    {
        _instance = ((T)(MonoBehaviour)this);
    }
    protected virtual void OnDestroy()
    {
        _instance = null;
    }   
}
