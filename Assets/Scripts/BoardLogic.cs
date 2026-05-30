using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardLogic
{
    public static CheckedResult FindMatches(MyGrid grid)
    {
        CheckedResult check = new();

        VerticalCheck(grid, check);
        HorizontalCheck(grid, check);

        return check;
    }

    public static void ClearMatched(HashSet<Diamon> diamonds)
    {
        foreach (var item in diamonds)
        {
            item.Deactivate();
        }
    }

    public static int GetScore(CheckedResult res, int comboChain = 1)
    {
        int score = res.GetAllScore();
        return score;
    }

    public static HashSet<RefillData> Refill(MyGrid _grid, List<EDiamonType> refillDiamonType)
    {
        HashSet<RefillData> refillData = new();
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = _grid.Height - 1; j >= 0; j--)
            {
                GridCell cell = _grid.GetCell(i, j);

                if (cell.HasActivateDiamon())
                    break;

                RefillData rd = new RefillData()
                {
                    Cell = cell,
                    RespawnPosition = _grid.GridToWorld(new Vector2Int(i, j + _grid.Height))
                };

                int ramdon = Utilities.RandomInt(0, refillDiamonType.Count);
                cell.Diamond.Type = refillDiamonType[ramdon];
                cell.Diamond.Activate();
                refillData.Add(rd);
            }
        }
        return refillData;
    }

    public static void ApplyGravity(MyGrid grid)
    {
        for (int i = 0; i < grid.Width; i++)
        {
            int down = 0;
            int up = 0;
            while (up < grid.Height)
            {
                GridCell cur = grid.GetCell(i, up);
                GridCell pre = grid.GetCell(i, down);

                if (!cur.HasActivateDiamon()) 
                {
                    up++;
                    continue;
                }
                if (up != down)
                {
                    grid.SwapDiamon(i, down, i, up);
                }

                down++;
                up++;
            }
        }
    }

    private static void VerticalCheck(MyGrid grid, CheckedResult check)
    {
        for (int i = 0; i < grid.Width; i++)
        {
            int idx = 1;
            for (int j = 1; j < grid.Height; j++)
            {
                GridCell cur = grid.GetCell(i, j);
                GridCell pre = grid.GetCell(i, j - 1);

                //if (cur.Diamond == null || pre.Diamond == null)
                //    continue;

                if (cur.Diamond.Type == pre.Diamond.Type)
                {
                    idx++;
                }
                else
                {
                    if (idx >= 3)
                    {
                        MatchedData matchedData = new MatchedData();
                        for (int k = 1; k <= idx; k++)
                        {
                            matchedData.AddDiamond(grid.GetCell(i, j - k).Diamond);
                            check.AddDiamond(grid.GetCell(i, j - k).Diamond);
                        }
                        check.AddData(matchedData);
                    }
                    idx = 1;
                }
            }
            if (idx >= 3)
            {
                MatchedData matchedData = new MatchedData();
                for (int k = 1; k <= idx; k++)
                {
                    matchedData.AddDiamond(grid.GetCell(i, grid.Height - k).Diamond);
                    check.AddDiamond(grid.GetCell(i, grid.Height - k).Diamond);
                }
                check.AddData(matchedData);
            }
        }
    }

    private static void HorizontalCheck(MyGrid grid, CheckedResult checkedData)
    {
        for (int i = 0; i < grid.Height; i++)
        {
            int idx = 1;
            for (int j = 1; j < grid.Width; j++)
            {
                GridCell cur = grid.GetCell(j, i);
                GridCell pre = grid.GetCell(j - 1, i);

                //if (cur.Diamond == null || pre.Diamond == null)
                //    continue;

                if (cur.Diamond.Type == pre.Diamond.Type)
                {
                    idx++;
                }
                else
                {
                    if (idx >= 3)
                    {
                        MatchedData matchedData = new MatchedData();
                        for (int k = 1; k <= idx; k++)
                        {
                            matchedData.AddDiamond(grid.GetCell(j - k, i).Diamond);
                            checkedData.AddDiamond(grid.GetCell(j - k, i).Diamond);
                        }
                        checkedData.AddData(matchedData);
                    }
                    idx = 1;
                }
            }

            if (idx >= 3)
            {
                MatchedData matchedData = new();
                for (int k = 1; k <= idx; k++)
                {
                    matchedData.AddDiamond(grid.GetCell(grid.Width - k, i).Diamond);
                    checkedData.AddDiamond(grid.GetCell(grid.Width - k, i).Diamond);
                }
                checkedData.AddData(matchedData);
            }
        }
    }
}
