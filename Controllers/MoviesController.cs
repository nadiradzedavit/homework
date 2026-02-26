using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker.Data;
using MovieTracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTracker.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string genre, string platform, string status, 
            int? minYear, int? maxYear, 
            decimal? minRating, decimal? maxRating, 
            string language, string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["DirectorSortParm"] = sortOrder == "Director" ? "director_desc" : "Director";
            ViewData["YearSortParm"] = sortOrder == "Year" ? "year_desc" : "Year";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";
            ViewData["DurationSortParm"] = sortOrder == "Duration" ? "duration_desc" : "Duration";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var movies = from m in _context.Movies
                         select m;

            if (!String.IsNullOrEmpty(genre)) movies = movies.Where(s => s.Genre.Contains(genre));
            if (!String.IsNullOrEmpty(platform)) movies = movies.Where(s => s.Platform.Contains(platform));
            if (!String.IsNullOrEmpty(status)) movies = movies.Where(s => s.WatchStatus == status);
            if (minYear.HasValue) movies = movies.Where(s => s.ReleaseYear >= minYear.Value);
            if (maxYear.HasValue) movies = movies.Where(s => s.ReleaseYear <= maxYear.Value);
            if (minRating.HasValue) movies = movies.Where(s => s.Rating >= minRating.Value);
            if (maxRating.HasValue) movies = movies.Where(s => s.Rating <= maxRating.Value);
            if (!String.IsNullOrEmpty(language)) movies = movies.Where(s => s.Language.Contains(language));

            movies = sortOrder switch
            {
                "title_desc" => movies.OrderByDescending(s => s.Title),
                "Director" => movies.OrderBy(s => s.Director),
                "director_desc" => movies.OrderByDescending(s => s.Director),
                "Year" => movies.OrderBy(s => s.ReleaseYear),
                "year_desc" => movies.OrderByDescending(s => s.ReleaseYear),
                "Rating" => movies.OrderBy(s => s.Rating),
                "rating_desc" => movies.OrderByDescending(s => s.Rating),
                "Duration" => movies.OrderBy(s => s.Duration),
                "duration_desc" => movies.OrderByDescending(s => s.Duration),
                "Date" => movies.OrderBy(s => s.WatchedDate),
                "date_desc" => movies.OrderByDescending(s => s.WatchedDate),
                _ => movies.OrderBy(s => s.Title),
            };

            int pageSize = 10;
            return View(await PaginatedList<Movie>.CreateAsync(movies.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Director,Genre,ReleaseYear,Rating,WatchStatus,Platform,Duration,Language,WatchedDate,Notes")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Director,Genre,ReleaseYear,Rating,WatchStatus,Platform,Duration,Language,WatchedDate,Notes")] Movie movie)
        {
            if (id != movie.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
