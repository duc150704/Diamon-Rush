using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardLogic
{
    public static HashSet<GridCell> FindMatches(MyGrid grid)
    {
        HashSet<GridCell> matched = new();

        VerticalCheck(grid, matched);
        HorizontalCheck(grid, matched);

        return matched;
    }

    private static void Swap(Diamon a, Diamon b)
    {
        Diamon tmp = a;
        a = b;
        b = tmp;
    }

    public static HashSet<RefillData> Refill(MyGrid _grid)
    {
        HashSet<RefillData> refillData = new();
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = _grid.Height - 1; j >= 0; j--)
            {
                GridCell cell = _grid.GetCell(i, j);

                if (cell.HasActiveDiamon())
                    break;

                RefillData rd = new RefillData()
                {
                    Cell = cell,
                    RespawnPosition = _grid.GridToWorld(new Vector2Int(i, j + _grid.Height))
                };

                cell.Diamond.Type = DiamondTypeExtension.Next(5); // sua sau
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

                if (!cur.HasActiveDiamon()) 
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

    private static void VerticalCheck(MyGrid grid, HashSet<GridCell> matched)
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
                        for (int k = 1; k <= idx; k++)
                        {
                            matched.Add(grid.GetCell(i, j - k));
                        }
                    }
                    idx = 1;
                }
            }
            if (idx >= 3)
            {
                for (int k = 1; k <= idx; k++)
                {
                    matched.Add(grid.GetCell(i, grid.Height - k));
                }
            }
        }
    }

    private static void HorizontalCheck(MyGrid grid, HashSet<GridCell> matched)
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
                        for (int k = 1; k <= idx; k++)
                        {
                            matched.Add(grid.GetCell(j - k, i));
                        }
                    }
                    idx = 1;
                }
            }

            if (idx >= 3)
            {
                for (int k = 1; k <= idx; k++)
                {
                    matched.Add(grid.GetCell(grid.Width - k, i));
                }
            }
        }
    }
}
