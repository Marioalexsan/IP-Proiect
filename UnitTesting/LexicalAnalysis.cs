/*============================================================
*
* File:     LexicalAnalysis.cs
* Authors:  Miron Alexandru
* Purpose:  Defines tests for the CppParser module.
*
===========================================================*/

using CppParser.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTesting;

[TestClass]
public class LexicalAnalysis
{
    [TestMethod]
    public void TokenizerTest1()
    {
        string input =  Resources.lexicalAnalysisSampleSource1;

        var output = Tokenizer.Tokenize(input);

        Assert.IsFalse(output.Any(x => x.TokenType == TokenTypes.Unrecognized));
        Assert.IsFalse(output.Any(x => x.TokenType == TokenTypes.Invalid));
    }
}