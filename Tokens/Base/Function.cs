using System.Text.RegularExpressions;
namespace Calculator.Tokens;

public abstract class Function : IToken
{
    public abstract string Name { get; }

    public abstract float Calculate(float number);

    private readonly static Regex Digit = new(@"\d");

    public Function()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new Exception($"Function {GetType()} from assembly {GetType().Assembly} have empty Name property!");
        }
        if (Digit.IsMatch(Name))
        {
            throw new Exception($"Function {GetType()} from assembly {GetType().Assembly} have digit in Name property!");
        }
    }

    public override string ToString()
    {
        return $"[{Name}]<";
    }
}
