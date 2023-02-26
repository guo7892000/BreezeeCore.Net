using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

/*********************************************************************		
 * �������ƣ�		
 * ������𣺽ӿ�		
 * �������ߣ��ƹ���		
 * �������ڣ�2022/11/5 22:29:28		
 * ����˵����		
 * ���ʵ�ַ��guo7892000@126.com		
 * ΢ �� �ţ�BreezeeHui		
 * �޸���ʷ��		
 *      2022/11/5 22:29:28 �½� �ƹ��� 		
 *******************************************************************/
namespace Breezee.Core.WinFormUI
{
    /// <summary>
    /// ����ί��
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate int EventPagingHandler(EventPagingArg e);

    /// <summary>
    /// ��ҳ�ؼ�����
    /// </summary>
    public partial class GridPager : UserControl
    {
        #region ���캯��
        public GridPager()
        {
            InitializeComponent();
            SetNavigatorVisible(false);
        } 
        #endregion

        #region ��������
        public event EventPagingHandler EventPaging;
        /// <summary>
        /// ÿҳ��ʾ��¼��
        /// </summary>
        private int _pageSize = 20;
        /// <summary>
        /// ÿҳ��ʾ��¼��
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                GetPageCount();
            }
        }

        private int _nMax = 0;
        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int NMax
        {
            get { return _nMax; }
            set
            {
                _nMax = value;
                GetPageCount();
            }
        }

        private int _pageCount = 0;
        /// <summary>
        /// ҳ��=�ܼ�¼��/ÿҳ��ʾ��¼��
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        private int _pageCurrent = 0;
        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int PageCurrent
        {
            get { return _pageCurrent; }
            set { _pageCurrent = value; }
        }

        public BindingNavigator RecordNavigator
        {
            get { return this.bindingNavigator; }
        }
        #endregion

        #region ��ȡҳ����˽�з���
        private void GetPageCount()
        {
            if (this.NMax > 0)
            {
                this.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.NMax) / Convert.ToDouble(this.PageSize)));
            }
            else
            {
                this.PageCount = 0;
            }
        } 
        #endregion

        #region ��ҳ�ؼ����ݰ󶨵ķ���
        /// <summary>
        /// ��ҳ�ؼ����ݰ󶨵ķ���
        /// </summary>
        public void Bind()
        {
            if (this.EventPaging != null)
            {
                this.NMax = this.EventPaging(new EventPagingArg(this.PageCurrent));
            }

            if (this.PageCurrent > this.PageCount)
            {
                this.PageCurrent = this.PageCount;
            }
            if (this.PageCount == 1)
            {
                this.PageCurrent = 1;
            }
            lblPageCount.Text = this.PageCount.ToString();
            this.lblMaxPage.Text = "��" + this.NMax.ToString() + "����¼";
            this.txtCurrentPage.Text = this.PageCurrent.ToString();

            if (this.PageCurrent == 1)
            {
                this.btnPrev.Enabled = false;
                this.btnFirst.Enabled = false;
            }
            else
            {
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }

            if (this.PageCurrent == this.PageCount)
            {
                this.btnLast.Enabled = false;
                this.btnNext.Enabled = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }

            if (this.NMax == 0)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
            }
        } 
        #endregion

        #region ��һҳ��ť�¼�
        private void btnFirst_Click(object sender, EventArgs e)
        {
            PageCurrent = 1;
            this.Bind();
        } 
        #endregion

        #region ǰһҳ��ť�¼�
        private void btnPrev_Click(object sender, EventArgs e)
        {
            PageCurrent -= 1;
            if (PageCurrent <= 0)
            {
                PageCurrent = 1;
            }
            this.Bind();
        }
        #endregion

        #region ��һҳ��ť�¼�
        private void btnNext_Click(object sender, EventArgs e)
        {
            this.PageCurrent += 1;
            if (PageCurrent > PageCount)
            {
                PageCurrent = PageCount;
            }
            this.Bind();
        }
        #endregion

        #region ���һҳ��ť�¼�
        private void btnLast_Click(object sender, EventArgs e)
        {
            PageCurrent = PageCount;
            this.Bind();
        }
        #endregion

        #region GO��ť�¼�
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (this.txtCurrentPage.Text != null && txtCurrentPage.Text != "")
            {
                if (Int32.TryParse(txtCurrentPage.Text, out _pageCurrent))
                {
                    this.Bind();
                }
                else
                {
                    MessageBox.Show("�������ָ�ʽ����");
                }
            }
        } 
        #endregion

        #region ��ǰҳ�س��¼�
        private void txtCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Bind();
            }
        } 
        #endregion

        #region �������ݵ����Ƿ�ɼ�
        public void SetNavigatorVisible(bool IsVisible)
        {

            bindingNavigatorMoveFirstItem.Visible = IsVisible;
            bindingNavigatorMovePreviousItem.Visible = IsVisible;
            bindingNavigatorSeparator.Visible = IsVisible;
            bindingNavigatorPositionItem.Visible = IsVisible;
            bindingNavigatorCountItem.Visible = IsVisible;
            bindingNavigatorSeparator1.Visible = IsVisible;
            bindingNavigatorMoveNextItem.Visible = IsVisible;
            bindingNavigatorMoveLastItem.Visible = IsVisible;
            bindingNavigatorSeparator2.Visible = IsVisible;
        } 
        #endregion

    }

    /// <summary>
    /// �Զ����¼����ݻ���
    /// </summary>
    public class EventPagingArg : EventArgs
    {
        private int _intPageIndex;
        public EventPagingArg(int PageIndex)
        {
            _intPageIndex = PageIndex;
        }
    }
}
