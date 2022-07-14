namespace VegetableShop.Api.Common
{
    public static class Exceptions
    {
        public const string BadRequest = "Bad Request";
        public const string UserNotFound = "User Not Found";
        public const string UserNotExist = "User Not Exist";
        public const string EmailExist = "Email exist ";
        public const string EmailNotFound = "Email not found ";
        public const string EmailBlocked = "Email blocked ";
        public const string UsernameExist = "Username exist ";
        public const string LoginFail = "Please check email and password again";
        public const string CreateFail = "Create fail ";
        public const string UpdateFail = "Update fail ";
        public const string DeleteteFail = "Delete fail ";
        public const string InvalidRequest = "Invalid request ";
        public const string InvalidToken = "Invalid token ";
        public const string ValidateTokenFail = "Validate token fail";
        public const string NullId = "Id is null";
        public const string RoleNameExist = "Role name exist";
        public const string RoleNotFound = "Role not found";
        public const string ProductNotFound = "Product not found";
        public const string ProductNameExist = "Product name exist";
        public const string CategoryNotFound = "Category not found";
        public const string CategoryNameNotExist = "Category name not exist";
        public const string CategoryNameExist = "Category name exist";
        public const string OrderNotFound = "Order not found";
    }
}
