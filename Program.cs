class Program
{
    private static void Main(string[] args)
    {
        // string input = Console.ReadLine() ?? "";
        string input = "+-+2+2*-3*-+-1/2";

        IElement[] result = Lexer.Scan(input);

        Console.WriteLine("| " + string.Join(" | ", result) + " |");

        Parser.GetAST(result);
    }
}