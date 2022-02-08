using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {


        private readonly HttpClient _client;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient client, IPhotoStockService photoStockService,PhotoHelper photoHelper)
        {
            _client = client;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper; 
            
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {


            var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

            if(resultPhotoService != null)
            {
                courseCreateInput.Picture = resultPhotoService.Url;
            }
            



            var response = await _client.PostAsJsonAsync<CourseCreateInput>("courses",courseCreateInput);


            return response.IsSuccessStatusCode;



        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _client.DeleteAsync($"courses/{courseId}");


            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            //startup da base url de catalog path tarafı services.AddHttpclients'da tanımladığımız. localhost:5000/services/catalog/categories
            var response = await _client.GetAsync("categories");
            //200 dönmediyse
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            //startup da base url de catalog path tarafı services.AddHttpclients'da tanımladığımız. localhost:5000/services/catalog/courses
            var response = await _client.GetAsync("courses");
            //200 dönmediyse
            if (!response.IsSuccessStatusCode) 
            {
                return null;
            }

            var responseSuccess= await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return responseSuccess.Data;

        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            //startup da base url de catalog path tarafı services.AddHttpclients'da tanımladığımız. localhost:5000/services/catalog/courses
            var response = await _client.GetAsync($"courses/GetAllByUserId/{userId}");
            //200 dönmediyse
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            responseSuccess.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);

            });

            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            //startup da base url de catalog path tarafı services.AddHttpclients'da tanımladığımız. localhost:5000/services/catalog/courses
            var response = await _client.GetAsync($"courses/{courseId}");
            //200 dönmediyse
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                //update de input hiden dan gelen picture yi siliyoruz.
                await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhotoService.Url;
            }


            var response = await _client.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);


            return response.IsSuccessStatusCode;
        }
    }
}
