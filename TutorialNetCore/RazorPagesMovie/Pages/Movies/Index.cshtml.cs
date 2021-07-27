using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        [BindProperty(SupportsGet = true )]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Rating { get; set; }
        public SelectList Ratings { get; set; }
        public async Task OnGetAsync()
        {
            //Busca os FIlmes no banco
            var movies = from m in _context.Movie select m;
            
            //Busca os Generos no banco
            IQueryable<string> genreQuery = from q in _context.Movie orderby q.Genre select q.Genre;

            //Popula o Select que vai ficar disponível na View Index - Sem repetir os Generos
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

            // Classificações
            IQueryable<string> ratings = from r in _context.Movie orderby r.Rating select r.Rating;

            Ratings = new SelectList(await ratings.Distinct().ToArrayAsync());

           


            //Verifica se os parametros de filtro estão preenchidos e executa a consulta no banco de dados
            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(t => t.Title.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(q => q.Genre == MovieGenre);
            }
            if (!string.IsNullOrEmpty(Rating))
            {
                movies = movies.Where(r => r.Rating == Rating);
            }
           
            
            Movie = await movies.ToListAsync();
        }
    }
}
