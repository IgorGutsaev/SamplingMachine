using MPT.SamplingMachine.ApiClient;

namespace FunctionApp
{
    public class SamplingMachineApiClientBuilder
    {
        public static SamplingMachineApiClient GetClient()
            => new SamplingMachineApiClient(x => {
                x.Url = /* builder.Configuration["ApiUrl"] ??*/ "https://ogmento-api.azurewebsites.net/";
                x.Email = "smytten1@filuet.com";
                x.Password = "87UsQaYnXB";
            });
    }
}
