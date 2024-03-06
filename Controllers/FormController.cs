/*using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FieldAPI.Data; // Assuming FormModel is defined in this namespace

namespace FieldAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly FieldDbContext _context;

        public FormController(FieldDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveFormData([FromBody] FormModel formData)
        {
            try
            {
                // Assuming you have a DbSet<FormModel> in FieldDbContext named Forms
                _context.FormModels.Add(formData);
                await _context.SaveChangesAsync();
                return Ok(formData.Id); // Return the saved Id
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving form data: {ex.Message}");
            }
        }
    }
}


*/
// FormController.cs
using Microsoft.AspNetCore.Mvc;
using FieldAPI.Data;
using FieldAPI.Models;
using System;
using System.Threading.Tasks;

namespace FieldAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly FieldDbContext _context;

        public FormController(FieldDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SaveFormData([FromBody] FormModel formData)
        {
            try
            {
                // Check if the form data is null
                if (formData == null)
                {
                    return BadRequest("Form data cannot be null.");
                }

                // Save the form data to the database
                await _context.Fields.InsertOneAsync(formData); // Use appropriate method provided by MongoDB driver
                // Alternatively, you can use InsertManyAsync if you're inserting multiple documents

                return Ok("Form data saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving form data: {ex.Message}");
            }
        }
    }
}

