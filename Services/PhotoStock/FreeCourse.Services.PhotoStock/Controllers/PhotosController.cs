using FreeCourse.Services.PhotoStock.DTOs;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CostumBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken) //cancellationtoken alma sebebi fotoğraf geldiğinde 20sn sürüyor gelmesi endpointi çağıran yer işlemi sonlandırırsa fotoğraf kaydetme de sonlansın.
        {
            if(photo != null && photo.Length>0)
            {

                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photo.FileName);


                using (var stream = new FileStream(path, FileMode.Create)) 
                    await photo.CopyToAsync(stream, cancellationToken);

                //http://www.photostock.api.com/photos/adsadas.jpg 
                var returnPath = photo.FileName;

                PhotoDTO photoDTO = new() { Url = returnPath };
                //dd
                return CreateActionResultInstance(Response<PhotoDTO>.Success(photoDTO,200));


            }

            return CreateActionResultInstance(Response<PhotoDTO>.Fail("Photo is empty",400));
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoURL)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photoURL);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }
            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
