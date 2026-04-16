using System.Text.RegularExpressions;
using Calculator.Tokens;

partial class Lexer
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex DublicateSpace();

    public static IEnumerable<IToken> ParseTokens(string expression)
    {
        expression = FormatExpression(expression);
        string cleanExpression = GetCleanExpression(expression);

        return GetTokens(cleanExpression);
    }

    private static string FormatExpression(string expression)
    {
        expression = expression.Replace(".", ",");
        expression = expression.ToLower();
        expression = DublicateSpace().Replace(expression, " ");

        return expression;
    }

    private static string GetCleanExpression(string expression)
    {
        string result = "";

        foreach (char c in expression)
        {
            if (Collector.AllTokenNames.Contains(c.ToString()))
            {
                result += " " + c + " ";
            }
            else
            {
                result += c;
            }
        }

        return result;
    }

    private static IEnumerable<IToken> GetTokens(string cleanExpression)
    {
        var tokensStr = GetTokensStr(cleanExpression);

        return BuildTokens(tokensStr);
    }

    private static IEnumerable<string> GetTokensStr(string result)
    {
        return result
            .Split(" ")
            .Where(IsValidString);
    }

    private static bool IsValidString(string str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }

    private static IEnumerable<IToken> BuildTokens(IEnumerable<string> tokensStrArray)
    {
        return tokensStrArray.Select(GetCurrentToken);
    }

    private static IToken GetCurrentToken(string item)
    {
        if (float.TryParse(item, out float number))
        {
            return new Number(number);
        }
        if (Collector.FunctionNames.Contains(item))
        {
            return Collector.Functions.First(x => x.Name == item);
        }
        if (Collector.OperatorNames.Contains(item))
        {
            return Collector.Operators.First(x => x.Symbol.ToString() == item);
        }

        throw GetException(item);
    }

    private static Exception GetException(string item)
    {
        const string HELP_MESSAGE_ERROR = "Syntax Error: {0}\nAre you mean '{1} <oparation>'? It is need SPACE after function.";
        const string UNKNOWN_FUNCTION_MESSAGE_ERROR = "Syntax Error - UNKNOW token: {0}";

        foreach (var name in Collector.FunctionNames)
        {
            if (item.Contains(name))
            {
                return new Exception(string.Format(HELP_MESSAGE_ERROR, item, name));
            }
        }

        return new Exception(string.Format(UNKNOWN_FUNCTION_MESSAGE_ERROR, item));
    }
}