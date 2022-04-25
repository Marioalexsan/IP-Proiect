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
            token ??= CheckStringLiteral(stringView);
            token ??= CheckCharacterLiteral(stringView);
            token ??= CheckPunctuator(stringView);
            token ??= CheckIdentifier(stringView);

            if (char.IsWhiteSpace(stringView[0]))
            {
                for (int index = 0; index < stringView.Length && char.IsWhiteSpace(stringView[index]); index++)
                {
                    start++;
                }
                continue;
            }
            

            token ??= new Token(TokenTypes.Unrecognized, stringView[0].ToString(), 1);

            tokenList.Add(token);
            start += token.ConsumedCharacters;
        }

        return tokenList;
    }

    private static Token? CheckBooleanLiteral(ReadOnlySpan<char> stringView)
    {
        if (stringView == "true" || stringView == "false")
            return new Token(TokenTypes.BooleanLiteral, stringView.ToString(), stringView.Length);

        return null;
    }

    private static Token? CheckPointerLiteral(ReadOnlySpan<char> stringView)
    {
        if (stringView == "nullptr")
            return new Token(TokenTypes.PointerLiteral, stringView.ToString(), stringView.Length);

        return null;
    }

    private static Token? CheckStringLiteral(ReadOnlySpan<char> stringView)
    {
        if (stringView[0] != '"')
            return null;

        int index = 1;
        while (index < stringView.Length && (stringView[index] != '"' || stringView[index - 1] == '\\'))
        {
            index++;
        }

        if (stringView[index] != '"')
            return new Token(TokenTypes.Invalid, stringView[1..index].ToString(), index + 1);

        return new Token(TokenTypes.StringLiteral, stringView[1..index].ToString(), index + 1);
    }

    private static Token? CheckCharacterLiteral(ReadOnlySpan<char> stringView)
    {
        if (stringView[0] != '\'')
            return null;

        int index = 1;
        while (index < stringView.Length && (stringView[index] != '\'' || stringView[index - 1] == '\\'))
        {
            index++;
        }

        if (stringView[index] != '\'')
            return new Token(TokenTypes.Invalid, stringView[1..index].ToString(), index + 1);

        return new Token(TokenTypes.CharacterLiteral, stringView[1..index].ToString(), index + 1);
    }

    /*
    private static Token? CheckNumericLiteral(ReadOnlySpan<char> stringView)
    {
        bool invalid = false;
        int end = 0;

        // Hexadecimal literal
        if (stringView.StartsWith("0x") || stringView.StartsWith("0X"))
        {
            for (int index = 2; index < stringView.Length; index++)
            {
                invalid = invalid && CppStuff.IsOctalDigit(stringView[index]);
            }
        }

        // Binary Literal
        else if (stringView.StartsWith("0b") || stringView.StartsWith("0B"))
        {
            for (int index = 2; index < stringView.Length && CppStuff.IsBinaryDigit(stringView[index]); index++)
            {
                invalid = invalid && CppStuff.IsOctalDigit(stringView[index]);
            }
        }

        // Octal literal
        else if (stringView[0] == '0')
        {
            for (int index = 1; index < stringView.Length && CppStuff.IsOctalDigit(stringView[index]); index++)
            {
                invalid = invalid && CppStuff.IsOctalDigit(stringView[index]);
            }
        }

        // Decimal literal
        else
        {

        }

        // Integer suffix

        return null;
    }
    */

    private static Token? CheckIdentifier(ReadOnlySpan<char> stringView)
    {
        if ('0' <= stringView[0] && stringView[0] <= '9')
            return null;  // Cannot start with a digit

        if (!CppStuff.IsIdentifierCharacter(stringView[0]))
            return null;

        for (int index = 1; index < stringView.Length; index++)
        {
            if (!CppStuff.IsIdentifierCharacter(stringView[index]))
            {
                return new Token(TokenTypes.Identifier, stringView[..index].ToString(), index);
            }
        }

        return new Token(TokenTypes.Identifier, stringView.ToString(), stringView.Length);
    }

    private static Token? CheckComment(ReadOnlySpan<char> stringView)
    {
        if (stringView.StartsWith("//"))
        {
            for (int index = 2; index < stringView.Length; index++)
            {
                if (stringView[index] == '\n')
                {
                    int end = stringView[index - 1] == '\r' ? index - 1 : index;

                    return new Token(TokenTypes.Comment, stringView[2..end].ToString(), index + 1);
                }
            }
        }

        if (stringView.StartsWith("/*"))
        {
            for (int index = 2; index < stringView.Length; index++)
            {
                if (stringView[index] == '*' && stringView[index + 1] == '/')
                {
                    return new Token(TokenTypes.Comment, stringView[2..(index + 1)].ToString(), index);
                }
            }
        }

        return null;
    }

    private static Token? CheckPunctuator(ReadOnlySpan<char> stringView)
    {
        foreach (var punctuator in CppStuff.Punctuators)
        {
            if (stringView.StartsWith(punctuator))
            {
                return new Token(TokenTypes.Punctuator, stringView[..1].ToString(), 1);
            }
        }

        return null;
    }

    private static Token? CheckKeyword(ReadOnlySpan<char> stringView)
    {
        foreach (var keyword in CppStuff.Keywords)
        {
            if (stringView.StartsWith(keyword))
            {
                return new Token(TokenTypes.Keyword, stringView[..keyword.Length].ToString(), keyword.Length);
            }
        }

        return null;
    }
}
