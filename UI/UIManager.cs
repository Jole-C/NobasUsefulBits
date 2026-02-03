using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// UI manager service supporting multiple UIs that can be pushed and popped.
/// </summary>

public class UIManager : ServiceMonoBehaviour
{
    List<GameObject> uiStack = new List<GameObject>();

    public ReadOnlyCollection<GameObject> UIStack;
    public int UIDepth => uiStack.Count + 1;

    protected override void Awake()
    {
        ServiceLocator.RegisterService(this);
        DontDestroyOnLoad(this);

        UIStack = uiStack.AsReadOnly();
    }

    public void PushUI(GameObject ui)
    {
        GameObject newUI = Instantiate(ui, default, default, transform);

        uiStack.Add(newUI);
    }

    public void PushUI<T>(GameObject ui, out T uiControllerReference) where T : UI
    {
        GameObject newUI = Instantiate(ui, default, default, transform);

        uiControllerReference = newUI.GetComponent<T>();

        uiStack.Add(newUI);
    }

    public void PopUI()
    {
        if (uiStack.Count <= 0)
        {
            Debug.LogError("Trying to pop from an empty stack.");
            return;
        }

        int lastElement = uiStack.Count - 1;
        GameObject ui = uiStack[lastElement];

        uiStack.RemoveAt(lastElement);

        Destroy(ui);
    }

    public bool RemoveUI(GameObject ui)
    {
        return uiStack.Remove(ui);
    }

    public void SwitchUI(GameObject ui)
    {
        ClearStack();
        PushUI(ui);
    }

    public void SwitchUI<T>(GameObject ui, out T uiControllerReference) where T : UI
    {
        ClearStack();
        PushUI<T>(ui, out uiControllerReference);
    }

    void ClearStack()
    {
        foreach (GameObject ui in uiStack)
        {
            Destroy(ui);
        }

        uiStack.Clear();
    }

    void OnDestroy()
    {
        ServiceLocator.UnregisterService<UIManager>();
    }
}
