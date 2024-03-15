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


        public IActionResult HomeUser()
        {
            var data = dataSQLContext;
            return View(data);
        }

        public IActionResult Brand(int maloai)
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

        public IActionResult Spotlight()
        {
            List<SuKien> spotlight = dataSQLContext.SuKiens.Where(x => x.LOAISK == "TD" && x.TRANGTHAI == true).OrderByDescending(x => x.MASK).ToList();
            return View(spotlight);
        }

        public IActionResult Offers()
        {
            List<SuKien> offers = dataSQLContext.SuKiens.Where(x => x.LOAISK == "UD" && x.TRANGTHAI == true).OrderByDescending(x => x.MASK).ToList();
            return View(offers);
        }

        [HttpGet]
        public IActionResult PremisesUser()
        {
            List<MatBang> list_premises_user = dataSQLContext.MatBangs.ToList();
            return View(list_premises_user);
        }

        [HttpGet]
        public IActionResult PremisesUserDetail(int? mamb)
        {
            if (mamb != null)
            {
                MatBang? premises = dataSQLContext.MatBangs.SingleOrDefault(x => x.MAMB == mamb);
                if (premises != null)
                {
                    return View(premises);
                }


            }
            return RedirectToAction("MatBang", "Custome");
        }
        [HttpGet]
        public IActionResult EventDetail(int? mask)
        {   
            if (mask != null)
            {
                SuKien? suKien = dataSQLContext.SuKiens.SingleOrDefault(x => x.MASK == mask);
                if (suKien != null)
                {
                    return View(suKien);
                }
            }
                return RedirectToAction("EventDetail", "Custome");
        }
        [HttpGet]
        public IActionResult Search(string query)
        {
            query = query.ToLower().Trim();


            // Tìm kiếm sự kiện
            var eventResults = dataSQLContext.SuKiens
                .Where(e =>
                            EF.Functions.Like(e.TENSK.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.LOAISK.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.THOIGIAN.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.VITRI.ToLower().Trim(), $"%{query}%") ||
                            EF.Functions.Like(e.MOTA.ToLower().Trim(), $"%{query}%")
                            )
                .ToList();

            // Tìm kiếm đối tác
            var partnerResults = dataSQLContext.DoiTacs
                .Where(p =>
                            EF.Functions.Like(p.TENDT.ToLower().Trim(), $"%{query}%")
                )
                .ToList();


            // Thực hiện tìm kiếm gần đúng sử dụng Entity Framework Core
            var premisesResults = dataSQLContext.MatBangs
                .Where(item =>
                                EF.Functions.Like(item.TENMB.ToLower().Trim(), $"%{query}%") ||
                                EF.Functions.Like(item.VITRI.ToLower().Trim(), $"%{query}%") ||
                                EF.Functions.Like(item.MOTA.ToLower().Trim(), $"%{query}%")
                )
                .ToList();




            // Kết hợp kết quả từ cả ba loại :
            var combinedResults = new List<object>();
            combinedResults.AddRange(eventResults);
            combinedResults.AddRange(partnerResults);
            combinedResults.AddRange(premisesResults);


            return PartialView("_SearchResults", combinedResults);
        }






    }



}