using System;

// Token: 0x02000045 RID: 69
public class FixedSizeQueue<T>
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600020A RID: 522 RVA: 0x0000BE61 File Offset: 0x0000A061
	public int Count
	{
		get
		{
			return this._count;
		}
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000BE69 File Offset: 0x0000A069
	public FixedSizeQueue(int maxSize)
	{
		this._maxSize = maxSize;
		this._array = new T[maxSize];
		this._currentIndex = 0;
		this._count = 0;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000BE92 File Offset: 0x0000A092
	public void Add(T item)
	{
		this._array[this._currentIndex] = item;
		this.IncrementIndex();
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000BEAC File Offset: 0x0000A0AC
	public T Get(int index)
	{
		int num = (this._currentIndex - this._count + index + this._maxSize) % this._maxSize;
		return this._array[num];
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000BEE3 File Offset: 0x0000A0E3
	public void IncrementIndex()
	{
		if (this._count < this._maxSize)
		{
			this._count++;
		}
		this._currentIndex = (this._currentIndex + 1) % this._maxSize;
	}

	// Token: 0x0400015A RID: 346
	private readonly int _maxSize;

	// Token: 0x0400015B RID: 347
	private int _currentIndex;

	// Token: 0x0400015C RID: 348
	private int _count;

	// Token: 0x0400015D RID: 349
	private T[] _array;
}
