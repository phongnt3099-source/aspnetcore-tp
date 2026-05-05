using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThienPhucDental.MultiTenancy.HostDashboard.Dto;

namespace ThienPhucDental.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}