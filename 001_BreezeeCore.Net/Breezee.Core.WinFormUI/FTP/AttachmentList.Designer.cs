﻿namespace Breezee.Core.WinFormUI
{
    partial class AttachmentList
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
            this.pnlAttachList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // pnlAttachList
            // 
            this.pnlAttachList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAttachList.Location = new System.Drawing.Point(0, 0);
            this.pnlAttachList.Name = "pnlAttachList";
            this.pnlAttachList.Size = new System.Drawing.Size(175, 64);
            this.pnlAttachList.TabIndex = 0;
            // 
            // AttachmentList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.pnlAttachList);
            this.Name = "AttachmentList";
            this.Size = new System.Drawing.Size(175, 64);
            this.Load += new System.EventHandler(this.AttachmentList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlAttachList;

    }
}
