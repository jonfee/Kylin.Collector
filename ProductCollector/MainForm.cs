using ProductCollector.Collector;
using ProductCollector.Data;
using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductCollector
{
    public partial class MainForm : Form
    {
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

                BindCategory();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (NeedCollectCategories.Any())
            {
                MessageBox.Show(string.Join(",", NeedCollectCategories.Select(p => p.Name)));
            }
            else
            {
                MessageBox.Show("请选择需要采集的商品分类");
            }
        }

        /// <summary>
        /// 从采集站更新商品分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkUpdateCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Categories = service.UpdateCategories();

            BindCategory();
        }

        /// <summary>
        /// 绑定商品分类到控件
        /// </summary>
        private void BindCategory()
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
        }

        /// <summary>
        /// 这是专门为寻找分类准备的
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private TempCategory findTempCategory(long categoryId)
        {
            //定义查找方法
            var find = Fix<TempCategory, TempCategory>(f => (cat) =>
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

        /// <summary>
        /// 不动点算子函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        Func<T, TResult> Fix<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> g)
        {
            return (x) => g(Fix(g))(x);
        }
    }
}
