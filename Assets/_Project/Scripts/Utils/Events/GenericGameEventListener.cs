using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GenericGameEventListener<T> : MonoBehaviour {

    
    protected abstract void OnEnable();

    protected abstract void OnDisable();

    public abstract void OnEventRaised(T t);
    
}
