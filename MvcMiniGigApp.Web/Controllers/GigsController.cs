namespace MvcMiniGigApp.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using MvcMiniGigApp.Domain;    
    using MvcMiniGigApp.Services;
    using PagedList;

    public class GigsController : Controller
    {
        private IGigService gigService;

        public GigsController(IGigService _gigService)
        {
            this.gigService = _gigService;
        }

        // GET: Gigs
        public ActionResult Index(int? page)
        {
            var listUnpaged = this.gigService.GetGigs();

            const int pageSize = 5;

            int pageNumber = page ?? 1;

            var listPaged = listUnpaged.ToPagedList(pageNumber, pageSize);

            return View(listPaged);
        }

        // GET: Gigs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gig = this.gigService.GetGig(id.Value);

            if (gig == null)
            {
                return HttpNotFound();
            }

            return View(gig);
        }

        // GET: Gigs/Create
        public ActionResult Create()
        {
            ViewBag.MusicGenreId = new SelectList(this.gigService.GetMusicGenres(), "Id", "Category");

            return View();
        }

        // POST: Gigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,GigDate,MusicGenreId")] Gig gig)
        {
            if (ModelState.IsValid)
            {
                this.gigService.CreateGig(gig);

                return RedirectToAction("Index");
            }

            ViewBag.MusicGenreId = new SelectList(this.gigService.GetMusicGenres(), "Id", "Category", gig.MusicGenreId);

            return View(gig);
        }

        // GET: Gigs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gig = this.gigService.GetGig(id.Value);

            if (gig == null)
            {
                return HttpNotFound();
            }

            ViewBag.MusicGenreId = new SelectList(this.gigService.GetMusicGenres(), "Id", "Category", gig.MusicGenreId);

            return View(gig);
        }

        // POST: Gigs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,GigDate,MusicGenreId")] Gig gig)
        {
            if (ModelState.IsValid)
            {
                this.gigService.UpdateGig(gig);

                return RedirectToAction("Index");
            }

            ViewBag.MusicGenreId = new SelectList(this.gigService.GetMusicGenres(), "Id", "Category", gig.MusicGenreId);

            return View(gig);
        }

        // GET: Gigs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gig = this.gigService.GetGig(id.Value);

            if (gig == null)
            {
                return HttpNotFound();
            }

            return View(gig);
        }

        // POST: Gigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.gigService.DeleteGig(id);

            return RedirectToAction("Index");
        }
    }
}