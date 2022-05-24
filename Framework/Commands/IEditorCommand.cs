/*============================================================
*
* File:     IEditorCommand.cs
* Authors:  Damian Gabriel-Mihai, Miron Alexandru
* Purpose:  Defines the IEditorCommand interface for editor actions.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Commands;
/// <summary>
/// Represents an editor command that can be done and undone.
/// </summary>
public interface IEditorCommand
{
    /// <summary>
    /// Execute this command.
    /// </summary>
    public void Redo();

    /// <summary>
    /// Undo this command.
    /// </summary>
    public void Undo();

    /// <summary>
    /// Try merging this command with another.
    /// </summary>
    /// <param name="next">The command to merge with.</param>
    /// <returns>The new command if the merge succeeded, null otherwise.</returns>
    public IEditorCommand? TryMerge(IEditorCommand next);
}
