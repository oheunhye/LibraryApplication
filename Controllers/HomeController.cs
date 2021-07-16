using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryApplication.Context;
using LibraryApplication.Models;

namespace LibraryApplication.Controllers
{
    public class HomeController : Controller
    {
        //DB인스턴스화
        private LibraryDb db = new LibraryDb();

        // GET: Home
        public ActionResult Index()
        {
            List<Book> books = new List<Book>();

            #region Paging&Searching
            int totalCount = 0;
            int maxListCount = 3;
            int pageNum = 1;

            //매개변수page null체크
            if (Request.QueryString["page"] != null)
                pageNum = Convert.ToInt32(Request.QueryString["page"]);

            //검색키워드
            string strKeyword = Request.QueryString["keyword"] ?? string.Empty;
            string strSearchKind = Request.QueryString["searchKind"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(strKeyword))
            {
                //결과Row를 maxListCount만큼 가져옴
                books = db.Books.OrderBy(x => x.Book_U)
                                            .Skip((pageNum - 1) * maxListCount)
                                            .Take(maxListCount).ToList();

                totalCount = db.Books.Count();
            }
            else
            {
                switch (strSearchKind)
                {
                    case "Title":
                        books = db.Books.Where(x => x.Title.Contains(strKeyword)).ToList();
                            break;
                    case "Writer":
                        books = db.Books.Where(x => x.Writer.Contains(strKeyword)).ToList();
                        break;
                    case "Publisher":
                        books = db.Books.Where(x => x.Publisher.Contains(strKeyword)).ToList();
                        break;
                    default:
                        break;
                }

                books = books.OrderBy(x => x.Book_U)
                                        .Skip((pageNum - 1) * maxListCount)
                                        .Take(maxListCount).ToList();

                totalCount = books.Count();
            }


            //데이터전달
            ViewBag.Page = pageNum;
            ViewBag.TotalCount = totalCount;
            ViewBag.MaxListCount = maxListCount;
            ViewBag.SearchKind = strSearchKind;

            #endregion END Paging&Searching

            //쿼리실행과 같은 결과를 View로 보냄
            return View(books);
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하세요. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하세요.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Book_U,Title,Writer,Summary,Publisher,Published_date")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Home/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하세요. 
        // 자세한 내용은 https://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하세요.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Book_U,Title,Writer,Summary,Publisher,Published_date")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
