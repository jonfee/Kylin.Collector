using ProductCollector.BackState;
using ProductCollector.Core;
using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
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

        int _categoriesCount = -1;
        /// <summary>
        /// 当前采集服务的商品分类数
        /// </summary>
        int CategoriesCount
        {
            get
            {
                if (_categoriesCount < 0)
                {
                    int count = 0;

                    var sum = Tools.Fix<IEnumerable<TempCategory>, int>(s => (data) =>
                    {
                        foreach (var cat in data)
                        {
                            if (cat == null) continue;

                            count++;

                            if (cat.Child != null && cat.Child.Count() > 0)
                            {
                                s(cat.Child);
                            }
                        }

                        return count;
                    });

                    _categoriesCount = sum(Categories);
                }

                return _categoriesCount;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            bindData();

            NeedCollectCategories = new List<TempCategory>();
        }

        /// <summary>
        /// 初始化时绑定
        /// </summary>
        void bindData()
        {
            Writer.OutputForm = this;
            Writer.write = serviceCallback;

            //Form
            this.Text = $"产品采集器（v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}）";

            var sites = CollectorConfig.Instance.SiteList;
            sites.Insert(0, new CollectorSite { Type = 0, Name = "请选择", Url = string.Empty });

            //采集站点下拉框
            this.cbSourceSite.ValueMember = "type";
            this.cbSourceSite.DisplayMember = "name";
            this.cbSourceSite.DataSource = sites;

            this.btnLoadLocationCategory.Visible = false;
            this.btnDownloadCategory.Visible = false;

            //启动服务按钮不可用（初始化）
            this.btnStart.Enabled = false;
            //停止服务按钮不可用（初始化）
            this.btnStop.Enabled = false;
        }

        /// <summary>
        /// 采集站选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSourceSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (site != null) resetProgressBar();

            var currentSite = this.cbSourceSite.SelectedItem as CollectorSite;

            if (site == null || site.Name != currentSite.Name)
            {
                service = null;
                this.Categories = new List<TempCategory>();
                this.NeedCollectCategories = new List<TempCategory>();
                this.panelCategory.Controls.Clear();

                switch (currentSite.Type)
                {
                    case 1: //天猫超市
                        service = new TmallChaoShi.TmallChaoShiService();
                        break;
                }

                site = currentSite;
            }

            checkLoadCategory();
            checkServiceStatus();
        }

        /// <summary>
        /// 从本地加载商品分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadLocationCategory_Click(object sender, EventArgs e)
        {
            resetProgressBar();

            if (service != null)
            {
                Writer.writeInvoke(new MessageState { Text = "从本地加载商品分类……" });
                
                Categories = service.GetLocationCategories();

                bindCategory();

                Writer.writeInvoke(new MessageState { Text = "加载完成！" });
            }
            else
            {
                Writer.writeInvoke(new MessageState { Text = "服务异常，无法从本地加载商品分类！" });
            }
        }

        /// <summary>
        /// 从远程采集站下载最新商品分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownloadCategory_Click(object sender, EventArgs e)
        {
            resetProgressBar();

            if (service != null)
            {
                Writer.writeInvoke(new MessageState { Text = "从远程下载商品分类……" });

                Categories = service.UpdateCategories();

                bindCategory();

                Writer.writeInvoke(new MessageState { Text = "下载完成！" });
            }
            else
            {
                Writer.writeInvoke(new MessageState { Text = "服务异常，无法从远程下载商品分类！" });
            }
        }

        /// <summary>
        /// 采集服务开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            resetProgressBar();

            if (NeedCollectCategories.Any())
            {
                service.Start(this.NeedCollectCategories);
            }
            else
            {
                MessageBox.Show("请选择需要采集的商品分类");
            }

            checkServiceStatus();
        }

        /// <summary>
        /// 停止采集服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            resetProgressBar();

            service.Stop();

            checkServiceStatus();
        }

        /// <summary>
        /// 检验商品分类加载
        /// </summary>
        void checkLoadCategory()
        {
            if (service != null)
            {
                this.btnLoadLocationCategory.Visible = true;
                this.btnDownloadCategory.Visible = true;
            }
            else
            {
                this.btnLoadLocationCategory.Visible = false;
                this.btnDownloadCategory.Visible = false;
            }
        }

        /// <summary>
        /// 检验采集服务状态
        /// </summary>
        void checkServiceStatus()
        {
            if (service != null)
            {
                bool running = service.IsRunning();

                if (running)
                {
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = true;
                    this.cbSourceSite.Enabled = false;
                    this.btnLoadLocationCategory.Enabled = false;
                    this.btnDownloadCategory.Enabled = false;
                    this.panelCategory.Enabled = false;
                }
                else if (NeedCollectCategories.Any())
                {
                    this.btnStart.Enabled = true;
                    this.btnStop.Enabled = false;
                    this.cbSourceSite.Enabled = true;
                    this.btnLoadLocationCategory.Enabled = true;
                    this.btnDownloadCategory.Enabled = true;
                    this.panelCategory.Enabled = true;
                }
            }
            else
            {
                this.cbSourceSite.Enabled = true;
            }
        }

        /// <summary>
        /// 重置进度条
        /// </summary>
        void resetProgressBar()
        {
            Writer.writeInvoke(new ProgressState { Max = 0, Value = 0 });
        }

        #region 绑定商品分类到控件
        /// <summary>
        /// 绑定商品分类到控件
        /// </summary>
        void bindCategory()
        {
            _categoriesCount = -1;

            this.btnLoadLocationCategory.Enabled = this.btnDownloadCategory.Enabled = false;

            this.panelCategory.Controls.Clear();

            lock (locker)
            {
                int count = 0;

                int row = 0; int itemW = 130; int itemH = 20; int deepPadW = 20;

                int prevDeep = 0;

                Action<TempCategory, int> addCheckBox = null;

                addCheckBox = (cat, idx) =>
                {
                    Writer.writeInvoke(new MessageState { Text = $"加载：{cat.Name}" });

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
                    Writer.writeInvoke(new BackState.ProgressState { Max = CategoriesCount, Value = ++count });
                };

                for (var i = 0; i < Categories.Count(); i++)
                {
                    addCheckBox(Categories.ElementAt(i), i);
                }
            }

            this.btnLoadLocationCategory.Enabled = this.btnDownloadCategory.Enabled = true;
        }

        /// <summary>
        /// 商品分类CheckBox选择后处理
        /// 1、父子分类联动选择或取消
        /// 2、重新统计需要采集的分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void categoryChanged(object sender, EventArgs e)
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
        TempCategory findTempCategory(long categoryId)
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
        void serviceCallback(CallBackState state)
        {
            if (state is MessageState)
            {
                writeMessage(state as MessageState);
            }
            else if (state is StatisticsState)
            {
                writeStatistics(state as StatisticsState);
            }
            else if (state is ProgressState)
            {
                writeProgressBar(state as ProgressState);
            }
        }

        /// <summary>
        /// 输出进度
        /// </summary>
        /// <param name="state"></param>
        void writeProgressBar(ProgressState state)
        {
            if (state == null) return;

            if (state.Value > state.Max) state.Value = state.Max;

            this.progressBar.Maximum = state.Max;
            this.progressBar.Value = state.Value;
        }

        /// <summary>
        /// 输出统计信息
        /// </summary>
        /// <param name="state"></param>
        void writeStatistics(StatisticsState state)
        {
            if (state == null) return;

            string txt = $"商品采集：{state.FinishProducts} / {state.TotalProducts}";

            this.lbStatistics.Text = txt;
        }

        /// <summary>
        /// 输出消息
        /// </summary>
        /// <param name="state"></param>
        void writeMessage(MessageState state)
        {
            if (state == null) return;

            if (state.PadTime)
            {
                state.Text += DateTime.Now.ToString(" --yyyy-MM-dd HH:mm:ss");
            }
            this.rtxtMsg.AppendText(state.Text);
            this.rtxtMsg.AppendText("\n");
            this.rtxtMsg.Focus();
        }

        #endregion
    }
}
