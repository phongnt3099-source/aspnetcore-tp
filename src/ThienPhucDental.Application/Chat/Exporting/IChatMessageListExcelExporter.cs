using System.Collections.Generic;
using Abp;
using ThienPhucDental.Chat.Dto;
using ThienPhucDental.Dto;

namespace ThienPhucDental.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
