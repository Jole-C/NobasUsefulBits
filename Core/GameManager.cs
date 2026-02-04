using UnityEngine;
using UnityEngine.SceneManagement;
using Noba.Service;

/// <summary>
/// Basic GameManager class.
/// </summary>
/// 

namespace Noba.Example
{
    public class GameManager : ServiceMonoBehaviour
    {
        [SerializeField] string startingScene = "Menu";

        protected override void Awake()
        {
            ServiceLocator.RegisterService(this);
            DontDestroyOnLoad(this);
        }

        protected override void Start()
        {
            SceneManager.LoadScene(startingScene);
        }

        void OnDestroy()
        {
            ServiceLocator.UnregisterService<GameManager>();
        }
    }
}