using VegetableShop.Api.Dto.Category;
using VegetableShop.Api.Dto.Products;
using VegetableShop.Api.Dto.Role;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Dto
{
    public class CreateResponse : Response
    {
        public AppUserDto appUserDto { get; set; }
        public AppRoleDto appRoleDto { get; set; }
        public ProductDto productDto { get; set; }
        public CategoryDto categoryDto { get; set; }
        public CreateResponse(AppUserDto userDto, string msg)
        {
            IsSuccess = true;
            appUserDto = userDto;
            Message = msg;
        }

        public CreateResponse(AppRoleDto roleDto, string msg)
        {
            IsSuccess = true;
            appRoleDto = roleDto;
            Message = msg;
        }

        public CreateResponse(ProductDto productDto, string msg)
        {
            IsSuccess = true;
            this.productDto = productDto;
            Message = msg;
        }

        public CreateResponse(CategoryDto categoryDto, string msg)
        {
            IsSuccess = true;
            this.categoryDto = categoryDto;
            Message = msg;
        }

        public CreateResponse(string msg)
        {
            IsSuccess = false;
            Message = msg;
        }
    }
}
