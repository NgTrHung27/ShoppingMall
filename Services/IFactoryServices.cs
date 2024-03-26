using Microsoft.AspNetCore.Mvc;
using QLTTTM.models;

namespace DAPM.Services
{
    public interface IFactoryServices<T> where T : class
    {
        //List Method use for Services
        void Add(T entity, IFormFile? image, IWebHostEnvironment webHostEnvironment, IFormFile? tieude, IFormFile? chudao, List<IFormFile>? noidung);
        void Update(T entity, IFormFile? tieude, IFormFile chudao, List<IFormFile> noidung, IWebHostEnvironment webHostEnvironment);
        void Delete(int? id, IWebHostEnvironment webHostEnvironment);
    }
}
