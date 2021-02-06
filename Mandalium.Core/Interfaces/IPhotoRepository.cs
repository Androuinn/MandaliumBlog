using System.Threading.Tasks;
using Mandalium.Core.Helpers.Pagination;
using Mandalium.Core.Models;

namespace Mandalium.Core.Interfaces
{
    public interface IPhotoRepository
    {

        /// <summary>
        /// Ass the photo given by the entity(To Cloudinary)
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        Task<int> AddPhoto(Photo photo);

        /// <summary>
        /// Gets all photos specified with the user parameters
        /// </summary>
        /// <param name="userParams"></param>
        /// <returns></returns>
        Task<PagedList<Photo>> GetPhotos(UserParams userParams);
    } 
}