using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.TmallChaoShi.Result
{
    /// <summary>
    /// 商品详细信息结果
    /// </summary>
    public class ProductSetupResult
    {
        public Api Api { get; set; }

        public DetailsResult Detail { get; set; }

        private string _initApi;
        public string InitApi
        {
            get
            {
                return _initApi.GetFullLink();
            }
            set
            {
                _initApi = value;
            }
        }

        private string _initExtensionApi;
        public string InitExtensionApi
        {
            get
            {
                return _initExtensionApi.GetFullLink();
            }
            set
            {
                _initExtensionApi = value;
            }
        }

        public ItemDO ItemDO { get; set; }
    }

    public class Api
    {
        private string _descUrl;
        public string DescUrl
        {
            get
            {
                return _descUrl.GetFullLink();
            }
            set
            {
                _descUrl = value;
            }
        }

        private string _fetchDcUrl;
        public string FetchDcUrl
        {
            get
            {
                return _fetchDcUrl.GetFullLink();
            }
            set
            {
                _fetchDcUrl = value;
            }
        }

        private string _httpsDescUrl;
        public string HttpsDescUrl
        {
            get
            {
                return _httpsDescUrl.GetFullLink();
            }
            set
            {
                _httpsDescUrl = value;
            }
        }
    }

    public class DetailsResult
    {
        public string DefaultItemPrice { get; set; }
    }

    public class ItemDO
    {
        public string BrandId { get; set; }

        public string CategoryId { get; set; }

        public string ItemId { get; set; }

        public string ReservePrice { get; set; }

        public string RootCatId { get; set; }

        public string SpuId { get; set; }

        public string Title { get; set; }

        public string Weight { get; set; }
    }
}
