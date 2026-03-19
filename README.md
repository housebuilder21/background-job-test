# background-job-test
A MudBlazor application that showcases using Hangfire with SQLite. See the [Hangfire docs](https://docs.hangfire.io/en/latest/) to find out more.

This specifically tries to use the [Hangfire SQLite](https://github.com/raisedapp/Hangfire.Storage.SQLite) package in order to store Hangfire's scheduled jobs to a local database file. The usual choice is SQL Server, so there's a commented out `UseSqlServerStorage()` method used where you can set a connection string in `secrets.json` or `appsettings.json`.

When you run the program, the home page is the page testing out some of Hangfire's functions. Have the terminal window that Visual Studio opens with the app visible to you since it relies on console output to display messages and logging.

You can also see all jobs the scheduled, succeeded, etc. via Hangfire's dedicated console. It is hosted on the same port as the web app, but only accessible to the local host. For example, if the web app itself is hosted on port **5206**, the URL would be something like: **http://localhost:5206/hangfire**

## Using Hangfire

Creating a background job with Hangfire is quite easy. Just invoke the `BackgroundJob.Enqueue()` method with a callback method that does what you want to do.

```C#
BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"))
```

The `Enqueue()` method returns the ID of the job it just created. It's very handy if you need the reference the job later to do something like delete it or view the status.

```C#
// Create the job.
string lastJobId = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

// Get the status.
IStorageConnection connection = JobStorage.Current.GetConnection();
JobData jobData = connection.GetJobData(lastJobId);
Console.WriteLine(jobData.State);

// Delete the job.
BackgroundJob.Delete(lastJobId);
```

This project also tests out using Dependency Injection with enscheduling jobs. From what it looks like, it's as simple as adding `<YourServiceHere>` to the `Enqueue()` method:
```C#
BackgorundJob.Enqueue<YourServiceHere>(service => ...)
```