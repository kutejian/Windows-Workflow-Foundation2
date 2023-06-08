using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1
{
    public sealed class CodeActivity1 : CodeActivity
    {
        public InArgument<string> Reason { get; set; }
        public InArgument<int> Money { get; set; }
        protected override void Execute(CodeActivityContext context)
        {

            // 获取 Text 输入参数的运行时值 读取输入参数
            string reson = context.GetValue(this.Reason);
            // 获取 Text 输入参数的运行时值
            int money = context.GetValue(this.Money);





            Console.WriteLine("原因" + reson + "金额" + money);
        }
    }
}
