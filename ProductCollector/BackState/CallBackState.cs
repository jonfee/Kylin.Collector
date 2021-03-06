﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCollector.BackState
{
    /// <summary>
    /// 回调对象类型
    /// </summary>
    public enum StateType
    {
        /// <summary>
        /// 消息
        /// </summary>
        Message,
        /// <summary>
        /// 作业进度
        /// </summary>
        Progress,
        /// <summary>
        /// 统计
        /// </summary>
        Statistics
    }

    /// <summary>
    /// 回调对象抽象基类
    /// </summary>
    public abstract class CallBackState
    {
        public abstract StateType Type { get; }
    }

    /// <summary>
    /// 消息回调对象
    /// </summary>
    public class MessageState : CallBackState
    {
        public override StateType Type => StateType.Message;

        /// <summary>
        /// 消息文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否需要填充时间
        /// </summary>
        public bool PadTime { get; set; }

        private Color _color;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color Color
        {
            get
            {
                if (_color == null) _color = Color.Black;

                return _color;
            }
            set
            {
                _color = value;
            }
        }
    }

    /// <summary>
    /// 进度回调对象
    /// </summary>
    public class ProgressState : CallBackState
    {
        public override StateType Type => StateType.Progress;

        /// <summary>
        /// 最大总值
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// 抓取统计回调对象
    /// </summary>
    public class StatisticsState : CallBackState
    {
        public override StateType Type => StateType.Statistics;

        /// <summary>
        /// 总商品数
        /// </summary>
        public int TotalProducts { get; set; }

        /// <summary>
        /// 已完成商品数
        /// </summary>
        public int FinishProducts { get; set; }
    }
}
