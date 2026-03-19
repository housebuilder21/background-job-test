namespace ExampleServiceLibrary
{
    public class ExampleService
    {
        private readonly bool _testBoolean;
        private readonly string _defaultMessage;

        public bool TestBooleanOption { get => _testBoolean; }
        public string DefaultMessage { get => _defaultMessage; }

        public ExampleService(bool testBoolean, string defaultMessage)
        { 
            _testBoolean = testBoolean;
            _defaultMessage = defaultMessage;
        }

        public void SendMessage(string? message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine(DefaultMessage);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
