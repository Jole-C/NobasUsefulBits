using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    protected UIDocument document;
    protected VisualTreeAsset tree;
    protected UIManager manager;

    void Awake()
    {
        InitialiseUI();
        InitialiseReferences();
    }

    protected virtual void Start()
    {
        if (ServiceLocator.TryGetService(out manager))
        {
            document.sortingOrder = manager.UIDepth;
        }
    }

    void InitialiseUI()
    {
        document = GetComponent<UIDocument>();
        tree = document.visualTreeAsset;
    }

    protected virtual void InitialiseReferences()
    {

    }

    protected T Query<T>(string query) where T : VisualElement
    {
        return document.rootVisualElement.Query<T>(query);
    }
}
