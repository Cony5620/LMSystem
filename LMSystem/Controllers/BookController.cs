using LMSystem.DAO;
using LMSystem.Models.Entities;
using LMSystem.Models.ViewModels;
using LMSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        private readonly IBookService _bookService;

        public BookController(IBookService bookService, ICategoryService categoryService, IAuthorService authorService, IPublisherService publisherService)
        {
            this._bookService = bookService;
            _categoryService = categoryService;
            _authorService = authorService;
            _publisherService = publisherService;
        }
        public IActionResult Entry()
        {
            BindCategoryData();
            BindAuthorData();
            BindPublisherData();
            return View();
        }

        private void BindPublisherData()
        {
            IEnumerable<PublisherViewModel> Publisher = _publisherService.GetPublishers();
            ViewBag.Publisher = Publisher;
        }

        private void BindAuthorData()
        {
            IEnumerable<AuthorViewModel> Author = _authorService.GetAuthors();
            ViewBag.Author = Author;

        }

        private void BindCategoryData()
        {
            IEnumerable<CategoryViewModel> Category = _categoryService.GetCategories();
            ViewBag.Category = Category;

        }

        [HttpPost]
        public IActionResult Entry(BookViewModel bookViewModel)
        {
            try
            {
               _bookService.Create(bookViewModel);
                ViewData["Info"] = "Successfully save to the system";
                ViewData["status"] = true;
            }
            catch (Exception e)
            {

                ViewData["Info"] = "Errp save to the system" + e.Message;
                ViewData["status"] = false;
            }
            BindCategoryData();
            BindAuthorData();
            BindPublisherData();
            return View();
        }


        public IActionResult List()
        {
            var book = _bookService.GetAll().ToList();
            return View(book);
        }
        public IActionResult Edit(string id)
        {
           var bookView = _bookService.GetById(id);
            BindCategoryData();
            BindAuthorData();
            BindPublisherData();
            return View(bookView);
        }
        [HttpPost]
        public IActionResult Update(BookViewModel bookViewModel)
        {
            try
            {
                _bookService.Update(bookViewModel);

                TempData["Info"] = "Successfully update to the system";
                TempData["status"] = true;
            }
            catch (Exception e)
            {

                TempData["Info"] = "Errp update to the system" + e.Message;
                TempData["status"] = false;
            }
           

            return RedirectToAction("list");
        }
        public IActionResult Delete(string id)
        {
            try
            {
                _bookService.Delete(id);

                TempData["info"] = "Successfully delete to the system";
                TempData["status"] = true;


            }
            catch (Exception e)
            {

                TempData["info"] = "Error occrur when deleting to the system" + e.Message;
                TempData["status"] = false;
            }
            return RedirectToAction("list");
        }
    }
}
