static class Parser
{
    public static void GetAST(IElement[] elements)
    {
        System.Console.WriteLine("\nFormating:");

        List<List<(Operator op, int index)>> selected = [];
        List<(IElement op, int index)> formated = [];

        for (int i = 0; i < elements.Length; i++)
        {
            IElement? item = elements[i];

            if (item is Operator op)
            {
                if (!selected.Any())
                    selected.Add([]);

                selected.Last().Add((op, i));
            }
            else if (item is Number number)
            {
                formated.Add((number, i));
                selected.Add([]);
            }
        }

        selected.RemoveAt(selected.Count - 1);


        foreach (var item in selected)
        {
            if (item.Count == 1)
            {
                formated.Add(item[0]);
                Console.WriteLine($"It`s normal format! [{item[0].op}]");
                continue;
            }

            bool isHadNotSimpleOperator = false;
            bool lastIsSimpleOperator = false;
            int countMinus = 0;

            Operator? resultOperator = null;

            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].op.Character == "-")
                {
                    lastIsSimpleOperator = true;
                    countMinus++;
                }
                else if (item[i].op.Character == "+")
                {
                    lastIsSimpleOperator = true;
                }
                else
                {
                    resultOperator = item[i].op;

                    if (lastIsSimpleOperator || isHadNotSimpleOperator)
                    {
                        throw new Exception("Syntax Error");
                    }

                    isHadNotSimpleOperator = true;
                }
            }

            if (resultOperator == null)
            {
                if (countMinus % 2 == 0)
                    resultOperator = new Operator("+");
                else
                    resultOperator = new Operator("-");
            }
            else if (countMinus % 2 != 0)
            {
                resultOperator.IsNegative = true;
            }

            formated.Add((resultOperator, item[0].index));

            Console.WriteLine("Formated: " + resultOperator);
        }

        var result = formated.OrderBy(a => a.index).Select(x => x.op).ToList();

        if (result.First() is Operator @operator)
        {
            if (@operator.Character == "-")
            {
                result.RemoveAt(0);

                if (result.First() is not Number number)
                    throw new Exception("Syntax Error");

                result[0] = new Number(-number.Value);
            }
            else
            {
                if (@operator.Character == "+")
                    result.RemoveAt(0);
                else
                    throw new Exception("Syntax Error");
            }
        }

        if (result.Last() is Operator)
        {
            throw new Exception("Syntax Error!");
        }

        foreach (var item in result)
        {
            Console.Write(item + " ");
        }

        Console.WriteLine();

        for (int i = 0; i < result.Count; i++)
        {
            
        }
    }
}