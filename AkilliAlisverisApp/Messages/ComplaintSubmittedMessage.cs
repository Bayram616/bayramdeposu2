using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AkilliAlisverisApp.Messages
{
    public class ComplaintSubmittedMessage : ValueChangedMessage<(bool isSuccess, string message)>
    {
        public ComplaintSubmittedMessage(bool isSuccess, string message)
            : base((isSuccess, message)) { }

        public bool IsSuccess => Value.isSuccess;
        public string Message => Value.message;
    }
}
