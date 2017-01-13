using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFolderDomain.Interfaces;
using EFolderDomain.Infrastructure;
using EFolderDomain.Model;
using System.Collections.Generic;

namespace UnitTestEFolderBackUp
{
    public class NonClass
    {
        static NonClass()
        {

        }

    }
    public class MockCsvToDoFormatter : CsvToDoFormatter
    {
        public string Format(IEnumerable<ToDoItem> lst)
        {
            return getStringForStream(lst);
        }
    }

    [TestClass]
    public class ToDoFormatterTest
    {
        [TestMethod]
        public void ProcessPlainToDoItem()
        {
            ToDoItem item = new ToDoItem();
            item.ToDoItemId = 1;
            item.Status = ToDoItemStatus.Done;
            item.Subject = "ItemSubject";
            item.UserName = "ItemName";
            item.DueDate = DateTime.Parse("12/25/2016 16:44");
            MockCsvToDoFormatter formatter = new MockCsvToDoFormatter();

            //ItemName;1;ItemSubject;12/25/16 16:44;Done
            string result = formatter.Format(new List<ToDoItem>() { item });
            string expected = "ItemName;1;ItemSubject;12/25/16 16:44;Done\r\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EmbeddedCommasMustBeQuoted()
        {
            ToDoItem item = new ToDoItem();
            item.ToDoItemId = 1;
            item.Status = ToDoItemStatus.Done;
            item.Subject = "Item, Subject";
            item.UserName = "Item, Name";
            item.DueDate = DateTime.Parse("12/25/2016 16:44");
            MockCsvToDoFormatter formatter = new MockCsvToDoFormatter();

            string result = formatter.Format(new List<ToDoItem>() { item });
            string expected = "\"Item, Name\";1;\"Item, Subject\";12/25/16 16:44;Done\r\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DoubleQuoteMustBeQuoted()
        {
            ToDoItem item = new ToDoItem();
            item.ToDoItemId = 1;
            item.Status = ToDoItemStatus.Done;
            item.Subject = "ItemSubject";
            item.UserName = "Item\"Name";
            item.DueDate = DateTime.Parse("12/25/2016 16:44");
            MockCsvToDoFormatter formatter = new MockCsvToDoFormatter();

            string result = formatter.Format(new List<ToDoItem>() { item });
            string expected = "\"Item\"\"Name\";1;ItemSubject;12/25/16 16:44;Done\r\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EmbeddedCommasOrDoubleQuoteMustBeQuoted()
        {
            ToDoItem item = new ToDoItem();
            item.ToDoItemId = 1;
            item.Status = ToDoItemStatus.Done;
            item.Subject = "Item, Subject";
            item.UserName = "Item, \"Name\"";
            item.DueDate = DateTime.Parse("12/25/2016 16:44");
            MockCsvToDoFormatter formatter = new MockCsvToDoFormatter();

            //ItemName;1;ItemSubject;12/25/16 16:44;Done
            string result = formatter.Format(new List<ToDoItem>() { item });
            string expected = "\"Item, \"\"Name\"\"\";1;\"Item, Subject\";12/25/16 16:44;Done\r\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void EmbeddedCommasOrDoubleQuoteMustBeQuotedTwoItems()
        {
            ToDoItem item1 = new ToDoItem();
            item1.ToDoItemId = 1;
            item1.Status = ToDoItemStatus.Done;
            item1.Subject = "Item, Subject";
            item1.UserName = "Item, \"Name\"";
            item1.DueDate = DateTime.Parse("12/25/2016 16:44");

            ToDoItem item2 = new ToDoItem();
            item2.ToDoItemId = 1;
            item2.Status = ToDoItemStatus.Done;
            item2.Subject = "Item, Subject";
            item2.UserName = "ItemName";
            item2.DueDate = DateTime.Parse("12/25/2016 16:44");

            MockCsvToDoFormatter formatter = new MockCsvToDoFormatter();

            //ItemName;1;ItemSubject;12/25/16 16:44;Done
            string result = formatter.Format(new List<ToDoItem>() { item1, item2 });
            string expected = "\"Item, \"\"Name\"\"\";1;\"Item, Subject\";12/25/16 16:44;Done\r\nItemName;1;\"Item, Subject\";12/25/16 16:44;Done\r\n";

            Assert.AreEqual(expected, result);
        }
    }
}
