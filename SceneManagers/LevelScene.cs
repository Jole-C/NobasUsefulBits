using UnityEngine;
using Noba.Service;
using Noba.Scene;

//todo 

namespace Noba.Scene
{
    public class LevelScene : SceneHandler
    {
        [SerializeField] GameObject uiPrefab;

        protected override void Start()
        {
            if (ServiceLocator.TryGetService(out UIManager uiManager))
            {
                uiManager.SwitchUI(uiPrefab);
            }
        }
    }
}
