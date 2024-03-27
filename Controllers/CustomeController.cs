using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTTTM.models;
using System.Linq; // Đảm bảo bạn đã thêm using này
using System.Security.Claims;
using QLTTTM.Datas;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using DAPM.Interfaces;
using DAPM.StrategyConcreteClasses;


namespace DAPM.Controllers
{
    public class CustomeController : Controller
    {
        private DataSQLContext dataSQLContext;
        private readonly IAuthenticationStrategy _googleAuthenticationStrategy;
        private readonly IAuthenticationStrategy _facebookAuthenticationStrategy;

        public CustomeController()
        {
            _googleAuthenticationStrategy = new GoogleAuthenticationStrategy();
            _facebookAuthenticationStrategy = new FacebookAuthenticationStrategy();
            dataSQLContext = SingletonDbContext.Instance;
        }

        public async Task GoogleLogin(string returnUrl)
        {
            var redirectUri = Url.Action("GoogleLoginCallback");
            if (redirectUri != null)
            {
                await _googleAuthenticationStrategy.ChallengeAsync(HttpContext, redirectUri);
            }
        }

        public async Task FacebookLogin(string returnUrl)
        {
            await _facebookAuthenticationStrategy.ChallengeAsync(HttpContext, returnUrl);
        }

        public async Task<IActionResult> GoogleLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var identity = result.Principal.Identities.FirstOrDefault();
            if (identity != null)
            {
                var claims = identity.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

                if (claims.Any())
                {
                    return RedirectToAction("HomeUser");
                }
            }

            return RedirectToAction("LoginUser");
        }

        public IActionResult LoginUser()
        {
            return View();
        }

        public IActionResult GoogleRedirect()
        {
            var claims = User.Claims.ToList();
            var emailClaim = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email));
            var nameClaim = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name));

            if (emailClaim != null && nameClaim != null)
            {

                return RedirectToAction("HomeUser", "Custome");
            }

            return RedirectToAction("HomeUser");
        }

        public IActionResult HomeUser()
        {
            return View();
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

                ViewBag.tenloadt = loaiDoiTac?.TENLOAI;

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