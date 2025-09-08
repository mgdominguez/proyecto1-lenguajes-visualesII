using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApiCodeFirst.Models.DTO;
using WebApiCodeFirst.Repositorios.IRepositorios;

namespace WebApiCodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usRepo;
        private readonly IMapper _mapper;
        protected RespuestaApiDTO _respuestaApi;

        public UsuariosController(IUsuarioRepositorio usRepo, IMapper mapper)
        {
            _usRepo = usRepo;
            _mapper = mapper;
            _respuestaApi = new RespuestaApiDTO();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _usRepo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var usuario in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(usuario));
            }
            return Ok(listaUsuariosDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _usRepo.GetUsuario(usuarioId);

            if (itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);

            return Ok(itemUsuarioDto);
        }

        //[AllowAnonymous]
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool validarNombreUsuarioUnico = _usRepo.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            if (!validarNombreUsuarioUnico)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario ya existe");
                return BadRequest(_respuestaApi);
            }

            var usuario = await _usRepo.Registro(usuarioRegistroDto);
            if (usuario == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error en el registro de usuario");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            return Ok(_respuestaApi);
        }

        //[AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuestaLogin = await _usRepo.Login(usuarioLoginDto);

            if (respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuestaLogin;
            return Ok(_respuestaApi);
        }
    }
}