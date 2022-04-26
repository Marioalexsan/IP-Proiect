using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.LexicalAnalysis;

internal static class Tokenizer
{
    public static List<Token> Tokenize(string input)
    {
        List<Token> tokenList = new List<Token>();
        int start = 0;

        while (start < input.Length)
        {
            ReadOnlySpan<char> stringView = input.AsSpan(start);

            Token? token = null;

            token ??= CheckComment(stringView);
            token ??= CheckIntegerLiteral(stringView);
            token ??= CheckFloatLiteral(stringView);
            token ??= CheckStringLiteral(stringView);
            token ??= CheckCharacterLiteral(stringView);
            token ??= CheckBooleanLiteral(stringView);
            token ??= CheckPointerLiteral(stringView);
            token ??= CheckKeyword(stringView);
            token ??= CheckPunctuator(stringView);
            token ??= CheckIdentifier(stringView);

            // TODO: Refactor Token to contain the source code position
            token ??= ConsumeWhiteSpace(stringView);

            // If this branch gets hit, it means that the lexer sucks
            token ??= new Token(TokenTypes.Unrecognized, stringView[0].ToString(), 1);

            tokenList.Add(token);
            start += token.SourceLength;
        }

        return tokenList;
    }

    private static Token? ConsumeWhiteSpace(ReadOnlySpan<char> source)
    {
        if (!char.IsWhiteSpace(source[0]))
            return null;

        int end = 1;
        while (end < source.Length && char.IsWhiteSpace(source[end]))
        {
            end++;
        }

        return new Token(TokenTypes.WhiteSpace, source[..end].ToString(), end);
    }

    private static Token? CheckBooleanLiteral(ReadOnlySpan<char> source)
    {
        if (source == "true" || source == "false")
            return new Token(TokenTypes.BooleanLiteral, source == "true", source.Length);

        return null;
    }

    private static Token? CheckPointerLiteral(ReadOnlySpan<char> source)
    {
        if (source == "nullptr")
            return new Token(TokenTypes.PointerLiteral, 0, source.Length);

        return null;
    }

    private static Token? CheckStringLiteral(ReadOnlySpan<char> source)
    {
        if (source.Length < 2)
            return null;

        if (source[0] != '"')
            return null;

        int index = 1;

        while (index < source.Length && source[index] != '"')
        {
            if (source[index] == '\\' && index + 1 < source.Length)
            {
                index++;
            }

            index++;
        }

        if (index >= source.Length)
            return new Token(TokenTypes.Invalid, source[1..index].ToString(), source.Length);

        return new Token(TokenTypes.StringLiteral, source[1..index].ToString(), index + 1);
    }

    private static Token? CheckCharacterLiteral(ReadOnlySpan<char> source)
    {
        if (source[0] != '\'')
            return null;

        int index = 1;
        while (index < source.Length && source[index] != '\'')
        {
            if (source[index] == '\\' && index + 1 < source.Length)
            {
                index++;
            }

            index++;
        }

        if (index >= source.Length)
            return new Token(TokenTypes.Invalid, source[1..index].ToString(), source.Length);

        return new Token(TokenTypes.CharacterLiteral, source[1], index + 1);
    }

