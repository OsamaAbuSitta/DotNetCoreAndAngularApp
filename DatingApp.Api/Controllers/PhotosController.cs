using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using System.Threading.Tasks;
using DatingApp.Api.Dtos;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using DatingApp.Api.Models;
using System.Linq;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IDatingRepository Repo;
        private readonly IMapper Mapper;
        private readonly IOptions<CloudinarySettings> CloudinarySettings;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
        {
            this.CloudinarySettings = cloudinarySettings;
            this.Mapper = mapper;
            this.Repo = repo;

            Account account = new Account(
                cloudinarySettings.Value.CoudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await Repo.GetPhoto(id);
            var photoToReturn = Mapper.Map<PhotoToReturnDto>(photo);
            return Ok(photoToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,
        [FromForm] PhotoForCreationDto photoForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await Repo.GetUser(userId);

            var uploadResult = new ImageUploadResult();

            if (photoForCreation.file.Length > 0)
            {
                using (var stream = photoForCreation.file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(photoForCreation.file.Name, stream),
                        Transformation = new Transformation()
                                            .Width(500).Height(500)
                                            .Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;

            var photo = Mapper.Map<Photo>(photoForCreation);

            photo.IsMain = !user.Photos.Any(u => u.IsMain);

            user.Photos.Add(photo);
            
            if (await Repo.SaveAll())
            {
                var photoToReturn = Mapper.Map<PhotoToReturnDto>(photo);
                return CreatedAtRoute("GetPhoto",new {id = photo.Id},photoToReturn);
            }
            return BadRequest();
        }

            [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await Repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await Repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await Repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if (await Repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await Repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await Repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    Repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                Repo.Delete(photoFromRepo);
            }

            if (await Repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}