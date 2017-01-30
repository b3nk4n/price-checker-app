using System.ComponentModel.DataAnnotations;
using UWPCore.Framework.Storage;

namespace AmaScan.Common
{
    public enum AmazonRegion
    {
        Auto,
        [Display(Name = "Autralia (.com.au)")]
        Australia,
        [Display(Name = "Brazil (.com.br)")]
        Brazil,
        [Display(Name = "Canada (.ca)")]
        Canada,
        [Display(Name = "China (.cn)")]
        China,
        [Display(Name = "France (.fr)")]
        France,
        [Display(Name = "Germany (.de)")]
        Germany,
        [Display(Name = "India (.in)")]
        India,
        [Display(Name = "Italy (.it)")]
        Italy,
        [Display(Name = "Japan (.jp)")]
        Japan,
        [Display(Name = "Mexico (.com.mx)")]
        Mexico,
        [Display(Name = "Netherlands (.nl)")]
        Netherlands,
        [Display(Name = "Spain (.es)")]
        Spain,
        [Display(Name = "United Kingdom (.co.uk)")]
        UnitedKingdom,
        [Display(Name = "USA (.com)")]
        USA
    }

    public static class AppSettings
    {
        public static StoredObjectBase<string> Region = new LocalObject<string>("_region_", AmazonRegion.Auto.ToString());
    }
}
