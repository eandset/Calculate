namespace Calculator.Tokens.Functions;

public class CustomF : Function
{
    public override string Name => "cusf";
    public override float Calculate(float number)
    {
        if (number % 2 == 0)
            return number;
        else
            return number + 1;
    }
}
