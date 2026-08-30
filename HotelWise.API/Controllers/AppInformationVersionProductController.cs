using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers;

/// <summary>
/// Controlador para consulta de metadados, versão do produto e informações operacionais do runtime.
/// </summary>
[ApiController]
[Route("api/[controller]/v1")]
public class AppInformationVersionProductController : ControllerBase
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="AppInformationVersionProductController"/>.
    /// </summary>
    public AppInformationVersionProductController()
    {
    }

    /// <summary>
    /// Retorna as informações do produto e versão formatadas como string descritiva.
    /// </summary>
    /// <returns>String formatada com os dados do produto.</returns>
    [HttpGet("GetAppInformationVersionProductString")]
    public async Task<ActionResult<string>> GetString()
    {
        await Task.FromResult(0);
        return Ok(LogAppHelper.ShowInformationVersionProductString());
    }

    /// <summary>
    /// Retorna o objeto estruturado contendo a versão, ambiente e metadados do produto.
    /// </summary>
    /// <returns>Lista contendo o DTO de metadados da aplicação.</returns>
    [HttpGet("GetAppInformationVersionProduct")]
    public async Task<ActionResult<List<AppInformationVersionProductDto>>> Get()
    {
        await Task.FromResult(0);
        var responseVO = LogAppHelper.GetInformationVersionProduct();
        if (responseVO != null)
        {
            List<AppInformationVersionProductDto> response = new List<AppInformationVersionProductDto> { responseVO };
            return Ok(response);
        }
        return NotFound(responseVO);
    }
}

