using AutoMapper;
using WebApiCodeFirst.Models;
using WebApiCodeFirst.Models.DTO;

namespace WebApiCodeFirst.Mappers
{
    public class ApiMapper : Profile
    {
        public ApiMapper()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();

            // CrearPeliculaDto -> Pelicula: ignore Imagen (IFormFile) and other db-specific fields
            CreateMap<CrearPeliculaDto, Pelicula>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore())
                .ForMember(dest => dest.RutaIMagen, opt => opt.Ignore())
                .ForMember(dest => dest.RutaLocalIMagen, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Clasificacion, opt => opt.MapFrom(src => (Pelicula.TipoClasificacion)src.Clasificacion));

            CreateMap<PeliculaDto, Pelicula>()
                .ForMember(dest => dest.Imagen, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.RutaLocalIMagen, opt => opt.Ignore())
                .ForMember(dest => dest.RutaIMagen, opt => opt.Ignore())
                .ForMember(dest => dest.Clasificacion, opt => opt.MapFrom(src => (Pelicula.TipoClasificacion)src.Clasificacion));

            // Pelicula -> PeliculaDto (map enum explicitly)
            CreateMap<Pelicula, PeliculaDto>()
                .ForMember(dest => dest.Clasificacion, opt => opt.MapFrom(src => (PeliculaDto.TipoClasificacion)src.Clasificacion));
        }
    }
}