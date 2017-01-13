using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFolderDomain.Model
{
    public enum ToDoItemStatus
    {
        None,
        Done
    }

    public class ToDoItem
    {
        public int ToDoItemId { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public DateTime DueDate { get; set; }
        public ToDoItemStatus Status { get; set; }

    }
}
