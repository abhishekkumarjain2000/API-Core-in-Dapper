using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

namespace DapperCrudOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SuperHeroController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public async Task<ActionResult<SuperHero>> GetAllSuperHeros()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            //IEnumerable<SuperHero> heros = await connection.QueryAsync<SuperHero>("select * from SuperHeros");
            IEnumerable<SuperHero> heros = await SelectAllHeros(connection);
            return Ok(heros);
        }
        private static async Task<IEnumerable<SuperHero>> SelectAllHeros(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHero>("Select * from superheros");
        }
        [HttpGet("{heroID}")]
        public async Task<ActionResult<SuperHero>> GetHero(int heroID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryAsync<SuperHero>("select * from SuperHeros where ID=@ID", new { ID = heroID });
            return Ok(hero);
        }
        [HttpDelete("{heroID}")]
        public async Task<ActionResult<SuperHero>> DeleteHero(int heroID)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryAsync<SuperHero>("Delete from SuperHeros where ID=@ID", new { ID = heroID });
            return Ok(await SelectAllHeros(connection));
        }
        [HttpPost]
        public async Task<ActionResult<SuperHero>> CreateSupreHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into SuperHeros(Name,FirstName,LastName,Place)values(@Name,@FirstName,@LastName,@Place)", hero);
            return Ok(await SelectAllHeros(connection));
        }
        [HttpPut]
        public async Task<ActionResult<SuperHero>> UpdateSupreHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update SuperHeros set Name=@Name,FirstName=@FirstName,LastName=@LastName,Place=@place where ID=@ID", hero);
            return Ok(await SelectAllHeros(connection));
        }
    }
}
