using System;
using System.ComponentModel;

namespace idongG.Domec.PlcDA.ToolManage
{
    /// <summary>
    /// 抽象工具类，实现ITool接口的基本功能
    /// </summary>
    public abstract class ATool : ITool
    {
        /// <summary>
        /// 工具启用状态
        /// </summary>
        public bool IsEnabled { get; set; }

        private bool _isRunning;

        /// <summary>
        /// 工具运行状态
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// 初始化工具
        /// </summary>
        public virtual void Initialize()
        {
            // 基本初始化逻辑
            IsEnabled = true;
        }

        /// <summary>
        /// 启动工具
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 停止工具
        /// </summary>
        public virtual void Stop()
        {
            // 基本停止逻辑
            _isRunning = false;
        }

        /// <summary>
        /// 重命名工具
        /// </summary>
        /// <param name="newName">新名称</param>
        /// <returns>重命名成功返回true，名称重复或无效返回false</returns>
        public virtual bool Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return false;

            NickName = newName;
            return true;
        }

        public string Type { get; set; }
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }

        public abstract void Dispose();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)  //属性变更通知
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception)
            {
            }
        }
    }
}