using System.Data.Entity;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QLTTTM.models;


namespace DAPM.Controllers
{
    public class ClientController : Controller
    {
        private DataSQLContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ClientController(DataSQLContext context, IWebHostEnvironment _webHostEnvironment)
        {
            dbContext = context;
            webHostEnvironment = _webHostEnvironment;

        }


        [HttpGet]
        [ActionName("AddContact")]
        public IActionResult AddContact(int MAMB)
        {
            TempData["MAMB"] = MAMB;
            return View();
        }



        [HttpPost]
        [ActionName("AddContact")]
        public async Task<IActionResult> AddContact(KhachHang model, int mamb)
        {

            if (ModelState.IsValid)
            {
                await dbContext.KhachHangs.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("HomeUser", "Custome");
            }

            TempData["MAMB"] = model.MAMB;
            return View(model);
        }


        [HttpPost]
        [ActionName("ChangeStatusKH")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusKH(int makh)
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            KhachHang? khachHang = dbContext.KhachHangs.SingleOrDefault(x => x.MAKH == makh);
            if (khachHang != null)
            {
                if (!khachHang.TRANGTHAI)
                {
                    MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == khachHang.MAMB);
                    if (matBang != null)
                    {
                        bool check = matBang.TRANGTHAI;
                        bool oppositeCheck = !check;
                        matBang.TRANGTHAI = oppositeCheck;
                        dbContext.MatBangs.Update(matBang);
                        await dbContext.SaveChangesAsync();
                        return RedirectToAction("AddTenants", "Client", new { makh = makh });
                    }
                }
                else
                {
                    HopDongMatBang? hopDongMatBang = dbContext.HopDongMatBangs.SingleOrDefault(x => x.MAKH == makh);
                    if (hopDongMatBang != null)
                    {
                        MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == khachHang.MAMB);
                        HopDong? hopDong = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == hopDongMatBang.MAHD);

                        if (matBang != null && hopDong != null)
                        {
                            khachHang.TRANGTHAI = false;
                            matBang.TRANGTHAI = false;
                            dbContext.MatBangs.Update(matBang);
                            dbContext.KhachHangs.Update(khachHang);
                            dbContext.HopDongMatBangs.Remove(hopDongMatBang);
                            dbContext.HopDongs.Remove(hopDong);
                            await dbContext.SaveChangesAsync();
                            return RedirectToAction("TenantInfo", "Admin");
                        }

                    }
                }
            }
            return RedirectToAction("HomeAdmin", "Admin", new { MANV = manv });
        }



        [HttpGet]
        [ActionName("AddTenants")]
        public IActionResult AddTenants(int makh)
        {
            if (makh != 0)
            {
                var model = dbContext.KhachHangs.SingleOrDefault(x => x.MAKH == makh);
                if (model != null)
                {
                    KhachHangAndHopDong khachHangAndHopDong = new KhachHangAndHopDong();
                    khachHangAndHopDong.KHModels = model;
                    return View(khachHangAndHopDong);
                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }


        [HttpPost]
        [ActionName("AddTenants")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTenants(KhachHangAndHopDong hdktModel)
        {
            if (ModelState.IsValid)
            {
                KhachHang? khachHang = dbContext.KhachHangs.FirstOrDefault(x => x.MAKH == hdktModel.KHModels.MAKH);
                // Lưu dữ liệu vào cơ sở dữ liệu
                if (khachHang != null)
                {
                    khachHang.HOTEN = hdktModel.KHModels.HOTEN;
                    khachHang.NGAYSINH = hdktModel.KHModels.NGAYSINH;
                    khachHang.GIOITINH = hdktModel.KHModels.GIOITINH;
                    khachHang.SDT = hdktModel.KHModels.SDT;
                    khachHang.EMAIL = hdktModel.KHModels.EMAIL;
                    khachHang.DIACHI = hdktModel.KHModels.DIACHI;
                    khachHang.TRANGTHAI = hdktModel.KHModels.TRANGTHAI;
                    khachHang.MAMB = hdktModel.KHModels.MAMB;

                    dbContext.KhachHangs.Update(khachHang);
                }

                await dbContext.HopDongs.AddAsync(hdktModel.HDModels);
                await dbContext.SaveChangesAsync();

                var hopdongIDMAX = dbContext.HopDongs.Max(x => x.MAHD);
                HopDongMatBang hopDongMatBang = new HopDongMatBang();
                hopDongMatBang.MAHD = hopdongIDMAX;
                hopDongMatBang.MAKH = hdktModel.KHModels.MAKH;
                hopDongMatBang.MAMB = hdktModel.KHModels.MAMB;

                dbContext.HopDongMatBangs.Add(hopDongMatBang);
                dbContext.SaveChanges();

                return RedirectToAction("TenantInfo", "Admin");
            }


            return View(hdktModel);


        }






        //Thuc hien xoa doi tac : 
        [HttpPost]
        [ActionName("DeleteTenants")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTenants(int? makh)
        {
            if (makh != null)
            {
                KhachHang? khachHang = dbContext.KhachHangs.SingleOrDefault(x => x.MAKH == makh);
                if (khachHang != null)
                {
                    dbContext.KhachHangs.Remove(khachHang);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("TenantInfo", "Admin");
                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }



        //Thuc hien xoa khach hang va hop dong khach thue :
        //Thuc hien xoa doi tac : 
        [HttpPost]
        [ActionName("XoaHDKH")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaHDKH(int? mahdkh)
        {
            if (mahdkh != null)
            {
                HopDongMatBang? hopDongMatBang = dbContext.HopDongMatBangs.SingleOrDefault(x => x.MAHD == mahdkh);
                if (hopDongMatBang != null)
                {
                    KhachHang? khachHang = dbContext.KhachHangs.SingleOrDefault(x => x.MAKH == hopDongMatBang.MAKH);
                    HopDong? hopDong = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == mahdkh);
                    MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == hopDongMatBang.MAMB);
                    if (khachHang != null && hopDong != null && matBang != null)
                    {


                        matBang.TRANGTHAI = false;
                        dbContext.MatBangs.Update(matBang);
                        dbContext.HopDongMatBangs.Remove(hopDongMatBang);
                        dbContext.HopDongs.Remove(hopDong);
                        dbContext.KhachHangs.Remove(khachHang);
                        await dbContext.SaveChangesAsync();
                        return RedirectToAction("TenantInfo", "Admin");
                    }


                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }





        //Chuc nang cap nhat DT : 
        [HttpGet]
        [ActionName("UpdateContract")]
        public async Task<IActionResult> UpdateContract(int? mahdkh)
        {
            if (mahdkh != null)
            {
                HopDongMatBang? hopDongMatBang = dbContext.HopDongMatBangs.SingleOrDefault(x => x.MAHD == mahdkh);
                if (hopDongMatBang != null)
                {
                    HopDong? hopDong1 = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == hopDongMatBang.MAHD);
                    KhachHang? khachHang = dbContext.KhachHangs.SingleOrDefault(x => x.MAKH == hopDongMatBang.MAKH);
                    if (hopDong1 != null && khachHang != null)
                    {
                        KhachHangAndHopDong khachHangAndHopDong = new KhachHangAndHopDong();
                        khachHangAndHopDong.KHModels = khachHang;
                        khachHangAndHopDong.HDModels = hopDong1;
                        return View(khachHangAndHopDong);
                    }
                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }



        [HttpPost]
        [ActionName("UpdateContract")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateContract(KhachHangAndHopDong model, int MAKH, int MAHD, int MAMB)
        {
            if (ModelState.IsValid)
            {
                HopDong? existingHopdong = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == MAHD);
                KhachHang? khachHang = dbContext.KhachHangs.SingleOrDefault(x => x.MAKH == MAKH);
                MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == MAMB);


                if (existingHopdong != null && khachHang != null && matBang != null)
                {

                    khachHang.HOTEN = model.KHModels.HOTEN;
                    khachHang.NGAYSINH = model.KHModels.NGAYSINH;
                    khachHang.GIOITINH = model.KHModels.GIOITINH;
                    khachHang.SDT = model.KHModels.SDT;
                    khachHang.EMAIL = model.KHModels.EMAIL;
                    khachHang.DIACHI = model.KHModels.DIACHI;
                    khachHang.TRANGTHAI = model.KHModels.TRANGTHAI;
                    khachHang.MAMB = MAMB;



                    existingHopdong.TENHDDT = model.HDModels.TENHDDT;
                    existingHopdong.LOAIHOPDONG = model.HDModels.LOAIHOPDONG;
                    existingHopdong.THOIHAN = model.HDModels.THOIHAN;
                    existingHopdong.HIEULUC = model.HDModels.HIEULUC;
                    existingHopdong.MANV = model.HDModels.MANV;


                    dbContext.KhachHangs.Update(khachHang);
                    dbContext.HopDongs.Update(existingHopdong);
                    dbContext.SaveChangesAsync();
                    return RedirectToAction("TenantInfo", "Admin");
                }
            }
            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            return View(model);
        }
    }
}