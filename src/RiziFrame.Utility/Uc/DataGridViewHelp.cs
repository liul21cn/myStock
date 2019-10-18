using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RiziFrame.Utility.Uc
{
    public static class DataGridViewHelp
    {
        private static string m_SelectedChkboxColumn = "exChkSelected";
        private static Color m_ColumFrozenBgColor = Color.Beige;

        public static string SelectedChkboxColumn
        {
            get { return DataGridViewHelp.m_SelectedChkboxColumn; }
            set { DataGridViewHelp.m_SelectedChkboxColumn = value; }
        }

        #region 设置字段Align
        public static void SetColAlign(DataGridViewTextBoxColumn col, DataGridViewContentAlignment align)
        {
            col.DefaultCellStyle.Alignment = align;
        }
        #endregion

        #region 设置字段冻结
        /// <summary>
        /// 设置字段冻结
        /// </summary>
        /// <param name="col">字段</param>
        /// <param name="color">背景颜色</param>
        public static void SetColumFrozen(DataGridViewTextBoxColumn col, Color color)
        {
            col.Frozen = true;           //设置是否滚动
            col.DefaultCellStyle.BackColor = color;
        }
        public static void SetColumFrozen(DataGridViewCheckBoxColumn col, Color color)
        {
            col.Frozen = true;           //设置是否滚动
            col.DefaultCellStyle.BackColor = color;
        }
        /// <summary>
        /// 设置字段冻结
        /// </summary>
        /// <param name="col">字段</param>
        public static void SetColumFrozen(DataGridViewTextBoxColumn col)
        {
            SetColumFrozen(col, m_ColumFrozenBgColor);
        }
        public static void SetColumFrozen(DataGridViewCheckBoxColumn col)
        {
            SetColumFrozen(col, m_ColumFrozenBgColor);
        }
        #endregion

        #region 选择并设置当前行
        /// <summary>
        /// 选择并设置当前行
        /// </summary>
        /// <param name="dg">DataGridView对象</param>
        /// <param name="rownum">行号</param>
        public static void SelectRow(DataGridView dg, int rownum)
        {
            if (rownum < 0) return;
            dg.ClearSelection();
            dg.Rows[rownum].Selected = true;
            // 通过cell选择跳转到当前行
            dg.CurrentCell = dg.Rows[rownum].Cells[0];
        }
        #endregion

        #region 跳转到指定行
        /// <summary>
        ///  跳转到指定行
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="rownum"></param>
        public static void ScrollingRow(DataGridView dg, int rownum)
        {
            if (rownum < 0) return;
            dg.ClearSelection();
            dg.Rows[rownum].Selected = true;
            // 通过cell选择跳转到当前行
            dg.FirstDisplayedScrollingRowIndex = rownum;
        }
        #endregion

        #region 给DataGridView对象选择列
        /// <summary>
        /// 给DataGridView对象选择列
        /// </summary>
        /// <param name="dg"></param>
        public static DataGridViewCheckBoxColumn EnabledSelectedChkbox(DataGridView dgv, bool showText)
        {
            string strText = "";
            int iWidth = 40;

            #region 设置属性
            if (showText)
            {
                strText = "全选";
                iWidth = 80;
            }
            else
            {
                iWidth = 40;
            }
            #endregion

            #region 添加Check控件

            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();

            colCB.HeaderCell = cbHeader;

            colCB.Name = m_SelectedChkboxColumn;
            colCB.ThreeState = false;
            colCB.FalseValue = false;
            colCB.TrueValue = true;
            //colCB.FalseValue = 0;
            //colCB.TrueValue = 1;            
            colCB.HeaderText = strText;
            colCB.Width = iWidth;
            colCB.Frozen = true;

            cbHeader.OnCheckBoxClicked += (a) =>
            {
                dgv.Rows.OfType<DataGridViewRow>().ToList().ForEach(t => t.Cells[0].Value = a);
            };
            dgv.Columns.Insert(0, colCB);
            #endregion
            //this.dgvMain.Columns.Add(colCB); 

            //cbHeader.OnCheckBoxClicked +=
            //    new CheckBoxClickedHandler(cbEvent);

            return colCB;
        }
        #endregion


        public static DataGridViewCheckBoxColumn EnabledSelectedChkbox(DataGridView dg)
        {
            return EnabledSelectedChkbox(dg, false);
        }

        #region 设置column只读状态
        /// <summary>
        /// 设置column只读状态
        /// </summary>
        /// <param name="dg">DataGridView对象</param>
        /// <param name="excludeColName">排除的字段</param>
        /// <param name="flag">true=readonly，false=解除只读</param>
        public static void ColumnReadOnly(DataGridView dg, string excludeColName, bool flag)
        {
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                if (dg.Columns[i].Name.ToUpper() != excludeColName.ToUpper())
                {
                    dg.Columns[i].ReadOnly = flag;
                }
                else
                {
                    dg.Columns[i].ReadOnly = false;
                }
            }
        }

        /// <summary>
        /// 设置column自读状态
        /// </summary>
        /// <param name="dg">DataGridView对象</param>
        /// <param name="excludeColName">排除的字段</param>        
        public static void ColumnReadOnly(DataGridView dg, string excludeColName)
        {
            ColumnReadOnly(dg, excludeColName, true);
        }

        public static void ColumnReadOnly(DataGridView dg)
        {
            ColumnReadOnly(dg, m_SelectedChkboxColumn, true);
        }

        #endregion

        #region 设置DataGridView的样式
        /// <summary>
        /// 设置DataGridView的样式
        /// </summary>
        /// <param name="dg"></param>
        public static void SetStyleGrid(DataGridView dg, bool QiOuRowBgColor = true )
        {
            SetStyleGrid(dg,  26, QiOuRowBgColor);
        }

        public static void SetStyleGrid(DataGridView dg, int colHeadersHeight, bool QiOuRowBgColor = true)
        {
            //dg.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            dg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dg.ColumnHeadersHeight = colHeadersHeight;

            //dg.GridColor = Color.BlueViolet;
            //dg.BorderStyle = BorderStyle.Fixed3D;
            //dg.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dg.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dg.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            //dg.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            //dg.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;

            // 设置行选模式
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 禁止新行, 去掉最后一行空行, 该代码放在此处，会引起设计时，无法看到“新增行”
            // 所以放在“DataGridViewHelp”类中
            dg.AllowUserToAddRows = false;

            #region 设置“奇偶行背景色”
            if (QiOuRowBgColor)
            {
                //单数行为黄色
                Color jsColor = Color.FromArgb(255, 225, 215);
                SetBgColorJO(dg, jsColor);
            }
            #endregion

        }

        /// <summary>
        /// 设置背景颜色：奇偶交替
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="jsColor"></param>
        public static void SetBgColorJO(DataGridView dg, Color jsColor)
        {
            //所有的列的背景色设定为水色
            dg.RowsDefaultCellStyle.BackColor = Color.White;
            dg.AlternatingRowsDefaultCellStyle.BackColor = jsColor;
        }

        #endregion


        #region 显示序列号;加入DataGridView 的 RowPostPaint事件
        /// <summary>
        /// 显示序列号;加入DataGridView 的 RowPostPaint事件
        /// </summary>
        /// <param name="dg">DataGridView控件</param>
        /// <param name="e">DataGridView的DataGridViewRowPostPaintEventArgs参数</param>
        public static void SequenceColumn(DataGridView dg, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush solidBrush;
            solidBrush = new SolidBrush(dg.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, solidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
        }
        #endregion

        #region 设置dgv的序号列，但需要序号列=id
        /// <summary>
        /// 设置dgv的序号列，但需要序号列=id
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static int ShowNumCode(DataGridView dgv)
        {
            //自动整理序列号
            int coun = dgv.RowCount;
            int i = 0;
            if (coun == 1)
            {
                dgv.Rows[0].Cells[0].Value = 1;
            }
            else if (coun > 1)
            {
                for (i = 0; i <= coun - 1; i++)
                {
                    dgv.Rows[i].Cells[0].Value = i + 1;
                }
            }
            return coun;
        }
        #endregion


    }
}
