
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastBlock : MonoBehaviour
{
    public int color = -1;
    public Vector2Int pos = new Vector2Int(-1,-1);
    
    public void Select()
    {
        BlastGame.blastManager.SelectBlock(pos.x, pos.y);
    }

    private void OnMouseDown()
    {
        BlastGame.blastManager.SelectBlock(pos.x, pos.y);
    }
}
