using BLL.DTO;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        int CreateProduct(FullProductDto dto);
        void UpdateProduct(FullProductDto dto);
        void DeleteProduct(int id);
        List<ProductDto> GetAllProducts();
        List<ProductDto> GetProductsByCategory(int categoryId);
        ProductDto? GetProductById(int id);
        List<ProductShortDto> GetShortProductList();
        List<ProductDto> GetProductsBySearch(string searchText, int? categoryId = null);
    }
}
