namespace VegetableShop.Mvc.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public string? Token { get; set; }

        public string? RefreshToken { get; set; }

        public Response(string msg)
        {
            Message = msg;
        }
        public Response(bool result = true)
        {
            IsSuccess = result;
        }
        public Response()
        {

        }
    }
}
