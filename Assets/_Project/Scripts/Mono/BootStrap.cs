using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts.Mono
{
    public class BootStrap : MonoBehaviour
    {
        public Camera m_camera;
        private EntityManager m_entityManager;

        private void Awake()
        {
            
            m_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            var screenInfoEntity = m_entityManager.CreateEntity(ComponentType.ReadOnly<ScreenDataComponent>());

            var screenSize = GetScreenSize();

            var sys = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<OffScreenDetectionSystem>();
            sys.SetSingleton(new ScreenDataComponent()
            {
                m_width = screenSize.x,
                m_height = screenSize.y,
            });
        }


        void Start()
        { 
            
            m_entityManager.CreateEntity(typeof(InputComponentData));
        
        }
        
        private float2 GetScreenSize()
        {
            var bottomLeftCornerPosition = m_camera.ViewportToWorldPoint(new Vector3(0, 0));
            var topRightCornerPosition = m_camera.ViewportToWorldPoint(new Vector3(1, 1));
            var heightOfScreen = topRightCornerPosition.y - bottomLeftCornerPosition.y;
            var widthOfScreen = topRightCornerPosition.x - bottomLeftCornerPosition.x;
            var size = new float2(widthOfScreen, heightOfScreen);
            return size;
        }
    }
}


public struct ScreenDataComponent : IComponentData
{
    public float m_height;
    public float m_width;
}