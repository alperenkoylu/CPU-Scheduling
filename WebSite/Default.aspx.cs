using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrueRandomGenerator;

public partial class _Default : System.Web.UI.Page
{
    protected class Process
    {
        public String name { get; set; }
        public int arrival_time { get; set; }
        public int burst_time { get; set; }
        public int finish_time { get; set; }
        public int waiting_time { get; set; }
    }

    static Random r = new Random();
    static public int[] numberList;
    static public int elapsed_time;
    static public int QueueA_rate;
    static public int QueueB_rate;
    static public int[] AWTofQA;
    static public int[] AWTofQB;
    static public string TheQueueIs = "";
    static bool checkFirst = true;
    static int totalLenOfQA;
    static int totalLenOfQB;

    static int avg_QLen_A;
    static int avg_QLen_B;

    static int seq;

    protected void Page_Load(object sender, EventArgs e)
    {
        pnlAlert.Visible = false;
        numberList = GetRandom(0, 250);
        if (!IsPostBack)
        {
            QueueA_rate = 70;
            QueueB_rate = 30;

            checkFirst = true;

            Timer1.Enabled = false;
            LinkButton1.Text = "<i class=\"fas fa-play\"></i>&nbsp;Start Simulate";
            LinkButton1.CssClass = "btn btn-success btn-lg btn-block";
            simulating.Visible = false;
        }
    }

    protected int[] GetRandom(int min, int max)
    {
        Random randNum = new Random();
        return Enumerable
            .Repeat(0, 50)
            .Select(i => randNum.Next(min, max))
            .ToArray();
    }

    public static string Name()
    {
        int num = r.Next(0, 26);
        string let1 = ((char)('a' + num)).ToString();

        num = r.Next(0, 26);
        string let2 = ((char)('a' + num)).ToString();

        num = r.Next(0, 26);
        string let3 = ((char)('a' + num)).ToString();

        return (let1 + let2 + let3).ToUpper();
    }

    protected Queue<Process> CreateProcessesQueue(int min, int max)
    {
        Queue<Process> q = new Queue<Process>();

        for (int i = 0; i < r.Next(min, max); i++)
        {
            Process addToQueue = new Process
            {
                name = "P-" + Name(),
                arrival_time = numberList[r.Next(0, 50)],
                burst_time = numberList[r.Next(0, 50)],
                finish_time = 0,
                waiting_time = 0
            };

            q.Enqueue(addToQueue);
        }

        return q;
    }

    protected Queue<Process> MakeItFCFS(Queue<Process> q)
    {
        List<Process> l = q.ToList();

        int size = l.Count;
        for (int i = 1; i < size; i++)
        {
            for (int j = 0; j < (size - i); j++)
            {
                if (l[j].arrival_time > l[j + 1].arrival_time)
                {
                    Process temp = l[j];
                    l[j] = l[j + 1];
                    l[j + 1] = temp;
                }
            }
        }

        q.Clear();

        foreach (var item in l)
        {
            q.Enqueue(item);
        }

        return q;
    }

    protected Queue<Process> MakeItSJF(Queue<Process> q)
    {
        List<Process> l = q.ToList();

        int size = l.Count;
        for (int i = 1; i < size; i++)
        {
            for (int j = 0; j < (size - i); j++)
            {
                if (l[j].burst_time > l[j + 1].burst_time)
                {
                    Process temp = l[j];
                    l[j] = l[j + 1];
                    l[j + 1] = temp;
                }
            }
        }

        q.Clear();

        foreach (var item in l)
        {
            q.Enqueue(item);
        }

        return q;
    }

