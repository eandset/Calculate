internal class Lexer
{
    static readonly string[] operations = Operator.GetOperators();

    public static IElement[] Scan(string expression)
    {
        expression = expression.Replace(" ", "");

        List<string> strElements = [];

        foreach (char element in expression)
        {
            string strElement = element.ToString();

            if (IsOperator(strElement))
            {
                strElements.Add(strElement);
            }
            else if (IsValidPartOfNumber(strElement))
            {
                if (strElements.Count == 0 || IsOperator(strElements.Last()))
                {
                    strElements.Add(strElement);
                }
                else
                {
                    strElements[^1] = strElements.Last() + element;
                }
            }
            else
            {
                throw new Exception("Invalid character");
            }
        }

        var elements = Format(strElements).ToArray();

        return elements;
    }

    private static IEnumerable<IElement> Format(List<string> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i] = elements[i].Replace(".", ",");

            string? item = elements[i];

            if (!IsValidElement(item, out var number))
                throw new Exception("Invalid element: " + item);

            if (number.HasValue)
                yield return new Number(number.Value);
            else
                yield return new Operator(item);
        }
    }

    private static bool IsValidPartOfNumber(string element)
    {
        return element == "," || element == "." || int.TryParse(element, out _);
    }

    private static bool IsValidElement(string element, out float? number)
    {
        number = null;

        return IsOperator(element) || TryParseNumber(element, out number);
    }

    private static bool TryParseNumber(string element, out float? number)
    {
        bool status = float.TryParse(element, out var result);

        number = status ? result : null;

        return status;
    }

    private static bool IsOperator(string element)
    {
        return operations.Contains(element);
    }
}

interface IElement
{
    string ToString();
}

class Number(float element) : IElement
{
    public override string ToString()
    {
        return Value.ToString();
    }

    public readonly float Value = element;
}

class Operator : IElement
{
    private static readonly Dictionary<string, (Func<float, float, float> action, byte importantLevel)> rules = new()
    {
        {"+", ((x, y) => x + y, 1)},
        {"-", ((x, y) => x - y, 1)},
        {"*", ((x, y) => x * y, 2)},
        {"/", ((x, y) => x / y, 2)},
    };

    public static string[] GetOperators() => rules.Select(x => x.Key).ToArray();

    public readonly byte ImportantLevel;
    public string Character;
    
    private readonly Func<float, float, float> Action;

    public Operator(string element)
    {
        if (rules.TryGetValue(element, out var value))
        {
            ImportantLevel = value.importantLevel;
            Action = value.action;
            Character = element;
        }
        else
        {
            throw new Exception("Unknow operator: " + element);
        }
    }

    public override string ToString()
    {
        return $"<{Character}{(IsNegative ? "-" : "")}>";
    }

    public float Calculate(float left, float right)
    {
        int multiplier = IsNegative ? -1 : 1;

        return Action(left, right * multiplier);
    }

    public bool IsNegative { get; set; } = false;
}