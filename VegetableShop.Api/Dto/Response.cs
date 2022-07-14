namespace VegetableShop.Api.Dto
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public string? Token { get; set; }

        public string? RefreshToken { get; set; }
        public int UserId { get; set; }

        public Response(string msg)
        {
            Message = msg;
        }
        public Response()
        {
            IsSuccess = true;
        }
    }
}
