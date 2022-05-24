/*============================================================
*
* File:     FormPresenter.cs
* Authors:  Miron Alexandru
* Purpose:  Defines the FormPresenter class, which implements the
*           IPresenter interface.
*
===========================================================*/

using Framework.MVP;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Presenter;
public class FormPresenter : IPresenter
{
    private Compiler.Compiler Compiler = new Compiler.Compiler();

    private IModel? _model;
    public IModel Model
    {
        get => _model ?? throw new Exception("Model has not been set.");
        set => _model = value;
    }

    private IView? _view;
    public IView View
    {
        get => _view ?? throw new Exception("View has not been set.");
        set => _view = value;
    }

    public event EventHandler<Project?>? ProjectUpdated;

    public void OpenProject(object? sender, OpenProjectArgs e)
    {
        Model.Project?.SaveXML();

        var newProject =  new Project()
        {
            FilePath = e.FilePath
        };

        if (!newProject.LoadFromXML(e.FilePath))
        {
            View.ShowMessage("Couldn't open project!");
            return;
        }

        Model.Project = newProject;

        Compiler.WorkingDirectory = Path.GetDirectoryName(Model.Project.FilePath) ?? "";
        ProjectUpdated?.Invoke(this, Model.Project);
    }

    public void CreateProject(object? sender, CreateProjectArgs e)
    {
        Model.Project?.SaveXML();
        Model.Project = new Project()
        {
            ProjectTitle = e.Name,
            FilePath = Path.Combine(e.FolderPath, e.Name + ".xml")
        };
        Model.Project.SaveXML();

        Compiler.WorkingDirectory = Path.GetDirectoryName(Model.Project.FilePath) ?? "";
        ProjectUpdated?.Invoke(this, Model.Project);
    }

    public void OpenFile(object? sender, OpenFileArgs e)
    {

    }

    public void CreateFile(object? sender, CreateFileArgs e)
    {
        if (Model.Project == null)
            return;

        Model.Project.AddFileToProject(e.Name, e.FolderPath);

        ProjectUpdated?.Invoke(this, Model.Project);
    }

    public void Save(object? sender, EventArgs e)
    {
        Model.Project?.SaveXML();
    }

    public void BuildProject(object? sender, EventArgs e)
    {
        if (Model.Project == null)
        {
            View.ShowMessage("You must have a project open to build!");
            return;
        }

        Compiler.BuildFiles(Model.Project.Files.Select(x => x.FilePath).ToList(), "");

        View.ShowMessage("Build Status: \n\n\n" + Compiler.StatusText);
    }

    public void RunProject(object? sender, EventArgs e)
    {
        if (Model.Project == null)
        {
            View.ShowMessage("You must have a project open to run!");
            return;
        }

        Compiler.RunFileInTerminal(new Compiler.CompileOptions()
        {
            AbsoluteFilePaths = Model.Project.Files.Select(x => x.FilePath).ToList(),
            CommandLineArguments = "",
            OutputName = Model.Project.ProjectTitle
        });
    }
}
