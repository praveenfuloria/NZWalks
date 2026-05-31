using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        private readonly IMapper mapper;

        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            this.imageRepository = imageRepository;
            this.mapper = mapper;
        }
        //POST : api/Images/Upload
        [HttpPost]
        [Route("Upload")]

        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            ValidateImageUpload(imageUploadRequestDto);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Convert to DTO to Domain Model
            var imageDomainModel = new Image
            {
                File = imageUploadRequestDto.File,
                FileName = imageUploadRequestDto.FileName,
                FileDescription = imageUploadRequestDto.FileDescription,
                FileSizeInBytes = imageUploadRequestDto.File.Length,
                FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName)
            };

            //User repository to Upload Image
           await imageRepository.UploadAsync(imageDomainModel);

            return Ok(mapper.Map<ImageUploadRequestDto>(imageDomainModel));


        }

        private void ValidateImageUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowedFileExtension = new string[] { ".jpg", ".jped",".png" };

            if(!allowedFileExtension.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
            {
                ModelState.AddModelError("File", "UnSupported File");
            }

            if (imageUploadRequestDto.File.Length > 1048760)
            {
                ModelState.AddModelError("File", "File Size more than 10 Mb");
            }


        }
    }
}
