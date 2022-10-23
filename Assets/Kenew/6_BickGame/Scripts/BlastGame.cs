using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BlastGame : MonoBehaviour
{
    public static int failCount = 0;

    private int findBlock = 100;
    public int SuffleScore = 999999;
    public int DisturbScore = 999999;
    private bool suffled = false;
    private bool disturbed = false;
    public GameObject DisturbObj1, DisturbObj2;

    public GameObject blockBaseObj;
    public List<Sprite> blockList = new List<Sprite>();
    public Vector2 BlockDistacne = new Vector2(1, 1);
    public Vector2 BlockSize = new Vector2(1, 1);

    public static BlastGame blastManager;
    private GameObject[,] blockArray;
    private bool[,] visitCheck;
    private int score;

    public int width, height;
    public Vector2 Pos = new Vector2(0, -5);

    private int currentStage = 1;

    public Text stageText;
    public Text scoreText;

    private void Awake()
    {
        blastManager = this;
    }

    public void StarHelp()
    {
        for (int i = 0; i < height; i++)
        {
            blockArray[0, i].GetComponent<SpriteRenderer>().sprite = blockList[0];
            blockArray[0, i].GetComponent<BlastBlock>().color = 0;
        }
    }

    public void SelectBlock(int x, int y)
    {
        GameObject startObj = blockArray[x, y];

        var blocks = LinkedBlocksFind(startObj);

        // 연결된 블록들이 2이하 일경우 종료
        if (blocks.Count <= 2)
        {
            // Debug.Log("클릭한 블록 수 : " + blocks.Count);
            return;
        }

        KeyValuePair<int, int> data = PopBlock(blocks);

        score += data.Value * 15 - 35;
        scoreText.text = score.ToString();

        RequireCheck(data.Key, data.Value);
        if (score >= SuffleScore && !suffled)
        {
            BoardSuffle();
            suffled = true;
        }

        if (score >= DisturbScore && !disturbed)
        {
            StartCoroutine(disturbCo());
            disturbed = true;
        }

        if (!AllLinkedBlocksFind())
        {
            // 현재 스테이지 새로고침
            InitGame(currentStage);
        }
    }
    IEnumerator disturbCo()
    {
        DisturbObj1.SetActive(true);
        DisturbObj2.SetActive(true);

        DisturbObj1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        DisturbObj2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        for (int i = 1; i <= 50; i++)
        {
            DisturbObj1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * 0.02f);
            DisturbObj2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(10);

        for (int i = 49; i >= 0; i--)
        {
            DisturbObj1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * 0.02f);
            DisturbObj2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i * 0.02f);
            yield return new WaitForSeconds(0.01f);
        }

        DisturbObj1.SetActive(false);
        DisturbObj2.SetActive(false);

    }
    private KeyValuePair<int, int> PopBlock(List<GameObject> list)
    {
        SoundManagers.Instance.PlayBGM("BlockPop");

        int count = 0;
        int color = -1;
        Vector2Int pos;
        GameObject obj;
        for (int i = 0; i < list.Count; i++)
        {
            obj = list[i];
            color = obj.GetComponent<BlastBlock>().color;
            pos = obj.GetComponent<BlastBlock>().pos;
            count += 1;
            Destroy(obj);
            blockArray[pos.x, pos.y] = null;
        }

        FillBlank();
        return new KeyValuePair<int, int>(color, count);
    }

    void FillBlank()
    {
        GameObject obj;
        int colorIndex;
        Vector2Int size = new Vector2Int(width - 1, 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blockArray[i, j] == null)
                {
                    for (int k = j; k < height - 1; k++)
                    {
                        blockArray[i, k] = blockArray[i, k + 1];
                    }
                    blockArray[i, height - 1] = null;
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blockArray[i, j] == null)
                {
                    colorIndex = Random.Range(0, blockList.Count);
                    obj = Instantiate(blockBaseObj);
                    obj.GetComponent<SpriteRenderer>().sprite = blockList[colorIndex];
                    obj.GetComponent<BlastBlock>().color = colorIndex;
                    obj.GetComponent<BlastBlock>().pos = new Vector2Int(i, j);
                    blockArray[i, j] = obj;
                    obj.transform.localScale = BlockSize;
                }

                blockArray[i, j].transform.position = new Vector2((-size.x / 2f + i) * BlockDistacne.x, j * BlockDistacne.y) + Pos;
                blockArray[i, j].GetComponent<BlastBlock>().pos = new Vector2Int(i, j);
            }
        }
    }

    void BoardSuffle()
    {
        SoundManagers.Instance.PlaySFX("");

        int a, b;
        Vector2 PosTemp;
        GameObject objTemp;
        Vector2Int pos;
        for (int i = 0; i < width; i++)
        {
            a = Random.Range(0, width);
            b = Random.Range(0, width);
            for (int j = 0; j < height; j++)
            {
                objTemp = blockArray[a, j];
                blockArray[a, j] = blockArray[b, j];
                blockArray[b, j] = objTemp;

                pos = blockArray[a, j].GetComponent<BlastBlock>().pos;
                blockArray[a, j].GetComponent<BlastBlock>().pos = blockArray[b, j].GetComponent<BlastBlock>().pos;
                blockArray[b, j].GetComponent<BlastBlock>().pos = pos;

                PosTemp = blockArray[a, j].transform.position;
                blockArray[a, j].transform.position = blockArray[b, j].transform.position;
                blockArray[b, j].transform.position = PosTemp;
            }
        }

        for (int i = 0; i < height; i++)
        {
            a = Random.Range(0, height);
            b = Random.Range(0, height);
            for (int j = 0; j < width; j++)
            {
                objTemp = blockArray[j, a];
                blockArray[j, a] = blockArray[j, b];
                blockArray[j, b] = objTemp;

                pos = blockArray[j, a].GetComponent<BlastBlock>().pos;
                blockArray[j, a].GetComponent<BlastBlock>().pos = blockArray[j, b].GetComponent<BlastBlock>().pos;
                blockArray[j, b].GetComponent<BlastBlock>().pos = pos;

                PosTemp = blockArray[j, a].transform.position;
                blockArray[j, a].transform.position = blockArray[j, b].transform.position;
                blockArray[j, b].transform.position = PosTemp;
            }
        }
    }

    List<GameObject> LinkedBlocksFind(GameObject startBlock)
    {
        List<Vector2Int> dPos = new List<Vector2Int>();
        dPos.Add(new Vector2Int(0, 1));
        dPos.Add(new Vector2Int(0, -1));
        dPos.Add(new Vector2Int(1, 0));
        dPos.Add(new Vector2Int(-1, 0));

        int color = startBlock.GetComponent<BlastBlock>().color;

        List<GameObject> list = new List<GameObject>();
        Vector2Int startPos = startBlock.GetComponent<BlastBlock>().pos;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                visitCheck[i, j] = false;
        }
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPos);

        Vector2Int nowPos, tempPos;
        while (queue.Count > 0)
        {
            nowPos = queue.Dequeue();
            list.Add(blockArray[nowPos.x, nowPos.y]);
            visitCheck[nowPos.x, nowPos.y] = true;
            for (int i = 0; i < 4; i++)
            {
                tempPos = nowPos + dPos[i];
                if (tempPos.x < 0 || tempPos.x >= width || tempPos.y < 0 || tempPos.y >= height) continue;
                //if (!visitCheck[tempPos.x, tempPos.y] && blockArray[tempPos.x, tempPos.y] != null && blockArray[tempPos.x, tempPos.y].GetComponent<BlastBlock>().color == color)
                if (!visitCheck[tempPos.x, tempPos.y] && blockArray[tempPos.x, tempPos.y].GetComponent<BlastBlock>().color == color)
                {
                    queue.Enqueue(tempPos);
                    visitCheck[tempPos.x, tempPos.y] = true;
                }
            }
        }

        foreach (var li in list)
        {
            Debug.Log(li.GetComponent<BlastBlock>().color);
        }

        return list;
    }

    /// <summary>
    /// 필드에 같은 종류의 블록이 3개이상 있는지 확인합니다.
    /// </summary>
    private bool AllLinkedBlocksFind()
    {
        List<Vector2Int> dPos = new List<Vector2Int>();
        dPos.Add(new Vector2Int(0, 1));
        dPos.Add(new Vector2Int(0, -1));
        dPos.Add(new Vector2Int(1, 0));
        dPos.Add(new Vector2Int(-1, 0));

        List<GameObject> list = new List<GameObject>();

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int color = blockArray[w, h].GetComponent<BlastBlock>().color;
                Vector2Int startPos = blockArray[w, h].GetComponent<BlastBlock>().pos;
                list = new List<GameObject>();

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        visitCheck[i, j] = false;
                    }
                }

                Queue<Vector2Int> queue = new Queue<Vector2Int>();
                queue.Enqueue(startPos);

                Vector2Int nowPos, tempPos;
                while (queue.Count > 0)
                {
                    nowPos = queue.Dequeue();
                    list.Add(blockArray[nowPos.x, nowPos.y]);
                    visitCheck[nowPos.x, nowPos.y] = true;
                    for (int i = 0; i < 4; i++)
                    {
                        tempPos = nowPos + dPos[i];
                        if (tempPos.x < 0 || tempPos.x >= width || tempPos.y < 0 || tempPos.y >= height)
                        {
                            continue;
                        }

                        if (!visitCheck[tempPos.x, tempPos.y] &&
                            blockArray[tempPos.x, tempPos.y].GetComponent<BlastBlock>().color == color)
                        {
                            queue.Enqueue(tempPos);
                            visitCheck[tempPos.x, tempPos.y] = true;
                        }
                    }
                }

                // 찾은게 3개이상있다면
                if (list.Count >= 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public float limite_time = 60;
    public Text remainText;
    public Color EmergencyColor = new Color(255, 0, 0, 255);
    public Color NormalColor = new Color(58, 58, 58, 255);

    private void FixedUpdate()
    {
        limite_time -= Time.deltaTime;
        remainText.text = Mathf.Round(limite_time).ToString();
        remainText.color = limite_time <= 20 ? EmergencyColor : NormalColor;

        if (limite_time <= 0f)
        {
            StageFailed();
            StartCoroutine(wait());
        }
    }


    private IEnumerator wait()
    {
        limite_time = 0;
        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0;
    }

    public void StageFailed()
    {
        failCount++;
        WSceneManager.instance.OpenGameFailUI();
    }

    public void InitGame(int stageLevel)
    {
        SoundManager.PlaySFX(Shake);
        CameraController.Instance.OnShake(2.5f, 2.5f, false, true);

        if (blockArray != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (blockArray[i, j] != null)
                    {
                        Destroy(blockArray[i, j].gameObject);
                    }
                }
            }
        }

        suffled = false;
        blockArray = new GameObject[width, height];
        visitCheck = new bool[width, height];

        int colorIndex;

        Vector2Int size = new Vector2Int(width - 1, 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                colorIndex = Random.Range(0, blockList.Count);
                GameObject obj = Instantiate(blockBaseObj);
                obj.GetComponent<SpriteRenderer>().sprite = blockList[colorIndex];
                obj.GetComponent<BlastBlock>().color = colorIndex;
                obj.GetComponent<BlastBlock>().pos = new Vector2Int(i, j);
                blockArray[i, j] = obj;

                obj.transform.position = new Vector2((-size.x / 2f + i) * BlockDistacne.x, j * BlockDistacne.y) + Pos;
                obj.transform.localScale = BlockSize;
            }
        }

        if (!AllLinkedBlocksFind())
        {
            // 현재 스테이지 새로고침
            InitGame(currentStage);
        }
        else
        {
            if (!isRequireInit)
            {
                RequireItemInit();
                RequireItemListUIUpdate();

                isRequireInit = true;
            }
        }
    }

    private bool isRequireInit = false;

    private void StageClear()
    {
        isRequireInit = false;

        PlayerDataXref.instance.ClearGame(GAME_INDEX.Tree_Little_Pigs, currentStage);
        WSceneManager.instance.OpenGameClearUI();

        if (currentStage == PlayerDataXref.instance.GetTargetState_ToOpenNextChapter(GAME_INDEX.Tree_Little_Pigs))
        {
            PlayerDataXref.instance.OpenChapter(GAME_INDEX.Tree_Little_Pigs + 1);
        }
        else
        {

        }
        if(currentStage == PlayerDataXref.instance.GetMaxStageNumber(GAME_INDEX.Tree_Little_Pigs))
        {
            PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.TREE_LITTLE_PIGS_ALL_CLEAR);
            if(failCount == 0)
            {
                PlayerDataXref.instance.SetAchieveSuccess(ACHEIVE_INDEX.HUMAN_BRICK);
            }
        }
        //Moru
        //stageText.text = currentStage + " STAGE";
    }

    private List<RequireBlock> requireItems = new List<RequireBlock>();

    private void RequireItemInit()
    {
        for (int i = 0; i < stageDifficultyBlastDesign[currentStage].listNumber; i++)
        {
            int blockID = Random.Range(0, blockList.Count);
            int blockNum = Random.Range(stageDifficultyBlastDesign[currentStage].itemMinNumber,
                                        stageDifficultyBlastDesign[currentStage].itemMaxNumber);

            var blockInfo = new RequireBlock();
            blockInfo.blockID = blockID;
            blockInfo.blockNum = blockNum;

            requireItems.Add(blockInfo);
            Debug.Log(blockInfo.blockID + ", " + blockInfo.blockNum);
        }
    }

    public Transform requireListParent;
    public GameObject requireItemBaseObj;

    public String[] itemNames;

    private List<GameObject> requireListObjs = new List<GameObject>();

    // 필요한 아이템 리스트 UI 업데이트
    private void RequireItemListUIUpdate()
    {
        if (requireListObjs.Count > 0)
        {
            foreach (var requireListObj in requireListObjs)
            {
                Destroy(requireListObj);
            }
        }

        requireListObjs.Clear();

        for (int i = 0; i < requireItems.Count; i++)
        {
            requireListObjs.Add(Instantiate(requireItemBaseObj, requireListParent));

            requireListObjs[i].transform.GetChild(0).GetComponent<Image>().sprite = blockList[requireItems[i].blockID];
            requireListObjs[i].transform.GetChild(1).GetComponent<Text>().text = itemNames[requireItems[i].blockID] + " x " + requireItems[i].blockNum;
        }
    }

    public List<StageDifficultyBlast> stageDifficultyBlastDesign;

    [SerializeField] private AudioClip BGM;
    [SerializeField] private AudioClip PuzzleOn;
    [SerializeField] private AudioClip Shake;
    [SerializeField] private AudioClip Click;

    private void Start()
    {
        // 난이도 조절
        stageDifficultyBlastDesign = new List<StageDifficultyBlast>();

        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(10, 15, 1, 60));
        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(20, 30, 1, 55));
        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(20, 25, 2, 50));
        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(25, 30, 2, 45));
        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(15, 20, 3, 40));

        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(20, 25, 4, 40));
        stageDifficultyBlastDesign.Add(new StageDifficultyBlast(25, 30, 4, 40));

        currentStage = PlayerDataXref.instance.GetCurrentStage().StageNum;

        stageText.text = currentStage + " STAGE";

        SoundManager.PlayBGM(BGM);

        InitGame(currentStage);
    }

    public void RequireCheck(int blockID, int number)
    {
        foreach (var block in requireItems)
        {
            if (block.blockID == blockID)
            {
                if (block.blockNum - number <= 0)
                {
                    requireItems.Remove(block);
                    break;
                }
                else
                {
                    block.blockNum -= number;
                }
            }
        }

        if (requireItems.Count <= 0)
        {
            StageClear();
        }

        RequireItemListUIUpdate();
    }
}

public class StageDifficultyBlast
{
    // 필요한 재료의 최소 값 ~ 최대 값
    public int itemMinNumber;
    public int itemMaxNumber;
    // 필요한 리스트 수
    public int listNumber;
    // 시간 
    public int timeOut;

    public StageDifficultyBlast(int itemMinNumber, int itemMaxNumber, int listNumber, int timeOut)
    {
        this.itemMinNumber = itemMinNumber;
        this.itemMaxNumber = itemMaxNumber;
        this.listNumber = listNumber;
        this.timeOut = timeOut;
    }
}

public class RequireBlock
{
    public int blockID;
    public int blockNum;
}