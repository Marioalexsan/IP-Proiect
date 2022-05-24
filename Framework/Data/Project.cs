/*============================================================
*
* File:     Project.cs
* Authors:  Damian Gabriel-Mihai
* Purpose:  Manages the process of parsing data from the established xml format
*
===========================================================*/
using Framework.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

/// <summary>
/// Defines the operations required to configure the xml related to the project
/// </summary>
namespace View.XMLParsing
{
    public class Project
    {
        /// <summary>
        /// Path to the xml config
        /// </summary>
        public string FilePath { get; set; } = "";
        /// <summary>
        /// References to the file included in the project
        /// </summary>
        public List<FileInstance> Files { get; } = new();
        /// <summary>
        /// Info about the project
        /// </summary>
        public string ProjectTitle { get; set; } = "";
        /// <summary>
        /// Used when the user creates a new file. The new reference is added into the XML and ready to use now or next project start-up.
        /// </summary>
        public void AddFileToProject(string fileName, string folderPath)
        {
            string path = Path.Combine(folderPath, fileName);

            File.Create(path).Close();

            Files.Add(new FileInstance(path));
            SaveXML();
        }
        /// <summary>
        /// Parse project data (xml location,title,included files) into data structures. 
        /// </summary>
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
                    Files.Add(new FileInstance(node.InnerText)
                    {
                        Contents = File.ReadAllText(node.InnerText)
                    });
                }
            }
        }
        /// <summary>
        /// Parse from data structures into xml format.
        /// </summary>
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


            foreach (var file in Files)
            {
                File.WriteAllText(file.FilePath, file.Contents);
            }

            System.IO.File.WriteAllText(FilePath, xmlFormat);
        }
    }
}

