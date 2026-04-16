namespace Calculator.Tokens.Operators;

public class Degree : Operator
{
    public override char Symbol => '^';

    public override float Calculate(float l, float r) => (float)Math.Pow(l, r);
}
