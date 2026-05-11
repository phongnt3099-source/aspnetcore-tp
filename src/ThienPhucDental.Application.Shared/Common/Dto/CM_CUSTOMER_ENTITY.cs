using System;
using System.Collections.Generic;
using System.Text;

namespace ThienPhucDental.Common.Dto
{
    public class CM_CUSTOMER_ENTITY: PagedAndSortedInputDto
    {
        public string CUS_ID { get; set; }
        public string CUS_CODE { get; set; }
        public string CUS_DOB { get; set; }
        public int? CUS_GENDER { get; set; }
        public string CUS_PHONE { get; set; }
        public string CUS_NAME { get; set; }
        public string CUS_PHONE2 { get; set; }
        public string CUS_EMAIL { get; set; }
        public string CUS_ADDRESS { get; set; }
        public string CUS_WARD { get; set; }
        public string CUS_CITY { get; set; }
        public string CUS_ADDRESS_FULL { get; set; }
        public string CUS_MEDICAL_HISTORY { get; set; }
        public string CUS_MEDICAL_HISTORY_NOTES { get; set; }
        public string CUS_JOB { get; set; }
        public string CUS_ETHNICITY { get; set; }
        public string CUS_NATIONALITY { get; set; }
        public string CUS_CCCD { get; set; }
        public string NOTES { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string UPDATE_ID { get; set; }
        public DateTime? UPDATE_DT { get; set; }
        public string RECORD_STATUS { get; set; }
        public bool IsHuyetAp { get; set; }
        public bool IsDongKinh { get; set; }
        public bool IsMauKhongDong { get; set; }
        public bool IsBenhTim { get; set; }
        public bool IsTieuDuongType1 { get; set; }
        public bool IsTieuDuongType2 { get; set; }
        public bool IsDiUng { get; set; }
        public bool IsSocPhanVe { get; set; }
        public bool IsStentVanh { get; set; }
        public bool IsDotQuy { get; set; }
        public bool IsLoangXuong { get; set; }
        public int AGE { get; set; }
        public int SN { get; set; }
        public int debtAmount { get; set; }
        public int totalPayment { get; set; }


        public string APP_ID { get; set; }

        public string SLOT_NAME { get; set; }
        public string TIME_DISTANCE { get; set; }
        public string FORMATTED_DATE{ get; set; }
        public string STATUS_COLOR { get; set; }
        public string ONLY_TIME { get; set; }
    }
}
