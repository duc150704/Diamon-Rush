using System;
using System.Collections.Generic;

public static class WinChecker
{
    public static event Action OnWin;
    public static event Action OnLose;

    private static List<IWinCondition> _winConditions = new List<IWinCondition>();

    public static void AddWinCondition(LevelSO condition)
    {
        if(condition.TargetScore > 0)
        {
            _winConditions.Add(new ScoreCondition(condition.TargetScore));
        }
    }

    public static void ClearCondition()
        => _winConditions.Clear();

    public static void Check(WinCheckData data)
    {
        int completedConditions = 0;
        foreach (var item in _winConditions)
        {

            item.Check(data);

            if (item.IsFailed)
            {
                OnLose?.Invoke();
                return;
            }
                
            if(item.IsCompleted)
                completedConditions++;
        }

        if (completedConditions == _winConditions.Count)
            OnWin?.Invoke();
    }
}
