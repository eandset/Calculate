using Calculator.Tokens;
using Calculator.Tokens.Operators;

static class Parser
{
    struct ParsedOperator(Operator main, Operator? sign = null)
    {
        public Operator Main { get; set; } = main;
        public Operator? Sign { get; set; } = sign;

        public override string ToString()
        {
            return Main.ToString() + " " + (Sign?.ToString() ?? "NULL");
        }
    }

    public static void GetAT(IEnumerable<IToken> tokens)
    {
        var groups = new List<List<Operator>>();

        foreach (IToken token in tokens)
        {
            if (token is Operator op)
            {
                if (groups.Count == 0)
                {
                    groups.Add([]);
                }

                groups[^1].Add(op);
            }
            else if (groups.Count == 0 || groups[^1].Count > 0)
            {
                groups.Add([]);
            }
        }

        groups.RemoveAt(groups.Count - 1);

        var formated = new ParsedOperator[groups.Count];

        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];

            if (group.Count == 1)
            {
                if (i == 0 && !(group[0] is Plus or Minus))
                {
                    throw new InvalidOperationException("Syntax Error: first element can be <Minus> or <Plus> or <Any Function> or <Any Number> only");
                }

                formated[i] = new(group[0]);
                continue;
            }

            int nonPlusMinusCount = group.Count(x => !(x is Plus or Minus));
            if (nonPlusMinusCount > 1)
            {
                throw new Exception("Syntax Error: only <Plus> and <Minus> can be more 1 in one group");
            }

            int minusCount = group.Count(x => x is Minus);
            var mainOperator = group.FirstOrDefault(x => !(x is Plus or Minus));

            if (mainOperator == null)
            {
                formated[i] = new(GetOperatorByMinusCount(minusCount));
            }
            else
            {
                formated[i] = new(mainOperator);
                
                if (minusCount != 0)
                    formated[i].Sign = GetOperatorByMinusCount(minusCount);
            }
        }

        Console.WriteLine("Formated");
        
        for (int i = 0; i < formated.Length; i++)
        {
            Console.WriteLine("\t" + formated[i]);
        }
    }

    private static Operator GetOperatorByMinusCount(int minusCount)
    {
        return (minusCount % 2 == 0) ? Operator.GetOperator<Plus>() : new Minus();
    }
}
