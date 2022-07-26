using AutoMapper;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Products;
using Microsoft.EntityFrameworkCore;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Common;
using System.Net.Http.Headers;
using VegetableShop.Api.Services.Storage;
using VegetableShop.Api.Dto.Page;

namespace VegetableShop.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private const string CONTENT_FOLDER_NAME = "Images/Products";
        private readonly string _imagePath;
        public ProductService(AppDbContext appDbContext, IMapper mapper, IStorageService storageService, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _storageService = storageService;
            _configuration = configuration;
            _imagePath = $"{_configuration["BaseAddress"]}";
        }

        public async Task<CreateResponse> CreateAsync(CreateProductDto createProductDto)
        {
            string errors = "";
            if (createProductDto is null)
            {
                throw new AppException(Exceptions.BadRequest);
            }
            var result = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Name == createProductDto.Name);
            if (result is not null)
            {
                errors = string.Concat(errors, Exceptions.ProductNameExist);
            }
            if (await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == createProductDto.CategoryId) is null)
            {
                errors = string.Concat(errors, Exceptions.CategoryNotFound);
            }
            if (errors.Length > 0)
            {
                throw new AppException(errors);
            }
            var product = _mapper.Map<Product>(createProductDto);
            product.DateUpdated = null;
            if (product.Stock == 0)
            {
                product.Status = "Unavailable";
            }
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    product.ImagePath = await SaveFileAsync(createProductDto.Image);
                    await _appDbContext.Products.AddAsync(product);
                    await _appDbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
            });
            return new CreateResponse(_mapper.Map<ProductDto>(product), Messages.CreateSuccess);
        }

        public async Task<bool> DeLeteAsync(int id)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        if (await DeleteFilePathAsync(product.ImagePath))
                        {
                            _appDbContext.Products.Remove(product);
                            await _appDbContext.SaveChangesAsync();
                            await trans.CommitAsync();
                            return;
                        }
                        throw new AppException(Exceptions.DeleteteFail);
                    }
                    _appDbContext.Products.Remove(product);
                    await _appDbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
            });
            return true;
        }

        public async Task<PageResult<ProductDto>> GetAllAsync(GetProductPageRequest request)
        {
            var query = from pro in _appDbContext.Products
                        join cate in _appDbContext.Categories on pro.CategoryId equals cate.Id
                        where pro.Status == "Available"
                        select new { pro, cate.Name };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pro.CategoryId == request.CategoryId);
            }
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
               .Take(request.PageSize)
               .Select(x => new ProductDto()
               {
                   Id = x.pro.Id,
                   Name = x.pro.Name,
                   Stock = x.pro.Stock,
                   Price = x.pro.Price,
                   DateCreated = x.pro.DateCreated,
                   DateUpdated = x.pro.DateUpdated,
                   ImagePath = $"{_imagePath}{x.pro.ImagePath}",
                   CategoryName = x.Name,
                   Status = x.pro.Status,
                   Description = x.pro.Description
               })
               .ToListAsync();
            var pageResult = new PageResult<ProductDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pageResult;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _appDbContext.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                throw new KeyNotFoundException(Exceptions.ProductNotFound);
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                throw new AppException(Exceptions.BadRequest);
            }
            var result = _appDbContext.Products.FirstOrDefault(x => x.Id == id);
            if (await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Id == updateProductDto.CategoryId) is null)
            {
                throw new AppException(Exceptions.CategoryNameNotExist);
            }

            var product = _mapper.Map(updateProductDto, result);
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (updateProductDto.Image is not null)
                    {
                        product.ImagePath = await SaveFileAsync(updateProductDto.Image);
                        await DeleteFilePathAsync(product.ImagePath);
                    }
                    _appDbContext.Products.Update(product);
                    await _appDbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
            });
            return true;
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + CONTENT_FOLDER_NAME + "/" + fileName;
        }
        private async Task<bool> DeleteFilePathAsync(string filePath)
        {
            return await _storageService.DeleteFilePathAsync(filePath);
        }

        public async Task<bool> UpdateStock(int id, int quantity)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                throw new AppException(Exceptions.ProductNotFound);
            }
            product.Stock -= quantity;
            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusProduct(int id)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                throw new KeyNotFoundException(Exceptions.ProductNotFound);
            }
            product.Status = "Unavailable";
            _appDbContext.Products.Update(product);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
