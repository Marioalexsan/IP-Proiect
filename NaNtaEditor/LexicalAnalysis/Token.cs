using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.LexicalAnalysis;

[DebuggerDisplay("Token = {TokenType}, Value = {Value}, SourceLength = {SourceLength}")]
internal class Token
{
    public Token(TokenTypes tokenType, object value, int consumedCharacters)
    {
        SourceLength = consumedCharacters;
        Value = value;
        TokenType = tokenType;
    }

    public TokenTypes TokenType { get; }

    public object Value { get; }

    public int SourceLength { get; }
}
