using System;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;

// Token: 0x0200022A RID: 554
public class ParticleDisplayer : MonoBehaviour
{
	// Token: 0x060010BD RID: 4285 RVA: 0x00056345 File Offset: 0x00054545
	private void Awake()
	{
		if (ParticleDisplayer.singleton == null)
		{
			ParticleDisplayer.singleton = this;
			this.LoadSettings(true);
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x0000777A File Offset: 0x0000597A
	private void Start()
	{
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x00056368 File Offset: 0x00054568
	private void Update()
	{
		if (this.displayParticles)
		{
			this.DisplayParticles();
			this.displayParticles = false;
		}
		Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
		Graphics.DrawMeshInstancedIndirect(this.mesh, 0, this.material, bounds, this.argsBuffer, 0, null, ShadowCastingMode.Off, true, 0, null, LightProbeUsage.BlendProbes);
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x000563C5 File Offset: 0x000545C5
	private void UpdateInstanceCount()
	{
		this.args[1] = (uint)this.maxParticles;
		this.argsBuffer.SetData(this.args);
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x000563E6 File Offset: 0x000545E6
	private void OnDisable()
	{
		ComputeBuffer computeBuffer = this.position_buffer_1;
		if (computeBuffer != null)
		{
			computeBuffer.Release();
		}
		this.position_buffer_1 = null;
		ComputeBuffer computeBuffer2 = this.argsBuffer;
		if (computeBuffer2 != null)
		{
			computeBuffer2.Release();
		}
		this.argsBuffer = null;
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x00056418 File Offset: 0x00054618
	public void AddParticle(Vector3 position, Vector3 rotation, float startSize)
	{
		if (this.currentParticleIndex >= this.maxParticles)
		{
			this.currentParticleIndex = 0;
		}
		float num = UnityEngine.Random.Range(this.minSize, this.maxSize);
		this.matrices[this.currentParticleIndex] = Matrix4x4.TRS(position, Quaternion.Euler(rotation), new Vector3(num, num, num));
		this.currentParticleIndex++;
		this.displayParticles = true;
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x00056486 File Offset: 0x00054686
	public void DisplayParticles()
	{
		this.position_buffer_1.SetData(this.matrices);
		this.material.SetBuffer("position_buffer_1", this.position_buffer_1);
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x000564B0 File Offset: 0x000546B0
	public void LoadSettings(bool loadAtStart = false)
	{
		int num = this.maxParticles;
		this.maxParticles = SettingsHelper.GetBloodParticleDisplayerMaxCount();
		if (loadAtStart)
		{
			this.matrices = new Matrix4x4[this.maxParticles];
			for (int i = 0; i < this.maxParticles; i++)
			{
				this.matrices[i] = Matrix4x4.TRS(new Vector3(UnityEngine.Random.Range(-1f, 1f), -100f, UnityEngine.Random.Range(-1f, 1f)), Quaternion.identity, new Vector3(0.1f, 0.1f, 0.1f));
			}
			this.position_buffer_1 = new ComputeBuffer(this.maxParticles, 64);
			this.position_buffer_1.SetData(this.matrices);
			this.args = new uint[5];
			this.args[0] = this.mesh.GetIndexCount(0);
			this.args[1] = (uint)this.maxParticles;
			this.args[2] = this.mesh.GetIndexStart(0);
			this.args[3] = this.mesh.GetBaseVertex(0);
			this.args[4] = 0U;
			this.argsBuffer = new ComputeBuffer(1, this.args.Length * 4, ComputeBufferType.DrawIndirect);
			this.argsBuffer.SetData(this.args);
			return;
		}
		if (this.maxParticles != num)
		{
			Array.Resize<Matrix4x4>(ref this.matrices, this.maxParticles);
			if (this.currentParticleIndex >= this.maxParticles)
			{
				this.currentParticleIndex = 0;
			}
			ComputeBuffer computeBuffer = this.position_buffer_1;
			if (computeBuffer != null)
			{
				computeBuffer.Release();
			}
			this.position_buffer_1 = new ComputeBuffer(this.maxParticles, 64);
			this.UpdateInstanceCount();
			this.DisplayParticles();
		}
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x00056658 File Offset: 0x00054858
	public void ClearParticles()
	{
		for (int i = 0; i < this.maxParticles; i++)
		{
			this.matrices[i] = Matrix4x4.TRS(new Vector3(UnityEngine.Random.Range(-1f, 1f), -100f, UnityEngine.Random.Range(-1f, 1f)), Quaternion.identity, new Vector3(0.1f, 0.1f, 0.1f));
		}
		this.displayParticles = true;
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x000566D0 File Offset: 0x000548D0
	public void AddBloodSpatter(ParticleCollisionEvent particleCollisionEvent)
	{
		Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.forward * -1f, particleCollisionEvent.normal).eulerAngles;
		this.AddParticle(particleCollisionEvent.intersection, eulerAngles, UnityEngine.Random.Range(this.minSize, this.maxSize));
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x00056720 File Offset: 0x00054920
	private void CreateTestParticles()
	{
		this.elapsedFromLastPriorityUpdate += Time.deltaTime;
		if (this.elapsedFromLastPriorityUpdate > this.priorityUpdateFrequency)
		{
			this.elapsedFromLastPriorityUpdate -= this.priorityUpdateFrequency;
			for (int i = 0; i < this.testAmount; i++)
			{
				this.AddParticle(new Vector3(UnityEngine.Random.Range(-1f, 1f), 0.001f, UnityEngine.Random.Range(-1f, 1f)), new Vector3(90f, 0f, 0f), UnityEngine.Random.Range(this.minSize, this.maxSize));
			}
		}
	}

	// Token: 0x04000C20 RID: 3104
	public static ParticleDisplayer singleton;

	// Token: 0x04000C21 RID: 3105
	public int maxParticles = 20000;

	// Token: 0x04000C22 RID: 3106
	private int currentParticleIndex;

	// Token: 0x04000C23 RID: 3107
	public float minSize = 0.05f;

	// Token: 0x04000C24 RID: 3108
	public float maxSize = 0.1f;

	// Token: 0x04000C25 RID: 3109
	public GameObject bloodSpatterPrefab;

	// Token: 0x04000C26 RID: 3110
	public Mesh mesh;

	// Token: 0x04000C27 RID: 3111
	public Material material;

	// Token: 0x04000C28 RID: 3112
	private ComputeBuffer position_buffer_1;

	// Token: 0x04000C29 RID: 3113
	private ComputeBuffer argsBuffer;

	// Token: 0x04000C2A RID: 3114
	private Matrix4x4[] matrices;

	// Token: 0x04000C2B RID: 3115
	private uint[] args;

	// Token: 0x04000C2C RID: 3116
	private bool displayParticles;

	// Token: 0x04000C2D RID: 3117
	public float priorityUpdateFrequency = 0.2f;

	// Token: 0x04000C2E RID: 3118
	private float elapsedFromLastPriorityUpdate;

	// Token: 0x04000C2F RID: 3119
	public int testAmount = 100;
}
