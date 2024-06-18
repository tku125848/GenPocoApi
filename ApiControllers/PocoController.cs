using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using WebApplication1.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PocoController : ControllerBase
    {
        private IConfiguration _configuration;
        public PocoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<PocoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PocoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"{id}";
        }


        // POST api/<PocoController>
        // Change: sp to sql 
        [HttpPost]
        public IActionResult Post([FromForm] string cn, [FromForm] string sql, [FromForm] string cls)
        {

            //string apikeyname = "";
            //string apikey = "";

            //string pattern = @"^[A-Za-z0-9_]+$";

            //// 使用正則表達式進行驗證
            //Match match = Regex.Match(sp, pattern);

            AppSettings settings = new AppSettings(_configuration);
            bool flg = false;
            string k = "";
            string v = string.Empty;
            foreach (KeyValuePair<string, string> item in settings.apikeyvalues)
            {
                //Response.Write(string.Format("{0} : {1}<br/" + ">", item.Key, item.Value));
                k = item.Key;
                if (Request.Headers.ContainsKey(k))
                {
                    v = Request.Headers[k].ToString();
                    if (v.Equals(item.Value))
                    {
                        flg = true;
                        break;
                    }
                }
            }

            string connstr = _configuration.GetConnectionString(cn);
            string res = "";
            if (flg)
            {
                using (var connection = new SqlConnection(connstr))
                {
                    res = connection.GenerateClass($"{sql};", className: cls);
                }
            } else
            {
                return BadRequest();
            }
            return Content($"{res}");
        }


    }
}
