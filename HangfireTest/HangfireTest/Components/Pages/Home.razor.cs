using ExampleServiceLibrary;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Components;

namespace HangfireTest.Components.Pages
{
    partial class Home
    {
        private string _lastJobId;
        private string _message;
        private int _spanNumber;
        private TimeSpans _selectedTimeSpan;
        private string _lastJobStatus;

        private string HangfireConsoleLink => NavigationManager.ToAbsoluteUri("/hangfire").ToString();

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            // Enqueue a job.
            // https://docs.hangfire.io/en/latest/getting-started/index.html
            _lastJobId = BackgroundJob.Enqueue(() => Console.WriteLine("Initailized. Hello, world!"));
            RefreshStatus();

            await base.OnInitializedAsync();
        }

        private void ScheduleDITest()
        {
            // Enqueueing a job with dependency injection.
            // https://docs.hangfire.io/en/latest/background-methods/passing-dependencies.html
            _lastJobId = BackgroundJob.Enqueue<ExampleService>((service) => service.SendMessage());
            RefreshStatus();
        }

        private void ScheduleInputTest()
        {
            // Schedule a job with a service.
            // https://docs.hangfire.io/en/latest/background-methods/calling-methods-with-delay.html
            var scheduledTime = DateTimeOffset.UtcNow + GetTimeSpan();
            _lastJobId = BackgroundJob.Schedule<ExampleService>((service) => 
                service.SendMessage(_message), scheduledTime);

            RefreshStatus();
        }

        private void UpdateLastJob()
        {
            // Berid of the old scheduled job.
            BackgroundJob.Delete(_lastJobId);

            // Replace it with a new one.
            var newTime = DateTimeOffset.UtcNow + GetTimeSpan();
            _lastJobId = BackgroundJob.Schedule<ExampleService>((service) =>
                service.SendMessage(_message), newTime);

            RefreshStatus();
        }

        private void RefreshStatus()
        {
            // Getting a job's status. Taken from @yngndrw on GitHub
            // https://github.com/HangfireIO/Hangfire/issues/397
            IStorageConnection connection = JobStorage.Current.GetConnection();
            JobData jobData = connection.GetJobData(_lastJobId);
            _lastJobStatus = jobData.State;
        }

        private TimeSpan GetTimeSpan()
        {
            return _selectedTimeSpan switch
            {
                TimeSpans.Seconds => TimeSpan.FromSeconds(_spanNumber),
                TimeSpans.Minutes => TimeSpan.FromMinutes(_spanNumber),
                TimeSpans.Hours => TimeSpan.FromHours(_spanNumber),
                TimeSpans.Days => TimeSpan.FromDays(_spanNumber),
                _ => throw new ArgumentException("Unkown timespan to use.")
            };
        }

        public enum TimeSpans
        {
            Seconds,
            Minutes,
            Hours,
            Days
        }
    }
}
