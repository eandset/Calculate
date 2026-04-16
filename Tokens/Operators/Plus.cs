namespace Calculator.Tokens.Operators;

public class Plus : Operator
{
    public override char Symbol => '+';

    public override float Calculate(float l, float r) => l + r;
}
