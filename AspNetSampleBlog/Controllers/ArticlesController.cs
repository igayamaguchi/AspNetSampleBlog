﻿using AspNetSampleBlog.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetSampleBlog.Models;
using AspNetSampleBlog.Repositories;
using AspNetSampleBlog.ViewModels;

namespace AspNetSampleBlog.Controllers
{
    public class ArticlesController : Controller
    {
        private MvcBasicContext db = new MvcBasicContext();
        private IArticleRepository articleRepository;
        private ITagRepository tagRepository;


        public ArticlesController() : this(new ArticleRepository(), new TagRepository()) { }

        public ArticlesController(IArticleRepository articleRepository, ITagRepository tagRepository)
        {
            this.articleRepository = articleRepository;
            this.tagRepository = tagRepository;
        }

        // GET: Articles
        public ActionResult Index()
        {
            return View(articleRepository.All());
        }

        public ActionResult Year(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = articleRepository.FindByYear((int)id);

            if (article == null || article.Count() == 0)
            {
                return HttpNotFound();
            }

            return View("~/Views/Home/Index.cshtml", new HomeViewModel { Articles = article, Tags = tagRepository.All(), Years = articleRepository.YearList() });
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleRepository.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,ImagePath,Created")] Article article)
        {
            if (ModelState.IsValid)
            {
                articleRepository.Save(article);
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleRepository.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,ImagePath,Created")] Article article)
        {
            if (ModelState.IsValid)
            {
                articleRepository.Update(article);
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = articleRepository.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = articleRepository.Find(id);
            articleRepository.Delete(article);
            return RedirectToAction("Index");
        }

        // TODO: Move to repository
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
