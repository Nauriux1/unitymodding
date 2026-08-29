using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000105 RID: 261
public class CommandInvoker
{
	// Token: 0x0600087A RID: 2170 RVA: 0x00029E46 File Offset: 0x00028046
	public static void ExecuteCommand(ICommand command, bool disableExecute = false)
	{
		if (!disableExecute)
		{
			command.Execute();
		}
		CommandInvoker._undoStack.Add(command);
		if (CommandInvoker._undoStack.Count > 30)
		{
			CommandInvoker._undoStack.RemoveAt(0);
		}
		CommandInvoker._redoStack.Clear();
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00029E80 File Offset: 0x00028080
	public static void UndoCommand()
	{
		if (CommandInvoker._undoStack.Count > 0)
		{
			ICommand command = CommandInvoker._undoStack.Last<ICommand>();
			CommandInvoker._undoStack.Remove(command);
			CommandInvoker._redoStack.Push(command);
			command.Undo();
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00029EC4 File Offset: 0x000280C4
	public static void RedoCommand()
	{
		if (CommandInvoker._redoStack.Count > 0)
		{
			ICommand command = CommandInvoker._redoStack.Pop();
			CommandInvoker._undoStack.Add(command);
			command.Execute();
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00029EFA File Offset: 0x000280FA
	public static void ClearAll()
	{
		CommandInvoker._undoStack.Clear();
		CommandInvoker._redoStack.Clear();
	}

	// Token: 0x040005DC RID: 1500
	private static List<ICommand> _undoStack = new List<ICommand>();

	// Token: 0x040005DD RID: 1501
	private static Stack<ICommand> _redoStack = new Stack<ICommand>();
}
