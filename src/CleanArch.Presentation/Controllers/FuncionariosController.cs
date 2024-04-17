using CleanArch.Application.Abstractions;
using CleanArch.Application.Common;
using CleanArch.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : Controller
    {
        private readonly IFuncionarioService _funcionarioService;
        public FuncionariosController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> Get()
        {
            var result = await _funcionarioService.List();
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if(id == Guid.Empty)
                return BadRequest("Id inválido");

            var result = await _funcionarioService.GetById(id);
            return result.Succeeded ? Ok(result) : BadRequest(result);


        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFuncionarioDto dto)
        {
            if (dto is null) return BadRequest("Dados inválidos");

            var result = await _funcionarioService.Create(dto);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }


        [HttpPut]
        public async Task<IActionResult> Update( UpdateFuncionarioDto dto)
        {
            if (dto is null) return BadRequest("Dados inválidos");

            var result = await _funcionarioService.Update(dto);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            if (id == Guid.Empty)
                return BadRequest("Id inválido");

            var result = await _funcionarioService.Remove(id);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
