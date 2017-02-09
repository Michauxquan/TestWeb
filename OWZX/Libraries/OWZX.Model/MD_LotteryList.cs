﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Model
{
    public class MD_LotteryList
    {
        /// <summary>
        /// 最新竞猜期号
        /// </summary>
        public string LastNumber { get; set; }
        /// <summary>
        /// 剩余时间(S)
        /// </summary>
        public int RemainS { get; set; }
        /// <summary>
        /// 今日盈亏
        /// </summary>
        public int TdProf { get; set; }
        /// <summary>
        /// 今日参与期数
        /// </summary>
        public int TdBettNum { get; set; }
        /// <summary>
        /// 胜率
        /// </summary>
        public decimal WinPercent { get; set; }
        /// <summary>
        /// 上一期结果
        /// </summary>
        public MD_Lottery Prev_Lottery { get; set; }
        /// <summary>
        /// 往期开奖数据
        /// </summary>
        public List<MD_Lottery> LotteryList { get; set; }
    }

    public enum LotteryType
    {
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        dd28 = 1,
        /// <summary>
        /// 加拿大28
        /// </summary>
        cakeno28 = 2,
        /// <summary>
        /// 急速28
        /// </summary>
        js28 = 3,
        /// <summary>
        /// 蛋蛋36
        /// </summary>
        dd36 = 4,
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        cakeno36 = 5,
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        hg28 = 6,
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        pkgj = 7,
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        pkgyj = 8,
        /// <summary>
        /// 蛋蛋28
        /// </summary>
        ddlhb = 9
    }
}