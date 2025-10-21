using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCodeFirst.Models;
using WebApiCodeFirst.Models.DTO;
using WebApiCodeFirst.Repositorios.IRepositorios;

namespace WebApiCodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CrearPelicula([FromForm] CrearPeliculaDto crearPeliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (crearPeliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La película ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);

            //subir la imagen
            if (crearPeliculaDto.Imagen != null && crearPeliculaDto.Imagen.Length > 0)
            {
                // Leer la imagen una vez en memoria
                using var ms = new MemoryStream();
                await crearPeliculaDto.Imagen.CopyToAsync(ms);
                var imageBytes = ms.ToArray();

                // Guardar los bytes en la entidad si decides almacenar blob
                pelicula.Imagen = imageBytes;

                // Nombre y ruta usando GUID
                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(crearPeliculaDto.Imagen.FileName);
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImagenesPeliculas");
                Directory.CreateDirectory(carpeta);
                var ubicacionDirectorio = Path.Combine(carpeta, nombreArchivo);

                // Escribir archivo en disco utilizando los bytes leídos
                await System.IO.File.WriteAllBytesAsync(ubicacionDirectorio, imageBytes);

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                pelicula.RutaIMagen = baseUrl + "/ImagenesPeliculas/" + nombreArchivo;
                pelicula.RutaLocalIMagen = ubicacionDirectorio;
            }
            else
            {
                pelicula.RutaIMagen = "https://placehold.co/600x400";
            }

            _pelRepo.CrearPelicula(pelicula);
            return Ok();
        }
    }
}