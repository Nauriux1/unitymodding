using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using MoveClasses;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils
{
	// Token: 0x02000276 RID: 630
	internal class Generic
	{
		// Token: 0x06001247 RID: 4679 RVA: 0x0005F504 File Offset: 0x0005D704
		public static T DeepClone<T>(T obj)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Position = 0L;
				result = (T)((object)binaryFormatter.Deserialize(memoryStream));
			}
			return result;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0005F55C File Offset: 0x0005D75C
		public static GameObject GetParentWithComponent(GameObject gameObject, Type component)
		{
			if (gameObject.GetComponent(component) != null)
			{
				return gameObject;
			}
			Transform parent = gameObject.transform.parent;
			if (parent != null)
			{
				return Generic.GetParentWithComponent(parent.gameObject, component);
			}
			return null;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0005F5A0 File Offset: 0x0005D7A0
		public static GameObject GetParentWithName(GameObject gameObject, string name)
		{
			if (gameObject.name == name)
			{
				return gameObject;
			}
			Transform parent = gameObject.transform.parent;
			if (parent != null)
			{
				return Generic.GetParentWithName(parent.gameObject, name);
			}
			return null;
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0005F5E0 File Offset: 0x0005D7E0
		public static GameObject GetParentWithUpperCaseName(GameObject gameObject)
		{
			if (!gameObject.name.Replace("_", "").Any(new Func<char, bool>(char.IsLower)))
			{
				return gameObject;
			}
			Transform parent = gameObject.transform.parent;
			if (parent != null)
			{
				return Generic.GetParentWithUpperCaseName(parent.gameObject);
			}
			return null;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0005F63C File Offset: 0x0005D83C
		public static bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
		{
			Rect rect = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
			Rect other = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);
			return rect.Overlaps(other);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0005F6C4 File Offset: 0x0005D8C4
		public static GameObject FindChildObject(Transform parent, string childName, string parentName = null)
		{
			GameObject gameObject = null;
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (child.name == childName && (string.IsNullOrEmpty(parentName) || (child.parent != null && child.parent.name == parentName)))
				{
					return child.gameObject;
				}
				gameObject = Generic.FindChildObject(child, childName, parentName);
				if (gameObject != null)
				{
					break;
				}
			}
			return gameObject;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0005F740 File Offset: 0x0005D940
		public static GameObject FindChildObjectWithNameContains(Transform parent, string childName)
		{
			GameObject gameObject = null;
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (child.name.ToLower().Contains(childName.ToLower()))
				{
					return child.gameObject;
				}
				gameObject = Generic.FindChildObject(child, childName, null);
				if (gameObject != null)
				{
					break;
				}
			}
			return gameObject;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0005F79C File Offset: 0x0005D99C
		public static List<GameObject> FindChildObjectsWithComponent(GameObject gameObject, Type component)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				if (child.GetComponent(component) != null)
				{
					list.Add(child.gameObject);
				}
				list.AddRange(Generic.FindChildObjectsWithComponent(child.gameObject, component));
			}
			return list;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0005F800 File Offset: 0x0005DA00
		public static List<T> FindComponentsInChildObjects<T>(GameObject gameObject)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				T component = child.GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
				list.AddRange(Generic.FindComponentsInChildObjects<T>(child.gameObject));
			}
			return list;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0005F860 File Offset: 0x0005DA60
		public static Vector3 PointOnPlaneBetweenTwoPoints(Plane p, Vector3 a, Vector3 b)
		{
			Vector3 vector = b - a;
			vector.Normalize();
			Vector3 lhs = p.ClosestPointOnPlane(Vector3.zero);
			Vector3 normal = p.normal;
			Vector3 rhs = normal;
			float d = (Vector3.Dot(lhs, normal) - Vector3.Dot(a, rhs)) / Vector3.Dot(vector, rhs);
			return a + vector * d;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0005F8B6 File Offset: 0x0005DAB6
		public static bool DoubleEquals(double float1, double float2)
		{
			return Math.Abs(float1 - float2) < 0.004999999888241291;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0005F8CB File Offset: 0x0005DACB
		public static bool FloatEquals(float float1, float float2)
		{
			return Math.Abs(float1 - float2) < 0.0001f;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0005F8DC File Offset: 0x0005DADC
		public static bool DoubleIsGreaterThan(double greater, double smaller)
		{
			return greater > smaller && !Generic.DoubleEquals(greater, smaller);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0005F8EE File Offset: 0x0005DAEE
		public static int CompareDouble(double x, double y)
		{
			if (Generic.DoubleEquals(x, y))
			{
				return 0;
			}
			return x.CompareTo(y);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0005F904 File Offset: 0x0005DB04
		public static bool ConvertToRoundedFloat(string textValue, out float convertedValue, out string newTextValue)
		{
			convertedValue = 0f;
			newTextValue = textValue;
			float x;
			if (float.TryParse(textValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out x))
			{
				float num = MathF.Round(x, 2);
				if (textValue.Substring(textValue.Replace(',', '.').LastIndexOf('.') + 1).Length > 2)
				{
					newTextValue = num.ToString("F");
				}
				convertedValue = num;
				return true;
			}
			return false;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0005F978 File Offset: 0x0005DB78
		public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
		{
			float num = Vector3.Dot(Vector3.Cross(up, fwd), targetDir);
			if (num > 0f)
			{
				return 1f;
			}
			if (num < 0f)
			{
				return -1f;
			}
			return 0f;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0005F9B4 File Offset: 0x0005DBB4
		public static float GetDirectionDotProductBetweenTwoTransforms(Transform point1, Transform point2)
		{
			return Vector3.Dot((point2.position - point1.position).normalized, point1.forward);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0005F9E8 File Offset: 0x0005DBE8
		public static Vector3 GetClosestPointOnLine(Vector3 a, Vector3 b, Vector3 point, bool clamp, out float t)
		{
			Vector3 vector = b - a;
			Vector3 lhs = point - a;
			t = Vector3.Dot(lhs, vector) / vector.sqrMagnitude;
			if (clamp)
			{
				t = Mathf.Clamp01(t);
			}
			return a + t * vector;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0005FA34 File Offset: 0x0005DC34
		public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2, bool useLineSegments = false)
		{
			float num = 0f;
			float num2 = 0f;
			return Generic.ClosestPointsOnTwoLines(out closestPointLine1, out closestPointLine2, out num, out num2, linePoint1, lineVec1, linePoint2, lineVec2, useLineSegments);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0005FA60 File Offset: 0x0005DC60
		public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, out float positionOnLine1, out float positionOnLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2, bool useLineSegments = false)
		{
			closestPointLine1 = Vector3.zero;
			closestPointLine2 = Vector3.zero;
			positionOnLine1 = 0f;
			positionOnLine2 = 0f;
			float num = Vector3.Dot(lineVec1, lineVec1);
			float num2 = Vector3.Dot(lineVec1, lineVec2);
			float num3 = Vector3.Dot(lineVec2, lineVec2);
			float num4 = num * num3 - num2 * num2;
			if (num4 != 0f)
			{
				Vector3 rhs = linePoint1 - linePoint2;
				float num5 = Vector3.Dot(lineVec1, rhs);
				float num6 = Vector3.Dot(lineVec2, rhs);
				float num7 = (num2 * num6 - num5 * num3) / num4;
				float num8 = (num * num6 - num5 * num2) / num4;
				if (useLineSegments)
				{
					if (num7 > 1f)
					{
						num7 = 1f;
					}
					if (num7 < 0f)
					{
						num7 = 0f;
					}
					if (num8 > 1f)
					{
						num8 = 1f;
					}
					if (num8 < 0f)
					{
						num8 = 0f;
					}
				}
				positionOnLine1 = num7;
				positionOnLine2 = num8;
				closestPointLine1 = linePoint1 + lineVec1 * num7;
				closestPointLine2 = linePoint2 + lineVec2 * num8;
				return true;
			}
			return false;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0005FB78 File Offset: 0x0005DD78
		public static Vector3 ClampRotation(Vector3 targetRotation, float? x = null, float? y = null, float? z = null)
		{
			return Generic.ClampRotation(targetRotation, (x == null) ? null : (x * (float)-1), x, (y == null) ? null : (y * (float)-1), y, (z == null) ? null : (z * (float)-1), z);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0005FC40 File Offset: 0x0005DE40
		public static Vector3 ClampRotation(Vector3 targetRotation, float? xMin = null, float? xMax = null, float? yMin = null, float? yMax = null, float? zMin = null, float? zMax = null)
		{
			targetRotation = Generic.ConvertToNegativeAndPositiveRotation(targetRotation);
			if (xMin != null && xMax != null)
			{
				targetRotation.x = Mathf.Clamp(targetRotation.x, xMin.Value, xMax.Value);
			}
			if (yMin != null && yMax != null)
			{
				targetRotation.y = Mathf.Clamp(targetRotation.y, yMin.Value, yMax.Value);
			}
			if (zMin != null && zMax != null)
			{
				targetRotation.z = Mathf.Clamp(targetRotation.z, zMin.Value, zMax.Value);
			}
			return targetRotation;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0005FCEC File Offset: 0x0005DEEC
		public static Vector3 ConvertToNegativeAndPositiveRotation(Vector3 targetRotation)
		{
			if (targetRotation.x > 180f)
			{
				targetRotation.x -= 360f;
			}
			if (targetRotation.y > 180f)
			{
				targetRotation.y -= 360f;
			}
			if (targetRotation.z > 180f)
			{
				targetRotation.z -= 360f;
			}
			return targetRotation;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0005FD5C File Offset: 0x0005DF5C
		public static bool SaveJsonToFile(string path, string json)
		{
			try
			{
				File.WriteAllText(path, json);
				return true;
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0005FD90 File Offset: 0x0005DF90
		public static string LoadJsonFromFile(string filePath)
		{
			string result = "";
			try
			{
				if (File.Exists(filePath))
				{
					using (StreamReader streamReader = new StreamReader(filePath))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
			return result;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0005FDF0 File Offset: 0x0005DFF0
		public static bool IsConnectionAlive(float lastMessageTime, float timeout)
		{
			return Time.time - lastMessageTime < timeout * Time.timeScale;
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0005FE02 File Offset: 0x0005E002
		public static bool IsQuaternionApproximate(Quaternion q1, Quaternion q2, float precision)
		{
			return Mathf.Abs(Quaternion.Dot(q1, q2)) >= 1f - precision;
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0005FE1C File Offset: 0x0005E01C
		public static void SaveRenderTexture(RenderTexture rt, string path)
		{
			RenderTexture.active = rt;
			Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)rt.width, (float)rt.height), 0, 0);
			RenderTexture.active = null;
			byte[] bytes = texture2D.EncodeToPNG();
			File.WriteAllBytes(path, bytes);
			UnityEngine.Object.Destroy(texture2D);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0005FE80 File Offset: 0x0005E080
		public static Texture2D GetImageFromPath(string filename)
		{
			if (File.Exists(filename))
			{
				byte[] array = File.ReadAllBytes(filename);
				if (Generic.CheckBytesAreAcceptableImage(array))
				{
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(array);
					return texture2D;
				}
			}
			return null;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0005FEB8 File Offset: 0x0005E0B8
		public static bool SaveTexture2DToFileAsPNG(string path, Texture2D image)
		{
			try
			{
				byte[] bytes = image.EncodeToPNG();
				File.WriteAllBytes(path, bytes);
				return true;
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0005FEF4 File Offset: 0x0005E0F4
		public static bool CheckBytesAreAcceptableImage(byte[] bytes)
		{
			byte[] array = new byte[]
			{
				137,
				80,
				78,
				71
			};
			byte[] array2 = new byte[]
			{
				byte.MaxValue,
				216,
				byte.MaxValue,
				224
			};
			byte[] array3 = new byte[]
			{
				byte.MaxValue,
				216,
				byte.MaxValue,
				225
			};
			if (array.SequenceEqual(bytes.Take(array.Length)))
			{
				return true;
			}
			if (array2.SequenceEqual(bytes.Take(array2.Length)))
			{
				return true;
			}
			if (array3.SequenceEqual(bytes.Take(array3.Length)))
			{
				return true;
			}
			Debug.Log("Selected file is not a supported image type");
			return false;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0005FF7C File Offset: 0x0005E17C
		public static Texture2D ResizeTexture2D(Texture2D tex, int maxSize = 1024)
		{
			try
			{
				if (tex.width > maxSize || tex.height > maxSize)
				{
					int num = tex.width;
					int num2 = tex.height;
					if (num > maxSize)
					{
						num = maxSize;
					}
					if (num2 > maxSize)
					{
						num2 = maxSize;
					}
					RenderTexture renderTexture = new RenderTexture(num, num2, 24);
					RenderTexture.active = renderTexture;
					Graphics.Blit(tex, renderTexture);
					Texture2D texture2D = new Texture2D(num, num2);
					texture2D.ReadPixels(new Rect(0f, 0f, (float)num, (float)num2), 0, 0);
					texture2D.Apply();
					return texture2D;
				}
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
			return tex;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00060014 File Offset: 0x0005E214
		public static byte[] Texture2DToJpgEncodedByteArray(Texture2D tex)
		{
			byte[] array = null;
			try
			{
				int num = 90;
				while ((array == null || array.Length > SettingsHelper.customPlayerTextureMaxBytes) && num > 10)
				{
					array = tex.EncodeToJPG(num);
					num -= 10;
				}
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
			return array;
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00060064 File Offset: 0x0005E264
		public static string CreateBackupForFile(string path)
		{
			string text = null;
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				string extension = Path.GetExtension(path);
				text = Generic.GetUniqueFilePath(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path), extension, true);
				File.Copy(path, text);
			}
			return text;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000600A8 File Offset: 0x0005E2A8
		public static bool CreateErrorBackupForMovesetClass(MoveSet moveset)
		{
			try
			{
				string contents = JsonConvert.SerializeObject(moveset, MoveSetHelpers.GetJsonSerializerSettings());
				File.WriteAllText(Generic.GetUniqueFilePath(SettingsHelper.GetMoveSetsSaveFolder(), LocalizationHelpers.LocalizedText("txt_error_backup_filename", Array.Empty<object>()), ".json", true), contents);
				return true;
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			return false;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00060104 File Offset: 0x0005E304
		public static string GetUniqueFilePath(string folderPath, string filename, string fileType = ".json", bool alwaysAddNumber = true)
		{
			string text = null;
			int num = 0;
			string text2 = null;
			if (!alwaysAddNumber)
			{
				text2 = filename + fileType;
				text = Path.Combine(folderPath, text2);
			}
			while (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text) || File.Exists(text))
			{
				text2 = filename + string.Format("_{0}{1}", num, fileType);
				text = Path.Combine(folderPath, text2);
				num++;
			}
			return text;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00060169 File Offset: 0x0005E369
		public static void DeleteFile(string path)
		{
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				File.Delete(path);
			}
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x00060184 File Offset: 0x0005E384
		public static void CopyFileToLocation(string destinationPath, string filePath, string newFileName = null)
		{
			if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
			{
				string extension = Path.GetExtension(filePath);
				if (string.IsNullOrEmpty(newFileName))
				{
					newFileName = Path.GetFileNameWithoutExtension(filePath);
				}
				string uniqueFilePath = Generic.GetUniqueFilePath(Path.GetDirectoryName(destinationPath), newFileName, extension, false);
				File.Copy(filePath, uniqueFilePath);
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x000601CE File Offset: 0x0005E3CE
		public static Type GetPlayerInputManagerAi(CustomAiObject customAiObject)
		{
			if (customAiObject != null && customAiObject.GetType() == typeof(ContinuousAttackAiObject))
			{
				return typeof(PlayerInputAiManagerContinuousAttack);
			}
			return typeof(PlayerInputAIManager);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00060208 File Offset: 0x0005E408
		public static string GetLocalizedList(List<string> wordList)
		{
			List<string> list = new List<string>();
			string text = "";
			foreach (string text2 in wordList)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					list.Add(LocalizationHelpers.LocalizedText(text2, Array.Empty<object>()));
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				string text3 = list[i];
				if (i == 0)
				{
					text = text3;
				}
				else if (i == list.Count - 1)
				{
					text = LocalizationHelpers.LocalizedText("txt_separator_and", new object[]
					{
						text,
						text3
					});
				}
				else
				{
					text = LocalizationHelpers.LocalizedText("txt_separator_comma", new object[]
					{
						text,
						text3
					});
				}
			}
			return text;
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000602E0 File Offset: 0x0005E4E0
		public static Color BloodColour(BloodColourType bloodColourType)
		{
			if (bloodColourType == BloodColourType.Blue)
			{
				return new Color(0f, 0f, 0.75f);
			}
			return new Color(0.3294117f, 0f, 0f);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0006030F File Offset: 0x0005E50F
		public static Color BloodParticleColour(BloodColourType bloodColourType)
		{
			if (bloodColourType == BloodColourType.Blue)
			{
				return new Color(0f, 0f, 0.75f);
			}
			return new Color(0.2264151f, 0f, 0f);
		}
	}
}
