using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace UFC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UFC_Controller : ControllerBase
    {

        private static List<UFC> fighters = new List<UFC>
        { 
            new UFC {
                Id = 1,
                Name = "The LastStyleBender",
                FirstName = "Israel",
                LastName = "Adesanya",
                Nationality = "Nigerian-New Zealander",
                Height = 193,
                WeightClass = "Middleweight",
                Fav_basketball_player = ""
            },
            new UFC {
                Id = 2,
                Name = "The Predator",
                FirstName = "Francis",
                LastName = "Ngannou",
                Nationality = "Cameroonian",
                Height = 193,
                WeightClass = "Heavyweight",
                Fav_basketball_player = ""
            },
            new UFC {
                Id = 3,
                Name = "The Nigerian Nightmare",
                FirstName = "Kamaru",
                LastName = "Usman",
                Nationality = "Nigerian-American",
                Height = 183,
                WeightClass = "Welterweight",
                Fav_basketball_player = ""
            },
            new UFC {
                Id = 4,
                Name = "The Notorius",
                FirstName = "Conor",
                LastName = "McGregor",
                Nationality = "Irish",
                Height = 175,
                WeightClass = "Lightweight",
                Fav_basketball_player = ""
            }

        };

        //for database
        /*private readonly DataContext _context;
        public UFC_Controller(DataContext context)
        {
            _context = context;
        }*/


        [HttpGet]
        public async Task<ActionResult<List<UFC>>> Get()
        {
            return Ok(fighters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UFC>> Get(int id)     
        {
            //calling the external api and using the data from the api to set a random favourite basketball player to our
            //local api of ufc fighters
            ExternalAPI data = new ExternalAPI(); 
            Random random_number = new Random();
            int player_id = random_number.Next(1,3092);
            string url = $"https://www.balldontlie.io/api/v1/players/{player_id}";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string json = (new WebClient()).DownloadString(url);
            var player_data = System.Text.Json.JsonSerializer.Deserialize<ExternalAPI>(json);
            var fighter = fighters.Find(p => p.Id == id);
            fighter.Fav_basketball_player = player_data.first_name + " " + player_data.last_name;   
            return Ok(fighter);    
        }

        [HttpPost]
        public async Task<ActionResult<List<UFC>>> AddFighter(UFC fighter)
        {
            fighters.Add(fighter);  
            return Ok(fighters);


            //using database
            //await _context.SaveChangesAsync();
            //return Ok(await _context.fighters.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<UFC>>> UpdateFighter(UFC request)
        {
            var fighter = fighters.Find(p => p.Id == request.Id);
            if (fighter == null)
                return BadRequest("Fighter not found.");

            fighter.Name = request.Name;
            fighter.FirstName = request.FirstName;
            fighter.LastName = request.LastName;
            fighter.Nationality = request.Nationality;  
            fighter.Height = request.Height;
            fighter.WeightClass = request.WeightClass;

            return Ok(fighters);
            
            //for database

            /*var ufcfightersdb = await _context.fighters.FindAsync(request.Id);
            if (ufcfightersdb == null)
                return BadRequest("Fighter not found.");

            ufcfightersdb.Name = request.Name;
            ufcfightersdb.FirstName = request.FirstName;
            ufcfightersdb.LastName = request.LastName;
            ufcfightersdb.Nationality = request.Nationality;
            ufcfightersdb.WeightClass = request.WeightClass;
            ufcfightersdb.Height = request.Height;

            await _context.SaveChangesAsync();  

            return Ok(await _context.fighters.ToListAsync());*/
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<UFC>>> Delete(int id)
        {
            var fighter = fighters.Find(h=> h.Id == id);
            if(fighter == null) 
                return BadRequest("Hero Not Found.");

            fighters.Remove(fighter);
            return Ok(fighters);
            
            //for database
            /*var ufcfightersdb = await _context.fighters.FindAsync(id);
            if (ufcfightersdb == null)
                return BadRequest("Fighter not found.");

            _context.fighters.Remove(ufcfightersdb);   
            await _context.SaveChangesAsync();

            return Ok(await _context.fighters.ToListAsync());*/
        }
    }
}
