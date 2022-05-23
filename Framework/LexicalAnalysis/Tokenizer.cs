/*============================================================
*
* File:     Tokenizer.cs
* Authors:  Miron Alexandru
* Purpose:  Implements tokenization for C++ source code.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.LexicalAnalysis;

/// <summary>
/// Tokenizer class that can parse raw C++ source code.
/// </summary>
public static class Tokenizer
{
    /// <summary>
    /// Parses the input string and returns a list of tokens.
    /// </summary>
    public static List<Token> Tokenize(ReadOnlySpan<char> input)
    {
        List<Token> tokenList = new List<Token>();
        int start = 0;

        while (start < input.Length)
        {
            ReadOnlySpan<char> stringView = input[start..];

            Token? token = null;

            token ??= CheckComment(stringView);
            token ??= CheckFloatLiteral(stringView);
            token ??= CheckIntegerLiteral(stringView);
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
            start += token.Length;
        }

        return tokenList;
    }

    private static Token? ConsumeWhiteSpace(ReadOnlySpan<char> source)
    {
        if (source.Length == 0)
            return null;

        int index = 0;

        while (index < source.Length && char.IsWhiteSpace(source[index]))
        {
            index++;
        }

        return new Token(TokenTypes.WhiteSpace, null, index);
    }

    private static Token? CheckBooleanLiteral(ReadOnlySpan<char> source)
    {
        if (source != "true" && source != "false")
            return null;

        return new Token(TokenTypes.BooleanLiteral, source[0] == 't', source.Length);
    }

    private static Token? CheckPointerLiteral(ReadOnlySpan<char> source)
    {
        if (source != "nullptr")
            return null;

        return new Token(TokenTypes.PointerLiteral, null, source.Length);
    }

    private static Token? CheckStringLiteral(ReadOnlySpan<char> source)
    {
        if (source.Length == 0 || source[0] != '"')
            return null;

        int index = 1;

        while (index < source.Length && source[index] != '"')
        {
            // Escape next character if applicable
            if (source[index] == '\\' && index + 1 < source.Length)
                index++;

            // Apply current character
            index++;
        }

        return new Token(TokenTypes.StringLiteral, source[1..index].ToString(), index + 1, index < source.Length);
    }

    private static Token? CheckCharacterLiteral(ReadOnlySpan<char> source)
    {
        if (source.Length == 0 || source[0] != '\'')
            return null;

        int index = 1;
        int characterCount = 0;

        while (index < source.Length && source[index] != '\'')
        {
            // Escape next character if applicable
            if (source[index] == '\\')
                index++;

            // Apply current character
            index++;
            characterCount++;
        }

        return new Token(TokenTypes.CharacterLiteral, source[1], index + 1, characterCount == 1 && index < source.Length);
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
            index += 2;

            // Read digit sequence 1 (integer)
            while (index < source.Length && (CppStuff.IsHexaDigit(source[index]) || source[index] == '\''))
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
                while (index < source.Length && (CppStuff.IsHexaDigit(source[index]) || source[index] == '\''))
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
                while (index < source.Length && (char.IsDigit(source[index]) || source[index] == '\''))
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

            // Single quotes between two digits are allowed, everything else is an error
            for (int i = 0; i < index; i++)
            {
                if (source[index] == '\'')
                {
                    if (i != 0 && !CppStuff.IsHexaDigit(source[index - 1]))
                    {
                        erroneous = true;
                        break;
                    }

                    if (i + 1 < index && !CppStuff.IsHexaDigit(source[index + 1]))
                    {
                        erroneous = true;
                        break;
                    }
                }
            }

            return new Token(TokenTypes.FloatLiteral, source[..index].ToString(), index, !erroneous);
        }

        // Decimal style
        else if (char.IsDigit(source[0]) || source[0] == '.')
        {
            // Read digit sequence 1
            while (index < source.Length && (char.IsDigit(source[index]) || source[index] == '\''))
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
                while (index < source.Length && (char.IsDigit(source[index]) || source[index] == '\''))
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

                while (index < source.Length && (char.IsDigit(source[index]) || source[index] == '\''))
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

            // Single quotes between two digits are allowed, everything else is an error
            for (int i = 0; i < index; i++)
            {
                if (source[index] == '\'')
                {
                    if (i != 0 && !char.IsDigit(source[index - 1]))
                    {
                        erroneous = true;
                        break;
                    }

                    if (i + 1 < index && !char.IsDigit(source[index + 1]))
                    {
                        erroneous = true;
                        break;
                    }
                }
            }

            return new Token(TokenTypes.FloatLiteral, source[..index].ToString(), index, !erroneous);
        }

        // Uhhhhh
        else
        {
            return null;
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

            // unsigned int
            if (charsLeft >= 1 && char.ToLowerInvariant(source[index]) == 'u')
            {
                erroneous = erroneous || usedSign;
                usedSign = true;
                index++;
            }

            // long long int
            else if (charsLeft >= 2 && source[index..(index + 2)].ToString().ToLowerInvariant() == "ll" && source[index] == source[index + 1])
            {
                erroneous = erroneous || usedSize;
                usedSize = true;
                index += 2;
            }

            // long int
            // This branch must be after `ll` otherwise it picks up the first `l` from long long
            else if (charsLeft >= 1 && char.ToLowerInvariant(source[index]) == 'l')
            {
                erroneous = erroneous || usedSize;
                usedSize = true;
                index++;
            }

            // std::size_t
            else if (charsLeft >= 1 && char.ToLowerInvariant(source[index]) == 'z')
            {
                erroneous = erroneous || usedSize;
                usedSize = true;
                index++;
            }
        }

        if (!erroneous)
            return new Token(TokenTypes.IntegerLiteral, source[0..index].ToString(), index);

        return new Token(TokenTypes.Invalid, source[0..index].ToString(), index);
    }

    private static Token? CheckIdentifier(ReadOnlySpan<char> source)
    {
        if (source.Length == 0 || char.IsDigit(source[0]) || !CppStuff.IsIdentifierCharacter(source[0]))
            return null;  // Cannot start with a digit

        int index = 1;

        while (index < source.Length && CppStuff.IsIdentifierCharacter(source[index]))
        {
            index++;
        }

        return new Token(TokenTypes.Identifier, source[..index].ToString(), index);
    }

    private static Token? CheckComment(ReadOnlySpan<char> source)
    {
        int end = 2;
        int commentEnd;

        if (source.StartsWith("//"))
        {
            commentEnd = source.IndexOf('\n');
            end = commentEnd + 1;

            if (commentEnd == -1)
            {
                commentEnd = source.Length;
                end = commentEnd;
            }
            else if (source[commentEnd - 1] == '\r')
            {
                commentEnd--;
            }

            return new Token(TokenTypes.Comment, source[2..commentEnd].ToString(), end);
        }

        else if (source.StartsWith("/*"))
        {
            bool isValid = true;
            commentEnd = source.IndexOf("*/");
            
            if (commentEnd == -1)
            {
                commentEnd = source.Length;
                isValid = false;
            }

            end = commentEnd + 2;


            return new Token(TokenTypes.Comment, source[2..commentEnd].ToString(), end, isValid);
        }

        return null;
    }

    private static Token? CheckPunctuator(ReadOnlySpan<char> source)
    {
        // Try for max length

        Token? token = null;
        int maxLength = 0;

        foreach (var punctuator in CppStuff.Punctuators)
        {
            if (source.StartsWith(punctuator) && maxLength < punctuator.Length)
            {
                token = new Token(TokenTypes.Punctuator, source[..punctuator.Length].ToString(), punctuator.Length);
                maxLength = token.Length;
            }
        }

        return token;
    }

    private static Token? CheckKeyword(ReadOnlySpan<char> source)
    {
        // Try for max length

        Token? token = null;
        int maxLength = 0;

        foreach (var keyword in CppStuff.Keywords)
        {
            if (source.StartsWith(keyword) && maxLength < keyword.Length)
            {
                token = new Token(TokenTypes.Keyword, source[..keyword.Length].ToString(), keyword.Length);
                maxLength = token.Length;
            }
        }

        return token;
    }
}
