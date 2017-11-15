using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcMiniGigApp.Domain;
using DisconnectedGenericRepository;
using PagedList;

namespace MvcMiniGigApp.Controllers
{
    public class GigsController : Controller
    {
        private GenericRepository<Gig> gigRepository;
        private GenericRepository<MusicGenre> musicGenreRepository;

        public GigsController(GenericRepository<Gig> _gigRepository,
                                GenericRepository<MusicGenre> _musicGenreRepository)
        {
            gigRepository = _gigRepository;
            musicGenreRepository = _musicGenreRepository;
        }

        // GET: Gigs
        public ActionResult Index(int? page)
        {
            var listUnpaged = gigRepository.AllInclude(g => g.MusicGenre);

            const int pageSize = 10;
            int pageNumber = (page ?? 1);
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
            var gig = gigRepository.FindByInclude(c => c.Id == id.Value, c => c.MusicGenre).FirstOrDefault();
            if (gig == null)
            {
                return HttpNotFound();
            }
            return View(gig);
        }

        // GET: Gigs/Create
        public ActionResult Create()
        {
            ViewBag.MusicGenreId = new SelectList(musicGenreRepository.All(), "Id", "Category");
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
                gigRepository.Insert(gig);
                return RedirectToAction("Index");
            }

            ViewBag.MusicGenreId = new SelectList(musicGenreRepository.All(), "Id", "Category", gig.MusicGenreId);
            return View(gig);
        }

        // GET: Gigs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gig gig = gigRepository.FindByKey(id.Value);
            if (gig == null)
            {
                return HttpNotFound();
            }
            ViewBag.MusicGenreId = new SelectList(musicGenreRepository.All(), "Id", "Category", gig.MusicGenreId);
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
                gigRepository.Update(gig);
                return RedirectToAction("Index");
            }
            ViewBag.MusicGenreId = new SelectList(musicGenreRepository.All(), "Id", "Category", gig.MusicGenreId);
            return View(gig);
        }

        // GET: Gigs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gig = gigRepository.FindByInclude(c => c.Id == id.Value, c => c.MusicGenre).FirstOrDefault();

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
            gigRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}