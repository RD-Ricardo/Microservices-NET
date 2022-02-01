using GeekShooping.ProductAPI.Data.ValueObjects;
using GeekShooping.ProductAPI.Repository;
using GeekShooping.ProductAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;
        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductVO>> FindByID(long id)
        {
            var result = await _repository.FindById(id);

            if(result == null) return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {
            var results = await _repository.FindAll();

            if (results == null) return NotFound();

            return Ok(results);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductVO>> Create(ProductVO model)
        {

            if (model == null) return BadRequest();

            var result = await _repository.Create(model);

            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ProductVO>> Update(ProductVO model)
        {

            if (model == null) return BadRequest();

            var result = await _repository.Update(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Delete(long id)
        {
             var status = await _repository.Delete(id);
            if (!status)
            {
                return BadRequest();
            }

            return Ok(status);
        }
    }
}
