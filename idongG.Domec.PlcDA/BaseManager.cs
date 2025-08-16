using idongG.Domec.PlcDA.Save;
using Newtonsoft.Json;
using idongG.Domec.PlcDA.Logs;

namespace idongG.Domec.PlcDA;

/// <summary>
/// 管理器基类，提供通用的管理功能
/// </summary>
/// <typeparam name="T">管理的对象类型</typeparam>
public abstract class BaseManager : IDisposable
{
    private bool _disposed = false;
    public string SavePath { get; set; }
    private List<IEntity> _itemList = new List<IEntity>();
    public INewLog Log { get; set; }

    /// <summary>
    /// 带日志记录器的构造函数
    /// </summary>
    /// <param name="logger">日志记录器实例</param>
    public BaseManager(INewLog logger)
    {
        Log = logger;
    }

    /// <summary>
    /// 添加项目
    /// </summary>
    /// <param name="item">要添加的项目</param>
    /// <returns>添加成功返回true，项目已存在返回false</returns>
    public bool Add(IEntity item)
    {
        if (_itemList.Contains(item)) return false;
        _itemList.Add(item);
        return true;
    }

    /// <summary>
    /// 按名称添加项目
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="nickName"></param>
    /// <returns></returns>
    public bool Add<T>(string nickName) where T : IEntity, new()
    {
        if (_itemList.Any(i => i.NickName == nickName))
        {
            Log.Add($"项目 {nickName} 已存在，无法添加重复项目。");
            return false;
        }
        var item = new T { NickName = nickName };
        return Add(item);
    }

    /// <summary>
    /// 删除项目
    /// </summary>
    /// <param name="item">要删除的项目</param>
    /// <returns>删除成功返回true，项目不存在返回false</returns>
    public bool Remove(IEntity item) => _itemList.Remove(item);

    /// <summary>
    /// 根据名称删除项目
    /// </summary>
    /// <param name="name">要删除的项目名称</param>
    /// <returns>删除成功返回true，项目不存在返回false</returns>
    public bool Remove(string nickName)
    {
        var item = _itemList.FirstOrDefault(i => i.NickName == nickName);
        if (item != null) return Remove(item);
        return false;
    }

    /// <summary>
    /// 获取所有项目
    /// </summary>
    /// <returns>所有项目的列表</returns>
    public List<IEntity> GetAll() => _itemList;

    /// <summary>
    /// 将项目向上移动
    /// </summary>
    /// <param name="item">项目</param>
    /// <returns>移动成功返回true，项目不存在或已在顶部返回false</returns>
    public bool MoveUp(IEntity item)
    {
        var index = _itemList.IndexOf(item);
        if (index <= 0 || index >= _itemList.Count)
        {
            return false;
        }
        _itemList.RemoveAt(index);
        _itemList.Insert(index - 1, item);
        return true;
    }

    /// <summary>
    /// 将项目向下移动
    /// </summary>
    /// <param name="item">项目</param>
    /// <returns>移动成功返回true，项目不存在或已在底部返回false</returns>
    public bool MoveDown(IEntity item)
    {
        var index = _itemList.IndexOf(item);
        if (index < 0 || index >= _itemList.Count - 1)
        {
            return false;
        }
        _itemList.RemoveAt(index);
        _itemList.Insert(index + 1, item);
        return true;
    }

    /// <summary>
    /// 将项目移动到指定位置
    /// </summary>
    /// <param name="item">项目</param>
    /// <param name="newIndex">新位置索引</param>
    /// <returns>移动成功返回true，项目不存在或索引无效返回false</returns>
    public bool MoveToPosition(IEntity item, int newIndex)
    {
        var currentIndex = _itemList.IndexOf(item);
        if (currentIndex < 0 || newIndex < 0 || newIndex >= _itemList.Count)
        {
            return false;
        }
        _itemList.RemoveAt(currentIndex);
        _itemList.Insert(newIndex, item);
        return true;
    }

    /// <summary>
    /// 获取所有项目名称列表
    /// </summary>
    /// <returns>所有项目名称的列表</returns>
    public IEnumerable<string> GetAllNames() => _itemList.Select(item => item.NickName);

    /// <summary>
    /// 根据名称或ID获取IEntity对象
    /// </summary>
    /// <param name="identifier">项目名称或ID</param>
    /// <returns>找到的IEntity对象，未找到返回null</returns>
    public IEntity GetEntityByNameOrId(string idOrName)
    {
        // 首先尝试按名称查找
        var item = _itemList.FirstOrDefault(i => i.NickName == idOrName);
        if (item != null) return item;

        // 如果按名称未找到，尝试按ID查找
        if (Guid.TryParse(idOrName, out Guid id))
        {
            return _itemList.FirstOrDefault(i => i.Id == id);
        }

        return null;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <returns></returns>
    public virtual bool Save()
    {
        var fileInfo = new FileInfo(SavePath);
        var SaveData = new SaveDataClass() { Items = _itemList };
        var b = SaveDataClass.SaveJson(fileInfo, SaveData);
        if (b == true)
        {
            //StaticFunction.ShowSuccessTip($" {NickName}保存成功!");
        }
        return b;
    }

    /// <summary>
    /// 载入
    /// </summary>
    /// <returns></returns>
    public virtual bool Load()
    {
        var fileInfo = new FileInfo(SavePath);

        var d = SaveDataClass.LoadJson<SaveDataClass>(fileInfo);
        if (d == null)
        {
            return true;
        }
        Dispose();
        _itemList = d.Items;

        return true;
    }

    /// <summary>
    /// 销毁对象，实现IDisposable接口
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 销毁对象
    /// </summary>
    /// <param name="disposing">是否正在销毁托管资源</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // 销毁托管资源
                if (_itemList != null)
                {
                    foreach (var item in _itemList)
                    {
                        if (item is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                    _itemList.Clear();
                }
            }

            // 释放非托管资源

            _disposed = true;
        }
    }

    /// <summary>
    /// 终结器
    /// </summary>
    ~BaseManager()
    {
        Dispose(false);
    }
}