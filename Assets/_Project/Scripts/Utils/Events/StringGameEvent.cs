using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Events/String Game Event")]
public class StringGameEvent : GenericGameEvent<string>
{
    public override void Raise(string t)
    {
        base.Raise(t);
    }
}
