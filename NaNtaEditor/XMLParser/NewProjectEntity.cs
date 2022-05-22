using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor.XMLParser
{
    internal class NewProjectEntity
    {
        private string projectName = "";
        private string projectPath = "";
        private TextBox? outputPathBox = null;
        private TextBox? outputTitleBox = null;

        public void setPath(string newPath)
        {
            if(outputPathBox != null)
            {
                outputPathBox.Text = newPath;
            }
            this.projectPath = newPath;
        }
        public void setName(string newName)
        {
            this.projectName = newName;
        }
        public string getName()
        {
            if(this.projectName == "" && this.outputTitleBox != null)
            {
                return this.outputTitleBox.Text;
            }
            return this.projectName;
        }
        public string getPath()
        {
            if (this.projectPath == "" && this.outputPathBox != null)
            {
                return this.outputPathBox.Text;
            }
            return this.projectPath;
        }
        public void setLabelPath(TextBox newLabel)
        {
            this.outputPathBox = newLabel;
        }
        public void setLabelTitle(TextBox newLabel)
        {
            this.outputTitleBox = newLabel;
        }
    }
}
