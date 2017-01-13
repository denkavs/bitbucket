Instructions to test application.
1.	Start JAVA service  -"ThirdParty\JavaService\todoitemserver-1.0\bin\todoitemserver.bat". 
        In some cases you can get some error during run service: "The input line is too long. The syntax of the command is incorrect." The problem could be solved by reducing number of subfolders.
2.	Run ConsoleBackUp.exe file. ("app\consolebackup.exe").
2.1 	There are significant configuration settings for ConsoleBackUp.exe.
	They are saved in file: ConsoleBackUp.exe.config
	One of them:
	key="MaxNumberOfBackUpConcarrentOperations" value="5".

3.	Use Postman client to make calls. (or any other comfortable for you)
4.	For Creating ToDo backup do POST/ http://localhost:5454/api/backups call.
5.	For getting list of ToDo backups do GET / http://localhost:5454/api/backups call.
6.	Make export ToDo items do GET/ http://localhost:5454/api/exports/{id} where id is a number of backup. Example: "http://localhost:5454/api/exports/3"
7.	Every time when app creates backup it generates new portion of users and populates JAVA service with them.

8. 	To emulate concurrent backup requests you can use next tool "ThirdParty\TestClientApp\". Run this tool from folder "ThirdParty\TestClientApp\TestClientApp\bin\Debug\testclientapp.exe"
	Emulate tool has next significant parameters that could be changed in TestClientApp.exe.config file:
	key="TimeBetweenRequestsInMilliseconds" value="2000" 
	key="RequestCount" value="10"

How to build the application instructions:
Application was developed with VisualStudio 2015. Use "EFolderBackUp.sln" file to open solution.
Technologies: self hosted ASP.Net Web API 2 service.

Some addition help information:
Application consists from two controllers that process requests. Each controller uses IBackupService instance as entry point to domain layer. 
The IBackupService instance uses IBackupRepository, IToDoItemFetcher, IToDoRepository instances.

IBackupRepository - This interface represents methods to work with repository that keeps backups. Every startup application creates new repository for backup.

IToDoItemFetcher - is used for fetching ToDo items from third part JAVA service in Stream view. Returned Stream should be closed after work with him.

IToDoRepository - is responsible for keeping ToDo items. They are saved in separated file for each backup in "ToDoRep" folder. Every startup application creates new repository for ToDo items.

IToDoItemStreamParser - It is used to iterate thought ToDo items returned by JAVA service.

For decreasing memory usage all operations with ToDo items are performed via Stream.
