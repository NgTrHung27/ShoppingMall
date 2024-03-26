using System.Data.Entity;
using DAPM.Services;
using Microsoft.AspNetCore.Mvc;
using QLTTTM.Datas;
using QLTTTM.models;


namespace DAPM.Controllers{
    public class PremisesController: Controller{
        private DataSQLContext dbContext = SingletonDbContext.Instance;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFactoryServices<MatBang> factoryServices;

        public PremisesController(DataSQLContext context, IWebHostEnvironment _webHostEnvironment ){
            dbContext = context;
            webHostEnvironment = _webHostEnvironment;
            factoryServices = new PremisesServices();
        }


        [HttpGet]
        [ActionName("AddPremises")]
        public async Task<IActionResult> AddPremises(){
            return View();
        }

        [HttpPost]
        [ActionName("AddPremises")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPremises(MatBang mbModel, IFormFile tieude, IFormFile chudao, List<IFormFile> noidung)
        {
            if (ModelState.IsValid)
            {
                factoryServices.Add(mbModel, null, webHostEnvironment, tieude, chudao, noidung);

                return RedirectToAction("PremisesInfo", "Admin");
            }

            return View(mbModel);
        }

        //Thuc hien xoa mat bang : 
        [HttpPost]
        [ActionName("DeletePremises")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePremises(int? mamb){
            
            factoryServices.Delete(mamb, webHostEnvironment);
            return RedirectToAction("PremisesInfo", "Admin");

        }




        //Chuc nang cap nhat MB : 
        [HttpGet]
        [ActionName("UpdatePremises")]
        public async Task<IActionResult> UpdatePremises(int? mamb){
            if(mamb != null){
                MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == mamb);
                if(matBang != null){
                    return View(matBang);
                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }



        [HttpPost]
        [ActionName("UpdatePremises")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePremises(MatBang model, IFormFile tieude, IFormFile chudao , List<IFormFile> noidung)
        {
            if (ModelState.IsValid)
            {    
                return RedirectToAction("PremisesInfo", "Admin");
            }

            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            return View(model);
        }    
    }
}