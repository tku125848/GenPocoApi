using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Text.RegularExpressions;
using WebApplication1.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        private IConfiguration _configuration;
        public JsonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: api/<JsonController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<JsonController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JsonController>
        [HttpPost]
        public IActionResult Post([FromForm] string cn, [FromForm] string sp, [FromForm] string cls)
        {

            string apikeyname = "";
            string apikey = "";

            string pattern = @"^[A-Za-z0-9_]+$";

            // 使用正則表達式進行驗證
            Match match = Regex.Match(sp, pattern);

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
            if (match.Success && flg)
            {
                try
                {
                    using (var connection = new SqlConnection(connstr))
                    {
                        //res = connection.Gene1rateClass($"exec {sp};", className: cls);
                        var result = connection.Query(sp, null, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        res = JsonConvert.SerializeObject(result, Formatting.Indented);
                    }
                } catch(Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            } else
            {
                return BadRequest();
            }
            return Content($"{res}");
        }

        // PUT api/<JsonController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JsonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
