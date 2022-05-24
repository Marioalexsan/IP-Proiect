/*============================================================
*
* File:     IPresenter.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the IPresenter interface for the Model-View-Presenter
*           architecture.
*
===========================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Framework.MVP;

/// <summary>
/// Defines an interface for presenters in the Model-View-Presenter architecture.
/// </summary>
public interface IPresenter
{
    /// <summary>
    /// Gets or sets the view instance to use.
    /// </summary>
    public IView View { get; set; }

    /// <summary>
    /// Gets or sets the model instance to use.
    /// </summary>
    public IModel Model { get; set; }

    /// <summary>
    /// Raised whenever the project gets updated (or created, loaded, and closed)
    /// </summary>
    public event EventHandler<Project?>? ProjectUpdated;
    
    /// <summary>
    /// Opens an existing project.
    /// </summary>
    public void OpenProject(object? sender, OpenProjectArgs e);

    /// <summary>
    /// Creates a new project.
    /// </summary>
    public void CreateProject(object? sender, CreateProjectArgs e);

    /// <summary>
    /// Closes the current project, if any.
    /// </summary>
    public void CloseProject(object? sender, EventArgs e);

    /// <summary>
    /// Creates a file and adds it to the current project.
    /// </summary>
    public void CreateFile(object? sender, CreateFileArgs e);

    /// <summary>
    /// Deletes a file from the current project.
    /// </summary>
    public void DeleteFile(object? sender, DeleteFileArgs e);

    /// <summary>
    /// Saves the current project and all files opened.
    /// </summary>
    public void Save(object? sender, EventArgs e);

    /// <summary>
    /// Builds the project and produces an executable.
    /// </summary>
    public void BuildProject(object? sender, EventArgs e);

    /// <summary>
    /// Runs the project executable.
    /// </summary>
    public void RunProject(object? sender, EventArgs e);
}
