Programmer test assignment
==========================

Introduction
------------

The aim with this assignment is to provide a foundation for a technical
conversation. It will help us asses your design skills and your capability
to understand the problem at hand. It will also give us a starting point
for a technical conversation about both implementation details and other
considerations in code.

We do not expect this assignment to take you more then approximately four
hours. You do not have to provide us with a production ready solution in
this time. We do however expect the functions specified below to work as
intended when used as specified.

Since you have taken the time to complete this assignment we will of
cause also take the time to look at it and to give you feedback on it.
We will do this regardless of if we offer you a position or not.
This is the least we can give you when you invest this time in completing
the assignment.

Assignment
----------

*Description*

We would like you to write a small backup service for todo items. The service
will provide a REST API with the endpoints described below.

The todo items will be be fetched from the TodoItemServer provided. For
further information on the server, its API and how to start it see the README
which is located in the zip file.

*Functional requirements*

Your service will have three different client methods. All methods will
return HTTP status code 200, unless something goes wrong.

* Backup accounts
  This API will initiate a complete backup of all todo items in the
  TodoItemServer. The backup is asynchronous and the API will return the
  the id for the initiated backup.

  Request: POST /backups
  Request body: n/a
  Response body:
  ```
  {
	  “backupId”: <backupId>
  }
  ```

* List backups
  This API will list all backups that have initiated. Backup status is
  one of the following:
  * In progress
  * OK
  * Failed

  Request: GET /backups
  Request body: n/a
  Response body:
  ```
  [
	  {
		  “backupId”: “<backup id>”,
  		“date”: “<backup date>”,
	  	“status”: “<backup status>”
    },
    {
      …
    }
  ]
  ```

* Export backup
  This API will return the content of a specified backup id the CSV
  format specified below.

  Request: GET /exports/{backup id}
  Request body: n/a
  Response body:
  ```
  Username;TodoItemId;Subject;DueDate;Done
  {username};{todoitemid};{subject};{duedate};{done}
  ```

Non-functional requirements

* The application does not require persistent storage.
* The application can be started from the command line.
* It is possible to build the application from the command line. If it
  requires any additional installations they should be clearly specified
  and justified.

Delivery

The application will be delivered as an archive containing:
* All source code in a directory named source
* A compiled version of the application.
* A README with
    * Clear instruction on how to run the application on either Windows
      or OS X.
    * Instructions on how to build the application.
    * Any other thoughts or considerations you may have regarding the
      application and its implementation.

We are looking forward to receiving your assignment.
