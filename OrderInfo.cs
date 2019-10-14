using System.Collections.Generic;

namespace FastReportDemo
{
    class OrderInfo
    {
        public string orderno { get; set; }
        public string orderid { get; set; }
        public string ordermonth { get; set; }
        public string orderdate { get; set; }
        public string customer { get; set; }
        public string phone { get; set; }
        public string addr { get; set; }
        public decimal summoney { get; set; }
        public decimal sumnumber { get; set; }
        public decimal sumacreage { get; set; }
        public string operation { get; set; }
        public string sign { get; set; }
        public string lxdh { get; set; }
        public string lxdz { get; set; }
        public string bz { get; set; }
        public List<OrderDetail> Detail { get; internal set; }
    }

    public class OrderDetail
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string XH { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string GG { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string SL { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public string MJ { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string DJ { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string JE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }
    }
}
