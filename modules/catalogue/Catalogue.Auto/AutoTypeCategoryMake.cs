using System.ComponentModel;

namespace Catalogue.Auto
{
    public enum AutoTypeCategoryMake
    {
        Necunoscuta,//Car, Masini,Automobil
        Autoturism,//Car, Masini,Automobil
        Motocicleta, //Moto
        Rulota, //Caravane
        [Description("Autocamion peste 7,5 t")]
        Autocamion,
        Autobuz,//bus
        [Description("Camion semiremorcă")]
        CamionSemiremorca,
        [Description("Furgonetă sau autocamion până la 7,5 t")]
        Furgoneta,
        Motostivuitor,
        [Description("Remorcă")]
        Remorca,
        [Description("Semiremorcă")]
        Semiremorca,
        [Description("Utilaj de construcţii")]
        UtilajDeConstructii,
        [Description("Vehicul agricol")]
        VehiculAgricol
    }
}