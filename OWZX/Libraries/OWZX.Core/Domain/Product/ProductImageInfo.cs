using System;

namespace OWZX.Core
{
    /// <summary>
    /// 商品图片信息类
    /// </summary>
    public class ProductImageInfo
    {
        private int _pimgid;//商品图片id
        private int _pid;//商品id
        private string _showimg;//商品图片
        private int _ismain;//是否为主图
        private int _displayorder;//商品图片排序
        private string _description = "";//商品描述
        private string _productimgs = "";//商品图片

        /// <summary>
        /// 商品图片id
        /// </summary>
        public int PImgId
        {
            get { return _pimgid; }
            set { _pimgid = value; }
        }
        /// <summary>
        /// 商品id
        /// </summary>
        public int Pid
        {
            get { return _pid; }
            set { _pid = value; }
        }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ShowImg
        {
            get { return _showimg; }
            set { _showimg = value; }
        }
        /// <summary>
        /// 是否为主图
        /// </summary>
        public int IsMain
        {
            get { return _ismain; }
            set { _ismain = value; }
        }
        /// <summary>
        /// 商品图片排序
        /// </summary>
        public int DisplayOrder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImgs
        {
            set { _productimgs = value; }
            get { return _productimgs; }
        }
    }
}
