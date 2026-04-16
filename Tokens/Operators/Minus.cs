namespace Calculator.Tokens.Operators;

public class Minus : Operator
{
    public override char Symbol => '-';

    public override float Calculate(float l, float r) => l - r;
}
