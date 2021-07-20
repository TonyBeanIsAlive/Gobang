using UnityEngine;
using UnityEngine.EventSystems;

public class GobangManager : MonoBehaviour
{
    public static GobangManager Instance { get; private set; }

    public GobangUI gobangUI;

    public GameObject blackChessPrefab;
    public GameObject whiteChessPrefab;
    public GameObject blackChessPhantom;
    public GameObject whiteChessPhantom;
    public GameObject marker;

    public GameObject board;
    public GameObject blackPointsParent;
    public Material boardMaterial;

    public GobangBoard CrtBoard { get; private set; }
    public int chessCount { get; private set; }

    public ObjectPool<GobangBoard> boardPool;

    private bool player1Turn;

    private GobangAI AI1;
    private GobangAI AI2;

    private bool player1IsAI;
    private bool player2IsAI;
    private bool humanCanControl;

    private bool autoAITurn;
    private bool waitControlAI;


    private GameObject chessParent;
    public int IndexBuffer { get; set; } = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartGame(15, 15, true, false, 3, 3);
    }

    private void Update()
    {
        if (IndexBuffer != -1)
        {
            gobangUI.HideTips();
            SetPiece(IndexBuffer);
            IndexBuffer = -1;
        }

        if (!EventSystem.current.IsPointerOverGameObject()&& humanCanControl)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out raycastHit, 100);
            if (raycastHit.transform != null)
            {
                Vector3 pos = new Vector3(GetNearInt(raycastHit.point.x, 2, (CrtBoard.Width & 1) == 0 ? 1 : 0), 0, GetNearInt(raycastHit.point.z, 2, (CrtBoard.Height & 1) == 0 ? 1 : 0));

                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckEmpty(pos))
                    {
                        blackChessPhantom.SetActive(false);
                        whiteChessPhantom.SetActive(false);

                        SetPiece(pos);
                    }
                }
                else
                {
                    if (CheckEmpty(pos))
                    {
                        if (player1Turn)
                        {
                            whiteChessPhantom.SetActive(true);
                            whiteChessPhantom.transform.position = pos;
                        }
                        else
                        {
                            blackChessPhantom.SetActive(true);
                            blackChessPhantom.transform.position = pos;
                        }
                    }
                    else
                    {
                        blackChessPhantom.SetActive(false);
                        whiteChessPhantom.SetActive(false);
                    }
                }
            }
            else
            {
                blackChessPhantom.SetActive(false);
                whiteChessPhantom.SetActive(false);
            }
        }

    }

    public void ManualAI()
    {
        if (waitControlAI)
        {
            waitControlAI = false;
            if (player1Turn)
            {
                AI1.Thinking();
            }
            else
            {
                AI2.Thinking();
            }
        }
        else if (humanCanControl)
        {
            humanCanControl = false;
            if (player1Turn)
            {
                AI1.Thinking();
            }
            else
            {
                AI2.Thinking();
            }
        }
    }

    public void StartGame(int width, int height, bool player1IsAI, bool player2IsAI, int AI1Level, int AI2Level)
    {
        gobangUI.ShowTips("");
        blackChessPhantom.gameObject.SetActive(false);
        whiteChessPhantom.gameObject.SetActive(false);
        if (chessParent != null)
        {
            Destroy(chessParent);
        }
        chessParent = new GameObject();

        board.transform.localScale = new Vector3((width - 1) * 0.2f, 1, (height - 1) * 0.2f);
        boardMaterial.SetTextureScale("_MainTex", new Vector2(width - 1, height - 1));
        blackPointsParent.SetActive(width == 15 && height == 15);

        chessCount = 0;
        waitControlAI = false;
        this.player1IsAI = player1IsAI;
        AI1?.Abort();
        AI1 = new GobangAI(AI1Level, true);
        this.player2IsAI = player2IsAI;
        AI2?.Abort();
        AI2 = new GobangAI(AI2Level, false);

        CrtBoard?.Dispose();
        boardPool = new ObjectPool<GobangBoard>(null, null, (board) => { board.InitBoard(width, height); });
        CrtBoard = boardPool.New();

        marker.SetActive(false);
        player1Turn = true;

        autoAITurn = player1IsAI ^ player2IsAI;

        humanCanControl = !player1IsAI;


        if (player1IsAI )
        {
            if (autoAITurn)
            {
                AI1.Thinking();
            }
            else
            {
                waitControlAI = true;
            }
        }
    }

    public void SetPiece(int index)
    {
        SetPiece(CrtBoard.GetX(index), CrtBoard.GetY(index));
    }

    public void SetPiece(int x, int y)
    {
        GameObject newPiece;
        if (player1Turn)
        {
            newPiece = Instantiate(whiteChessPrefab, chessParent.transform);
        }
        else
        {
            newPiece = Instantiate(blackChessPrefab, chessParent.transform);
        }

        Vector3 targetPos = new Vector3(x * 2 - (CrtBoard.Width - 1), 0, y * 2 - (CrtBoard.Height - 1));
        newPiece.transform.position = targetPos;
        marker.transform.position = targetPos;
        marker.SetActive(true);

        chessCount++;

        CrtBoard[x, y] = player1Turn ? CellType.White : CellType.Black;

        if (CrtBoard.CheckWin(player1Turn))
        {
            //Debug.Log(player1Turn ? "Player1 Win!" : "Player2 Win!");
            gobangUI.ShowTips(player1Turn ? "Player1 Win!" : "Player2 Win!");
            return;
        }
        if (chessCount == CrtBoard.CellCount)
        {
            //Debug.Log("Board is full!");
            gobangUI.ShowTips("Board is full!");
            return;
        }

        player1Turn = !player1Turn;

        if (player1Turn)
        {
            if (player1IsAI)
            {
                humanCanControl = false;
                if (autoAITurn)
                {
                    AI1.Thinking();
                }
                else
                {
                    waitControlAI = true;
                }
            }
            else
            {
                humanCanControl = true;
            }
        }
        else
        {
            if (player2IsAI)
            {
                humanCanControl = false;
                if (autoAITurn)
                {
                    AI2.Thinking();
                }
                else
                {
                    waitControlAI = true;
                }
            }
            else
            {
                humanCanControl = true;
            }
        }
    }

    public void SetPiece(Vector3 pos)
    {
        int x = (Mathf.RoundToInt(pos.x) + CrtBoard.Width - 1) / 2;
        int z = (Mathf.RoundToInt(pos.z) + CrtBoard.Height - 1) / 2;
        SetPiece(x, z);
    }

    private float GetNearInt(float rawFloat, int magnification = 1, int offset = 0)
    {
        return Mathf.Round((rawFloat + offset) / magnification) * magnification - offset;
    }

    public bool CheckEmpty(Vector3 pos)
    {
        int x = (Mathf.RoundToInt(pos.x) + CrtBoard.Width - 1) / 2;
        int z = (Mathf.RoundToInt(pos.z) + CrtBoard.Height - 1) / 2;
        return CrtBoard[x, z] == CellType.Empty;
    }

    public void AIThinkingTips()
    {
        gobangUI.ShowTips("AIË¼¿¼ÖÐ");
    }
}
