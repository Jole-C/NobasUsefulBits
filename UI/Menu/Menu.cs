using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Example of a main menu UI using the UI system.
/// </summary>
public class Menu : UI
{
    [SerializeField] string scene = "Game";
    [SerializeField] GameObject optionsPrefab;

    Button startButton;
    Button optionsButton;
    Button quitButton;

    protected override void InitialiseReferences()
    {
        base.InitialiseReferences();

        startButton = Query<Button>("StartButton");
        startButton.clicked += OnStart;

        optionsButton = Query<Button>("OptionsButton");
        optionsButton.clicked += OnOptions;

        quitButton = Query<Button>("QuitButton");
        quitButton.clicked += OnQuit;
    }

    void OnStart()
    {
        Debug.Log(scene);

        SceneManager.LoadScene(scene);
        manager.PopUI();
    }

    void OnOptions()
    {
        manager.PushUI(optionsPrefab);
    }

    void OnQuit()
    {
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif 
    }

    void OnDestroy()
    {
        startButton.clicked -= OnStart;
        quitButton.clicked -= OnQuit;
    }
}
