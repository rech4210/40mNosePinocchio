using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moru.UI
{
    /// <summary>
    /// ���߿� ���丮 ���� ���� ����� ����
    /// </summary>
    public class CutSceneViewerUI : StackUIComponent
    {
        [SerializeField] Transform contents;
        [SerializeField] GameObject origin_Prefap;
        [SerializeField] public Image bickIMG;
        public StackUIComponent targetComp;
        public void Start()
        {
            for (int i = 0; i < StackUIManager.Instance.cutScenes.Count; i++)
            {
                var obj = Instantiate(origin_Prefap, contents);
                var comp = obj.GetComponent<CutSceneContentUI>();
                comp.Init(StackUIManager.Instance.cutScenes[i], targetComp);
            }
        }
    }
}