using EFolderDomain.Interfaces;
using System;
using System.Collections.Generic;
using EFolderDomain.Model;
using System.IO;
using System.Collections;
using EFolderDomain.Infrastructure;

namespace EFolderDomain.Plugins
{
    public class ToDoRepository : IToDoRepository
    {
        string repoPath; // This configuration parameter should be passed from outside
        string dateFormat = @"yyyy-MM-dd HH:mm:ss";

        public ToDoRepository()
        {
            this.repoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoRep");
        }

        public ToDoItem Add(ToDoItem item, int backUpId)
        {
            if(item != null)
            {
                CreateRepositoryIfNotExist(backUpId);
                AppendItem(item, backUpId);
            }
            return item;
        }

        public static ToDoItem DeserializeToDoItem(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            ToDoItem item;
            string[] fields = input.Split('#');
            try
            {
                item = new ToDoItem();
                item.UserName = fields[0];
                item.ToDoItemId = Convert.ToInt32(fields[1]);
                item.Subject = fields[2];
                item.DueDate = DateTime.Parse(fields[3]);
                item.Status = fields[4] == "done" ? ToDoItemStatus.Done : ToDoItemStatus.None;
            }
            catch
            {
                // TODO: Log error
                item = null;
            }
            return item;
        }

        private string getFileName(int backUpId)
        {
            return backUpId.ToString() + ".txt";
        }

        private void AppendItem(ToDoItem item, int backUpId)
        {
            string filePath = Path.Combine(this.repoPath, getFileName(backUpId));

            string text = string.Format(@"{0}#{1}#{2}#{3}#{4}", item.UserName, item.ToDoItemId, item.Subject, item.DueDate.ToString(this.dateFormat), item.Status == ToDoItemStatus.Done ? "done" : "none");
            StreamWriter sw = File.AppendText(filePath);
            sw.WriteLine(text);
            sw.Close();
        }

        private bool CreateRepositoryIfNotExist(int backUpId)
        {
            bool res = false;
            res = CreateFolderIfNeeded(this.repoPath);
            return res;
        }

        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return result;
        }

        public IEnumerator<ToDoItem> GetEnumerator(int backUpId)
        {
            string filePath = Path.Combine(this.repoPath, getFileName(backUpId));

            if (File.Exists(filePath))
            {
                return new ToDoItemEnumerator(filePath);
            }
            else
            {
                Logger.Log.Warn(string.Format("Invalidate file path -[{0}] had been gotted for backUpId [{1}]", filePath, backUpId));
            }
            return null;
        }
    }
}
