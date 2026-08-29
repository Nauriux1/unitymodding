using System;

// Token: 0x02000120 RID: 288
public interface ICommand
{
	// Token: 0x060008ED RID: 2285
	void Execute();

	// Token: 0x060008EE RID: 2286
	void Undo();
}
