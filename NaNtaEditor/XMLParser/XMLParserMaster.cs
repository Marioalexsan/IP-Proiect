using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Editor.XMLParser
{
    internal class XMLParserMaster
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        XmlDocument doc = new XmlDocument();

        Dictionary<string, string> info = new Dictionary<string, string>()
        {
            {"ProjectTitle", ""},
            {"DateCreated", ""},
        };

        List<string> files = new List<string>();
        //    {"Ceva.cpp", "path"},

        Dictionary<string, string> settings = new Dictionary<string, string>();
        //{"Set_1","value"}
        //{"Set_2","value"}

        public void openXML(string filePath,bool needDialog)
        {
            if (needDialog)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            //open xml file
            
                var fileType = filePath.Substring(filePath.Length - 3);

                Debug.WriteLine(filePath);
                Debug.WriteLine(fileType);

                if (fileType == "xml")
                {
                    doc.Load(filePath);

                    if (checkFileIntegrity(filePath))
                    {
                        Debug.WriteLine("OK integrity");

                        //parse from xml info fiels: info, files, settings

                        if (doc != null && doc.DocumentElement != null)
                        {

                            XmlNode? parent = doc.DocumentElement.SelectSingleNode("/Project");
                            if (parent != null)
                            {
                                //INFO:
                                foreach (KeyValuePair<string, string> pair in info)
                                {
                                    XmlNode? tempInfo = doc.DocumentElement.SelectSingleNode($"/Project/Info/{pair.Key}");
                                    if (tempInfo != null)
                                    {
                                        info[pair.Key] = tempInfo.InnerText;
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Child node for info is null");
                                    }
                                }

                                //FILES:
                                XmlNode? tempFiles = doc.DocumentElement.SelectSingleNode("/Project/Ref/Files");
                                if (tempFiles != null)
                                {
                                    foreach (XmlNode node in tempFiles)
                                    {
                                        Debug.WriteLine("\n node:", node.InnerText);
                                        files.Add(node.InnerText);
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Files tag is null!!!");
                                }

                                //SETIINGS:
                                //TODO: parse settings 
                            }
                            else
                            {
                                Debug.WriteLine("Cannot find parent node in XML!");
                            }
                        }
                        else
                        {
                            Debug.WriteLine("Error while parsing the XML file");
                        }
                    }
                    else
                    {
                        //TODO: out into console or modal
                        Debug.WriteLine("Wrong file integrity!!!");
                    }
                }
                else
                {
                    //TODO: out into console or modal
                    Debug.WriteLine("Wrong file format!!!");
                }
        }
        public bool checkFileIntegrity(string filePath)
        {
            //TODO
            return true;
        }

        public void checkData()
        {
            Debug.WriteLine("\nINFO:\n");
            foreach (KeyValuePair<string, string> pair in info)
            {
                Debug.WriteLine(pair.Key + "-" + pair.Value + "\n");
            }
            Debug.WriteLine("\nFILES:\n");
            Debug.WriteLine(files.Count);

            for (int i = 0; i < files.Count; i++)
            {
                Debug.WriteLine(files[i].ToString() + "\n");
            }
        }

        public List<String> getPaths()
        {
            return this.files;
        }

        public string getProjectName()
        {
            return info["ProjectTitle"];
        }
        public string getProjectDate()
        {
            return info["DateCreated"];
        }
        public string createNewProject(string projectName, string projectPath)
        {
            string newConfig = $@"
            <Project>
	            <Info>
		            <ProjectTitle>{(string)projectName}</ProjectTitle>
		            <DateCreated>{(string)DateTime.UtcNow.ToString("MM-dd-yyyy")}</DateCreated>
	            </Info>
	            <Ref>
		            <Files>
		            </Files>
	            </Ref>
	            <Settings>
	            </Settings>
            </Project>
            ";

            string completeTarget = projectPath + "/" + projectName + ".xml";
            File.WriteAllTextAsync(completeTarget, newConfig);
            return completeTarget;
        }
        public void clearFilePaths()
        {
            files.Clear();
        }
    }
}

