﻿using Earbook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Neptuo;
using Microsoft.Extensions.Options;
using Earbook.Models.Repositories;
using Earbook.Models.Options;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Earbook.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Storage storage;
        private readonly DataContext repository;

        public HomeController(IOptions<Storage> storage, DataContext repository)
        {
            Ensure.NotNull(storage, "storage");
            Ensure.NotNull(repository, "repository");
            this.storage = storage.Value;
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            QuizModel model = await repository.EnsurePendingQuizAsync(User.Identity.Name);
            await repository.SaveChangesAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Guid answer)
        {
            if (answer != Guid.Empty)
            {
                QuizModel model = await repository.FindPendingQuizAsync(User.Identity.Name);
                if (model != null)
                {
                    model.IsSuccess = model.Answer.Id == answer;
                    await repository.SaveChangesAsync();
                }
            }

            return await Index();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("nove-ousko")]
        public IActionResult NewEar() => View();

        [HttpPost("nove-ousko")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewEar([Required] string name, [Required] IFormFile picture)
        {
            Ensure.NotNull(name, "name");
            Ensure.NotNull(picture, "picture");

            string fileExtension = Path.GetExtension(picture.FileName);
            if (picture.Length > storage.MaxLength || !storage.SupportedExtensions.Contains(fileExtension))
            {
                ViewData["Message"] = "Nahraný soubor neodpovídá normám.";
                ViewData["MessageType"] = "danger";

                return View();
            }

            if (await repository.IsExistingEarAsync(name))
            {
                ViewData["Message"] = "Bohužel takto pojmenované ouško již máme.";
                ViewData["MessageType"] = "danger";

                return View();
            }

            AccountModel owner = await repository.FindAccountAsync(User.Identity.Name);
            Guid earId = await repository.AddEarAsync(owner, name);

            string fileName = earId.ToString() + fileExtension;
            string filePath = Path.Combine(storage.Path, fileName);
            using (Stream fileContent = new FileStream(filePath, FileMode.OpenOrCreate))
            using (Stream content = picture.OpenReadStream())
                await content.CopyToAsync(fileContent);

            await repository.SetEarFileNameAsync(earId, fileName);
            await repository.SaveChangesAsync();

            ViewData["Message"] = "Nové ouško úspěšně nahráno!";
            ViewData["MessageType"] = "success";
            return View();
        }

        [HttpGet("ear-picture/{filename}")]
        public async Task<IActionResult> EarPicture(string filename)
        {
            if (await repository.Ears.AnyAsync(e => e.FileName == filename))
                return File(System.IO.File.OpenRead(Path.Combine(storage.Path, filename)), "image/jpg");

            return NotFound();
        }
    }
}
