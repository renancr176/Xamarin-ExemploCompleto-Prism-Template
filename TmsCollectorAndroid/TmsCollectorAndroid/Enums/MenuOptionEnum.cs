using System.ComponentModel;

namespace TmsCollectorAndroid.Enums
{
    public enum MenuOptionEnum
    {
        [Description("Trocar de Usuário")]
        LogOut,
        [Description("Limpar")]
        Clear,
        [Description("Voltar")]
        GoBack,
        [Description("Desligar")]
        Exit
    }
}