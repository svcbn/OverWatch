using UnityEngine;

namespace TurretBulletLines
{
	
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[ExecuteInEditMode]
	public class TurretBulletLine : MonoBehaviour 
	{
		// Used to compute the average value of all the Vector3's components:
		static readonly Vector3 Average = new Vector3(1f/3f, 1f/3f, 1f/3f);

		#region private variables
		[SerializeField]
		public Material m_templateMaterial;
		[SerializeField] 
		private bool m_doNotOverwriteTemplateMaterialProperties;
		//The start position relative to the GameObject's origin
		[SerializeField] 
		private Vector3 m_startPos;
		// The end position relative to the GameObject's origin
		[SerializeField] 
		private Vector3 m_endPos = new Vector3(0f, 0f, 100f);
		[SerializeField] 
		private Color m_lineColor;
		[SerializeField] 
		private float m_lineWidth;
		[SerializeField]
		[Range(0.0f, 1.0f)]
		private float m_lightSaberFactor;
		private Material m_material;
		private MeshFilter m_meshFilter;
		#endregion

		#region properties
		/// Gets or sets the tmplate material.
		/// Setting this will only have an impact once. 
		/// Subsequent changes will be ignored.
		public Material TemplateMaterial
		{
			get { return m_templateMaterial; }
			set { m_templateMaterial = value; }
		}

		/// Gets or sets whether or not the template material properties
		/// should be used (false) or if the properties of this MonoBehavior
		/// instance should be used (true, default).
		/// Setting this will only have an impact once, and then only if it
		/// is set before TemplateMaterial has been assigned.
		public bool DoNotOverwriteTemplateMaterialProperties
		{
			get { return m_doNotOverwriteTemplateMaterialProperties; }
			set { m_doNotOverwriteTemplateMaterialProperties = value; }
		}
		
		/// Get or set the line color of this line's material
		public Color LineColor
		{
			get { return m_lineColor;  }
			set
			{
				CreateMaterial();
				if (null != m_material)
				{
					m_lineColor = value;
					m_material.color = m_lineColor;
				}
			}
		}

		/// Get or set the line width of this line's material
		public float LineWidth
		{
			get { return m_lineWidth; }
			set
			{
				CreateMaterial();
				if (null != m_material)
				{
					m_lineWidth = value;
					m_material.SetFloat("_LineWidth", m_lineWidth);
				}
				UpdateBounds();
			}
		}

		/// Get or set the light saber factor of this line's material
		public float LightSaberFactor
		{
			get { return m_lightSaberFactor; }
			set
			{
				CreateMaterial();
				if (null != m_material)
				{
					m_lightSaberFactor = value;
					m_material.SetFloat("_LightSaberFactor", m_lightSaberFactor);
				}
			}
		}

		/// Get or set the start position of this line's mesh
		public Vector3 StartPos
		{
			get { return m_startPos; }
			set
			{
				m_startPos = value;
				SetStartAndEndPoints(m_startPos, m_endPos);
			}
		}

		/// Get or set the end position of this line's mesh
		public Vector3 EndPos
		{
			get { return m_endPos; }
			set
			{
				m_endPos = value;
				SetStartAndEndPoints(m_startPos, m_endPos);
			}
		}
		#endregion
		
		#region methods
		/// Creates a copy of the template material for this instance
		private void CreateMaterial()
		{
			if (null == m_material || null == GetComponent<MeshRenderer>().sharedMaterial)
			{
				if (null != m_templateMaterial)
				{
					m_material = Material.Instantiate(m_templateMaterial);
					GetComponent<MeshRenderer>().sharedMaterial = m_material;
					SetAllMaterialProperties();
				}
				else 
				{
					m_material = GetComponent<MeshRenderer>().sharedMaterial;
				}
			}
		}

		/// Destroys the copy of the template material which was used for this instance
		private void DestroyMaterial()
		{
			if (null != m_material)
			{
				DestroyImmediate(m_material);
				m_material = null;
			}
		}

		/// Calculates the (approximated) _LineScale factor based on the object's scale.
		private float CalculateLineScale()
		{
			return Vector3.Dot(transform.lossyScale, Average);
		}

		/// Updates the line scaling of this line based on the current object scaling.
		public void UpdateLineScale()
		{
			if (null != m_material) 
			{
				m_material.SetFloat("_LineScale", CalculateLineScale());
			}
		}

