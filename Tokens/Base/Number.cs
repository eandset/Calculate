namespace Calculator.Tokens;

public class Number : IToken
{
    public float Value { get; }

    public Number(float value)
    {
        Value = value;
    }

    public Number(Number number)
    {
        Value = number.Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}