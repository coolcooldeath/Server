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
    public class QuestionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public QuestionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT ""Question"",""QuestionId"",""Answer"", ""Option1"", ""Option2"", ""Option3"", ""Option4""
                from ""Question""; 
            ";

           /* string query = @"
                select ""Question"",
                ""QuestionId""
                   from ""Question"" 
                    
            ";*/

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


        public bool isQuestionInTableName(QuestionObject cat)
        {
            string query = @"
                SELECT ""QuestionId"",""Question""
                from ""Question""
                where ""Question"".""Question"" = @Question; 
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Question", cat.Question);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();

                }
            }


            if (table.Rows.Count != 0)
            {
                return true;
            }
            else
                return false;


        }

        [HttpPost]
        public JsonResult Post(QuestionObject question)
        {


            string query = @"
                insert into ""Question"" (""Question"",""CategoryId"",""QuestionId"",""Answer"",""Option1"",""Option2"",""Option3"",""Option4"") 
                values (@Question,@CategoryId,@QuestionId,@Answer,@Option1,@Option2,@Option3,@Option4) 
            ";


            if (isQuestionInTableName(question)) { return new JsonResult("Такой вопрос уже есть!"); }
            else
            {
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
                NpgsqlDataReader myReader;
                using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Question", question.Question);
                        myCommand.Parameters.AddWithValue("@QuestionId", question.QuestionId);
                        myCommand.Parameters.AddWithValue("@CategoryId", question.CategoryId);
                        myCommand.Parameters.AddWithValue("@Answer", question.Answer);
                        myCommand.Parameters.AddWithValue("@Option1", question.Option1);
                        myCommand.Parameters.AddWithValue("@Option2", question.Option2);
                        myCommand.Parameters.AddWithValue("@Option3", question.Option3);
                        myCommand.Parameters.AddWithValue("@Option4", question.Option4);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();

                    }
                }

                return new JsonResult("Added Successfully");
            }
        }

        [HttpPut]
        public JsonResult Put(QuestionObject question)
        {
            string query = @"
                update ""Question""
                set ""Question"" = @Question,
                ""CategoryId"" = @CategoryId,
                ""Answer"" = @Answer,
                ""Option1"" = @Option1,
                ""Option2"" = @Option2,
                ""Option3"" = @Option3,
                ""Option4"" = @Option4
                where ""QuestionId""=@QuestionId 
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@QuestionId", question.QuestionId);
                    myCommand.Parameters.AddWithValue("@Question", question.Question);
                    myCommand.Parameters.AddWithValue("@CategoryId", question.CategoryId);
                    myCommand.Parameters.AddWithValue("@Answer", question.Answer);
                    myCommand.Parameters.AddWithValue("@Option1", question.Option1);
                    myCommand.Parameters.AddWithValue("@Option2", question.Option2);
                    myCommand.Parameters.AddWithValue("@Option3", question.Option3);
                    myCommand.Parameters.AddWithValue("@Option4", question.Option4);
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
                delete from ""Question""
                where ""QuestionId""=@QuestionId 
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("QuizAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@QuestionId", id);
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
