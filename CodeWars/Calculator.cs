using System.Collections.Generic;
public static class Kata {
    public static double Calculate(string s) =>
        Tree.Parse(s).Traverse();
}

public abstract class Node {
    public abstract double Evaluate();
}

public class CalculusNode : Node {
    public Node Left { get; set; }
    public Node Right { get; set; }
    string Operator { get; set; }
    public Func<double, double, double> Function { get; set; }

    public CalculusNode(Node left, Node right, Func<double, double, double> function, string @operator) {
        Left = left;
        Right = right;
        Function = function;
        Operator = @operator;
    }

    public override double Evaluate() {
        double left = Left.Evaluate();
        double right = Right.Evaluate();
        double result = Function(left, right);
        Console.WriteLine($"{left} {Operator} {right} =  {result}");
        return result;
    }
}

public class ValueNode : Node {
    public double Value { get; set; }

    public ValueNode(double value) =>
        Value = value;

    public override double Evaluate() =>
        Value;
}

public class Tree {
    public Node Root;

    public double Traverse() {
        return Root.Evaluate();
    }

    public static Tree Parse(string s) {
        string withoutSpaces = s.Replace(" ", "");
        Tree tree = new Tree();
        List<Token> tokens = new() { new Token(TokenType.Expression, withoutSpaces) };
        var node = ConvertToNode(tokens);
        tree.Root = node;
        return tree;
    }

    private static Node ConvertToNode(List<Token> tokens) {
        int tokenWithLeastPriorityIndex = GetTokenWithLeastPriority(tokens);
        
        Token leastPriorityToken = tokens[tokenWithLeastPriorityIndex];
        switch (leastPriorityToken.Type) {
            case TokenType.Operator: {
                Node left = ConvertToNode(tokens.Take(tokenWithLeastPriorityIndex).ToList());
                Node right = ConvertToNode(tokens.Skip(tokenWithLeastPriorityIndex+1).ToList());
                Node node = new CalculusNode(
                    left,
                    right,
                    _functions[leastPriorityToken.Value],
                    leastPriorityToken.Value
                );
                return node;
            }
            case TokenType.Value: {
                Node node = new ValueNode(double.Parse(leastPriorityToken.Value));
                return node;
            }
            case TokenType.Expression: {
                Node node = ConvertToNode(ConvertToTokens(leastPriorityToken.Value));
                return node;
            }
            default:
                throw new Exception($"Unknown token type: {leastPriorityToken.Type}");
        }
    }

    private static readonly Dictionary<string, Func<double, double, double>> _functions = new Dictionary<string, Func<double, double, double>>() {
        { "+", (a, b) => a + b },
        { "-", (a, b) => a - b },
        { "*", (a, b) => a * b },
        { "/", (a, b) => a / b },
        { "^", Math.Pow },
    };

    private static int GetTokenWithLeastPriority(List<Token> tokens) {
        int minPriority = GetTokenPriority(tokens[0]);
        int index = 0;
        
        for (var i = 1; i < tokens.Count; i++) {
            int tokenPriority = GetTokenPriority(tokens[i]);
            if (tokenPriority < minPriority) {
                minPriority  = tokenPriority;
                index = i;
            }
        }
        
        return index;
    }

    private static int GetTokenPriority(Token token) {
        return token.Type switch {
            TokenType.Operator => token.Value switch {
                "/" => 4,
                "*" => 3,
                "+" or "-" => 1,
                "^"        => 5,
                _          => throw new ArgumentOutOfRangeException()
            },
            TokenType.Expression => 99,
            TokenType.Value      => 100,
            _                    => throw new ArgumentOutOfRangeException()
        };
    }

    private static List<Token> ConvertToTokens(string s) {
        int currentCharIndex = 0;
        var tokens = new List<Token>();
        while (currentCharIndex < s.Length) {
            if (operators.Contains(s[currentCharIndex])) {
                tokens.Add(new Token(TokenType.Operator, s[currentCharIndex].ToString()));
                currentCharIndex++;
                continue;
            }

            if (s[currentCharIndex] == '(') {
                tokens.Add(new Token(TokenType.Expression, GetFullExpression(s, currentCharIndex, out currentCharIndex)));
                currentCharIndex++;
                continue;
            }

            if (char.IsDigit(s[currentCharIndex])) {
                tokens.Add(new Token(TokenType.Value, GetFullNumber(s, currentCharIndex, out currentCharIndex)));
                currentCharIndex++;
                continue;
            }
        }

        return tokens;
    }

    private static string GetFullExpression(string s, int currentCharIndex, out int lastCharIndex) {
        int openBrackets = 1;
        int closeBrackets = 0;
        for (int i = currentCharIndex + 1; i < s.Length; i++) {
            if (s[i] == '(')
                openBrackets++;

            if (s[i] == ')')
                closeBrackets++;

            if (closeBrackets == openBrackets) {
                lastCharIndex = i;
                return s.Substring(currentCharIndex + 1, i - currentCharIndex - 1);
            }
        }

        throw new ParseException();
    }

    private static string GetFullNumber(string s, int currentCharIndex, out int lastCharIndex) {
        for (int i = currentCharIndex; i <= s.Length; i++) {
            if (i == s.Length) {
                lastCharIndex = i - 1;
                return s.Substring(currentCharIndex, i - currentCharIndex);
            }

            if (!IsPartOfNumber(s[i])) {
                lastCharIndex = i - 1;
                return s.Substring(currentCharIndex, i - currentCharIndex);
            }
        }

        throw new ParseException();

        bool IsPartOfNumber(char c) =>
            char.IsDigit(c) || c == '.';
    }

    private static readonly List<char> operators = new List<char>() {
        '/', '+', '-', '*', '^'
    };
}

public class ParseException : Exception { }

public record Token(TokenType Type, string Value);

public enum TokenType {
    Value = 1,
    Operator = 2,
    Expression = 3,
}