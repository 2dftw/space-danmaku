namespace game.general;


public readonly struct Vec2
{
    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public readonly int X;
    public readonly int Y;
    
    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator /(Vec2 a, int b) => new(a.X / b, a.Y / b);

    public static double Distance(Vec2 a, Vec2 b) => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
}