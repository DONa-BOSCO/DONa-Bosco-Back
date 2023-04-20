﻿using API.Attributes;
using API.IServices;
using API.Models;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web.Http.Cors;
using static Entities.Entities.ProductItem;
using Base64FileModel = Entities.Entities.Base64FileModel;

namespace API.Controllers
{
    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IFileService _fileService;


        public ProductController(IProductService productService, IFileService fileService)
        {
            _productService = productService;
            _fileService = fileService;
        }

        //[HttpGet(Name = "GetProductByCriteria")]
        //public List<ProductItem> GetByCriteria([FromQuery] ProductFilter productFilter)
        //{
        //    return _productService.GetProductByCriteria(productFilter);
        //}
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpGet(Name = "GetProductById")]
        public List<ProductItem> GetProductById([FromQuery] int id)
        {
            return _productService.GetProductById(id);
        }
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpPatch(Name = "UpdateProduct")]
        public void UpdateProduct([FromBody] NewProductRequest newProductRequest)
        {
            try
            {
                var fileItem = new FileItem();

                fileItem.Id = 0;
                fileItem.Name = newProductRequest.FileData.FileName;
                fileItem.InsertDate = DateTime.Now;
                fileItem.UpdateDate = DateTime.Now;
                fileItem.Content = Convert.FromBase64String(newProductRequest.FileData.Base64FileContent);

                var fileId = _fileService.InsertFile(fileItem);

                var productItem = new ProductItem();
                productItem.Id = newProductRequest.ProductData.Id;
                productItem.Title = newProductRequest.ProductData.Title;
                productItem.Description = newProductRequest.ProductData.Description;
                productItem.Category = newProductRequest.ProductData.Category;
                productItem.Condition = newProductRequest.ProductData.Condition;
                productItem.Location = newProductRequest.ProductData.Location;
                productItem.IdPhotoFile = fileId;
                 _productService.UpdateProduct(productItem);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpDelete(Name = "DeleteProduct")]
        public void Delete([FromQuery] int id)
        {
            _productService.DeleteProduct(id);
        }
        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpDelete(Name = "DeactivateProduct")]
        public void DeactivateProduct([FromQuery] int id)
        {
            _productService.DeactivateProduct(id);
        }


        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpGet(Name = "GetAllProduct")]
        public List<ProductInfoModel> GetAllProduct()
        {
            try
            {
                List<ProductInfoModel> resultList = new List<ProductInfoModel>();

                var fileList = _fileService.GetAllImagesList();
                var productList = _productService.GetAllProduct();


                foreach (var prod in productList)
                {
                    ProductInfoModel resultItem = new ProductInfoModel();
                    resultItem.ProducItem = prod;

                    var fileItem = fileList.Where(f => f.Id == prod.IdPhotoFile).First();

                    Base64FileModel fileModel = new Base64FileModel();
                    fileModel.FileName = fileItem.Name;
                    fileModel.FileExtension = fileItem.FileExtension;
                    fileModel.Content = fileItem.Base64Content;

                    resultItem.Base64FileModel = fileModel;

                    resultList.Add(resultItem);
                }

                return resultList;
            }
            catch (Exception ex) { throw; }

        }


        [EndpointAuthorize(AllowsAnonymous = true)]
        [HttpPost(Name = "AddProduct")]
        public int AddProduct([FromBody] NewProductRequest newProductRequest)
        {
            try
            {
                var fileItem = new FileItem();

                fileItem.Id = 0;
                fileItem.Name = newProductRequest.FileData.FileName;
                fileItem.InsertDate = DateTime.Now;
                fileItem.UpdateDate = DateTime.Now;
                fileItem.Content = Convert.FromBase64String(newProductRequest.FileData.Base64FileContent);

                var fileId = _fileService.InsertFile(fileItem);

                var productItem = new ProductItem();
                productItem.Id = newProductRequest.ProductData.Id;
                productItem.Title = newProductRequest.ProductData.Title;
                productItem.Description = newProductRequest.ProductData.Description;
                productItem.Category = newProductRequest.ProductData.Category;
                productItem.Condition = newProductRequest.ProductData.Condition;
                productItem.Location = newProductRequest.ProductData.Location;
                productItem.IdPhotoFile = fileId;
                return _productService.AddProduct(productItem);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
 }

