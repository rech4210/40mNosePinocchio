using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public class StackUIManager : SingleToneMono<StackUIManager>
{
    [ShowInInspector] private Stack<StackUIComponent> ui_Stack = new Stack<StackUIComponent>();
    public StackUIComponent cur_UIComponent;
    public StackUIComponent start_UIComponent;
    public StackUIComponent close_UIComponent;
    public delegate void StackUI_Event(StackUIComponent stackUI);
    public event StackUI_Event pop_n_Push_Event;



    protected override void Awake()
    {
        base.Awake();
        if (ui_Stack == null)
        { ui_Stack = new Stack<StackUIComponent>(); }
        var UIComps = FindObjectsOfType<StackUIComponent>(true);
        foreach (var uicomps in UIComps)
        {
            uicomps.Hide();
        }
        start_UIComponent?.Show();

    }


    public void Pop()
    {
        ui_Stack.Pop().Hide();
        pop_n_Push_Event?.Invoke(ui_Stack.Peek());
    }

    public void Push(StackUIComponent comp)
    {
        ui_Stack.Push(comp);
        pop_n_Push_Event?.Invoke(comp);
    }

    public void QuitApplication()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif

    }

    public void GoToTargetUIComponent(StackUIComponent target)
    {
        if (target == null) return;
        if (ui_Stack.Peek() != start_UIComponent)
        {
            while (ui_Stack.Peek() == start_UIComponent)
            {
                ui_Stack.Pop().Hide();
            }
        }
        var comps = target.transform.GetComponentsInParent<StackUIComponent>(true);
        for (int i = comps.Length-1; i >= 0; i--)
        {
            comps[i].Show();
        }
    }


    private void Update()
    {
        if (ui_Stack.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (ui_Stack.Peek() == start_UIComponent)
                {
                    close_UIComponent.Show();
                    return;
                }
                Pop();
            }
        }
    }
}