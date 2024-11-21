namespace EcosaveAPI.Services.Interfaces
{
    public interface IGptService
    {
        Task<string> ObterDicaReducaoConsumoAsync();
    }
}
