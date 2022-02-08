﻿using AutoMapper;
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

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CourseService(IMapper mapper,IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDTO>>> GetAllAsync()
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

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Response<CourseDTO>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if(course == null)
            {
                return Response<CourseDTO>.Fail("Course Not Found", 404);
            }
            else
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
            }
        }

        public async Task<Response<List<CourseDTO>>> GetAllByUserIdAsync(string userId) 
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

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDTO)
        {
            //CreateDTO'yu course a çeviriyor.
            var newCourse = _mapper.Map<Course>(courseCreateDTO);

            newCourse.CreatedTime = DateTime.Now;

            // Ekleme işlemi yapılıyor
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDTO);
            // bul ve değiştir komutu 
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDTO.Id, updateCourse);

            if(result == null)
            {
                return Response<NoContent>.Fail("Course not found", 404);
            }

            return Response<NoContent>.Success(200);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("Not Found",404);
            }
        }
    }
}
