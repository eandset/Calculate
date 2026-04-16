var tokens = Lexer.ParseTokens(" -+  9  *+-+--   8,8+2.5*cusf cusf 1,12");
Console.WriteLine(string.Join(" | ", tokens));

Parser.GetAT(tokens);