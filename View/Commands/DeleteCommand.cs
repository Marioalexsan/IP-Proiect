/*============================================================
*
* File:     FormPresenter.cs
* Authors:  Damian Gabriel-Mihai, Florea Alexandru-Daniel
* Purpose:  Defines the DeleteCommand editor action.
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
/// Encapsulates a delete action.
/// </summary>
public class DeleteCommand : IEditorCommand
{
    /// <summary>
    /// The starting position of the text deleted.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// The text that was deleted.
    /// </summary>
    public string DeletedString { get; set; } = "";

    /// <summary>
    /// The target RTB to modify when undoing or redoing.
    /// </summary>
    public RichTextBox? Target { get; set; } = null;

    public void Redo()
    {
        if (Target == null)
            return;

        Target.Select(Position, DeletedString.Length);
        Target.SelectedText = "";
    }

    public void Undo()
    {
        if (Target == null)
            return;

        Target.Select(Position, 0);
        Target.SelectedText = DeletedString;
    }

    public IEditorCommand? TryMerge(IEditorCommand next)
    {
        if (next is not DeleteCommand command)
            return null;

        if (command.Target != Target)
            return null;

        // Avoid concatenating if more than a character is deleted
        if (command.DeletedString.Length > 1)
            return null;

        // Only concatenate text + text or whitespace + whitespace
        if (char.IsWhiteSpace(command.DeletedString[^1]) ^ char.IsWhiteSpace(DeletedString[0]))
            return null;


        if (command.Position + command.DeletedString.Length != Position)
            return null;

        return new DeleteCommand()
        {
            Position = command.Position,
            DeletedString = command.DeletedString + DeletedString,
            Target = Target
        };
    }
}
