/*============================================================
*
* File:     EventArgs.cs
* Authors:  Damian Gabriel-Mihai
* Purpose:  Defines the Memento class that can store the state
*           of editor commands.
*
===========================================================*/

using Framework.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data;

/// <summary>
/// Class that stores editor commands.
/// </summary>
public class Memento
{
    private List<IEditorCommand> _items = new List<IEditorCommand>();

    private int _lastApplied = -1;

    public int UndoableCount => _lastApplied + 1;

    public int RedoableCount => _items.Count - UndoableCount;

    public void Record(IEditorCommand command)
    {
        // Remove elements pending for redo
        if (RedoableCount > 0)
        {
            _items.RemoveRange(_lastApplied + 1, RedoableCount);
        }


        // Try to concatenate similar commands
        if (_items.Count > 0)
        {
            IEditorCommand? merged = _items[^1].TryMerge(command);

            // Update top with merged
            if (merged != null)
            {
                _items[^1] = merged;
                return;
            }
        }

        _items.Add(command);
        _lastApplied += 1;
    }

    public IEditorCommand? Undo()
    {
        if (_lastApplied < 0)
            return null;

        return _items[_lastApplied--];
    }

    public IEditorCommand? Redo()
    {
        if (_lastApplied + 1 >= _items.Count)
            return null;

        return _items[++_lastApplied];
    }

    public void Clear()
    {
        _items.Clear();
        _lastApplied = -1;
    }
}
