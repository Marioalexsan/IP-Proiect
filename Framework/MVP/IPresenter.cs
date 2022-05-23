using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.XMLParsing;

namespace Framework.MVP;
public interface IPresenter
{
    public IView View { get; set; }

    public IModel Model { get; set; }

    public event EventHandler<Project?>? ProjectUpdated;

    public void OpenProject(object? sender, OpenProjectArgs e);

    public void CreateProject(object? sender, CreateProjectArgs e);

    public void OpenFile(object? sender, OpenFileArgs e);

    public void CreateFile(object? sender, CreateFileArgs e);

    public void Save(object? sender, EventArgs e);
}
