using Abp.Dependency;

namespace ThienPhucDental.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}