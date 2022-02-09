using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Services.Interfaces;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Shared.Messages;
using MassTransit;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper,IDatabaseSettings databaseSettings,IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Shared.DTOs.Response<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach( var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Shared.DTOs.Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Shared.DTOs.Response<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if(course == null)
            {
                return Shared.DTOs.Response<CourseDTO>.Fail("Course Not Found", 404);
            }
            else
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                return Shared.DTOs.Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
            }
        }

        public async Task<Shared.DTOs.Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId) 
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();

                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Shared.DTOs.Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Shared.DTOs.Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDTO)
        {
            //CreateDTO'yu course a çeviriyor.
            var newCourse = _mapper.Map<Course>(courseCreateDTO);

            newCourse.CreatedTime = DateTime.Now;

            // Ekleme işlemi yapılıyor
            await _courseCollection.InsertOneAsync(newCourse);

            return Shared.DTOs.Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), 200);
        }

        public async Task<Shared.DTOs.Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDTO);
            // bul ve değiştir komutu 
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDTO.Id, updateCourse);

            if(result == null)
            {
                return Shared.DTOs.Response<NoContent>.Fail("Course not found", 404);
            }
            //eventlarda kuyruk belirlemeye gerek yok.
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent
            {
                CourseId = updateCourse.Id,
                UpdatedName = updateCourse.Name
            });

            return Shared.DTOs.Response<NoContent>.Success(204);
        }

        public async Task<Shared.DTOs.Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
            {
                return Shared.DTOs.Response<NoContent>.Success(204);
            }
            else
            {
                return Shared.DTOs.Response<NoContent>.Fail("Not Found",404);
            }
        }
    }
}
