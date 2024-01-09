using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTTTM.models;
using System.Linq; // Đảm bảo bạn đã thêm using này


namespace DAPM.Controllers
{
    public class CustomeController : Controller
    {


        private DataSQLContext dataSQLContext;

        public CustomeController(DataSQLContext data)
        {
            dataSQLContext = data;
        }


        public IActionResult TrangChu()
        {
            var data = dataSQLContext;
            return View(data);
        }

        public IActionResult ThuongHieu(int maloai)
        {
            if (maloai == 0)
            {
                List<DoiTac> doiTacs = dataSQLContext.DoiTacs.OrderByDescending(x => x.MALOAIDOITAC).ToList();
                ViewBag.tenloadt = "";
                return View(doiTacs);

            }
            else
            {
                List<DoiTac> doiTacs = dataSQLContext.DoiTacs.Where(x => x.MALOAIDOITAC == maloai).OrderByDescending(x => x.MADT).ToList();
                LoaiDoiTac? loaiDoiTac = dataSQLContext.LoaiDoiTacs.SingleOrDefault(x => x.MALOAIDT == maloai);

                ViewBag.tenloadt = loaiDoiTac.TENLOAI;

                return View(doiTacs);
            }
        }

        public IActionResult TieuDiem()
        {
            List<SuKien> tieudiem = dataSQLContext.SuKiens.Where(x => x.LOAISK == "TD" && x.TRANGTHAI == true).OrderByDescending(x => x.MASK).ToList();
            return View(tieudiem);
        }

        public IActionResult UuDai()
        {
            List<SuKien> uudai = dataSQLContext.SuKiens.Where(x => x.LOAISK == "UD" && x.TRANGTHAI == true).OrderByDescending(x => x.MASK).ToList();
            return View(uudai);
        }

        [HttpGet]
        public IActionResult MatBang()
        {
            List<MatBang> list_mb = dataSQLContext.MatBangs.ToList();
            return View(list_mb);
        }

        [HttpGet]
        public IActionResult ChiTietMB(int? mamb)
        {
            if (mamb != null)
            {
                MatBang? mb = dataSQLContext.MatBangs.SingleOrDefault(x => x.MAMB == mamb);
                if (mb != null)
                {
                    return View(mb);
                }


            }
            return RedirectToAction("MatBang", "Custome");
        }
        [HttpGet]
        public IActionResult ChiTietSK(int? mask)
        {   
            if (mask != null)
            {
                SuKien? suKien = dataSQLContext.SuKiens.SingleOrDefault(x => x.MASK == mask);
                if (suKien != null)
                {
                    return View(suKien);
                }
            }
                return RedirectToAction("ChiTietSK", "Custome");
        }
        [HttpGet]
        public IActionResult Search(string query)
        {
            query = query.ToLower().Trim();


            // Tìm kiếm sự kiện
            var sukienResults = dataSQLContext.SuKiens
                .Where(e =>
                            EF.Functions.Like(e.TENSK.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.LOAISK.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.THOIGIAN.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.VITRI.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.MOTA.ToLower().Trim(), $"%{query}%")
                            )
                .ToList();

            // Tìm kiếm đối tác
            var doitacResults = dataSQLContext.DoiTacs
                .Where(p =>
                            EF.Functions.Like(p.TENDT.ToLower().Trim(), $"%{query}%")
                )
                .ToList();


            // Thực hiện tìm kiếm gần đúng sử dụng Entity Framework Core
            var matbangResults = dataSQLContext.MatBangs
                .Where(item =>
                                EF.Functions.Like(item.TENMB.ToLower().Trim(), $"%{query}%") ||
                                EF.Functions.Like(item.VITRI.ToLower().Trim(), $"%{query}%") ||
                                EF.Functions.Like(item.MOTA.ToLower().Trim(), $"%{query}%")
                )
                .ToList();




            // Kết hợp kết quả từ cả ba loại :
            var combinedResults = new List<object>();
            combinedResults.AddRange(sukienResults);
            combinedResults.AddRange(doitacResults);
            combinedResults.AddRange(matbangResults);


            return PartialView("_SearchResults", combinedResults);
        }






    }



}