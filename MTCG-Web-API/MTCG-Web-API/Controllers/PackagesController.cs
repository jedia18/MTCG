using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MTCG_Web_API.Authentication;
using MTCG_Web_API.Models.Cards;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MTCG_Web_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly ILogger<PackagesController> _logger;

        public PackagesController(ILogger<PackagesController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Package[] packages)
        {
            try
            {
                using (var conn = new NpgsqlConnection("Server=localhost;Database=MTCG2-DB;Port=5432;User Id=postgres;Password=@Qedipost3@"))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //foreach (var package in packages)
                        //{
                        //    cmd.CommandText = $"INSERT INTO cards (Id, Name, Damage) VALUES ('{package.Id}', '{package.Name}', {package.Damage})";
                        //    await cmd.ExecuteNonQueryAsync();
                        //}

                        int[] cardIds = new int[packages.Length];
                        int i = 0;
                        foreach (var package in packages)
                        {
                            cmd.CommandText = $"INSERT INTO cards (Id, Name, Damage) VALUES ('{package.Id}', '{package.Name}', {package.Damage})";
                            int cardId = Convert.ToInt32(cmd.ExecuteScalar()); //get the idInt of the inserted card
                            cardIds[i++] = cardId;
                        }

                        cmd.CommandText = $"INSERT INTO packages (package_id, card_ids) VALUES (1, '{cardIds}')";
                        await cmd.ExecuteNonQueryAsync();

                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to insert packages.");
                _logger.LogError(ex, "Failed to insert packages.");
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class Package
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Damage { get; set; }
        //public int package_id { get; set; }
    }
}


//[Route("[controller]")]
//[ApiController]
//public class PackagesController : ControllerBase
//{
//    private readonly IConfiguration _configuration;
//    // Dependency injection to get the application path to photos' folder
//    private readonly IWebHostEnvironment _environment;

//    private readonly UserManager<ApplicationUser> userManager;

//    //, UserManager<ApplicationUser> userManager
//    public PackagesController(IConfiguration configuration, IWebHostEnvironment environment)
//    {
//        _configuration = configuration;
//        _environment = environment;
//    }

//    [HttpPost]
//    [Obsolete]
//    public JsonResult Post(MonsterCard card)
//    {
//        string query = @"
//            insert into cards(id,name,damage,element_type)
//            values              (@Id,@Name,@Damage,@Element_Type)
//        ";

//        // Getting data into the data table object 
//        DataTable table = new DataTable();
//        string sqlDataSource = _configuration.GetConnectionString("ConnString");

//        NpgsqlDataReader myReader;

//        using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
//        {
//            myCon.Open();
//            using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
//            {
//                myCommand.Parameters.AddWithValue("@Id", card.Id);
//                myCommand.Parameters.AddWithValue("@Name", card.Name);
//                myCommand.Parameters.AddWithValue("@Damage", card.Damage);
//                myCommand.Parameters.AddWithValue("@Element_Type", card.Element);

//                try
//                {
//                    myReader = myCommand.ExecuteReader();
//                }
//                catch (Exception ex)
//                {
//                    if (ex.Message == "23505: doppelter Schlüsselwert verletzt Unique-Constraint »users_pkey«")
//                    {
//                        return new JsonResult("User is already exist!");
//                    }
//                    else
//                    {
//                        return new JsonResult(ex);
//                    }

//                }
//                table.Load(myReader);

//                myReader.Close();
//                myCon.Close();
//            }
//        }

//        return new JsonResult("Added Successfully");
//    }

//}










//[HttpPost]
//public async Task<IActionResult> Post([FromBody] Package[] packages)
//{
//    try
//    {
//        int packageIdCounter = 0;
//        using (var conn = new NpgsqlConnection("Server=localhost;Database=MTCG1-DB;Port=5432;User Id=postgres;Password=@Qedipost3@"))
//        {
//            await conn.OpenAsync();
//            using (var cmd = new NpgsqlCommand())
//            {
//                cmd.Connection = conn;
//                foreach (var package in packages)
//                {
//                    if (packageIdCounter % 5 == 0)
//                    {
//                        packageIdCounter++;
//                    }
//                    package.package_id = packageIdCounter;
//                    cmd.CommandText = $"INSERT INTO packagess (Id, Name, Damage, package_id) VALUES ('{package.Id}', '{package.Name}', {package.Damage}, {package.package_id})";
//                    await cmd.ExecuteNonQueryAsync();
//                }
//            }
//        }
//        return Ok();
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError(ex, "Failed to insert packages.");
//        return StatusCode(500, ex.Message);
//    }
//}




