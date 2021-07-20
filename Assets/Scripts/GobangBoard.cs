using System;
using System.Collections.Generic;
using System.Linq;

public enum CellType
{
    Empty,
    White,
    Black,
    Void,
}

public class GobangBoard : IDisposable
{
    static readonly int[] ConnectFive = new int[] { 1, 1, 1, 1, 1, 99999999 };

    static readonly int[][] ScoreTable5 = new int[][]
    {
        new int[] { 0, 1, 1, 0, 0 ,5 },
        new int[] { 0, 0, 1, 1, 0 ,5 },
        new int[] { 1, 1, 0, 1, 0 ,20 },
        new int[] { 0, 1, 0, 1, 1 ,20 },
        new int[] { 0, 0, 1, 1, 1 ,50 },
        new int[] { 1, 1, 1, 0, 0 ,50 },
        new int[] { 0, 1, 1, 1, 0 ,100 },
        new int[] { 1, 1, 1, 0, 1 ,300 },
        new int[] { 1, 1, 0, 1, 1 ,300 },
        new int[] { 1, 0, 1, 1, 1 ,300 },
        new int[] { 1, 1, 1, 1, 0 ,500 },
        new int[] { 0, 1, 1, 1, 1 ,500 },
    };

    static readonly int[][] ScoreTable6 = new int[][]
    {
        new int[]{ 0, 1, 0, 1, 1, 0 ,500 },
        new int[]{ 0, 1, 1, 0, 1, 0 ,500 },
        new int[]{ 0, 1, 1, 1, 1, 0 ,5000 },
    };



    public int CellCount
    {
        get
        {
            return board.Length;
        }
    }
    public CellType this[int index]
    {
        get
        {
            if (index < 0 || index >= board.Length)
            {
                return CellType.Void;
            }
            return board[index];
        }
        set
        {
            board[index] = value;
        }
    }