    protected Queue<Process> SelectQueue(Queue<Process> q1, Queue<Process> q2, int q1Percentage, int q2Percentage)
    {
        double d = r.NextDouble();

        //TheQueueIs = (q1Percentage > q2Percentage) ? ((d * 100 <= q1Percentage) ? "A" : "B") : ((d * 100 <= q2Percentage) ? "B" : "A");
        if (q1Percentage >= q2Percentage)
        {
            if (d * 100 <= q1Percentage)
            {
                TheQueueIs = "A";
                return q1;
            }
            else
            {
                TheQueueIs = "B";
                return q2;
            }
        }
        else
        {
            if (d * 100 <= q2Percentage)
            {
                TheQueueIs = "B";
                return q2;
            }
            else
            {
                TheQueueIs = "A";
                return q1;
            }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Timer1.Enabled = (Timer1.Enabled) ? false : true;
        LinkButton1.Text = (Timer1.Enabled) ? "<i class=\"fas fa-pause\"></i>&nbsp;Stop Simulate" : "<i class=\"fas fa-play\"></i>&nbsp;Continue Simulate";
        LinkButton1.CssClass = (Timer1.Enabled) ? "btn btn-danger btn-lg btn-block" : "btn btn-warning btn-lg btn-block";

        simulating.Visible = (Timer1.Enabled) ? true : false;
    }

    int QA_AWT;
    int QB_AWT;

    Queue<Process> QA;
    Queue<Process> QB;

    int aaa;
    int bbb;

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        ltrLOG.Text = "";

        int zqa = (avg_QLen_A * 100) / ((avg_QLen_A + avg_QLen_B) + 1);
        int zqb = (avg_QLen_A * 100) / ((avg_QLen_A + avg_QLen_B) + 1);

        if (checkFirst)
        {
            QA = MakeItFCFS(CreateProcessesQueue(5, 20));
            QB = MakeItSJF(CreateProcessesQueue(3, 17));

            aaa = 0;
            bbb = 0;

            seq = 1;

            totalLenOfQA = 0;
            totalLenOfQB = 0;

            avg_QLen_A = 0;
            avg_QLen_B = 0;

            checkFirst = false;
        }
        else
        {
            zqa /= 2;
            zqb /= 2;

            QA = MakeItFCFS(CreateProcessesQueue(zqa - r.Next(1, 10), zqa + r.Next(1, 10)));
            QB = MakeItSJF(CreateProcessesQueue(zqb - r.Next(1, 10), zqb + r.Next(1, 10)));
        }

        rptQueueA.DataSource = QA;
        rptQueueA.DataBind();

        rptQueueB.DataSource = QB;
        rptQueueB.DataBind();

        Queue<Process> SelectedQueue;

        QA_AWT = 0;
        QB_AWT = 0;

        int QA_Count = QA.Count;
        int QB_Count = QB.Count;

        for (int i = 0; i < (QA_Count + QB_Count); i++)
        {
            if (QA.Count == 0 || QB.Count == 0)
            {
                if (QA.Count == 0)
                {
                    SelectedQueue = QB;
                }
                else
                {
                    SelectedQueue = QA;
                }
            }
            else
            {
                SelectedQueue = SelectQueue(QA, QB, QueueA_rate, QueueB_rate);
            }

            Process ThatOne = SelectedQueue.Dequeue();

            elapsed_time += ThatOne.burst_time + ((i == 0) ? ThatOne.arrival_time : 0);
            ThatOne.finish_time = elapsed_time;
            ThatOne.waiting_time = ThatOne.finish_time - ThatOne.arrival_time;

            ltrLOG.Text += "<span class=\"badge badge-warning\">" + ThatOne.name + "(" + ThatOne.burst_time + ")--->" + elapsed_time + "</span><br/>";

            if (TheQueueIs == "A")
            {
                QA_AWT += ThatOne.waiting_time;
            }
            else
            {
                QB_AWT += ThatOne.waiting_time;
            }
        }

        totalLenOfQA = totalLenOfQA + QA_Count;
        totalLenOfQB = totalLenOfQB + QB_Count;

        QA_AWT = QA_AWT / totalLenOfQA;
        QB_AWT = QB_AWT / totalLenOfQB;

        ltrQALen.Text = QA_Count.ToString();
        ltrQBLen.Text = QB_Count.ToString();

        ltrQAPick.Text = QueueA_rate.ToString();
        ltrQBPick.Text = QueueB_rate.ToString();

        double QA_Rat = (totalLenOfQA * 100) / (totalLenOfQA + totalLenOfQB);
        QueueA_rate = Convert.ToInt32(QA_Rat);
        QueueB_rate = 100 - QueueA_rate;

        ltrNewQAPick.Text = QueueA_rate.ToString();
        ltrNewQBPick.Text = QueueB_rate.ToString();

        pnlAlert.Visible = true;

        Chart1.Series["Series1"].Points.Add(QA_AWT);
        Chart1.Series["Series1"].Points.Add(QB_AWT);

        Chart1.Series["Series1"].Points[0].Color = ColorTranslator.FromHtml("#FFF3CD");
        Chart1.Series["Series1"].Points[1].Color = ColorTranslator.FromHtml("#D1ECF1");
        Chart1.Series["Series1"].Points[0].AxisLabel = "FCFS (" + QA_AWT.ToString() + ")";
        Chart1.Series["Series1"].Points[1].AxisLabel = "SJF (" + QB_AWT.ToString() + ")";

        double xqa = totalLenOfQA / seq;
        avg_QLen_A = Convert.ToInt32(xqa) + r.Next(3, 10);

        double xqb = totalLenOfQB / seq;
        avg_QLen_B = Convert.ToInt32(xqb) + r.Next(3, 10);

        totalA.Text = xqa.ToString();
        totalB.Text = xqb.ToString();

        seq++;
    }
}