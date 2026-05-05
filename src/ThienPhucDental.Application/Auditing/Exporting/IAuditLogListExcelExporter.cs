using System.Collections.Generic;
using ThienPhucDental.Auditing.Dto;
using ThienPhucDental.Dto;

namespace ThienPhucDental.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
