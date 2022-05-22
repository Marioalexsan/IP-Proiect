using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Editor.TabsHandler
{

    internal class TabsHandeler
    {
        List<TabPage> tabs_ = new List<TabPage>();

        public void insertTabs(TabControl tabControl, List<String> paths)
        {
            //remove old tabs
            List<TabPage> oldTabs= new List<TabPage>();

            foreach (TabPage tab in tabControl.TabPages)
            {
                oldTabs.Add(tab);

            }
            foreach(TabPage tab in oldTabs)
            {
                tabControl.TabPages.Remove(tab);
            }

            //add new tabs
                Debug.WriteLine("insertTabs");
            for(int i = 0; i < paths.Count; i++)
            {
                string title = "TabPage " + (tabControl.TabCount + 1).ToString();

                TabPage myTabPage = new TabPage(title);

                string fileContent = readFile(paths[i]);

                string[] test = paths[i].Split("\\");

                for(int j= 0; j < test.Length; j++)
                {
                    Debug.WriteLine(test[j] + "\n");
                }

                myTabPage.Text = test[test.Length-1];

                RichTextBox richTextBox1 = new RichTextBox();
                richTextBox1.Dock = DockStyle.Fill;

                richTextBox1.Text = readFile(paths[i]);


                myTabPage.Controls.Add(richTextBox1);
                 
                tabs_.Add(myTabPage);
                tabControl.TabPages.Add(myTabPage);
            }
        }

        public string readFile(string path)
        {
            string text = "Cannot Read File";

            try
            {
                 text = System.IO.File.ReadAllText(@path);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            return text;
        }
        public void deleteAllTabs(TabControl tabPages)
        {
            tabPages.TabPages.Clear();
        }
        
    }

}
