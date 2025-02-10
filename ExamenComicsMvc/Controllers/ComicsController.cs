using ExamenComicsMvc.Models;
using ExamenComicsMvc.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExamenComicsMvc.Controllers
{
    public class ComicsController : Controller
    {
        RepositoryComics repo;
        public ComicsController()
        {
            this.repo = new RepositoryComics();
        }
        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create
            (Comic comic)
        {
            this.repo.CreateComic(comic);
            return RedirectToAction("Index");
        }
        public IActionResult Details()
        {
            ViewData["COMICS"] = this.repo.GetComics();
            return View();
        }
        [HttpPost]
        public IActionResult Details
            (int idComic)
        {
            ViewData["COMICS"] = this.repo.GetComics();
            Comic comic = this.repo.FindComic(idComic);
            return View(comic);
        }
    }
}
