using System.Globalization;

namespace ThienPhucDental.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}