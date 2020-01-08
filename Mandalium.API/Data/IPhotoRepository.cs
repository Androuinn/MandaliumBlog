using System.Threading.Tasks;
using Mandalium.API.Dtos;
using Mandalium.API.Models;

namespace Mandalium.API.Data
{
    public interface IPhotoRepository
    {
        Task<int> AddPhoto(Photo photo);
    } 
}