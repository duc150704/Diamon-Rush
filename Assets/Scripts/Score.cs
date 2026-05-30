
public static class Score
{
    public static int Calculate(int count)
    {
        int score = 3;
        switch (count) 
        {
            case 4:
                score += 2;
                break;
            case 5:
                score += 3;
                break;
            case 6:
                score += 5;
                break;
        }
        return score;
    }
}
