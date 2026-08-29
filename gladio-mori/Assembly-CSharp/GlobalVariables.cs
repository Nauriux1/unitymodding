using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Utils;

// Token: 0x0200004B RID: 75
public static class GlobalVariables
{
	// Token: 0x0600021F RID: 543 RVA: 0x0000C184 File Offset: 0x0000A384
	public static void SaveMoveSets()
	{
		string path = Application.persistentDataPath + "/MoveSets.dat";
		FileStream fileStream;
		if (File.Exists(path))
		{
			fileStream = File.OpenWrite(path);
		}
		else
		{
			fileStream = File.Create(path);
		}
		new BinaryFormatter().Serialize(fileStream, MoveSetHelpers.MoveSets);
		fileStream.Close();
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
	public static void LoadMoveSets()
	{
		string path = Application.persistentDataPath + "/MoveSets.dat";
		FileStream fileStream = null;
		try
		{
			if (File.Exists(path))
			{
				fileStream = File.OpenRead(path);
				new BinaryFormatter();
				fileStream.Close();
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
			}
		}
	}
}
