using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Data
{
    public class Memento
    {
        //var1
        public Dictionary<FileInstance, List<string>> history { get; } = new();
        public Dictionary<FileInstance, int> currentIndexInHistory { get; } = new();

        //var2
        public Dictionary<FileInstance, List<ChangeEntity>> history_2 { get; } = new();
        public Dictionary<FileInstance, int> currentIndexInHistory_2 { get; } = new();

        //List<List<string>> history = new List<List<string>>();
        
        public void InitializeHistory(List<FileInstance> values)
        {
            //values = default value found inside each file
            for (int i = 0; i < values.Count; i++)
            {
                List<string> newFileHistory = new List<string>();
                newFileHistory.Add(values[i].Contents);
                history[values[i]] = newFileHistory;

                currentIndexInHistory[values[i]] = 0;
            }
        }
        public void TabContentUpdated(FileInstance fileKey, string newValue)
        {
            Debug.WriteLine("tab content updated");
            foreach ((var file, var list) in history)
            {
                if(file == fileKey)
                {
                    Debug.WriteLine("file founded:", fileKey.Contents);
                    history[file].Add(newValue);
                    
                    //cannot redo if the file is updated
                    currentIndexInHistory[fileKey] = history[fileKey].Count - 1;
                    Debug.WriteLine("curr index shift:"+currentIndexInHistory[fileKey]);
                }
            }

            CheckFilesHistory();
            CheckCurrentIndexes();
        }
        public string UndoTab(FileInstance fileKey)
        {
            if(currentIndexInHistory[fileKey] == 0)
            {
                return history[fileKey][0];
            }
            
            currentIndexInHistory[fileKey]--;

            return history[fileKey][currentIndexInHistory[fileKey]];
        
        }
        public string RedoTab(FileInstance fileKey)
        {

            if (currentIndexInHistory[fileKey] == history[fileKey].Count -1)
            {
                return history[fileKey][history[fileKey].Count - 1];
            }

            currentIndexInHistory[fileKey]++;

            return history[fileKey][currentIndexInHistory[fileKey]];


            return "A";
        }

        //temp functions
        public void CheckFilesHistory()
        {
            Debug.WriteLine("---> Check files history");
            foreach((var file, var list) in history)
            {
                Debug.WriteLine("--->new item:");
                foreach(var item in list)
                {
                    Debug.WriteLine("->item:", item);
                }
            }
        }
        public void CheckCurrentIndexes()
        {
            Debug.WriteLine("----> Check current indexes");
            foreach ((var file, var lastIndex) in currentIndexInHistory)
            {
                Debug.WriteLine("curr indexs:", lastIndex);
            }
        }
    }
}
