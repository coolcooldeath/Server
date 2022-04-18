using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select ""Question.Question"",
                 ""Question.QuestionId"",
                 ""QuestionOptions.Option1"",
                 ""QuestionOptions.Option2"",
                 ""QuestionOptions.Option3"",
                 ""QuestionOptions.Option4"",
                 ""QuestionOptions.Answer""
                from ""Question"" join ""QuestionOptions"" ON ""Question.QuestionId"" = ""QuestionOptions.QuestionId""
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(QuestionObject dep)
        {
            string query = @"
                insert into Department(DepartmentName)
                values (@DepartmentName)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.Question);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(QuestionObject dep)
        {
            string query = @"
                update Department
                set DepartmentName = @DepartmentName
                where DepartmentId=@DepartmentId 
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                   /* myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);*/
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                delete from Department
                where DepartmentId=@DepartmentId 
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
