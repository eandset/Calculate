using System.Reflection;
using Calculator.Tokens;

static class Collector
{
    public static IEnumerable<Function> Functions { get; }
    public static IEnumerable<Operator> Operators { get; }
    public static IEnumerable<IToken> AllTokens { get; }

    public static IEnumerable<string> FunctionNames { get; }
    public static IEnumerable<string> OperatorNames { get; }
    public static IEnumerable<string> AllTokenNames { get; }

    static Collector()
    {
        Functions = CollectTypes<Function>();
        Operators = CollectTypes<Operator>();

        FunctionNames = Functions.Select(x => x.Name);
        OperatorNames = Operators.Select(x => x.Symbol.ToString());

        AllTokens = Enumerable.Concat<IToken>(Functions, Operators);
        AllTokenNames = Enumerable.Concat(FunctionNames, OperatorNames);

        ThrowExceptionIfHaveDublicate();
    }

    public static void Init()
    {
        Console.WriteLine("Inited");
    }

    private static void ThrowExceptionIfHaveDublicate()
    {
        HashSet<string> names = [];

        foreach (var item in AllTokenNames)
        {
            if (!names.Add(item))
            {
                throw new Exception("Dublicate Name or Symbol in tokens: " + item);
            }
        }
    }

    private static IEnumerable<T> CollectTypes<T>() where T : class
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(x =>
        {
            return typeof(T).IsAssignableFrom(x) && !x.IsAbstract && x.GetConstructor(Type.EmptyTypes) != null;
        });

        foreach (var type in types)
        {
            yield return Activator.CreateInstance(type) as T ?? throw new Exception("Error collect");
        }
    }
}