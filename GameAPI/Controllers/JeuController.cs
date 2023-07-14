using GameAPI_Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.Entities_POCO;

namespace GameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JeuController : ControllerBase
    {
        private GameRepo _gameRepo;
        private GenreRepo _genreRepo;
        public JeuController(GameRepo gameRepo, GenreRepo genreRepo)
        {
            _gameRepo = gameRepo;
            _genreRepo = genreRepo;
        }

        private JeuDTO ToDTO(Jeu jeu)
        {
            return new JeuDTO
            {
                Id = jeu.Id,
                Titre = jeu.Titre,
                Description = jeu.Description,
                AnneeSortie = jeu.AnneeSortie,
                Note = jeu.Note,
                Genre = _genreRepo.GetById(jeu.GenreId).Nom
            };
        }

        private Jeu AjoutToPOCO(AjoutJeuDTO jeu)
        {
            return new Jeu
            {
                Titre = jeu.Titre,
                Description = jeu.Description,
                AnneeSortie = jeu.AnneeSortie,
                Note = jeu.Note,
                GenreId = jeu.GenreId
            };
        }

        [HttpGet]
        [Route("listeComplete")]
        public IActionResult GetAll()
        {            
            return Ok(_gameRepo.GetAll().Select(j => ToDTO(j)));
        }

        [HttpPost]
        public IActionResult Create([FromBody] AjoutJeuDTO nouveauJeu)
        {
            _gameRepo.AddGame(AjoutToPOCO(nouveauJeu));
            return Ok();
        }

        [HttpGet]
        [Route("byKeyword/{keyword}")]
        public IActionResult ByKeyword(string keyword)
        {
            return Ok(_gameRepo.GetByKeywork(keyword));
        }
        [HttpGet]
        [Route("byGenre/{genreId:int}")]
        public IActionResult ByGenre(int genreId)
        {
            return Ok(_gameRepo.GetByGenre(genreId));
        }
    }
}
