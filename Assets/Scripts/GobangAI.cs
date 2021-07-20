using System;
using System.Threading;

public class GobangAI
{
    private GobangManager manager;
    private int depth;

    private CellType selfType;
    private CellType opponentType;

    public GobangAI(int depth, bool isWhite)
    {
        manager = GobangManager.Instance;
        if ((depth & 1) != 1)
        {
            depth++;
        }
        this.depth = depth;
        selfType = isWhite ? CellType.White : CellType.Black;
        opponentType = isWhite ? CellType.Black : CellType.White;
    }
    Thread thread;

    public void Abort()
    {
        thread?.Abort();
    }

    public void Thinking()
    {
        if (GobangManager.Instance.chessCount == 0)
        {
            GobangManager.Instance.SetPiece(manager.CrtBoard.Width / 2, manager.CrtBoard.Height / 2);
            return;
        }

        GobangManager.Instance.AIThinkingTips();

        thread = new Thread(() => MiniMax(depth, manager.CrtBoard, true, float.MinValue, float.MaxValue));
        thread.Start();
    }

    private float MiniMax(int depth, GobangBoard crtBoard, bool selfTurn, float alpha, float beta)
    {
        if (depth == 0)
        {
            crtBoard.Evaluation(selfType == CellType.White, out float score);
            return score;
        }
        else
        {
            if (crtBoard.Evaluation(selfType == CellType.White, out float score))
            {
                UnityEngine.Debug.Log((1 + depth) * score);
                return (2 + depth) * score;
            }
        }


        if (selfTurn)
        {
            float best = float.MinValue;
            int bestIndex = 0;

            for (int i = 0; i < crtBoard.CellCount; i++)
            {
                if (crtBoard[i] == CellType.Empty && crtBoard.HasNear(i))
                {
                    using GobangBoard nextBoard = crtBoard.Clone();
                    nextBoard[i] = selfType;

                    float val = MiniMax(depth - 1, nextBoard, false, alpha, beta);
                    if (val > best)
                    {
                        bestIndex = i;
                    }
                    best = Math.Max(best, val);
                    alpha = Math.Max(alpha, best);

                    if (beta <= alpha)
                        break;
                }
            }

            if (depth == this.depth)
            {
                manager.IndexBuffer = bestIndex;
            }

            return best;
        }
        else
        {
            float best = float.MaxValue;

            for (int i = 0; i < crtBoard.CellCount; i++)
            {
                if (crtBoard[i] == CellType.Empty && crtBoard.HasNear(i))
                {
                    if (crtBoard[i] == CellType.Empty && crtBoard.HasNear(i))
                    {
                        using GobangBoard nextBoard = crtBoard.Clone();
                        nextBoard[i] = opponentType;

                        float val = MiniMax(depth - 1, nextBoard, true, alpha, beta);
                        best = Math.Min(best, val);
                        beta = Math.Min(beta, best);

                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return best;
        }
    }
}
