0.	Start JAVA service  -"\todoitemserver-1.0\bin\todoitemserver.bat"
1.	Run ConsoleBackUp.exe file. (ToDoItems_Backuper\app\consolebackup.exe).
2.	Use Postman client to make calls. (or any other comfortable for you)
3.	For Creating ToDo backup do POST/ http://localhost:5454/api/backups call.
4.	For getting list of ToDo backups do GET / http://localhost:5454/api/backups call.
5.	Make export ToDo items do GET/ http://localhost:5454/api/exports/{id} where id is a number of backup.
6.	Every time when app creates backup it generates new portion of users and populates JAVA service with them.

Application was developed with VisualStudio 2015. Use "EFolderBackUp.sln" file to open solution in VS2015.
Technologies: self hosted ASP.Net Web API 2 service.

Application consists from two controllers that process requests. Each controller uses IBackupService instance as entry point to domain layer. 
The IBackupService instance uses IBackupRepository, IToDoItemFetcher, IToDoRepository instances.

IBackupRepository - is responsible for keeping backups. Every startup application creates new repository for backup.

IToDoRepository - is responsible for keeping ToDoItems. They are saved in separated file for each backup in "ToDoRep" folder. Every startup application creates new repository for todo items.
IToDoItemFetcher - is used for fetching ToDo items from JAVA service.

For decreasing memory usage all operations with ToDo items are performed via Stream.
