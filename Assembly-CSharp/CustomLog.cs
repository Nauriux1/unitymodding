using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class CustomLog : MonoBehaviour
{
	// Token: 0x06000C98 RID: 3224 RVA: 0x0003D768 File Offset: 0x0003B968
	public void InitializeCustomLogger()
	{
		if (!this.useCustomLog)
		{
			return;
		}
		if (CustomLog.customLogger != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.logPath = Application.persistentDataPath + "/Logs/";
		this.CheckLogFolder();
		this.HandleExistingLogs();
		this.customLogging = false;
		if (Directory.Exists(this.logPath))
		{
			this.customLogging = true;
			this.logPath = this.logPath + "log" + DateTime.Now.ToString() + ".txt";
			this._writer = File.AppendText(this.logPath);
			this._writer.AutoFlush = true;
			this._writer.WriteLine("\n\n=============== Game started ================\n\n");
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Application.logMessageReceived += this.HandleLog;
			Debug.Log("Custom debugger has been setup");
		}
		CustomLog.customLogger = this;
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x0003D852 File Offset: 0x0003BA52
	private void OnDestroy()
	{
		if (this.customLogging)
		{
			Application.logMessageReceived -= this.HandleLog;
			if (this._writer != null)
			{
				this._writer.Close();
			}
		}
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0003D880 File Offset: 0x0003BA80
	private void HandleLog(string condition, string stackTrace, LogType type)
	{
		string value = string.Format("\n {0} {1} \n {2}\n {3}", new object[]
		{
			DateTime.Now,
			type,
			condition,
			stackTrace
		});
		this._writer.WriteLine(value);
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x0003D8C8 File Offset: 0x0003BAC8
	private void CheckLogFolder()
	{
		if (!Directory.Exists(this.logPath))
		{
			Directory.CreateDirectory(this.logPath);
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0003D8E4 File Offset: 0x0003BAE4
	private void HandleExistingLogs()
	{
		try
		{
			string[] files = Directory.GetFiles(this.logPath, "*.txt", SearchOption.TopDirectoryOnly);
			if (files.Length >= 5)
			{
				List<FileInfo> list = new List<FileInfo>();
				foreach (string text in files)
				{
					try
					{
						if (File.Exists(text))
						{
							FileInfo item = new FileInfo(text);
							list.Add(item);
						}
					}
					catch (Exception ex)
					{
						Debug.Log(ex.Message);
					}
				}
				list = (from x in list
				orderby x.CreationTime
				select x).Take(list.Count - 4).ToList<FileInfo>();
				foreach (FileInfo fileInfo in list)
				{
					File.Delete(fileInfo.FullName);
				}
			}
		}
		catch (Exception ex2)
		{
			Debug.Log(ex2.Message);
		}
	}

	// Token: 0x04000911 RID: 2321
	public static CustomLog customLogger;

	// Token: 0x04000912 RID: 2322
	private StreamWriter _writer;

	// Token: 0x04000913 RID: 2323
	private string logPath = "";

	// Token: 0x04000914 RID: 2324
	public bool useCustomLog;

	// Token: 0x04000915 RID: 2325
	private bool customLogging;
}
