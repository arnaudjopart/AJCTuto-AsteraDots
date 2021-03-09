using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Events/int Game Event")]
public class IntGameEvent : GenericGameEvent<int>
{
    public override void Raise(int t)
    {
        base.Raise(t);
    }
}
