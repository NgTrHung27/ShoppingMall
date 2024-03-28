using DAPM.Facade;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTTTM.Datas;
using QLTTTM.models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QLTTTM.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeFacade employeeFacade;
        DataSQLContext dbContext = SingletonDbContext.Instance;

        public EmployeeController(EmployeeFacade _employeeFacade)
        {
            employeeFacade = _employeeFacade;
        }

        [HttpPost]
        [ActionName("AddEmployee")]
        [ValidateAntiForgeryToken]
        
        /// <param name="nvModel">Nhân Viên cần thêm mới</param>
        /// <param name="file">Ảnh đại diện của Nhân Viên</param>
        
        public async Task<IActionResult> AddEmployee(NhanVien nvModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await employeeFacade.AddEmployee(nvModel, file);
                return RedirectToAction("EmployeeInfo", "Admin");
            }

            return View(nvModel);
        }


        
        [HttpPost]
        [ActionName("UpdateEmployee")]
        [ValidateAntiForgeryToken]

        /// <param name="model">Nhân Viên cần cập nhật</param>
        /// <param name="newAvatar">Ảnh đại diện mới của Nhân Viên</param>
        /// <!---->
        
        public async Task<IActionResult> UpdateEmployee(NhanVien model, IFormFile newAvatar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await employeeFacade.UpdateEmployee(model, newAvatar);
                    int? manv = HttpContext.Session.GetInt32("MANV");
                    if (manv != null && manv == model.MANV)
                    {
                        return RedirectToAction("Logout", "Admin");
                    }
                    return RedirectToAction("EmployeeInfo", "Admin");
                }
                catch (Exception ex)
                {
                    // Xử lý khi không tìm thấy Nhân Viên cần cập nhật
                    ModelState.AddModelError("", ex.Message);
                }
            }
            
            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            List<ChucVu> list_cv = dbContext.ChucVus.ToList();
            ViewBag.cvs = list_cv;
            return View(model);
        }

        [HttpPost]
        [ActionName("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(Account model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await employeeFacade.UpdateAccount(model);
                    return RedirectToAction("EmployeeInfo", "Admin");
                }
                catch (Exception ex)
                {
                    // Xử lý khi không tìm thấy Tài khoản cần cập nhật
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }
    }


 
}
