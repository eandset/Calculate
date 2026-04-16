namespace Calculator.Tokens;

public abstract class Operator : IToken
{
    private static class Box<T> where T : Operator, new()
    {
        public static T Operator { get; } = new();
    }

    public abstract char Symbol { get; }

    public abstract float Calculate(float left, float right);

    public static T GetOperator<T>() where T : Operator, new()
    {
        return Box<T>.Operator;
    }

    protected Operator()
    {
        if (char.IsWhiteSpace(Symbol))
        {
            throw new Exception($"Operator {GetType()} from assembly {GetType().Assembly} have empty Symbol property!");
        }
        if (char.IsDigit(Symbol))
        {
            throw new Exception($"Operator {GetType()} from assembly {GetType().Assembly} have digit in Symbol property!");
        }
    }

    public override string ToString()
    {
        return $"<{Symbol}>";
    }

    public override bool Equals(object? obj)
    {
        return obj != null && obj.GetType() == GetType();
    }

    public override int GetHashCode()
    {
        return Symbol.GetHashCode();
    }
}
