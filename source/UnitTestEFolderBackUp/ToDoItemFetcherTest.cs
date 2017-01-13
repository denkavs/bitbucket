using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using EFolderDomain.Interfaces;
using EFolderDomain.Plugins;
using System.Text;
using EFolderDomain.Model;
using System.Collections.Generic;

namespace UnitTestEFolderBackUp
{
    public class MockToDoItemParser : ToDoItemStreamParser
    {
        public MockToDoItemParser() { }
        public void InitStream(string input)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            this.Init(stream);
        }
    }

    [TestClass]
    public class ToDoItemFetcherTest
    {
        [TestMethod]
        public void Read_4_ToDoItems_FromTwoUsers()
        {
            MockToDoItemParser parser = new MockToDoItemParser();

            string input = @"[{
""id"": 0,
""username"": ""User_6361"",
""email"": ""User_6361@ibm.io"",
""todos"": [
    {
    ""id"": 25,
    ""subject"": ""subject_User_6361_N_0"",
    ""dueDate"": ""2016-12-27 12:46:13"",
    ""done"": false
    },
    {
    ""id"": 26,
    ""subject"": ""subject_User_6361_N_1"",
    ""dueDate"": ""2016-12-28 12:46:13"",
    ""done"": false
    }
]
},
{
""id"": 1,
""username"": ""User_3721"",
""email"": ""User_3721@yahoo.com"",
""todos"": [
    {
    ""id"": 121,
    ""subject"": ""subject_User_3721_N_0"",
    ""dueDate"": ""2016-12-27 15:13:22"",
    ""done"": false
    },
    {
    ""id"": 122,
    ""subject"": ""subject_User_3721_N_1"",
    ""dueDate"": ""2016-12-28 15:13:22"",
    ""done"": true
    }
]
}]";

            parser.InitStream(input);
            List<ToDoItem> items = new List<ToDoItem>();

            while (parser.Next())
            {
                items.Add(parser.Current());
            }
            Assert.AreEqual(4, items.Count);

            ((IDisposable)parser).Dispose();
        }

        [TestMethod]
        public void Read_2_ToDoItems_FromTwoUsers_LastUserEmpty()
        {
            MockToDoItemParser parser = new MockToDoItemParser();

            string input = @"[{
""id"": 0,
""username"": ""User_6361"",
""email"": ""User_6361@ibm.io"",
""todos"": [
    {
    ""id"": 25,
    ""subject"": ""subject_User_6361_N_0"",
    ""dueDate"": ""2016-12-27 12:46:13"",
    ""done"": false
    },
    {
    ""id"": 26,
    ""subject"": ""subject_User_6361_N_1"",
    ""dueDate"": ""2016-12-28 12:46:13"",
    ""done"": false
    }
]
},
{
""id"": 1,
""username"": ""User_3721"",
""email"": ""User_3721@yahoo.com"",
""todos"": [
]
}]";

            parser.InitStream(input);
            List<ToDoItem> items = new List<ToDoItem>();

            while (parser.Next())
            {
                items.Add(parser.Current());
            }
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual(25, items[0].ToDoItemId);
            Assert.AreEqual(26, items[1].ToDoItemId);
            Assert.AreEqual("subject_User_6361_N_0", items[0].Subject);
            Assert.AreEqual("subject_User_6361_N_1", items[1].Subject);

            ((IDisposable)parser).Dispose();
        }

        [TestMethod]
        public void Read_0_ToDoItems_FromTwoUsers_BothAreEmpty()
        {
            MockToDoItemParser parser = new MockToDoItemParser();

            string input = @"[{
""id"": 0,
""username"": ""User_6361"",
""email"": ""User_6361@ibm.io"",
""todos"": [
]
},
{
""id"": 1,
""username"": ""User_3721"",
""email"": ""User_3721@yahoo.com"",
""todos"": [
]
}]";

            parser.InitStream(input);
            List<ToDoItem> items = new List<ToDoItem>();

            while (parser.Next())
            {
                items.Add(parser.Current());
            }
            Assert.AreEqual(0, items.Count);

            ((IDisposable)parser).Dispose();
        }
    }
}
