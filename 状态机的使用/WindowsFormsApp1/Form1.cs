using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Security.Cryptography;
using System.ServiceModel.Activities.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public WorkflowApplication workflow;

        private void button3_Click_1(object sender, EventArgs e)
        {
            string reason = textBox3.Text;
            int money = int.Parse(textBox4.Text);

            Activity activity1 = new Activity1();
            workflow = new WorkflowApplication(activity1, new Dictionary<string, object>()
            {
                { "R1", reason }, { "M1", money }
            });



            string sql = "server=.;database=Microsoft.NET-Workflow;uid=sa;pwd=123";
            SqlWorkflowInstanceStore store = new SqlWorkflowInstanceStore(sql);
            workflow.InstanceStore = store;


            AutoResetEvent syncEvent = new AutoResetEvent(false);

            WorkFlowEvent(workflow, syncEvent);


            this.textBox1.Text = workflow.Id.ToString();
            workflow.Run();

            syncEvent.WaitOne();
        }
        private static void WorkFlowEvent(WorkflowApplication app, AutoResetEvent syncEvent)
        {
            #region 工作流生命周期事件
            app.Unloaded = delegate (WorkflowApplicationEventArgs er)
            {
                Console.WriteLine("工作流 {0} 卸载.", er.InstanceId);
            };
            app.Completed = delegate (WorkflowApplicationCompletedEventArgs er)
            {
                Console.WriteLine("工作流 {0} 完成.", er.InstanceId);
                syncEvent.Set();
            };
            app.Aborted = delegate (WorkflowApplicationAbortedEventArgs er)
            {
                Console.WriteLine("工作流 {0} 终止.", er.InstanceId);
            };
            app.Idle = delegate (WorkflowApplicationIdleEventArgs er)
            {
                Console.WriteLine("工作流 {0} 空闲.", er.InstanceId);
                syncEvent.Set(); //这里要唤醒，不让的话，当创建了一个书签之后，界面就卡死了。
            };
            app.PersistableIdle = delegate (WorkflowApplicationIdleEventArgs er)
            {
                Console.WriteLine("持久化");
                return PersistableIdleAction.Unload;
            };
            app.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs er)
            {
                Console.WriteLine("OnUnhandledException in Workflow {0}\n{1}",
       er.InstanceId, er.UnhandledException.Message);
                return UnhandledExceptionAction.Terminate;
            };
            #endregion
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            WorkflowApplication app = new WorkflowApplication(new Activity1());

            string sql = "server=.;database=Microsoft.NET-Workflow;uid=sa;pwd=123";

            SqlWorkflowInstanceStore store = new SqlWorkflowInstanceStore(sql);
            app.InstanceStore = store;


            app.Load(Guid.Parse(textBox1.Text)); //加载工作流实例

            bool isa = radioButton1.Checked ? true : false;

            app.ResumeBookmark("Approvel", isa);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
