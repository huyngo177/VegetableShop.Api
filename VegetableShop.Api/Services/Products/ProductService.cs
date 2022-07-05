using AutoMapper;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Products;
using Microsoft.EntityFrameworkCore;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Common;
using System.Net.Http.Headers;
using VegetableShop.Api.Services.Storage;

namespace VegetableShop.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private const string CONTENT_FOLDER_NAME = "user-content";
        public ProductService(AppDbContext appDbContext, IMapper mapper, IStorageService storageService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<CreateResponse> CreateAsync(CreateProductDto createProductDto)
        {
            string errors = "";
            if (createProductDto is null)
            {
                throw new BadHttpRequestException(Exceptions.BadRequest);
            }
            var result = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Name == createProductDto.Name);
            if (result is not null)
            {
                errors = string.Concat(errors, Exceptions.ProductNameExist);
            }
            if (await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Name == createProductDto.CategoryName) is null)
            {
                errors = string.Concat(errors, Exceptions.CategoryNameNotExist);
            }
            if (errors.Length > 0)
            {
                return new CreateResponse(errors);
            }
            var product = _mapper.Map<Product>(createProductDto);
            var category = _mapper.Map<Category>(createProductDto);
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    product.Category = category;
                    product.ImagePath = await SaveFile(createProductDto.Image);
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
            if (product is null)
            {
                return false;
            }
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        await _storageService.DeleteFileAsync(product.ImagePath);
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

        public IEnumerable<ProductDto> GetAll()
        {
            var products = _appDbContext.Products.Include(x => x.Category).ToList();
            IEnumerable<ProductDto> result = _mapper.Map<List<Product>, IEnumerable<ProductDto>>(products);
            return result;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _appDbContext.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                throw new BadHttpRequestException(Exceptions.ProductNotFound);
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                throw new BadHttpRequestException(Exceptions.BadRequest);
            }
            var result = _appDbContext.Products.FirstOrDefault(x => x.Id == id);
            if (result is null)
            {
                return false;
            }
            if (await _appDbContext.Categories.FirstOrDefaultAsync(x => x.Name == updateProductDto.CategoryName) is null)
            {
                throw new BadHttpRequestException(Exceptions.CategoryNameNotExist);
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
                        product.ImagePath = await SaveFile(updateProductDto.Image);
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

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + CONTENT_FOLDER_NAME + "/" + fileName;
        }
    }
}
