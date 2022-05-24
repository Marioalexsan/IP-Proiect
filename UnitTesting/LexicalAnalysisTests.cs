/*============================================================
*
* File:     LexicalAnalysis.cs
* Authors:  Miron Alexandru
* Purpose:  Defines tests for the LexicalAnalysis module.
*
===========================================================*/

using Framework.LexicalAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTesting;

/// <summary>
/// Tests for the LexicalAnalysis class.
/// </summary>
[TestClass]
public class LexicalAnalysisTests
{
    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_1()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource1).Let(x =>
        {
            Assert.IsTrue(x.All(t => t.IsValid));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_2()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource2).Let(x =>
        {
            Assert.IsTrue(x.All(t => t.IsValid));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_3()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource3).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_4()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource4).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_5()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource5).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_6()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource6).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_7()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource7).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_8()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource8).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_9()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource9).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenizer against a valid sample C++ program.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_SourceCode_10()
    {
        Tokenizer.Tokenize(Resources.lexicalAnalysisSampleSource10).Let(x =>
        {
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Unrecognized));
            Assert.IsFalse(x.Any(t => t.TokenType == TokenTypes.Invalid));
        });
    }

    /// <summary>
    /// Tests the tokenization of boolean literals.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_BooleanLiteral()
    {
        var values = new[]
        {
            "true",
            "false"
        };

        foreach (var value in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.BooleanLiteral);
                    Assert.AreEqual(t.Value, bool.Parse(value));
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of the pointer literal.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_PointerLiteral()
    {
        Tokenizer.Tokenize("nullptr").Let(x =>
        {
            Assert.AreEqual(x.Count, 1);

            x[0].Let(t =>
            {
                Assert.IsTrue(t.IsValid);
                Assert.IsTrue(t.TokenType == TokenTypes.PointerLiteral);
                Assert.AreEqual(t.Value, null);
            });
        });
    }


    /// <summary>
    /// Tests the tokenization of string literals.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_StringLiteral()
    {
        var values = new[]
        {
            "\"Hello world!\"",
            "\"Testerino esterino sterino terino erino rino ino no o\"",
            "\"\""
        };

        foreach (var value in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.StringLiteral);
                    Assert.AreEqual(t.Value, value[1..^1]);
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of character literals.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_CharacterLiteral()
    {
        var values = new[]
        {
            "'a'",
            "'b'",
            "'\\\\'",
            "'\\n'"
        };

        foreach (var value in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.CharacterLiteral);
                    Assert.AreEqual(t.Value, value[1]);
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of float literals.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_FloatLiteral()
    {
        var values = new Dictionary<string, object>
        {
            ["58."] = 58.0,
            ["4e2"] = 400.0,
            ["123.456e-67"] = 1.23456e-65,
            ["123.456e-67f"] = 0f,
            [".1E4f"] = 1000f,
            ["0x10.1p0"] = 16.025,
            ["0x1p5"] = 32.0,
            ["3.14'15'92"] = 3.141592,
            ["1.18e-4932l"] = 1.18e-4932,
            ["3.4028234e38f"] = 340282346638528859811704183484516925440f,
            ["3.4028234e38"] = 340282339999999992395853996843190976512.0,
            ["3.4028234e38l"] = 340282339999999999995912555211526242304.0
        };

        foreach ((var value, var parsed) in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.FloatLiteral);
                    Assert.AreEqual(t.Value, value);  // The token values are strings
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of integer literals.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_IntegerLiteral()
    {
        var values = new Dictionary<string, object>
        {
            ["123"] = 123,
            ["0123"] = 83,
            ["0x123"] = 291,
            ["0b10"] = 2,
            ["12345678901234567890ull"] = 12345678901234567890,
            ["12345678901234567890u"] = 12345678901234567890,
        };

        foreach ((var value, var parsed) in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.IntegerLiteral);
                    Assert.AreEqual(t.Value, value);  // The token values are strings
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of identifiers.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_CheckIdentifier()
    {
        var nonValues = CppStuff.Keywords.Concat(CppStuff.Punctuators);

        foreach (var nonValue in nonValues)
        {
            Tokenizer.Tokenize(nonValue).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsFalse(t.TokenType == TokenTypes.Identifier);
                });
            });
        }

        var values = new[]
        {
            "std",
            "cout",
            "cin",
            "MyClass",
            "testing",
            "True",
            "Double"
        };

        foreach (var value in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.Identifier);
                    Assert.AreEqual(t.Value, value);
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of comments.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_CheckComment()
    {
        var values = new Dictionary<string, object>
        {
            ["// Line comment"] = " Line comment",
            ["/* Block \n Comment */"] = " Block \n Comment ",
            ["// /* Still a line comment */"] = " /* Still a line comment */",
            ["/* // Still a block comment */"] = " // Still a block comment "
        };

        foreach ((var value, var parsed) in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.Comment);
                    Assert.AreEqual(t.Value, parsed);
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of punctuators.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_CheckPunctuator()
    {
        // Normally you don't want to depend on external data
        // But I think duplicating the punctuator database isn't useful
        var values = CppStuff.Punctuators;

        foreach (var value in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.Punctuator);
                    Assert.AreEqual(t.Value, value);
                });
            });
        }
    }

    /// <summary>
    /// Tests the tokenization of keywords.
    /// </summary>
    [TestMethod]
    public void TokenizerTest_CheckKeyword()
    {
        // Normally you don't want to depend on external data
        // But I think duplicating the keyword database isn't useful
        var values = CppStuff.Keywords;

        foreach (var value in values)
        {
            Tokenizer.Tokenize(value).Let(x =>
            {
                Assert.AreEqual(x.Count, 1);

                x[0].Let(t =>
                {
                    // Boolean literals are also keywords
                    if (t.TokenType == TokenTypes.BooleanLiteral)
                        return;

                    // Pointer literals are also keywords
                    if (t.TokenType == TokenTypes.PointerLiteral)
                        return;

                    Assert.IsTrue(t.IsValid);
                    Assert.IsTrue(t.TokenType == TokenTypes.Keyword);
                    Assert.AreEqual(t.Value, value);
                });
            });
        }
    }
}