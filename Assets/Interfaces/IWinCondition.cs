
public struct WinCheckData
{
    public int Score;
}

public interface IWinCondition
{
    public bool IsCompleted { get; }
    public bool IsFailed { get; }
    public void Check(WinCheckData data);
}

public class ScoreCondition : IWinCondition
{
    private int _targetScore;
    private int _currentScore;

    public bool IsCompleted => _currentScore >= _targetScore;
    public bool IsFailed => false;

    public ScoreCondition(int targetScore)
    {
        _targetScore = targetScore;
    }

    public void Check(WinCheckData data)
    {
        _currentScore = data.Score;
    }
}
