using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000280 RID: 640
	public static class RecordingHelper
	{
		// Token: 0x060012B2 RID: 4786 RVA: 0x0006172C File Offset: 0x0005F92C
		public static string GetRecordingDestination()
		{
			return Application.persistentDataPath + "/Replays/";
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0006173D File Offset: 0x0005F93D
		public static void SetupReplayDestination(string destination)
		{
			Directory.CreateDirectory(destination);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00061748 File Offset: 0x0005F948
		public static List<Recording> LoadRecordingsList(string destination)
		{
			List<Recording> list = new List<Recording>();
			Directory.CreateDirectory(destination);
			foreach (string text in from x in Directory.GetFiles(destination, "*.replay", SearchOption.TopDirectoryOnly)
			orderby x descending
			select x)
			{
				list.Add(new Recording
				{
					name = Path.GetFileName(text).Replace(".replay", ""),
					fileLength = new FileInfo(text).Length
				});
			}
			return list;
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00061800 File Offset: 0x0005FA00
		public static List<Recording> LoadRecordings(string destination)
		{
			List<Recording> list = new List<Recording>();
			Directory.CreateDirectory(destination);
			foreach (string path in from x in Directory.GetFiles(destination, "*.replay", SearchOption.TopDirectoryOnly)
			orderby x descending
			select x)
			{
				try
				{
					if (File.Exists(path))
					{
						using (FileStream fileStream = File.OpenRead(path))
						{
							Recording item = Serializer.Deserialize<Recording>(fileStream);
							list.Add(item);
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
			}
			return list;
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000618D0 File Offset: 0x0005FAD0
		public static void SaveRecording(string destination, Recording recording)
		{
			try
			{
				if (recording != null)
				{
					RecordingHelper.SetupReplayDestination(destination);
					destination = destination + recording.name + ".replay";
					using (FileStream fileStream = File.Create(destination))
					{
						Serializer.Serialize<Recording>(fileStream, recording);
					}
					GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_replay_saved", Array.Empty<object>()), 1f, false);
				}
			}
			catch (Exception message)
			{
				GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("txt_save_failed", Array.Empty<object>()), 1f, false);
				Debug.LogError(message);
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00061970 File Offset: 0x0005FB70
		public static Recording LoadRecording(string destination, string name)
		{
			Recording result = null;
			string path = Path.Combine(destination, name + ".replay");
			try
			{
				if (File.Exists(path))
				{
					using (FileStream fileStream = File.OpenRead(path))
					{
						result = Serializer.Deserialize<Recording>(fileStream);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			return result;
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000619E0 File Offset: 0x0005FBE0
		public static bool DeleteRecording(string destination, string fileName)
		{
			try
			{
				File.Delete(Path.Combine(destination, fileName + ".replay"));
				return true;
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00061A24 File Offset: 0x0005FC24
		public static bool RenameRecording(string destination, string oldFileName, string newFileName)
		{
			try
			{
				string text = Path.Combine(destination, newFileName + ".replay");
				if (!File.Exists(text))
				{
					File.Move(Path.Combine(destination, oldFileName + ".replay"), text);
					return true;
				}
				GeneralManager.CreateAlertDialog(LocalizationHelpers.LocalizedText("alert_replay_already_exists", Array.Empty<object>()), 1f, false);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}
	}
}
