using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.DataAccess;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.ProductDtos;
using System.Linq.Expressions;

namespace MyApi5.API.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }
        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Id 0 -dan boyuk olmalidir." });
            }
            try
            {
                var product = await _service.GetById(id);
                return Ok(product);
            }
            catch (ElementNotExistException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (name == null)
            {
                return BadRequest(new { message = "Name can not be empty or null" });
            }
            try
            {
                var product = await _service.GetByName(name);
                return Ok(product);

            }
            catch (ElementNotExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _service.GetAll(includes:"Category");
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductPostDto postDto)
        {
            try
            {
                await _service.Add(postDto);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductUpdateDto updateDto)
        {
            if (id <= 0)
            {
                return BadRequest(new { code = 400, message = "Id can not be less than zero." });
            }
            try
            {
                await _service.Update(id, updateDto);
                return StatusCode(200);
            }
            catch (ElementNotExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { statusCode = 400, message = "Id can not be less than zero." });
            }
            try
            {
                await _service.Delete(id);
                return StatusCode(200);
            }
            catch (ElementNotExistException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
