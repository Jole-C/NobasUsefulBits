using UnityEngine;

//todo 

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