    private static Token? CheckFloatLiteral(ReadOnlySpan<char> source)
    {
        bool erroneous = false;
        int index = 0;

        bool gotInteger = false;
        bool gotDelimiter = false;
        bool gotFraction = false;
        bool gotExponent = false;
        bool gotSuffix = false;

        if (source[index] == '-' || source[index] == '+')
            index++;

        // Hexadecimal style
        if (source.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
        {
            // Read digit sequence 1 (integer)
            while (index < source.Length && CppStuff.IsHexaDigit(source[index]))
            {
                gotInteger = true;
                index++;
            }

            // Check for delimiter
            if (index < source.Length && source[index] == '.')
            {
                gotDelimiter = true;
                index++;
            }

            // Read digit sequence 2 (fraction)
            if (gotDelimiter)
            {
                while (index < source.Length && CppStuff.IsHexaDigit(source[index]))
                {
                    gotFraction = true;
                    index++;
                }
            }

            // Read decimal exponent
            if (char.ToLowerInvariant(source[index]) == 'p')
            {
                index++;

                if (source[index] == '-' || source[index] == '+')
                    index++;

                // I think the digits in the exponent remain as decimal
                while (index < source.Length && char.IsDigit(source[index]))
                {
                    gotExponent = true;
                    index++;
                }
            }

            // Read suffix
            if ((gotExponent || gotDelimiter) && index < source.Length)
            {
                char suffix = char.ToLowerInvariant(source[index]);
                if (suffix == 'l' || suffix == 'f')
                {
                    gotSuffix = true;
                    index++;
                }
            }

            // If this check fails, it can't be considered as a float literal at all
            if (!(gotDelimiter || gotFraction || gotExponent || gotSuffix))
                return null;

            // Exponent is never optional for hex floats
            erroneous =
                !(gotInteger && gotExponent ||
                gotInteger && gotDelimiter && gotExponent ||
                gotDelimiter && gotExponent);


            if (erroneous)
                return new Token(TokenTypes.Invalid, source[..index].ToString(), index + 1);

            return new Token(TokenTypes.FloatLiteral, source[..index].ToString(), index + 1);
        }

        // Decimal style
        else
        {
            // Read digit sequence 1
            while (index < source.Length && char.IsDigit(source[index]))
            {
                gotInteger = true;
                index++;
            }

            // Check for delimiter
            if (index < source.Length && source[index] == '.')
            {
                gotDelimiter = true;
                index++;
            }

            // Read digit sequence 2
            if (gotDelimiter)
            {
                while (index < source.Length && char.IsDigit(source[index]))
                {
                    gotFraction = true;
                    index++;
                }
            }

            // Read decimal exponent
            if (char.ToLowerInvariant(source[index]) == 'e')
            {
                index++;

                if (source[index] == '-' || source[index] == '+')
                    index++;

                while (index < source.Length && char.IsDigit(source[index]))
                {
                    gotExponent = true;
                    index++;
                }
            }

            // Read suffix
            if ((gotExponent || gotDelimiter) && index < source.Length)
            {
                char suffix = char.ToLowerInvariant(source[index]);
                if (suffix == 'l' || suffix == 'f')
                {
                    gotSuffix = true;
                    index++;
                }
            }

            // If this check fails, it can't be considered as a float literal at all
            if (!(gotDelimiter || gotFraction || gotExponent || gotSuffix))
                return null;

            erroneous =
                !(gotInteger && gotExponent ||
                gotInteger && gotDelimiter ||
                gotDelimiter && gotExponent);

            if (erroneous)
                return new Token(TokenTypes.Invalid, source[..index].ToString(), index + 1);

            return new Token(TokenTypes.FloatLiteral, source[..index].ToString(), index + 1);
        }
    }
    
    private static Token? CheckIntegerLiteral(ReadOnlySpan<char> source)
    {
        bool erroneous = false;
        int start = 0;
        int index = 0;

        if (source[0] == '-' || source[0] == '+')
            index++;

        // Hexadecimal literal
        if (source.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
        {
            start = 2;
            for (index = start; index < source.Length && CppStuff.IsHexaDigit(source[index]); index++)
            {
                // Nothing
            }
        }

        // Binary Literal
        else if (source.StartsWith("0b", StringComparison.InvariantCultureIgnoreCase))
        {
            start = 2;
            for (index = start; index < source.Length && CppStuff.IsBinaryDigit(source[index]); index++)
            {
                // Nothing
            }
        }

        // Octal literal
        else if (source.StartsWith("0"))
        {
            index++;
            for (index = start; index < source.Length && CppStuff.IsOctalDigit(source[index]); index++)
            {
                // Nothing
            }
        }

        // Decimal literal
        else if (char.IsDigit(source[0]))
        {
            start = 0;
            for (index = start; index < source.Length && char.IsDigit(source[index]); index++)
            {
                // Nothing
            }
        }

        // It's not relevant
        else
        {
            return null;
        }

        // Integer suffix
        bool usedSize = false;
        bool usedSign = false;

        for (int tries = 2; tries > 0; tries--)
        {
            int charsLeft = source.Length - index;

            if (charsLeft >= 1 && char.ToLowerInvariant(source[index]) == 'u')
            {
                erroneous = erroneous || usedSign;
                usedSign = true;
                index++;
            }
            else if (charsLeft >= 1 && char.ToLowerInvariant(source[index]) == 'l')
            {
                erroneous = erroneous || usedSize;
                usedSize = true;
                index++;
            }
            else if (charsLeft >= 1 && char.ToLowerInvariant(source[index]) == 'z')
            {
                erroneous = erroneous || usedSize;
                usedSize = true;
                index++;
            }
            else if (charsLeft >= 2 && source[index..(index+2)].ToString().ToLowerInvariant() == "ll" && source[index] == source[index + 1])
            {
                erroneous = erroneous || usedSize;
                usedSize = true;
                index += 2;
            }
        }

        if (!erroneous)
            return new Token(TokenTypes.IntegerLiteral, source[0..index].ToString(), index + 1);

        return new Token(TokenTypes.Invalid, source[0..index].ToString(), index + 1);
    }

    private static Token? CheckIdentifier(ReadOnlySpan<char> source)
    {
        if ('0' <= source[0] && source[0] <= '9')
            return null;  // Cannot start with a digit

        if (!CppStuff.IsIdentifierCharacter(source[0]))
            return null;

        for (int index = 1; index < source.Length; index++)
        {
            if (!CppStuff.IsIdentifierCharacter(source[index]))
            {
                return new Token(TokenTypes.Identifier, source[..index].ToString(), index);
            }
        }

        return new Token(TokenTypes.Identifier, source.ToString(), source.Length);
    }

    private static Token? CheckComment(ReadOnlySpan<char> source)
    {
        if (source.StartsWith("//"))
        {
            for (int index = 2; index < source.Length; index++)
            {
                if (source[index] == '\n')
                {
                    int end = source[index - 1] == '\r' ? index - 1 : index;

                    return new Token(TokenTypes.Comment, source[2..end].ToString(), index + 1);
                }
            }

            return new Token(TokenTypes.Comment, source[2..].ToString(), source.Length);
        }

        if (source.StartsWith("/*"))
        {
            for (int index = 2; index < source.Length; index++)
            {
                if (source[index] == '*' && source[index + 1] == '/')
                {
                    return new Token(TokenTypes.Comment, source[2..(index + 1)].ToString(), index + 2);
                }
            }

            return new Token(TokenTypes.Comment, source[2..].ToString(), source.Length);
        }

        return null;
    }

    private static Token? CheckPunctuator(ReadOnlySpan<char> source)
    {
        foreach (var punctuator in CppStuff.Punctuators)
        {
            if (source.StartsWith(punctuator))
            {
                return new Token(TokenTypes.Punctuator, source[..1].ToString(), 1);
            }
        }

        return null;
    }

    private static Token? CheckKeyword(ReadOnlySpan<char> source)
    {
        foreach (var keyword in CppStuff.Keywords)
        {
            if (source.StartsWith(keyword))
            {
                return new Token(TokenTypes.Keyword, source[..keyword.Length].ToString(), keyword.Length);
            }
        }

        return null;
    }
}
