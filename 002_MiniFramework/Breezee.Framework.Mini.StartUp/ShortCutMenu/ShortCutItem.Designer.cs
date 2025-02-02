﻿namespace Breezee.Framework.Mini.StartUp
{
    partial class ShortCutItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMenuName = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCancelShortCutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoveFirst = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoveLast = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoveBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMoveBack = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMenuName
            // 
            this.lblMenuName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMenuName.AutoSize = true;
            this.lblMenuName.ContextMenuStrip = this.contextMenuStrip1;
            this.lblMenuName.Location = new System.Drawing.Point(3, 31);
            this.lblMenuName.Name = "lblMenuName";
            this.lblMenuName.Size = new System.Drawing.Size(41, 12);
            this.lblMenuName.TabIndex = 0;
            this.lblMenuName.Text = "label1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCancelShortCutMenu,
            this.tsmiMoveFirst,
            this.tsmiMoveLast,
            this.tsmiMoveBefore,
            this.tsmiMoveBack});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 136);
            // 
            // tsmiCancelShortCutMenu
            // 
            this.tsmiCancelShortCutMenu.Name = "tsmiCancelShortCutMenu";
            this.tsmiCancelShortCutMenu.Size = new System.Drawing.Size(180, 22);
            this.tsmiCancelShortCutMenu.Text = "取消";
            this.tsmiCancelShortCutMenu.Click += new System.EventHandler(this.tsmiCancelShortCutMenu_Click);
            // 
            // tsmiMoveFirst
            // 
            this.tsmiMoveFirst.Name = "tsmiMoveFirst";
            this.tsmiMoveFirst.Size = new System.Drawing.Size(180, 22);
            this.tsmiMoveFirst.Text = "移到最前";
            this.tsmiMoveFirst.Click += new System.EventHandler(this.tsmiMoveFirst_Click);
            // 
            // tsmiMoveLast
            // 
            this.tsmiMoveLast.Name = "tsmiMoveLast";
            this.tsmiMoveLast.Size = new System.Drawing.Size(180, 22);
            this.tsmiMoveLast.Text = "移到最后";
            this.tsmiMoveLast.Click += new System.EventHandler(this.tsmiMoveLast_Click);
            // 
            // tsmiMoveBefore
            // 
            this.tsmiMoveBefore.Name = "tsmiMoveBefore";
            this.tsmiMoveBefore.Size = new System.Drawing.Size(180, 22);
            this.tsmiMoveBefore.Text = "前移一位";
            this.tsmiMoveBefore.Click += new System.EventHandler(this.tsmiMoveBefore_Click);
            // 
            // tsmiMoveBack
            // 
            this.tsmiMoveBack.Name = "tsmiMoveBack";
            this.tsmiMoveBack.Size = new System.Drawing.Size(180, 22);
            this.tsmiMoveBack.Text = "后移一位";
            this.tsmiMoveBack.Click += new System.EventHandler(this.tsmiMoveBack_Click);
            // 
            // ShortCutItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.lblMenuName);
            this.Name = "ShortCutItem";
            this.Size = new System.Drawing.Size(127, 78);
            this.Load += new System.EventHandler(this.ShortCutItem_Load);
            this.MouseEnter += new System.EventHandler(this.ShortCutItem_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.ShortCutItem_MouseLeave);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMenuName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancelShortCutMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiMoveFirst;
        private System.Windows.Forms.ToolStripMenuItem tsmiMoveLast;
        private System.Windows.Forms.ToolStripMenuItem tsmiMoveBefore;
        private System.Windows.Forms.ToolStripMenuItem tsmiMoveBack;
    }
}
