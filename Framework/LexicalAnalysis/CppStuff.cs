/*============================================================
*
* File:     CppStuff.cs
* Authors:  Miron Alexandru
* Purpose:  Defines keywords and other lexical elements
*           for C++
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.LexicalAnalysis;

/// <summary>
/// <para> Contains data and methods for C++ syntax analysis. </para>
/// <see href="https://docs.microsoft.com/en-us/cpp/cpp/character-sets?view=msvc-170"/>
/// </summary>
public static class CppStuff
{
    static CppStuff()
    {
        Keywords = new List<string>()
        {
            "alignas",
            "alignof",
            "and",
            "and_eq",
            "asm",
            "atomic_cancel",
            "atomic_commit",
            "atomic_noexcept",
            "auto",
            "bitand",
            "bitor",
            "bool",
            "break",
            "case",
            "catch",
            "char",
            "char8_t",
            "char16_t",
            "char32_t",
            "class",
            "compl",
            "concept",
            "const",
            "consteval",
            "constexpr",
            "constinit",
            "const_cast",
            "continue",
            "co_await",
            "co_return",
            "co_yield",
            "decltype",
            "default",
            "delete",
            "do",
            "double",
            "dynamic_cast",
            "else",
            "enum",
            "explicit",
            "export",
            "extern",
            "false",
            "float",
            "for",
            "friend",
            "goto",
            "if",
            "inline",
            "int",
            "long",
            "mutable",
            "namespace",
            "new",
            "noexcept",
            "not",
            "not_eq",
            "nullptr",
            "operator",
            "or",
            "or_eq",
            "private",
            "protected",
            "public",
            "reflexpr",
            "register",
            "reinterpret_cast",
            "requires",
            "return",
            "short",
            "signed",
            "sizeof",
            "static",
            "static_assert",
            "static_cast",
            "struct",
            "switch",
            "synchronized",
            "template",
            "this",
            "thread_local",
            "throw",
            "true",
            "try",
            "typedef",
            "typeid",
            "typename",
            "union",
            "unsigned",
            "using",
            "virtual",
            "void",
            "volatile",
            "wchar_t",
            "while",
            "xor",
            "xor_eq"
        };

        Punctuators = new List<string>()
        {
            "!",
            "%",
            "^",
            "&",
            "*",
            "(",
            ")",
            "-",
            "+",
            "=",
            "{",
            "}",
            "|",
            "~",
            "[",
            "]",
            "\\",
            ";",
            "'",
            ":",
            "\"",
            "<",
            ">",
            "?",
            ",",
            ".",
            "/",
            "#"
        };
    }

    /// <summary>
    /// <see href="https://en.cppreference.com/w/cpp/keyword"/>
    /// </summary>
    public static IReadOnlyList<string> Keywords { get; }

    /// <summary>
    /// <see href="https://docs.microsoft.com/en-us/cpp/cpp/punctuators-cpp?view=msvc-170"/>
    /// </summary>
    public static IReadOnlyList<string> Punctuators { get; }

    /// <summary>
    /// Digit characters cannot be the first character of an identifier.
    /// <see href="https://docs.microsoft.com/en-us/cpp/cpp/identifiers-cpp?view=msvc-170"/>
    /// </summary>
    public static bool IsIdentifierCharacter(char c)
    {
        if (c == '_')
            return true;

        if ('0' <= c && c <= '9')
            return true;

        if ('a' <= c && c <= 'z')
            return true;

        if ('A' <= c && c <= 'Z')
            return true;

        return false;
    }

    /// <summary>
    /// Checks if the character can be an octal digit (0 - 7)
    /// </summary>
    public static bool IsOctalDigit(char c)
    {
        return '0' <= c && c <= '7';
    }

    /// <summary>
    /// Checks if the character can be a binary digit (0 - 1)
    /// </summary>
    public static bool IsBinaryDigit(char c)
    {
        return c == '0' || c == '1';
    }

    /// <summary>
    /// Checks if the character can be a hexadecimal digit (0 - 9, A - F)
    /// </summary>
    public static bool IsHexaDigit(char c)
    {
        if ('0' <= c && c <= '9')
            return true;

        if ('a' <= c && c <= 'f')
            return true;

        if ('A' <= c && c <= 'F')
            return true;

        return false;
    }

    /// <summary>
    /// Checks if the character can be used to separate digits.
    /// </summary>
    public static bool IsDigitSeparator(char c)
    {
        return c == '\'';
    }
}
