using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Drawing.Drawing2D;

namespace RiziFrame.Utility.Uc
{
    /// <summary>
    /// 扩展DataGrid控件
    /// </summary>
    [ToolboxBitmap(typeof(DataGrid))]
    public class DataGridViewEx : DataGridView
    {

        #region 设置当前编辑的行索引
        // 设置编辑行
        private int rowIndexEdit = 0;

        [Description("设置当前编辑的行索引"), DefaultValue(false), Browsable(true)]
        public int RowIndexEdit
        {
            get { return rowIndexEdit; }
            set { rowIndexEdit = value; }
        }
        #endregion

        private string _ColumnNames = "";           // 显示颜色的列
        private string _ColumnCondition = "";       // 设置列文本颜色显示的条件

        /// <summary>
        /// Required designer variable.
        /// </summary> 
        private bool _ShowRowNumber = true;         // 是否显示行号
        private string _RowTitleText = "";          // 行号的标题文件
        private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;


        #region 设置属性
        /// <summary>
        /// 设置是否显示行号
        /// </summary>
        [Description("设置显示行号"), DefaultValue(false), Browsable(true)]
        public bool ShowRowNumber
        {
            get { return this._ShowRowNumber; }
            set { this._ShowRowNumber = value; }
        }

        /// <summary>
        /// 设置行号的显示标题文本
        /// </summary>
        [Description("设置行号的列标题文本"), DefaultValue(""), Browsable(true)]
        public string RowTitleText
        {
            get { return this._RowTitleText; }
            set
            {
                this._RowTitleText = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 设置行号文本对齐方式
        /// </summary>
        [Description("设置行号文本对齐方式"), DefaultValue(HorizontalAlignment.Left), Browsable(true)]
        public HorizontalAlignment RowNumberAlign
        {
            get { return this._TextAlign; }
            set { this._TextAlign = value; }
        }


        /// <summary>
        /// 设置列文本颜色显示的条件
        /// </summary>
        public string ColumnCondition
        {
            get { return this._ColumnCondition; }
            set
            {
                if (value == null)
                {
                    this._ColumnCondition = "";
                }
                else
                {
                    this._ColumnCondition = value;
                }
                this.Invalidate();
            }
        }


        /// <summary>
        /// 显示颜色的列
        /// </summary>
        public string ColumnNames
        {
            get { return this._ColumnNames; }
            set
            {
                this._ColumnNames = value;
                this.Invalidate();
            }
        }
        #endregion

        SolidBrush solidBrush;

        public DataGridViewEx()
        {
            // 重绘 DataGridView, 添加“序号列”，序号列的宽度默认=41；           
            solidBrush = new SolidBrush(this.RowHeadersDefaultCellStyle.ForeColor);

            // 初始化“DataGridViewEx”属性
            this.InitAttr();
        }

        /// <summary>
        /// 初始化“DataGridViewEx”属性
        /// </summary>
        private void InitAttr()
        {
            //禁止自动生成列。 指定列,进行绑定，不绑定的列不显示。
            this.AutoGenerateColumns = false;
            // 禁止新行, 去掉最后一行空行, 该代码放在此处，会引起设计时，无法看到“新增行”
            // 所以放在“DataGridViewHelp”类中
            //this.AllowUserToAddRows = false;

        }


        /// <summary>
        /// 重绘 DataGridView, 添加“序号列”，序号列的宽度默认=41； 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (this._ShowRowNumber)
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, solidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
                base.OnRowPostPaint(e);
            }
        }

        #region 初始化组件
        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewEx
            // 
            this.RowHeadersWidth = 46;
            this.RowTemplate.Height = 26;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
