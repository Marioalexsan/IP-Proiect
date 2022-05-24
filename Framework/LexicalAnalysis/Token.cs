/*============================================================
*
* File:     Token.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the Token data class.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.LexicalAnalysis;

/// <summary>
/// Data class that represents a token.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public class Token
{
    public Token(TokenTypes tokenType, object? value, int length, bool isValid = true)
    {
        TokenType = tokenType;
        Value = value;
        Length = length;
        IsValid = isValid;
    }

    public TokenTypes TokenType { get; }

    /// <summary>
    /// For most tokens, represents the equivalent C# value of the C++ value parsed.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// For tokens that were parsed, tells if the token would be valid in C++.
    /// For instance, character literals with 0 or 2+ characters are invalid.
    /// </summary>
    public bool IsValid { get; }

    /// <summary>
    /// The start position of this token in the source code.
    /// </summary>
    public int Start { get; set; } = 0;

    /// <summary>
    /// The source code character count consumed by this token.
    /// </summary>
    public int Length { get; set; } = 0;

    public override string ToString()
    {
        return $"Token = {TokenType}, Value = {Value}, Length = {Length}";
    }
}
