using UnityEngine;

public enum Direction
{
    Up, Down, Left, Right, None
}
public static class DirectionCalculator
{
    public static Direction Normolize(Vector3 direction)
    {
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Direction dir = Direction.None;

        if (-30 <= angle && angle <= 30)
        {
            dir = Direction.Right;
        }
        else if (60 <= angle && angle <= 120)
        {
            dir = Direction.Up;
        }
        else if (-120 <= angle && angle <= -60)
        {
            dir = Direction.Down;
        }
        else if (angle >= 150 || angle <= -150)
        {
            dir = Direction.Left;
        }

        return dir;
    }
}
