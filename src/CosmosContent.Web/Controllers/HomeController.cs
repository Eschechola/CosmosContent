using CosmosContent.Data.Entities;
using CosmosContent.Data.Interfaces;
using CosmosContent.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CosmosContent.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICosmosService _cosmosService;

        public HomeController(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            IndexViewModel viewModel = null;
            try
            {
                var contents = _cosmosService.GetAll();
                
                viewModel = new IndexViewModel
                {
                    Contents = contents
                };
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu algum erro interno na aplicação, por favor tente novamente";
            }

            return View(viewModel);
        }

        [Route("/criar-conteudo")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/criar-conteudo")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] CreateViewModel form)
        {
            try
            {
                var briefingObject = new
                {
                    Theme = form.Theme,
                    BusinessArea = form.BusinessArea,
                    Observations = form.Observations
                };

                var content = new Content(briefingObject);

                _cosmosService.Create(content);

                TempData["Message"] = "Conteúdo criado com sucesso!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu algum erro interno na aplicação, por favor tente novamente";
            }

            return Redirect(Url.Content("~/"));
        }

        [Route("/deletar-conteudo/{id}")]
        public IActionResult Delete(string id)
        {
            DeleteViewModel viewModel = null;

            try
            {
                var content = _cosmosService.Get(id);
                viewModel = new DeleteViewModel
                {
                    Id = content.Id.ToString(),
                    BusinessArea = content.GetBriefingProperty("BusinessArea"),
                    Observations = content.GetBriefingProperty("Observations"),
                    Theme = content.GetBriefingProperty("Theme"),
                };
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu algum erro interno na aplicação, por favor tente novamente";
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("/deletar-conteudo")]
        public IActionResult Delete([FromForm]string id, bool confirm)
        {
            try
            {
                var content = _cosmosService.Get(id);
                _cosmosService.Remove(content);

                TempData["Message"] = "Conteúdo deletado com sucesso!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu algum erro interno na aplicação, por favor tente novamente";
            }

            return Redirect(Url.Content("~/"));
        }

        [Route("/alterar-conteudo/{id}")]
        public IActionResult Update(string id)
        {
            UpdateViewModel viewModel = null;

            try
            {
                var content = _cosmosService.Get(id);
                viewModel = new UpdateViewModel
                {
                    Id = content.Id.ToString(),
                    BusinessArea = content.GetBriefingProperty("BusinessArea"),
                    Observations = content.GetBriefingProperty("Observations"),
                    Theme = content.GetBriefingProperty("Theme"),
                };
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu algum erro interno na aplicação, por favor tente novamente";
            }

            return View(viewModel);
        }

        [Route("/alterar-conteudo")]
        public IActionResult Update([FromForm] UpdateViewModel form)
        {
            try
            {
                var newBriefing = new
                {
                    Theme = form.Theme,
                    BusinessArea = form.BusinessArea,
                    Observations = form.Observations
                };

                var content = _cosmosService.Get(form.Id);
                content.UpdateBriefing(newBriefing);

                _cosmosService.Update(content);

                TempData["Message"] = "Conteúdo alterado com sucesso!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu algum erro interno na aplicação, por favor tente novamente";
            }

            return Redirect(Url.Content("~/"));
        }
    }
}
