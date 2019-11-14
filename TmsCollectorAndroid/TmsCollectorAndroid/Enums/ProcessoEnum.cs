using System.ComponentModel;

namespace TmsCollectorAndroid.Enums
{
    public enum ProcessoEnum
    {
        [Description("EMBARQUE")]
        Embarque,
        [Description("DESEMBARQUE")]
        Desembarque,
        [Description("ETIQUETA MÃE")]
        EtiquetaMae,
        [Description("CONSULTA RÁPIDA")]
        ConsultaRapida,
        [Description("ENTREGA EMBARQUE")]
        EntregaEmbarque,
        [Description("ENTREGA RETORNO")]
        EntregaRetorno,
        [Description("LEITURA ÚNICA")]
        LeituraUnica,
        [Description("DESEMB. UNIVERSAL")]
        DesembarqueUniversal
    }
}