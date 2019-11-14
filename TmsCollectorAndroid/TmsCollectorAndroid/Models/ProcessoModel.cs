using Prism.Navigation;
using TmsCollectorAndroid.Enums;
using TmsCollectorAndroid.Extensions;

namespace TmsCollectorAndroid.Models
{
    public class ProcessoModel
    {
        public ProcessoEnum Processo { get; private set; }
        public string Nome => Processo.GetDescription();
        public string Pagina { get; private set; }
        public NavigationParameters NavigationParameters { get; private set; }

        public ProcessoModel(ProcessoEnum processo, string pagina)
        {
            Processo = processo;
            Pagina = pagina;
            NavigationParameters = new NavigationParameters();
            NavigationParameters.Add("processo", Processo);
        }

        public void AddNavigationParameters(string key, object value)
        {
            NavigationParameters.Add(key, value);
        }
    }
}