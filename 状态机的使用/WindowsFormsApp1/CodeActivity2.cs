using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApp1
{
    public sealed class CodeActivity2 : NativeActivity
    {

        //输出参数
        public OutArgument<bool> Apperove { get; set; }

        //必须让他等于true才能表示进入书签
        protected override bool CanInduceIdle => true;
        protected override void Execute(NativeActivityContext context)
        {
            Console.WriteLine("进入书签");
            var bookmark = context.CreateBookmark("Approvel", Func);
        }

        private void Func(NativeActivityContext context, Bookmark bookmark, object value)
        {

            Console.WriteLine("回调函数", value);
            context.SetValue(Apperove, Convert.ToBoolean(value));
        }
    }
}
