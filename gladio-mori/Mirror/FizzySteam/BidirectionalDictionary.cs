using System;
using System.Collections;
using System.Collections.Generic;

namespace Mirror.FizzySteam
{
	// Token: 0x020002C2 RID: 706
	public class BidirectionalDictionary<T1, T2> : IEnumerable
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x0006B8E8 File Offset: 0x00069AE8
		public IEnumerable<T1> FirstTypes
		{
			get
			{
				return this.t1ToT2Dict.Keys;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x0006B8F5 File Offset: 0x00069AF5
		public IEnumerable<T2> SecondTypes
		{
			get
			{
				return this.t2ToT1Dict.Keys;
			}
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0006B902 File Offset: 0x00069B02
		public IEnumerator GetEnumerator()
		{
			return this.t1ToT2Dict.GetEnumerator();
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x0006B914 File Offset: 0x00069B14
		public int Count
		{
			get
			{
				return this.t1ToT2Dict.Count;
			}
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0006B921 File Offset: 0x00069B21
		public void Add(T1 key, T2 value)
		{
			this.t1ToT2Dict[key] = value;
			this.t2ToT1Dict[value] = key;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0006B93D File Offset: 0x00069B3D
		public void Add(T2 key, T1 value)
		{
			this.t2ToT1Dict[key] = value;
			this.t1ToT2Dict[value] = key;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0006B959 File Offset: 0x00069B59
		public T2 Get(T1 key)
		{
			return this.t1ToT2Dict[key];
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0006B967 File Offset: 0x00069B67
		public T1 Get(T2 key)
		{
			return this.t2ToT1Dict[key];
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0006B975 File Offset: 0x00069B75
		public bool TryGetValue(T1 key, out T2 value)
		{
			return this.t1ToT2Dict.TryGetValue(key, out value);
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0006B984 File Offset: 0x00069B84
		public bool TryGetValue(T2 key, out T1 value)
		{
			return this.t2ToT1Dict.TryGetValue(key, out value);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0006B993 File Offset: 0x00069B93
		public bool Contains(T1 key)
		{
			return this.t1ToT2Dict.ContainsKey(key);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0006B9A1 File Offset: 0x00069BA1
		public bool Contains(T2 key)
		{
			return this.t2ToT1Dict.ContainsKey(key);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0006B9B0 File Offset: 0x00069BB0
		public void Remove(T1 key)
		{
			if (this.Contains(key))
			{
				T2 key2 = this.t1ToT2Dict[key];
				this.t1ToT2Dict.Remove(key);
				this.t2ToT1Dict.Remove(key2);
			}
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0006B9F0 File Offset: 0x00069BF0
		public void Remove(T2 key)
		{
			if (this.Contains(key))
			{
				T1 key2 = this.t2ToT1Dict[key];
				this.t1ToT2Dict.Remove(key2);
				this.t2ToT1Dict.Remove(key);
			}
		}

		// Token: 0x1700027D RID: 637
		public T1 this[T2 key]
		{
			get
			{
				return this.t2ToT1Dict[key];
			}
			set
			{
				this.t2ToT1Dict[key] = value;
				this.t1ToT2Dict[value] = key;
			}
		}

		// Token: 0x1700027E RID: 638
		public T2 this[T1 key]
		{
			get
			{
				return this.t1ToT2Dict[key];
			}
			set
			{
				this.t1ToT2Dict[key] = value;
				this.t2ToT1Dict[value] = key;
			}
		}

		// Token: 0x04000FB1 RID: 4017
		private Dictionary<T1, T2> t1ToT2Dict = new Dictionary<T1, T2>();

		// Token: 0x04000FB2 RID: 4018
		private Dictionary<T2, T1> t2ToT1Dict = new Dictionary<T2, T1>();
	}
}
