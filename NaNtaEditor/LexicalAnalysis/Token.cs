using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.LexicalAnalysis;

[DebuggerDisplay("Token = {TokenType}, Source = {Source}")]
internal class Token
{
    public Token(TokenTypes tokenType, string source, int consumedCharacters)
    {
        ConsumedCharacters = consumedCharacters;
        TokenType = tokenType;
        Source = source;
    }

    public int ConsumedCharacters { get; }

    public TokenTypes TokenType { get; }

    public string Source { get; }
}
