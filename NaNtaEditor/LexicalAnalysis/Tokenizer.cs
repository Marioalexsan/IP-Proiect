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
            token ??= CheckKeyword(stringView);
            token ??= CheckIntegerLiteral(stringView);
            token ??= CheckStringLiteral(stringView);
            token ??= CheckCharacterLiteral(stringView);
            token ??= CheckPunctuator(stringView);
            token ??= CheckIdentifier(stringView);

            token ??= ConsumeWhiteSpace(stringView);
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
        if (source[0] != '"')
            return null;

        int index = 1;
        while (index < source.Length && (source[index] != '"' || source[index - 1] == '\\'))
        {
            index++;
        }

        if (source[index] != '"')
            return new Token(TokenTypes.Invalid, source[1..index].ToString(), index + 1);

        return new Token(TokenTypes.StringLiteral, source[1..index].ToString(), index + 1);
    }

    private static Token? CheckCharacterLiteral(ReadOnlySpan<char> source)
    {
        if (source[0] != '\'')
            return null;

        int index = 1;
        while (index < source.Length && (source[index] != '\'' || source[index - 1] == '\\'))
        {
            index++;
        }

        if (source[index] != '\'' || index != 2)
            return new Token(TokenTypes.Invalid, source[1..index].ToString(), index + 1);

        return new Token(TokenTypes.CharacterLiteral, source[1], 3);
    }

    /*
    private static Token? CheckFloatLiteral(ReadOnlySpan<char> source)
    {
        bool erroneous = false;
        int start = 0;
        int index = 0;

        bool digitBefore = false;
        bool digitAfter = false;
        bool exponent = false;
        bool suffix = false;

        // Hexadecimal style
        if (source.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
        {

        }

        // Decimal style
        else
        {

        }
    }
    */

    private static Token? CheckIntegerLiteral(ReadOnlySpan<char> source)
    {
        bool erroneous = false;
        int start = 0;
        int index = 0;

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
        else if (source[0] == '0')
        {
            start = 1;
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
            return new Token(TokenTypes.NumericLiteral, source[0..index].ToString(), index + 1);

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
