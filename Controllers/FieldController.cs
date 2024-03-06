/*using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FieldAPI.Data;
using FieldAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FieldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly FieldDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FieldController(FieldDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Field>> GetFields()
        {
            var fields = _context.Fields.ToList();

            // Handle NULL values in FormData column
            foreach (var field in fields)
            {
                if (field.FormData == null)
                {
                    // Handle NULL value here, such as setting it to an empty string
                    field.FormData = "";
                }
            }

            return Ok(fields);
        }


        [HttpGet("{id}")]
        public ActionResult<Field> GetField(int id)
        {
            var field = _context.Fields.Find(id);
            if (field == null)
                return NotFound();
            return Ok(field);
        }

        [HttpPost]
        public ActionResult<Field> CreateField(Field field)
        {
            try
            {
                // Serialize the form data object to JSON string
                field.FormData = JsonConvert.SerializeObject(field.FormData);

                // Add the field to the database
                _context.Fields.Add(field);
                _context.SaveChanges();

                // Save fields to JSON file
                SaveFieldsToJson();

                return CreatedAtAction(nameof(GetField), new { id = field.Id }, field);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while saving the entity changes.");
            }
        }


        [HttpPut("{fieldName}")]
        public IActionResult UpdateField(string fieldName, Field field)
        {
            if (fieldName != field.FieldName)
                return BadRequest();

            var existingField = _context.Fields.FirstOrDefault(f => f.FieldName == fieldName);
            if (existingField == null)
                return NotFound();

            // Update properties of existingField with values from the request body
            existingField.Type = field.Type;
            existingField.Description = field.Description;
            existingField.Required = field.Required;
            existingField.FormData = field.FormData; // Update form data

            _context.SaveChanges();

            return NoContent();
        }





        [HttpDelete("{id}")]
        public IActionResult DeleteField(int id)
        {
            var field = _context.Fields.Find(id);
            if (field == null)
                return NotFound();

            _context.Fields.Remove(field);
            _context.SaveChanges();

            // Save fields to JSON file
            SaveFieldsToJson();

            return NoContent();
        }

        private void SaveFieldsToJson()
        {
            var fields = _context.Fields.ToList();

            // Convert Field objects to JSON
            var fieldsJson = JsonConvert.SerializeObject(fields, Newtonsoft.Json.Formatting.Indented);

            var directoryPath = Path.Combine(_environment.ContentRootPath, "Details");
            var fieldsFilePath = Path.Combine(directoryPath, "fields.json");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Write the JSON file
            System.IO.File.WriteAllText(fieldsFilePath, fieldsJson);
        }
    }
}

*/


// FieldController Class
using Microsoft.AspNetCore.Mvc;
using FieldAPI.Data;
using FieldAPI.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace FieldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {
        private readonly FieldDbContext _context;

        public FieldController(FieldDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Field>> GetFields()
        {
            var fields = _context.Fields.Find(_ => true).ToList();
            return Ok(fields);
        }

        [HttpGet("{id}")]
        public ActionResult<Field> GetField(int id)
        {
            var field = _context.Fields.Find(f => f.Id == id).FirstOrDefault();
            if (field == null)
                return NotFound();
            return Ok(field);
        }

        [HttpPost]
        public ActionResult<Field> CreateField(Field field)
        {
            try
            {
                _context.Fields.InsertOne(field);
                return CreatedAtAction(nameof(GetField), new { id = field.Id }, field);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while saving the entity changes.");
            }
        }

        [HttpPut("{fieldName}")]
        public IActionResult UpdateField(string fieldName, Field field)
        {
            var filter = Builders<Field>.Filter.Eq(f => f.FieldName, fieldName);
            var update = Builders<Field>.Update
                .Set(f => f.Type, field.Type)
                .Set(f => f.Description, field.Description)
                .Set(f => f.Required, field.Required)
                .Set(f => f.FormData, field.FormData); // Update other fields as needed

            var updateResult = _context.Fields.UpdateOne(filter, update);
            if (updateResult.ModifiedCount == 0)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteField(int id)
        {
            var deleteResult = _context.Fields.DeleteOne(f => f.Id == id);
            if (deleteResult.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }
    }
}

