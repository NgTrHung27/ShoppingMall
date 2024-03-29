using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using DAPM.Services;
using DAPM.StatePattern;
using Microsoft.AspNetCore.Mvc;
using QLTTTM.Datas;
using QLTTTM.models;


namespace DAPM.Controllers{
    public class EventController: Controller{
        private DataSQLContext dbContext = SingletonDbContext.Instance;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFactoryServices<SuKien> factoryServices;
        public EventController(DataSQLContext context, IWebHostEnvironment _webHostEnvironment ){
            dbContext = context;
            webHostEnvironment = _webHostEnvironment;
            factoryServices = new EventServices();
        } 


        [HttpGet]
        [ActionName("AddEvent")]
        public async Task<IActionResult> AddEvent(){
            return View();
        }
        /// <summary>
        /// Thêm 1 sự kiện mới
        /// </summary>
        /// <param name="skModel">Đối tượng SuKien chứa thông tin về sự kiện.</param>
        /// <param name="tieude">File chứa hình  tiêu đề của sự kiện.</param>
        /// <param name="chudao">File chứa hình chủ đạo của sự kiện.</param>
        /// <param name="noidung">Danh sách các file chứa nội dung của sự kiện.</param>
        /// <returns>Chuyển hướng đến trang EventInfo nếu thêm thành công ngược lại trả về View với model skModel.</returns>
        [HttpPost]
        [ActionName("AddEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(SuKien skModel, IFormFile tieude, IFormFile chudao, List<IFormFile> noidung)
        {
            if (ModelState.IsValid)
            {
                factoryServices.Add(skModel, null, webHostEnvironment, tieude, chudao, noidung);
                return RedirectToAction("EventInfo", "Admin");
            }

            return View(skModel);
        }

        //Xu ly chuc nang thay doi trang thai : 
        [HttpPost]
        [ActionName("ChangeStatusSK")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusSK(int mask)
        {
            SuKien? suKien = dbContext.SuKiens.SingleOrDefault(x => x.MASK == mask);
            if (suKien != null)
            {
                if (suKien.State == null)
                {
                    suKien.State = suKien.TRANGTHAI ? (IEventState)new ApprovedState() : new UnapprovedState();
                }
                suKien.State.Handle(suKien);
                suKien.TRANGTHAI = !suKien.TRANGTHAI; // Update the TRANGTHAI after Handle is called
                dbContext.SuKiens.Update(suKien);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("EventInfo", "Admin");
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }

        //Thuc hien xoa mat bang : 
        [HttpPost]
        [ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int? mask){
            factoryServices.Delete(mask, webHostEnvironment);
            return RedirectToAction("EventInfo", "Admin");

        }
        //Chuc nang cap nhat MB : 
        [HttpGet]
        [ActionName("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(int? mask){
            if(mask != null){
                SuKien? suKien = dbContext.SuKiens.SingleOrDefault(x => x.MASK == mask);
                if(suKien != null){
                    return View(suKien);
                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }
      
        [HttpPost]
        [ActionName("UpdateEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEvent(SuKien skmodel, IFormFile tieude, IFormFile chudao , List<IFormFile> noidung)
        {
            if (ModelState.IsValid)
            {
                factoryServices.Update(skmodel, tieude, chudao, noidung, webHostEnvironment);
                return RedirectToAction("EventInfo", "Admin");
            }
            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            return View(skmodel);
        }
    }
}