    public CellType this[int x, int y]
    {
        get
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return CellType.Void;
            }
            return board[x * Width + y];
        }
        set
        {
            board[x * Width + y] = value;
        }
    }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private CellType[] board;

    public GobangBoard() : this(15, 15) { }

    public GobangBoard(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        board = new CellType[width * height];
    }
    public void InitBoard(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        if (board.Length != width * height)
        {
            board = new CellType[width * height];
        }
    }

    public bool CheckWin(bool isWhite)
    {
        CellType checkType = isWhite ? CellType.White : CellType.Black;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (this[x, y] == checkType)
                {
                    if (this[x + 1, y] == checkType && this[x + 2, y] == checkType && this[x + 3, y] == checkType && this[x + 4, y] == checkType)//右
                    {
                        return true;
                    }
                    if (this[x, y + 1] == checkType && this[x, y + 2] == checkType && this[x, y + 3] == checkType && this[x, y + 4] == checkType)//上
                    {
                        return true;
                    }
                    if (this[x + 1, y + 1] == checkType && this[x + 2, y + 2] == checkType && this[x + 3, y + 3] == checkType && this[x + 4, y + 4] == checkType)//右上
                    {
                        return true;
                    }
                    if (this[x + 1, y - 1] == checkType && this[x + 2, y - 2] == checkType && this[x + 3, y - 3] == checkType && this[x + 4, y - 4] == checkType)//右下
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void Judgment5(CellType crtCellType, int index, ref int[] array1, ref int[] array2, ref bool flag1, ref bool flag2, ref CellType cellType)
    {
        switch (crtCellType)
        {
            case CellType.White:
                if (cellType == CellType.Black)
                {
                    flag1 = false;
                    flag2 = false;
                    cellType = CellType.Void;
                }
                else if (cellType != CellType.Void)
                {
                    if (cellType == CellType.White)
                    {
                        flag1 = true;
                        flag2 = true;
                    }
                    cellType = CellType.White;
                    array1[index] = 1;
                    array2[index] = 1;
                }
                break;
            case CellType.Black:
                if (cellType == CellType.White)
                {
                    flag1 = false;
                    flag2 = false;
                    cellType = CellType.Void;
                }
                else if (cellType != CellType.Void)
                {
                    if (cellType == CellType.Black)
                    {
                        flag1 = true;
                        flag2 = true;
                    }
                    cellType = CellType.Black;
                    array1[index] = 1;
                    array2[index] = 1;
                }
                break;
            case CellType.Empty:
                array1[index] = 0;
                array2[index] = 0;
                break;
            default:
                flag1 = false;
                flag2 = false;
                cellType = CellType.Void;
                break;
        }
    }
    public void Judgment6(CellType crtCellType, ref int[] array, ref bool flag, CellType cellType)
    {
        switch (crtCellType)
        {
            case CellType.White:
                if (cellType == CellType.Black)
                {
                    flag = false;
                }
                else if (cellType != CellType.Void)
                {
                    if (cellType == CellType.White)
                    {
                        flag = true;
                    }
                    array[5] = 1;
                }
                break;
            case CellType.Black:
                if (cellType == CellType.White)
                {
                    flag = false;
                }
                else if (cellType != CellType.Void)
                {
                    if (cellType == CellType.Black)
                    {
                        flag = true;
                    }
                    array[5] = 1;
                }
                break;
            case CellType.Empty:
                array[5] = 0;
                break;
            default:
                flag = false;
                break;
        }
    }

    int[] rightDir = new int[5];
    int[] rightDir2 = new int[6];
    int[] upDir = new int[5];
    int[] upDir2 = new int[6];
    int[] rightUpDir = new int[5];
    int[] rightUpDir2 = new int[6];
    int[] rightDownDir = new int[5];
    int[] rightDownDir2 = new int[6];

    public bool Evaluation(bool isWhite, out float totalPower)
    {
        totalPower = 0;
        bool  haveConnectFive = false;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                bool rightFlag = false;
                bool rightFlag2 = false;
                bool upFlag = false;
                bool upFlag2 = false;
                bool rightUpFlag = false;
                bool rightUpFlag2 = false;
                bool rightDownFlag = false;
                bool rightDownFlag2 = false;

                CellType rightCellType = CellType.Empty;
                CellType upCellType = CellType.Empty;
                CellType rightUpCellType = CellType.Empty;
                CellType rightDownCellType = CellType.Empty;

                for (int i = 0; i < 5; i++)
                {
                    CellType rct = this[x + i, y];
                    CellType uct = this[x, y + i];
                    CellType ruct = this[x + i, y + i];
                    CellType rdct = this[x + i, y - i];

                    Judgment5(rct, i, ref rightDir, ref rightDir2, ref rightFlag, ref rightFlag2, ref rightCellType);
                    Judgment5(uct, i, ref upDir, ref upDir2, ref upFlag, ref upFlag2, ref upCellType);
                    Judgment5(ruct, i, ref rightUpDir, ref rightUpDir2, ref rightUpFlag, ref rightUpFlag2, ref rightUpCellType);
                    Judgment5(rdct, i, ref rightDownDir, ref rightDownDir2, ref rightDownFlag, ref rightDownFlag2, ref rightDownCellType);
                }

                CellType rct6 = this[x + 5, y];
                CellType dct6 = this[x, y + 5];
                CellType ruct6 = this[x + 5, y + 5];
                CellType rdct6 = this[x + 5, y - 5];
                Judgment6(rct6, ref rightDir2, ref rightFlag2, rightCellType);
                Judgment6(dct6, ref upDir2, ref upFlag2, upCellType);
                Judgment6(ruct6, ref rightUpDir2, ref rightUpFlag2, rightUpCellType);
                Judgment6(rdct6, ref rightDownDir2, ref rightDownFlag2, rightDownCellType);

                if (rightFlag)
                {
                    totalPower += (rightCellType == CellType.White ? 1 : -1) * GetScore5(rightDir, ref haveConnectFive);
                }
                if (rightFlag2)
                {
                    totalPower += (rightCellType == CellType.White ? 1 : -1) * GetScore6(rightDir2 );
                }
                if (upFlag)
                {
                    totalPower += (upCellType == CellType.White ? 1 : -1) * GetScore5(upDir, ref haveConnectFive);
                }
                if (upFlag2)
                {
                    totalPower += (upCellType == CellType.White ? 1 : -1) * GetScore6(upDir2);
                }
                if (rightUpFlag)
                {
                    totalPower += (rightUpCellType == CellType.White ? 1 : -1) * GetScore5(rightUpDir, ref haveConnectFive);
                }
                if (rightUpFlag2)
                {
                    totalPower += (rightUpCellType == CellType.White ? 1 : -1) * GetScore6(rightUpDir2);
                }
                if (rightDownFlag)
                {
                    totalPower += (rightDownCellType == CellType.White ? 1 : -1) * GetScore5(rightDownDir, ref haveConnectFive);
                }
                if (rightDownFlag2)
                {
                    totalPower += (rightDownCellType == CellType.White ? 1 : -1) * GetScore6(rightDownDir2);
                }
            }
        }

        if (!isWhite)
        {
            totalPower = -totalPower;
        }
         
        return haveConnectFive;
    }

    private int GetScore5(int[] input, ref bool haveConnectFive)
    {
        if (CompareIntArray(input, ConnectFive, 5))
        {
            haveConnectFive = true;
            return ConnectFive[5];
        }

        foreach (var item in ScoreTable5)
        {
            if (CompareIntArray(input, item, 5))
            {
                return item[5];
            }
        }
        return 0;
    }

    private int GetScore6(int[] input)
    {
        foreach (var item in ScoreTable6)
        {
            if (CompareIntArray(input, item, 6))
            {
                return item[6];
            }
        }
        return 0;
    }

    private bool CompareIntArray(int[] arr1, int[] arr2, int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (arr1[i] != arr2[i])
            {
                return false;
            }
        }
        return true;
    }

    public GobangBoard Clone()
    {
        GobangBoard newGobangBoard = GobangManager.Instance.boardPool.New();
        board.CopyTo(newGobangBoard.board, 0);
        return newGobangBoard;
    }

    public bool HasNear(int index)
    {
        return HasNear(GetX(index), GetY(index));
    }

    public int GetX(int pos)
    {
        return pos / Width;
    }

    public int GetY(int pos)
    {
        return pos % Width;
    }

    public bool HasNear(int x, int y)
    {
        return (this[x + 1, y] == CellType.Black || this[x + 1, y] == CellType.White
             || this[x - 1, y] == CellType.Black || this[x - 1, y] == CellType.White
             || this[x, y + 1] == CellType.Black || this[x, y + 1] == CellType.White
             || this[x, y - 1] == CellType.Black || this[x, y - 1] == CellType.White
             || this[x + 1, y + 1] == CellType.Black || this[x + 1, y + 1] == CellType.White
             || this[x + 1, y - 1] == CellType.Black || this[x + 1, y - 1] == CellType.White
             || this[x - 1, y + 1] == CellType.Black || this[x - 1, y + 1] == CellType.White
             || this[x - 1, y - 1] == CellType.Black || this[x - 1, y - 1] == CellType.White);
    }

    public void Dispose()
    {
        GobangManager.Instance.boardPool.Store(this);
    }
}
