using IQF.Framework.IModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IQF.Framework.Util
{
	/// <summary>
	/// 增量数据加载器
	/// </summary>
	public class IncrDataLoader<TData>
	{
		private readonly IIncrDataSource<TData> incrData;

		private Dictionary<string, TData> datas = new Dictionary<string, TData>();

		private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

		private Thread thread = null;

		public IncrDataLoader(IIncrDataSource<TData> incrData)
		{
			this.incrData = incrData;
			this.IncrLoadInternal = 5 * 1000;
			this.TimesOnFullLoad = 60;
		}

		/// <summary>
		/// 增量加载间隔时间，单位：毫秒，默认5秒
		/// </summary>
		public int IncrLoadInternal { get; set; }

		/// <summary>
		/// 执行TimesOnFullLoad次增量加载后，开始执行全量加载，默认60次
		/// </summary>
		public int TimesOnFullLoad { get; set; }

		/// <summary>
		/// 加载数据时发生异常
		/// </summary>
		public event Action<Exception> OnLoadException;

		/// <summary>
		/// 执行增量加载
		/// </summary>
		/// <returns></returns>
		public bool Execute()
		{
			if (this.thread != null)
			{
				return true;
			}
			this.LoadOnce(0);//预先加载一次，防止立刻获取数据时无法获取数据
			thread = new Thread(ExecuteDataLoad);
			thread.Name = "thIncrDataLoader";
			thread.IsBackground = true;
			thread.Start();
			return true;
		}

		/// <summary>
		/// 获取当前所有数据
		/// </summary>
		/// <returns></returns>
		public List<TData> GetAll()
		{
			this.rwLock.EnterReadLock();
			try
			{
				return this.datas.Values.ToList();
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
		}

		/// <summary>
		/// 根据过滤条件获取数据
		/// </summary>
		/// <param name="filterCondition">过滤条件</param>
		/// <returns></returns>
		public List<TData> Get(Func<TData, bool> filterCondition)
		{
			this.rwLock.EnterReadLock();
			try
			{
				return this.datas.Values.Where(filterCondition).ToList();
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
		}

		/// <summary>
		/// 通过键值获取
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public TData Get(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return default(TData);
			}
			this.rwLock.EnterReadLock();
			try
			{
				if (this.datas.ContainsKey(key))
				{
					return this.datas[key];
				}
				return default(TData);
			}
			finally
			{
				this.rwLock.ExitReadLock();
			}
		}

		/// <summary>
		/// 获取主键
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string GetKey(TData data)
		{
			return this.incrData.GetKey(data);
		}

		/// <summary>
		/// 删除特定缓存
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			this.rwLock.EnterWriteLock();
			try
			{
				return this.datas.Remove(key);
			}
			finally
			{
				this.rwLock.ExitWriteLock();
			}
		}

		/// <summary>
		/// 执行数据加载操作
		/// </summary>
		private void ExecuteDataLoad()
		{
			var count = 1;//已进行预加载，跳过第一次全量加载
			while (true)
			{
				Thread.Sleep(this.IncrLoadInternal);
				try
				{
					LoadOnce(count);
				}
				catch (Exception e)
				{
					if (this.OnLoadException != null)
					{
						this.OnLoadException(e);
					}
				}
				finally
				{
					count++;
				}
			}
		}

		/// <summary>
		/// 加载一次
		/// </summary>
		/// <param name="count"></param>
		private void LoadOnce(int count)
		{
			if (count % TimesOnFullLoad == 0)
			{
				var all = this.incrData.LoadAll() ?? new List<TData>();
				var dict = all.ToDictionary(k => this.incrData.GetKey(k), v => v);
				this.rwLock.EnterWriteLock();
				try
				{
					this.datas = dict;
				}
				finally
				{
					this.rwLock.ExitWriteLock();
				}
			}
			else
			{
				var incrDatas = this.incrData.IncrLoad(this.datas.Values);
				this.rwLock.EnterWriteLock();
				try
				{
					foreach (var item in incrDatas)
					{
						var key = this.incrData.GetKey(item);
						if (this.datas.ContainsKey(key))
						{
							this.datas[key] = item;
						}
						else
						{
							this.datas.Add(key, item);
						}
					}
				}
				finally
				{
					this.rwLock.ExitWriteLock();
				}
			}
		}
	}
}
