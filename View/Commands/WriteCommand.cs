/*============================================================
*
* File:     FormPresenter.cs
* Authors:  Damian Gabriel-Mihai, Florea Alexandru-Daniel
* Purpose:  Defines the WriteCommand editor action.
*
===========================================================*/

using Framework.Commands;
using Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Commands;

/// <summary>
/// Encapsulates a write action.
/// </summary>
public class WriteCommand : IEditorCommand
{
    /// <summary>
    /// The starting position of the text written.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// The text that was written.
    /// </summary>
    public string WrittenString { get; set; } = "";

    /// <summary>
    /// The target RTB to modify when undoing or redoing.
    /// </summary>
    public RichTextBox? Target { get; set; } = null;

    public void Redo()
    {
        if (Target == null)
            return;

        Target.Select(Position, 0);
        Target.SelectedText = WrittenString;
    }

    public void Undo()
    {
        if (Target == null)
            return;

        Target.Select(Position, WrittenString.Length);
        Target.SelectedText = "";
    }

    public IEditorCommand? TryMerge(IEditorCommand next)
    {
        if (next is not WriteCommand command)
            return null;

        if (command.Target != Target)
            return null;

        // Avoid concatenating if more than a character is written
        if (command.WrittenString.Length > 1)
            return null;

        // Only concatenate text + text or whitespace + whitespace
        if (char.IsWhiteSpace(command.WrittenString[0]) ^ char.IsWhiteSpace(WrittenString[^1]))
            return null;

        if (Position + WrittenString.Length != command.Position)
            return null;

        return new WriteCommand()
        {
            Position = Position,
            WrittenString = WrittenString + command.WrittenString,
            Target = Target
        };
    }
}
