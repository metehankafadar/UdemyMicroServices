using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        [Display(Name="Kurs İsmi")]
        
        public string Name { get; set; }
        [Display(Name = "Fiyat")]
        
        public decimal Price { get; set; }
        [Display(Name = "Açıklama")]
        
        public string Description { get; set; }
        public string UserId { get; set; }
        [Display(Name = "Fotoğraf")]
        public string Picture { get; set; }
        public FeatureViewModel Feature { get; set; }
        [Display(Name = "Kurs Kategori")]
        public string CategoryId { get; set; }
        [Display(Name = "Kurs Resim")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
