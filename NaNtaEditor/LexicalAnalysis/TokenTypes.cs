using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.LexicalAnalysis;

internal enum TokenTypes
{
    Invalid = -1,
    Unrecognized = 0,
    Keyword,
    Identifier,
    NumericLiteral,
    BooleanLiteral,
    PointerLiteral,
    StringLiteral,
    CharacterLiteral,
    Punctuator,
    Comment
}