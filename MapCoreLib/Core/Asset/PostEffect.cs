using System.IO;
using MapCoreLib.Util;

namespace MapCoreLib.Core.Asset
{
    public class PostEffect
    {
        public struct Parameter
		{
			public object data;

			private string name;

			private string type;

			public Parameter(string name, string type, object data)
			{
				this.name = name;
				this.type = type;
				this.data = data;
			}

			public Parameter(BinaryReader br)
			{
				data = null;
				name = br.readDefaultString();
				type = br.readDefaultString();
				switch (type)
				{
				case "Float":
					data = br.ReadSingle();
					break;
				case "Float4":
				{
					data = new float[4];
					for (int i = 0; i < 4; i++)
					{
						((float[])data)[i] = br.ReadSingle();
					}
					break;
				}
				case "Texture":
					data = br.readDefaultString();
					break;
				case "Int":
					data = br.ReadInt32();
					// LogUtil.WriteLine("!\t PostEffect Parameter type Int unsure");
					break;
				default:
					// LogUtil.WriteLine("!\t Sub-asset: Parameter does not implement type: {0}", type);
					break;
				}
			}

			public void Save(BinaryWriter bw)
			{
				bw.writeDefaultString(name);
				bw.writeDefaultString(type);
				switch (type)
				{
				case "Float":
					bw.Write((float)data);
					break;
				case "Float4":
				{
					for (int i = 0; i < 4; i++)
					{
						bw.Write(((float[])data)[i]);
					}
					break;
				}
				case "Texture":
					bw.writeDefaultString((string)data);
					break;
				default:
					// LogUtil.log("!\t Sub-asset: Parameter does not implement type: {0}", type);
					break;
				}
			}
		}

		public string name;

		public Parameter[] parameters;

		public PostEffect(string name, Parameter[] parameters)
		{
			this.name = name;
			this.parameters = parameters;
		}

		public PostEffect(BinaryReader br)
		{
			name = br.readDefaultString();
			parameters = new Parameter[br.ReadInt32()];
			for (int i = 0; i < parameters.Length; i++)
			{
				ref Parameter reference = ref parameters[i];
				reference = new Parameter(br);
			}
		}

		public void Save(BinaryWriter bw)
		{
			bw.writeDefaultString(name);
			bw.Write(parameters.Length);
			Parameter[] array = parameters;
			foreach (Parameter p in array)
			{
				p.Save(bw);
			}
		}

		public static PostEffect Distortion()
		{
			return new PostEffect("Distortion", new Parameter[]{});
		}
		
		public static PostEffect LookupTable(string textureName, float value)
		{
			return new PostEffect("LookupTable", new Parameter[]
			{
				new Parameter("BlendFactor", "Float4", new float[] { value, 0f, 0f, 0f}),
				new Parameter("LookupTexture", "Texture", textureName),
			});
		}
		
		public static PostEffect Bloom(float value)
		{
			return new PostEffect("Bloom", new Parameter[]
			{
				new Parameter("Intensity", "Float", value),
			});
		}
    }
}