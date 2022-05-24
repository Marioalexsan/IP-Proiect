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
public class WriteCommand : IEditorCommand
{
    public int Position { get; set; }

    public string WrittenString { get; set; } = "";

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
