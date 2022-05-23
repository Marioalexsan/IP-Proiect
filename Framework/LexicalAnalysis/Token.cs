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
[DebuggerDisplay("ToString()")]
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

    public object? Value { get; }

    public bool IsValid { get; }

    public int Start { get; set; } = 0;

    public int Length { get; set; } = 0;

    public override string ToString()
    {
        return $"Token = {TokenType}, Value = {Value}, Length = {Length}";
    }
}
