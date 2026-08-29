using System;

// Token: 0x0200007D RID: 125
public interface IGameSettingsManager
{
	// Token: 0x0600037E RID: 894
	void SetRollingFeet(bool newRollingFeet);

	// Token: 0x0600037F RID: 895
	bool GetRollingFeet();

	// Token: 0x06000380 RID: 896
	void DestroyGameSettingsManager();

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000381 RID: 897
	// (set) Token: 0x06000382 RID: 898
	string SelectedMap { get; set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000383 RID: 899
	// (set) Token: 0x06000384 RID: 900
	int EquipmentPoints { get; set; }

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000385 RID: 901
	// (set) Token: 0x06000386 RID: 902
	int AiAmount { get; set; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000387 RID: 903
	// (set) Token: 0x06000388 RID: 904
	float TimeScaleMin { get; set; }

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000389 RID: 905
	// (set) Token: 0x0600038A RID: 906
	AllowedMovesetTypes AllowedMovesetTypes { get; set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600038B RID: 907
	// (set) Token: 0x0600038C RID: 908
	bool AllowEquipmentEdit { get; set; }

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600038D RID: 909
	// (set) Token: 0x0600038E RID: 910
	GameTypes GameType { get; set; }

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600038F RID: 911
	// (set) Token: 0x06000390 RID: 912
	bool UseStamina { get; set; }

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000391 RID: 913
	// (set) Token: 0x06000392 RID: 914
	bool UseDismemberment { get; set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000393 RID: 915
	// (set) Token: 0x06000394 RID: 916
	string WelcomeMessage { get; set; }

	// Token: 0x06000395 RID: 917
	void DisplayWelcomeMessage();

	// Token: 0x06000396 RID: 918
	void SaveGameSettings();

	// Token: 0x06000397 RID: 919
	void LoadGameSettings();

	// Token: 0x04000262 RID: 610
	public static IGameSettingsManager singleton;
}
