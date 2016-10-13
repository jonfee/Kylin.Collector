using ProductCollector.BackState;
using ProductCollector.Core;
using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductCollector
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 定义锁
        /// </summary>
        readonly object locker = new object();

        /// <summary>
        /// 当前采集站
        /// </summary>
        CollectorSite site;

        /// <summary>
        /// 当前采集服务
        /// </summary>
        ICollectorService service;

        /// <summary>
        /// 当前采集站下的所有分类
        /// </summary>
        IEnumerable<TempCategory> Categories;

        /// <summary>
        /// 需要采集的分类
        /// </summary>
        IEnumerable<TempCategory> NeedCollectCategories;

        public MainForm()
        {
            InitializeComponent();

            bindData();

            NeedCollectCategories = new List<TempCategory>();

            useForm = UseForm;
        }

        void bindData()
        {
            //Form
            this.Text = $"产品采集器（v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}）";

            var sites = CollectorConfig.Instance.SiteList;
            sites.Insert(0, new CollectorSite { Type = 0, Name = "请选择", Url = string.Empty });

            //采集站点下拉框
            this.cbSourceSite.ValueMember = "type";
            this.cbSourceSite.DisplayMember = "name";
            this.cbSourceSite.DataSource = sites;

            //启动服务按钮不可用（初始化）
            this.btnStart.Enabled = false;
            //停止服务按钮不可用（初始化）
            this.btnStop.Enabled = false;

            //更新商品分类按钮不可用（初始化）
            this.lkUpdateCategory.Visible = false;
        }

        /// <summary>
        /// 采集站选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSourceSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            site = this.cbSourceSite.SelectedItem as CollectorSite;

            switch (site.Type)
            {
                case 1: //天猫超市
                    service = new TmallChaoShi.TmallChaoShiService();
                    break;
            }

            if (service != null)
            {
                Categories = service.GetLocationCategories();

                useForm(BindCategory);
            }
        }

        /// <summary>
        /// 采集服务开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (NeedCollectCategories.Any())
            {
                service.Start(this.NeedCollectCategories, ServiceCallback);
            }
            else
            {
                MessageBox.Show("请选择需要采集的商品分类");
            }

            CheckServiceStatus();
        }

        /// <summary>
        /// 停止采集服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            service.Stop(ServiceCallback);

            CheckServiceStatus();
        }

        /// <summary>
        /// 从采集站更新商品分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkUpdateCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Categories = service.UpdateCategories();

            useForm(BindCategory);
        }

        private void CheckServiceStatus()
        {
            if (service != null)
            {
                bool running = service.IsRunning();

                if (running)
                {
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = true;
                }
                else
                {
                    this.btnStart.Enabled = true;
                    this.btnStop.Enabled = false;
                }
            }
        }

        #region 绑定商品分类到控件
        /// <summary>
        /// 绑定商品分类到控件
        /// </summary>
        private void BindCategory()
        {
            lock (locker)
            {
                this.panelCategory.Controls.Clear();

                int row = 0; int itemW = 130; int itemH = 20; int deepPadW = 20;

                int prevDeep = 0;

                Action<TempCategory, int> addCheckBox = null;

                addCheckBox = (cat, idx) =>
                {
                    //层级深度
                    int deep = cat.Layer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Count();

                    CheckBox ckbox = new CheckBox();
                    ckbox.Click += new EventHandler(categoryChanged);
                    ckbox.Width = deep < 3 ? itemW * 2 : itemW;
                    ckbox.Text = cat.Name;
                    ckbox.Tag = cat.Layer;

                    int px = 0; int py = 0;

                    if (deep < 3)
                    {
                        row++;
                        px = (deep - 1) * deepPadW;
                    }
                    else
                    {
                        if (prevDeep != deep || idx % 5 == 0) row++;
                        px = deepPadW * 2 + (idx % 5) * itemW;
                    }

                    py = (row - 1) * itemH;

                    ckbox.Location = new Point(px, py);

                    this.panelCategory.Controls.Add(ckbox);

                    prevDeep = deep;

                    if (cat.Child != null && cat.Child.Length > 0)
                    {
                        for (var i = 0; i < cat.Child.Length; i++)
                        {
                            addCheckBox(cat.Child[i], i);
                        }
                    }
                };

                for (var i = 0; i < Categories.Count(); i++)
                {
                    addCheckBox(Categories.ElementAt(i), i);
                }

                this.lkUpdateCategory.Visible = true;
            }
        }

        /// <summary>
        /// 商品分类CheckBox选择后处理
        /// 1、父子分类联动选择或取消
        /// 2、重新统计需要采集的分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void categoryChanged(object sender, EventArgs e)
        {
            //重新统计需要采集的分类
            var tempCollectorCategoies = new List<TempCategory>();

            var ckbox = (CheckBox)sender;

            if (ckbox == null) return;

            string tag = ckbox.Tag.ToString();

            var siblings = this.panelCategory.Controls;

            foreach (var control in siblings)
            {
                var tempCkBox = control as CheckBox;

                if (tempCkBox == null) continue;

                string _tag = tempCkBox.Tag.ToString();

                //子分类全选或取消
                if (_tag.StartsWith(tag)) tempCkBox.Checked = ckbox.Checked;

                //如果为取消，则父类取消
                if (ckbox.Checked == false && tag.StartsWith(_tag))
                {
                    tempCkBox.Checked = false;
                }

                //重新统计需要采集的分类
                if (tempCkBox.Checked)
                {
                    var layers = _tag.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (layers.Length > 2)
                    {
                        //当前分类ID
                        var catId = long.Parse(layers.LastOrDefault());

                        var cat = findTempCategory(catId);

                        if (cat != null) tempCollectorCategoies.Add(cat);
                    }
                }
            }

            NeedCollectCategories = tempCollectorCategoies;

            this.btnStart.Enabled = NeedCollectCategories.Any();
        }

        /// <summary>
        /// 这是专门为寻找分类准备的
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private TempCategory findTempCategory(long categoryId)
        {
            //定义查找方法
            var find = Tools.Fix<TempCategory, TempCategory>(f => (cat) =>
             {
                 TempCategory temp = null;

                 if (cat.CategoryId == categoryId)
                 {
                     temp = cat;
                 }
                 else if (cat.Child != null)
                 {
                     foreach (var c in cat.Child)
                     {
                         temp = f(c);
                         if (temp != null) break;
                     }
                 }

                 return temp;
             });

            TempCategory result = null;

            foreach (var cat in Categories)
            {
                result = find(cat);
                if (result != null) break;
            }

            return result;
        }

        #endregion

        #region 抓取服务回调方法

        /// <summary>
        /// 抓取服务回调方法
        /// </summary>
        /// <param name="state">回调对象</param>
        private void ServiceCallback(CallBackState state)
        {
            if (state is MessageState)
            {
                WriteMessage(state as MessageState);
            }
            else if (state is StatisticsState)
            {
                WriteStatistics(state as StatisticsState);
            }
        }

        /// <summary>
        /// 输出统计信息
        /// </summary>
        /// <param name="state"></param>
        void WriteStatistics(StatisticsState state)
        {
            if (state == null) return;

            useForm(() =>
            {
                string txt = $"采集结果：{state.FinishCategories}/{state.TotalCategories}个分类，{state.FinishProducts}/{state.TotalProducts}条商品数据";

                this.lbStatistics.Text = txt;
            });
        }

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="state"></param>
        void WriteMessage(MessageState state)
        {
            if (state == null) return;

            useForm(() =>
            {
                if (state.PadTime)
                {
                    state.Text += DateTime.Now.ToString(" --yyyy-MM-dd HH:mm:ss");
                }
                this.rtxtMsg.AppendText(state.Text);
                this.rtxtMsg.AppendText("\n");
                this.rtxtMsg.Focus();
            });
        }

        #endregion

        #region 支持异步线程和多线程中使用窗体控件

        /// <summary>
        /// 委托变量（使用窗体，并执行操作）
        /// </summary>
        private UseFormDelegate useForm;

        /// <summary>
        /// 定义委托（使用窗体，并执行操作）
        /// </summary>
        /// <param name="action"></param>
        private delegate void UseFormDelegate(Action action);

        /// <summary>
        /// 使用窗体，并执行操作
        /// </summary>
        /// <param name="action"></param>
        private void UseForm(Action action)
        {
            new Task(() =>
            {
                this.Invoke((EventHandler)delegate
                {
                    action();
                });
            }).Start();
        }

        #endregion
    }
}
