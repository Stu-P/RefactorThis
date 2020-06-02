namespace RefactorThis.Api.Models
{
    public class ErrorResponse
    {
        public ErrorResponse(string errorMsg)
        {
            Title = errorMsg;
        }

        public ErrorResponse()
        { }

        public string Title { get; set; }
    }
}