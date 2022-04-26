using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.LexicalAnalysis;

internal enum TokenTypes
{
    WhiteSpace = -3,
    Comment = -2,
    Invalid = -1,
    Unrecognized = 0,
    Keyword,
    Identifier,
    IntegerLiteral,
    FloatLiteral,
    BooleanLiteral,
    PointerLiteral,
    StringLiteral,
    CharacterLiteral,
    Punctuator
}