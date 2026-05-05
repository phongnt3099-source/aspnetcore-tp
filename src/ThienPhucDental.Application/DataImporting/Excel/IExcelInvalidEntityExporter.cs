using System.Collections.Generic;
using Abp.Dependency;
using ThienPhucDental.Dto;

namespace ThienPhucDental.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}