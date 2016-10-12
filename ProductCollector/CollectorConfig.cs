using ProductCollector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProductCollector
{
    public class CollectorConfig
    {
        private static CollectorConfig _instance;

        private static readonly object Locker = new object();

        public static CollectorConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new CollectorConfig();
                        }
                    }
                }

                return _instance;
            }
        }

        private CollectorConfig()
        {
            SiteList = new List<CollectorSite>();

            //加载采集器配置信息xml文档
            string xmlPath = Path.Combine(AppContext.BaseDirectory, "collector.xml");
            var xml = XElement.Load(xmlPath);

            //采集站点集合
            var collectors = from em in xml.Elements("collector") select em;

            //遍历采集站点
            foreach (var c in collectors)
            {
                var type = int.Parse(c.Attribute("type").Value);
                var name = c.Attribute("name").Value;
                var url = c.Attribute("url").Value;

                SiteList.Add(new CollectorSite { Type = type, Name = name, Url = url });
            }
        }

        public List<CollectorSite> SiteList;


    }
}
