using Framework.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace View.XMLParsing
{
    public class Project
    {
        public string FilePath { get; set; } = "";

        public List<FileInstance> Files { get; } = new();

        public string ProjectTitle { get; set; } = "";

        public void AddFileToProject(string fileName, string folderPath)
        {
            string path = Path.Combine(folderPath, fileName);

            File.Create(path).Close();

            Files.Add(new FileInstance(path));
            SaveXML();
        }

        public void LoadFromXML(string filePath)
        {
            XmlDocument doc = new XmlDocument();

            FilePath = filePath;

            var fileType = filePath[^3..];

            if (fileType != "xml")
                return;

            doc.Load(filePath);

            // TODO: Check file integrity

            if (doc.DocumentElement == null)
                return;

            XmlNode? parent = doc.DocumentElement.SelectSingleNode("/Project");

            if (parent == null)
                return;

            ProjectTitle = doc.DocumentElement.SelectSingleNode($"/Project/Info/ProjectTitle")!.InnerText;

            XmlNode? tempFiles = doc.DocumentElement.SelectSingleNode("/Project/Ref/Files");

            Files.Clear();

            if (tempFiles != null)
            {
                foreach (XmlNode node in tempFiles)
                {
                    Files.Add(new FileInstance(node.InnerText));
                }
            }
        }

        public void SaveXML()
        {
            string filesTags = "";

            foreach (var file in Files)
            {
                filesTags += "\t\t\t<File>" + file.FilePath + "</File>\n";
            }

            string xmlFormat = $@"
            <Project>
	            <Info>
		            <ProjectTitle>{ProjectTitle}</ProjectTitle>
	            </Info>
	            <Ref>
		            <Files>
                        {(string)filesTags}
		            </Files>
	            </Ref>
	            <Settings>
	            </Settings>
            </Project>
            ";

            System.IO.File.WriteAllText(FilePath, xmlFormat);
        }
    }
}

