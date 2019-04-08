﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntDeployWinform.Util;

namespace AntDeployWinform.Winform
{
    public partial class RollBack : Form
    {
        public RollBack(List<string> list)
        {
            InitializeComponent();

            Assembly assembly = typeof(Deploy).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("AntDeployWinform.Resources.Logo1.ico"))
            {
                if (stream != null) this.Icon = new Icon(stream);
            }


            this.listbox_rollback_list.Items.Clear();;
            foreach (var li in list)
            {
                var label = string.Empty;
                var content = li.JsonToObject<RollBackVersion>();
                if (!string.IsNullOrEmpty(content.Version))
                {
                    label = content.Version;
                }

                if (content.FormItemList != null && content.FormItemList.Any())
                {
                    var remark = content.FormItemList.FirstOrDefault(r => r.FieldName.Equals("remark"));
                    if (remark != null&&!string.IsNullOrEmpty(remark.TextValue))
                    {
                        label += remark.TextValue;
                    }
                }
                this.listbox_rollback_list.Items.Add(label);
            }

            SelectRollBackVersion = string.Empty;
        }


        public string SelectRollBackVersion { get; set; }

        private void RollBack_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectRollBackVersion))
            {
                this.DialogResult = DialogResult.Cancel;
            }
            
        }

        private void b_rollback_Rollback_Click(object sender, EventArgs e)
        {
            var selectItem = this.listbox_rollback_list.SelectedItem as string;
            if (string.IsNullOrEmpty(selectItem))
            {
                MessageBox.Show("please select rollback version!");
                return;
            }

            SelectRollBackVersion = selectItem;
            this.DialogResult = DialogResult.OK;
        }

        public void SetButtonName(string name)
        {
            this.b_rollback_Rollback.Text = name;
        }
    }

    class RollBackVersion
    {

        private string _args;
        public string Version { get; set; }

        public string Args
        {
            get { return this._args; }
            set
            {
                _args = value;
                if(!string.IsNullOrEmpty(value))this.FormItemList = value.JsonToObject<List<FormItem>>();
            }
        }

        public List<FormItem> FormItemList { get; set; } = new List<FormItem>();
    }

    /// <summary>
    /// 表单元素对象
    /// </summary>
    class FormItem
    {
        /// <summary>
        /// 字段名(表单域名称)
        /// </summary>
        public string FieldName;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName;

       

        /// <summary>
        /// 文本内容
        /// </summary>
        public string TextValue;
    }

}
