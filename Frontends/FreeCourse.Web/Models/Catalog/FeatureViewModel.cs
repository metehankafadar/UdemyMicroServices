using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Catalog
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs süresi")]
        [Required]
        public int Duration { get; set; }

    }
}
