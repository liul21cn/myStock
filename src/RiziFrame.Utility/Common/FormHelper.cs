using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RiziFrame.Utility.Common
{
    public class FormHelper
    {

        /// <summary>
        /// 判断子窗体是否打开，如果已经打开，则激活并显示，返回true; 否则返回false
        /// </summary>
        /// <param name="formName">要打开的子窗口Name</param>
        /// <returns></returns>
        public static bool IsChildWinOpened(string childFormClassName, Form mdiForm)
        {
            bool isOpen = false;

            //foreach (Form childrenForm in this.MdiChildren)
            foreach (Form cForm in mdiForm.MdiChildren)
            {
                // 检测是不是当前子窗口名称
                if (cForm.Name == childFormClassName)
                {
                    // 是，显示
                    cForm.Visible = true;
                    // 激活
                    cForm.Activate();
                    cForm.WindowState = FormWindowState.Maximized;
                    isOpen = true;
                }
            }
            return isOpen;
        }

        /// <summary>
        /// 利用反射功能打开Mdi窗口
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="nameSpaceName">表空间名</param>
        /// <param name="className">MdiForm窗口类名称</param>
        /// <param name="mdiForm">Mdi父窗体</param>
        public static void OpenMdiChildWin(string assemblyName, string nameSpaceName, string mdiFormClassName, Form mdiForm)
        {
            if (!IsChildWinOpened(mdiFormClassName, mdiForm))
            {
                //Form frm = new JcxxForm();
                //frm.MdiParent = this;
                //frm.WindowState = FormWindowState.Maximized;
                //frm.Show();                

                //string nameSpaceName = "zssWork.WinUI";
                //string assemblyName = "zssWork.WinUI";
                //string className = "JcxxForm";                
                string fullClassName = string.Format("{0}.{1}", nameSpaceName, mdiFormClassName);
                Form obj = ReflectionHelper.CreateInstance<Form>(fullClassName, assemblyName);
                Type type = obj.GetType();
                obj.MdiParent = mdiForm;
                obj.WindowState = FormWindowState.Maximized;
                obj.Show();

            }            
        }

    }
}
