using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProfiGroup.BusinessLogic;
using TestProfiGroup.DomainModel;
using TestProfiGroup.Web.Models;

namespace TestProfiGroup.Web.Controllers
{
    public class NotesController : Controller
    {
        private readonly INotesService _notesService;
        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = View("Error");

            base.OnException(filterContext);
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = _notesService.GetAllLocal().Select(FromDomainModel).ToList();
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IsNew = true;
            return View("Create");
        }
        
        [HttpPost]
        public ActionResult Create(NoteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = ToDomainModel(model);
                    _notesService.CreateLocal(obj);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.IsNew = true;
                    return View("Create", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("general", ex.Message);
                ViewBag.IsNew = true;
                return View("Create", model);
            }
        }

        // GET: Notes/Edit/5
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var obj = _notesService.GetLocal(id);
            if (obj.IsDeleted)
                return RedirectToAction("Index");

            var model = FromDomainModel(obj);
            ViewBag.IsNew = false;
            return View("Edit", model);
        }

        // POST: Notes/Edit/5
        [HttpPost]
        public ActionResult Edit(NoteViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = ToDomainModel(model);
                    _notesService.UpdateLocal(obj);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.IsNew = false;
                    return View("Edit", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("general", ex.Message);
                ViewBag.IsNew = false;
                return View("Edit", model);
            }
        }
        
        // POST: Notes/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            _notesService.DeleteLocal(id);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Synchronize()
        {
            _notesService.SynchronizeAll();
            return View("SyncSuccessful");
        }

        private static Note ToDomainModel(NoteViewModel viewModel)
        {
            return new Note
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Content = viewModel.Content
            };
        }

        private static NoteViewModel FromDomainModel(Note model)
        {
            return new NoteViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content
            };
        }
    }
}
