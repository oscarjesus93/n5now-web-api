namespace ApiN5now.CustomException
{
    public class MessageResponse
    {
        public string Message { get; set; }
        public MessageResponse(string message)
        {
            this.Message = message;
        }
    }
}
