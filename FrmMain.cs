using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace FastReportDemo
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            FastReport.Report report = new FastReport.Report();
            string fileName = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "steelorder.frx");
            report.Load(fileName);
            report.Page.Height = 1000;
            report.ShowPrepared();
        }

        internal void PrintOrder(OrderInfo order, bool preview = false)
        {
            string fileName = Path.Combine(Environment.CurrentDirectory, "steelorder.frx");
            FastReport.Report report = new FastReport.Report();
            report.Load(fileName);
            report.SetParameterValue("orderno", order.orderno);
            report.SetParameterValue("orderid", order.orderid);
            report.SetParameterValue("customer", order.customer);
            report.SetParameterValue("phone", order.phone);
            report.SetParameterValue("addr", order.addr);
            report.SetParameterValue("orderdate", order.orderdate);
            string detailno = "0", guage = "", number = "", acreage = "", price = "", money = "", note = "";
            for (int i = 1; i < 6; i++)
            {
                detailno = "detailno" + i.ToString();
                guage = "guage" + i.ToString();
                number = "number" + i.ToString();
                acreage = "acreage" + i.ToString();
                price = "price" + i.ToString();
                money = "money" + i.ToString();
                note = "note" + i.ToString();
                if (i <= order.Detail.Count)
                {
                    report.SetParameterValue(detailno, order.Detail[i - 1].XH);
                    report.SetParameterValue(guage, order.Detail[i - 1].GG);
                    report.SetParameterValue(number, (order.Detail[i - 1].SL.ToString()));
                    report.SetParameterValue(acreage, (order.Detail[i - 1].MJ.ToString()));
                    report.SetParameterValue(price, (order.Detail[i - 1].DJ.ToString()));
                    report.SetParameterValue(money, (order.Detail[i - 1].JE.ToString()));
                    report.SetParameterValue(note, order.Detail[i - 1].BZ);
                }
                else
                {
                    report.SetParameterValue(detailno, i.ToString());
                    report.SetParameterValue(guage, "");
                    report.SetParameterValue(number, "");
                    report.SetParameterValue(acreage, "");
                    report.SetParameterValue(price, "");
                    report.SetParameterValue(money, "");
                    report.SetParameterValue(note, "");
                }
            }
            report.SetParameterValue("sumnumber", (order.sumnumber.ToString()));
            report.SetParameterValue("sumacreage", order.sumacreage.ToString());
            report.SetParameterValue("summoney", (order.summoney.ToString()));

            report.SetParameterValue("dxje", RMBUtils.CmycurD(Convert.ToDecimal(order.summoney)));
            report.SetParameterValue("operation", order.operation);
            report.SetParameterValue("sign", order.sign);
            report.SetParameterValue("lxdz", order.lxdz);
            report.SetParameterValue("lxdh", order.lxdh);
            report.SetParameterValue("bz", order.bz);

            report.PrintSettings.ShowDialog = false;
            //FastReport环境变量设置（打印时不提示 "正在准备../正在打印..",一个程序只需设定一次，故一般写在程序入口）
            (new FastReport.EnvironmentSettings()).ReportSettings.ShowProgress = false;
            if (preview)
            {
                report.Prepare();
                report.ShowPrepared();
            }
            else
            {
                report.Print();
            }
        }

        /// <summary>
        /// 打印数据
        /// </summary>
        /// <returns></returns>
        private List<OrderInfo> GetPrintSource()
        {
            List<OrderInfo> orderInfos = new List<OrderInfo>();
            for (var i = 0; i < 3; i++)
            {
                OrderInfo orderInfo = new OrderInfo()
                {
                    orderid = i.ToString(),
                    lxdz = "上海市人民广场" + i.ToString(),
                    lxdh = "18930037387",
                    addr = "上海市人民广场" + i.ToString(),
                    bz = "备注" + i.ToString(),
                    operation = "张三" + i.ToString(),
                    customer = "病人姓名" + i.ToString(),
                    orderdate = DateTime.Today.ToString("yyyy年MM月dd日"),
                    ordermonth = DateTime.Today.Month.ToString(),
                    orderno = i.ToString(),
                    phone = "18930037387",
                    sign = "",
                    sumacreage = 100,
                    summoney = 1000 + i,
                    sumnumber = i,
                    Detail = new List<OrderDetail>()
                };
                for (var j = 0; j < 5; j++)
                {
                    if (j > 3)
                    {
                        orderInfo.Detail.Add(new OrderDetail()
                        {
                            DJ = "",
                            BZ = "",
                            GG = "",
                            JE = "",
                            MJ = "",
                            SL = "",
                            XH = ""
                        });
                    }
                    else
                    {
                        orderInfo.Detail.Add(new OrderDetail()
                        {
                            DJ = (100 + j).ToString(),
                            BZ = "备注" + j.ToString(),
                            GG = "规格" + j.ToString(),
                            JE = (10 + j).ToString(),
                            MJ = "99",
                            SL = j.ToString(),
                            XH = j.ToString()
                        });
                    }
                }
                orderInfos.Add(orderInfo);
            }
            return orderInfos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 解决思路是 模板固定打印的内容 模板中不需要用Data  直接参数传进去打印
            // 组装需要打印的数据，每页固定打印 20个项目（20是个举例，根据实际纸张情况而定）
            // 假设有数据集 50条 可以用 ==>   需要打印的页数为  pageNum=Math.Ceiling(Convert.ToDecimal(50) / 20);  应该算出等于3
            // for(i = 0 ; i< pageNum; i++){
            //      PrintTemplate(每页的打印数据); //调用打印函数
            // }

            var source = GetPrintSource();

            //遍历打印
            foreach (var order in source)
            {
                PrintOrder(order);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fileName = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportTemplate"), "收款58mm小票(1).frx");
            FastReport.Report report = new FastReport.Report();
            report.Load(fileName);

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("str1", typeof(string)));
            dt.Columns.Add(new DataColumn("str2", typeof(string)));
            dt.Columns.Add(new DataColumn("str3", typeof(string)));
            dt.Columns.Add(new DataColumn("str4", typeof(string)));

            for (var i = 0; i < 6; i++)
            {
                dt.Rows.Add(i.ToString(), i.ToString() + "单位", i.ToString(), i.ToString() + "Q");
            }
            report.RegisterData(dt, "list"); //注册数据源,单表

            report.SetParameterValue("company", "公司");
            report.SetParameterValue("patientName", "张三");
            report.SetParameterValue("cardNum", "123456789");
            report.SetParameterValue("seeDoctoeNumber", "123");
            report.SetParameterValue("heji", "99");
            report.SetParameterValue("pa", "1");
            report.SetParameterValue("relPay", "1");
            report.SetParameterValue("payType", "支付宝");
            report.SetParameterValue("userDataName", "189");
            report.SetParameterValue("phone", "18930037387");

            report.PrintSettings.ShowDialog = false;
            //report.PrintPrepared();
            report.Report.Prepare();
            //根据模板名称，查询对应打印机
            try
            {
                report.Print();
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接打印机，请检查打印机驱动或更换默认打印机。");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
