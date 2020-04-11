using System.Threading.Tasks;
using Mandalium.API.Dtos;
using Mandalium.API.Helpers;
using Mandalium.API.Models;

namespace Mandalium.API.Data
{
    public interface IPhotoRepository
    {
        Task<int> AddPhoto(Photo photo);

        Task<PagedList<Photo>> GetPhotos(UserParams userParams);
    } 
}