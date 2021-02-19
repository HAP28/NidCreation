using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public BlogController(IConfiguration configuration,IWebHostEnvironment environment)
        {
            this._configuration = configuration;
            this._environment = environment;
        }

        // GET: api/<BlogController>
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.Blog";
            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("BlogConnection");
            SqlDataReader dataReader;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }

        // GET: api/<BlogController>/5
        [HttpGet("{id}")]
        public JsonResult GetBlog(int id)
        {
            string query = @"select * from dbo.Blog where 
                BlogId = '" + id + "'";

            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("BlogConnection");
            SqlDataReader dataReader;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }

        // POST api/<BlogController>
        [HttpPost]
        public JsonResult Post(Blog data)
        {
            string query = @"insert into dbo.Blog (BlogTitle,BlogDescription,BlogImageName) values
                ('" + data.Title + "','" + data.Description + "','" + data.Image + "')";

            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("BlogConnection");
            SqlDataReader dataReader;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    con.Close();
                }
            }
            return new JsonResult("1 Row Added");
        }

        // PUT api/<BlogController>/5
        [HttpPut("{id}")]
        public JsonResult Put(int id, Blog data)
        {
            string query = @"update dbo.Blog set
                BlogTitle = '" + data.Title + "'," +
                "BlogDescription = '" + data.Description + "'," +
                "BlogImageName = '" + data.Image + "' where BlogId = '" + id + "'";

            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("BlogConnection");
            SqlDataReader dataReader;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    con.Close();
                }
            }
            return new JsonResult("1 Row Updated");
        }

        // DELETE api/<BlogController>/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
             string query = @"delete from dbo.Blog where
                BlogId = '" + id + "'";

            DataTable table = new DataTable();
            string dataSource = _configuration.GetConnectionString("BlogConnection");
            SqlDataReader dataReader;
            using (SqlConnection con = new SqlConnection(dataSource))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    dataReader = command.ExecuteReader();
                    table.Load(dataReader);

                    dataReader.Close();
                    con.Close();
                }
            }
            return new JsonResult("1 Row Deleted");
        }
        // DELETE api/<BlogController>/SaveImage
        [Route("SaveImage")]
        [HttpPost]
        public JsonResult SaveImage()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _environment.ContentRootPath + "/Photos/"+fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch(Exception)
            {
                return new JsonResult("default.png");
            }
        }
    }
}