		/// Sets all material properties (color, width, light saber factor, start-, endpos)
		private void SetAllMaterialProperties()
		{
			SetStartAndEndPoints(m_startPos, m_endPos);

			if (null != m_material)
			{
				if (!m_doNotOverwriteTemplateMaterialProperties)
				{
					m_material.color = m_lineColor;
					m_material.SetFloat("_LineWidth", m_lineWidth);
					m_material.SetFloat("_LightSaberFactor", m_lightSaberFactor);
				}
				UpdateLineScale();
			}
		}

		/// Calculate the bounds of this line based on start and end points,
		/// the line width, and the scaling of the object.
		private Bounds CalculateBounds()
		{
			var maxWidth = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
			var scaledLineWidth = maxWidth * LineWidth * 0.5f;

			var min = new Vector3(
				Mathf.Min(m_startPos.x, m_endPos.x) - scaledLineWidth,
				Mathf.Min(m_startPos.y, m_endPos.y) - scaledLineWidth,
				Mathf.Min(m_startPos.z, m_endPos.z) - scaledLineWidth
			);
			var max = new Vector3(
				Mathf.Max(m_startPos.x, m_endPos.x) + scaledLineWidth,
				Mathf.Max(m_startPos.y, m_endPos.y) + scaledLineWidth,
				Mathf.Max(m_startPos.z, m_endPos.z) + scaledLineWidth
			);
			
			return new Bounds
			{
				min = min,
				max = max
			};
		}

		/// Updates the bounds of this line according to the current properties, 
		/// which there are: start point, end point, line width, scaling of the object.
		public void UpdateBounds()
		{
			if (null != m_meshFilter)
			{
				var mesh = m_meshFilter.sharedMesh;
				Debug.Assert(null != mesh);
				if (null != mesh)
				{
					mesh.bounds = CalculateBounds();
				}
			}
		}

		/// Sets the start and end points - updates the data of the Mesh.
		public void SetStartAndEndPoints(Vector3 startPoint, Vector3 endPoint)
		{
			m_startPos = startPoint;
			m_endPos = endPoint;

			Vector3[] vertexPositions = {
				m_startPos,
				m_startPos,
				m_startPos,
				m_startPos,
				m_endPos,
				m_endPos,
				m_endPos,
				m_endPos,
			};
			
			Vector3[] other = {
				m_endPos,
				m_endPos,
				m_endPos,
				m_endPos,
				m_startPos,
				m_startPos,
				m_startPos,
				m_startPos,
			};

			if (null != m_meshFilter)
			{
				var mesh = m_meshFilter.sharedMesh;
				Debug.Assert(null != mesh);
				if (null != mesh)
				{
					mesh.vertices = vertexPositions;
					mesh.normals = other;
					UpdateBounds();
				}
			}
		}
		#endregion

		#region event functions
		void Start () 
		{
			Mesh mesh = new Mesh();
			m_meshFilter = GetComponent<MeshFilter>();
			m_meshFilter.mesh = mesh;
			SetStartAndEndPoints(m_startPos, m_endPos);
			mesh.uv = TurretBulletLineVertexData.TexCoords;
			mesh.uv2 = TurretBulletLineVertexData.VertexOffsets;
			mesh.SetIndices(TurretBulletLineVertexData.Indices, MeshTopology.Triangles, 0);
			CreateMaterial();
		}

		void OnDestroy()
		{
			if (null != m_meshFilter) 
			{
				if (Application.isPlaying) 
				{
					Mesh.Destroy(m_meshFilter.sharedMesh);
				}
				else 
				{
					Mesh.DestroyImmediate(m_meshFilter.sharedMesh);
				}
				m_meshFilter.sharedMesh = null;
			}
			DestroyMaterial();
		}
		
		void Update()
		{
			if (transform.hasChanged)
			{
				UpdateLineScale();
				UpdateBounds();
			}
		}

		void OnValidate()
		{
			// This function is called when the script is loaded or a value is changed in the inspector
            // (Called in the editor only).
			//  => make sure, everything stays up-to-date
			if(string.IsNullOrEmpty(gameObject.scene.name) || string.IsNullOrEmpty(gameObject.scene.path)) {
				return; // ...but not if a Prefab is selected! (Only if we're using it within a scene.)
			}
			CreateMaterial();
			SetAllMaterialProperties();
			UpdateBounds();
		}
	
		#endregion
	}
}