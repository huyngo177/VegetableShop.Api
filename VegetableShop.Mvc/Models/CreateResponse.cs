using VegetableShop.Mvc.Models.Categories;
using VegetableShop.Mvc.Models.Products;
using VegetableShop.Mvc.Models.Role;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.Models
{
    public class CreateResponse : Response
    {
        public UserViewModel userVm { get; set; }
        public RoleViewModel roleVm { get; set; }
        public ProductViewModel productVm { get; set; }
        public CategoryViewModel categoryVm { get; set; }
        public object obj { get; set; }

        public CreateResponse(UserViewModel userVm, string msg)
        {
            IsSuccess = true;
            this.userVm = userVm;
            Message = msg;
        }

        public CreateResponse(RoleViewModel roleVm, string msg)
        {
            IsSuccess = true;
            this.roleVm = roleVm;
            Message = msg;
        }

        public CreateResponse(ProductViewModel productVm, string msg)
        {
            IsSuccess = true;
            this.productVm = productVm;
            Message = msg;
        }

        public CreateResponse(CategoryViewModel categoryVm, string msg)
        {
            IsSuccess = true;
            this.categoryVm = categoryVm;
            Message = msg;
        }

        public CreateResponse(string msg)
        {
            IsSuccess = false;
            Message = msg;
        }
        public CreateResponse(object obj)
        {
            IsSuccess = false;
            this.obj = obj;
        }
        public CreateResponse()
        {
            IsSuccess = true;
        }
        public CreateResponse(bool result = false)
        {
            IsSuccess = result;
        }
    }
}
