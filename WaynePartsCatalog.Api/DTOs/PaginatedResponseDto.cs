namespace WaynePartsCatalog.Api.DTOs;

public class PaginatedResponseDto<T>
{
    // Registros devueltos para la pagina solicitada.
    public List<T> Content { get; set; } = [];

    public int Page { get; set; }

    public int Size { get; set; }

    public long TotalElements { get; set; }

    public int TotalPages { get; set; }

    public bool HasNext { get; set; }

    public bool HasPrevious { get; set; }

    // Tiempo que tarda el backend en procesar la consulta.
    public long ResponseTimeMs { get; set; }
}