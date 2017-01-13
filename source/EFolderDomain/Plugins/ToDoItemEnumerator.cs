using EFolderDomain.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Plugins
{
    internal class ToDoItemEnumerator : IEnumerator<ToDoItem>
    {
        private StreamReader reader = null;
        private string currentStr = string.Empty;
        private string filePath;

        public ToDoItemEnumerator(string filePath)
        {
            this.filePath = filePath;
            this.reader = new StreamReader(this.filePath);
        }

        public ToDoItem Current
        {
            get
            {
                return ToDoRepository.DeserializeToDoItem(this.currentStr);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return ToDoRepository.DeserializeToDoItem(this.currentStr);
            }
        }

        public bool MoveNext()
        {
            if(this.reader != null)
            {
                this.currentStr = this.reader.ReadLine();
            }
            return !string.IsNullOrEmpty(this.currentStr);
        }

        public void Reset()
        {
            if(this.reader != null)
            {
                this.reader.Close();
            }

            this.reader = new StreamReader(this.filePath);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                if(this.reader != null)
                {
                    this.reader.Close();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
