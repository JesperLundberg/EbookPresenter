using System;
using System.Diagnostics;
using EBookPresenter.Models;
using EBookPresenter.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EBookPresenter.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> Logger { get; }
        private IEBookRepository EBookRepository { get; }
        private IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger,
            IEBookRepository eBookRepository)
        {
            Configuration = configuration;
            Logger = logger;
            EBookRepository = eBookRepository;
        }

        public IActionResult Index()
        {
            //read cookie from Request object  
            var sortOrder = Request.Cookies["SortOrder"];
            
            var folderToRead = Configuration.GetSection("AppSettings").GetSection("FolderToRead").Value;

            var paginationFilter = new PaginationFilter(sortOrder);
            
            var viewModel = new EBookViewModel
            {
                EBooks = EBookRepository.GetAllEbooks(folderToRead, paginationFilter, out var totalItems),
                TotalItems = totalItems,
                SortOrder = sortOrder,
                PreviousPageUrl = GetPreviousPage(paginationFilter),
                NextPageUrl = GetNextPage(paginationFilter, totalItems)
            };

            return View(viewModel);
        }

        public IActionResult Index(EBookViewModel viewModel)
        {
            
            
            return View(viewModel);
        }

        private string GetNextPage(PaginationFilter paginationFilter, int totalItems)
        {
            // TODO: If next page is less than totalItems / pageSize
            // TODO: Return pageNumber + 1 link
            
            return string.Empty;
        }

        private string GetPreviousPage(PaginationFilter paginationFilter)
        {
            if (paginationFilter.PageNumber > 2)00
            {
                return string.Empty;
            }

            return "~/Home/";
        }

        public RedirectToActionResult ToggleSortOrder()
        {
            var sortOrder = Request.Cookies["SortOrder"];

            if (string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("alphabetic"))
            {
                SetCookie("SortOrder", "creation", 365);
            }
            else
            {
                SetCookie("SortOrder", "alphabetic", 365);
            }

            return RedirectToAction("Index");
        }

        private void SetCookie(string key, string value, int? expireTimeDays)
        {
            var option = new CookieOptions();

            if (expireTimeDays.HasValue)
            {
                option.Expires = DateTime.Now.AddDays(expireTimeDays.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddDays(10);
            }

            Response.Cookies.Append(key, value, option);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}