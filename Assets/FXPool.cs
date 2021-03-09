using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPool : MonoBehaviour
{
    public GameObject m_prefabFX;
    public GameObject m_prefabShipTrail;
    public GameObject m_prefabAsteroidExplosion;
    
    private static GameObject m_prefab;
    private static GameObject m_shiptrail;
    private static GameObject m_asteroidExplosionFx;

    // Start is called before the first frame update
    void Start()
    {
        m_prefab = m_prefabFX;
        m_shiptrail = m_prefabShipTrail;
        m_asteroidExplosionFx = m_prefabAsteroidExplosion;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GetFx()
    {
        GameObject instance = Instantiate(m_prefab);
        return instance;
    }

    public static GameObject GetShipTrail()
    {
        GameObject instance = Instantiate(m_shiptrail);
        return instance;
    }
    
    public static GameObject GetExplosionFx()
    {
        GameObject instance = Instantiate(m_asteroidExplosionFx);
        return instance;
    }
}
