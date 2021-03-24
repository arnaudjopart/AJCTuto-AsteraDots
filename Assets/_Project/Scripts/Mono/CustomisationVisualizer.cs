using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects.Achievements;
using UnityEngine;

public class CustomisationVisualizer : MonoBehaviour
{
    public MeshRenderer m_renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        ShipCustomizationManager.RegisterToCustomEvent(OnSelection);
    }

    private void OnSelection(Material arg0)
    {
        m_renderer.material = arg0;
    }
    
}
