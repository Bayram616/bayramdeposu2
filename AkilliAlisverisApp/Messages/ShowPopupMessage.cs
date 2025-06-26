namespace AkilliAlisverisApp.Messages
{
    public class ShowPopupMessage
    {
        public string Message { get; }

        public ShowPopupMessage(string message)
        {
            Message = message;
        }
    }
}