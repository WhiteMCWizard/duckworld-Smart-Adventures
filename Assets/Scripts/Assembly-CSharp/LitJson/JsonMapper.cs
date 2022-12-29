using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LitJson
{
	public class JsonMapper
	{
		public interface IHasJsonCallback
		{
			void JsonCallback();
		}

		[CompilerGenerated]
		private sealed class _003CRegisterExporter_003Ec__AnonStorey19E<T>
		{
			internal ExporterFunc<T> exporter;

			internal void _003C_003Em__133(object obj, JsonWriter writer)
			{
				exporter((T)obj, writer);
			}
		}

		[CompilerGenerated]
		private sealed class _003CRegisterImporter_003Ec__AnonStorey19F<TJson, TValue>
		{
			internal ImporterFunc<TJson, TValue> importer;

			internal object _003C_003Em__134(object input)
			{
				return importer((TJson)input);
			}
		}

		private static int max_nesting_depth;

		private static IFormatProvider datetime_format;

		private static IDictionary<Type, ExporterFunc> base_exporters_table;

		private static IDictionary<Type, ExporterFunc> custom_exporters_table;

		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;

		private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table;

		private static IDictionary<Type, ArrayMetadata> array_metadata;

		private static readonly object array_metadata_lock;

		private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops;

		private static readonly object conv_ops_lock;

		private static IDictionary<Type, ObjectMetadata> object_metadata;

		private static readonly object object_metadata_lock;

		private static IDictionary<Type, IList<PropertyMetadata>> type_properties;

		private static readonly object type_properties_lock;

		private static JsonWriter static_writer;

		private static readonly object static_writer_lock;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache10;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache11;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache12;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache13;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache14;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache15;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache16;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache17;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache18;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache19;

		[CompilerGenerated]
		private static ExporterFunc _003C_003Ef__am_0024cache1A;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1B;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1C;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1D;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1E;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache1F;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache20;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache21;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache22;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache23;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache24;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache25;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache26;

		[CompilerGenerated]
		private static ImporterFunc _003C_003Ef__am_0024cache27;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache28;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache29;

		[CompilerGenerated]
		private static WrapperFactory _003C_003Ef__am_0024cache2A;

		static JsonMapper()
		{
			array_metadata_lock = new object();
			conv_ops_lock = new object();
			object_metadata_lock = new object();
			type_properties_lock = new object();
			static_writer_lock = new object();
			max_nesting_depth = 100;
			array_metadata = new Dictionary<Type, ArrayMetadata>();
			conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
			object_metadata = new Dictionary<Type, ObjectMetadata>();
			type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
			static_writer = new JsonWriter();
			datetime_format = DateTimeFormatInfo.InvariantInfo;
			base_exporters_table = new Dictionary<Type, ExporterFunc>();
			custom_exporters_table = new Dictionary<Type, ExporterFunc>();
			base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			RegisterBaseExporters();
			RegisterBaseImporters();
		}

		private static void AddArrayMetadata(Type type)
		{
			//Discarded unreachable code: IL_00db
			if (array_metadata.ContainsKey(type))
			{
				return;
			}
			ArrayMetadata value = default(ArrayMetadata);
			value.IsArray = type.IsArray;
			if (type.GetInterface("System.Collections.IList") != null)
			{
				value.IsList = true;
			}
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!(propertyInfo.Name != "Item"))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(int))
					{
						value.ElementType = propertyInfo.PropertyType;
					}
				}
			}
			lock (array_metadata_lock)
			{
				try
				{
					array_metadata.Add(type, value);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private static void AddObjectMetadata(Type type)
		{
			//Discarded unreachable code: IL_01ac
			if (object_metadata.ContainsKey(type))
			{
				return;
			}
			ObjectMetadata value = default(ObjectMetadata);
			if (type.GetInterface("System.Collections.IDictionary") != null)
			{
				value.IsDictionary = true;
			}
			value.Properties = new Dictionary<string, PropertyMetadata>();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.GetCustomAttributes(typeof(JsonIgnore), true).Length > 0)
				{
					continue;
				}
				if (propertyInfo.Name == "Item")
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(string))
					{
						value.ElementType = propertyInfo.PropertyType;
					}
				}
				else
				{
					PropertyMetadata value2 = default(PropertyMetadata);
					value2.Info = propertyInfo;
					value2.Type = propertyInfo.PropertyType;
					value.Properties.Add(getPropertyName(propertyInfo), value2);
				}
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.GetCustomAttributes(typeof(JsonIgnore), true).Length <= 0)
				{
					PropertyMetadata value3 = default(PropertyMetadata);
					value3.Info = fieldInfo;
					value3.IsField = true;
					value3.Type = fieldInfo.FieldType;
					value.Properties.Add(getPropertyName(fieldInfo), value3);
				}
			}
			lock (object_metadata_lock)
			{
				try
				{
					object_metadata.Add(type, value);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private static void AddTypeProperties(Type type)
		{
			//Discarded unreachable code: IL_011e
			if (type_properties.ContainsKey(type))
			{
				return;
			}
			IList<PropertyMetadata> list = new List<PropertyMetadata>();
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.GetCustomAttributes(typeof(JsonIgnore), true).Length <= 0 && !(propertyInfo.Name == "Item"))
				{
					PropertyMetadata item = default(PropertyMetadata);
					item.Info = propertyInfo;
					item.IsField = false;
					list.Add(item);
				}
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.GetCustomAttributes(typeof(JsonIgnore), true).Length <= 0)
				{
					PropertyMetadata item2 = default(PropertyMetadata);
					item2.Info = fieldInfo;
					item2.IsField = true;
					list.Add(item2);
				}
			}
			lock (type_properties_lock)
			{
				try
				{
					type_properties.Add(type, list);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		private static MethodInfo GetConvOp(Type t1, Type t2)
		{
			//Discarded unreachable code: IL_00b1
			lock (conv_ops_lock)
			{
				if (!conv_ops.ContainsKey(t1))
				{
					conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
				}
			}
			if (conv_ops[t1].ContainsKey(t2))
			{
				return conv_ops[t1][t2];
			}
			MethodInfo method = t1.GetMethod("op_Implicit", new Type[1] { t2 });
			lock (conv_ops_lock)
			{
				try
				{
					conv_ops[t1].Add(t2, method);
					return method;
				}
				catch (ArgumentException)
				{
					return conv_ops[t1][t2];
				}
			}
		}

		private static object ReadValue(Type inst_type, JsonReader reader)
		{
			object value = reader.Value;
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			if (reader.Token == JsonToken.Null)
			{
				if (inst_type == typeof(int))
				{
					return 0;
				}
				if (!inst_type.IsClass && (!inst_type.IsGenericType || inst_type.GetGenericTypeDefinition() != typeof(Nullable<>)))
				{
					throw new JsonException(string.Format(string.Concat("Can't assign null to an instance of type {0} (", value, ")"), inst_type));
				}
				return null;
			}
			if (reader.Token == JsonToken.Double || reader.Token == JsonToken.Int || reader.Token == JsonToken.Long || reader.Token == JsonToken.String || reader.Token == JsonToken.Boolean)
			{
				Type type = reader.Value.GetType();
				if (inst_type.IsAssignableFrom(type))
				{
					return reader.Value;
				}
				if (custom_importers_table.ContainsKey(type) && custom_importers_table[type].ContainsKey(inst_type))
				{
					ImporterFunc importerFunc = custom_importers_table[type][inst_type];
					return importerFunc(reader.Value);
				}
				if (base_importers_table.ContainsKey(type) && base_importers_table[type].ContainsKey(inst_type))
				{
					ImporterFunc importerFunc2 = base_importers_table[type][inst_type];
					return importerFunc2(reader.Value);
				}
				if (inst_type.IsEnum)
				{
					return Enum.Parse(inst_type, reader.Value.ToString());
				}
				MethodInfo convOp = GetConvOp(inst_type, type);
				if (convOp != null)
				{
					return convOp.Invoke(null, new object[1] { reader.Value });
				}
				if (type == typeof(string))
				{
					string text = (string)reader.Value;
					if (string.IsNullOrEmpty(text))
					{
						return null;
					}
					object obj = ReadValue(inst_type, new JsonReader(text));
					if (obj != null)
					{
						return obj;
					}
				}
				throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, type, inst_type));
			}
			object obj2 = null;
			if (reader.Token == JsonToken.ArrayStart)
			{
				AddArrayMetadata(inst_type);
				ArrayMetadata arrayMetadata = array_metadata[inst_type];
				if (!arrayMetadata.IsArray && !arrayMetadata.IsList)
				{
					throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
				}
				IList list;
				Type elementType;
				if (!arrayMetadata.IsArray)
				{
					list = (IList)Activator.CreateInstance(inst_type);
					elementType = arrayMetadata.ElementType;
				}
				else
				{
					list = new ArrayList();
					elementType = inst_type.GetElementType();
				}
				while (true)
				{
					object obj3 = ReadValue(elementType, reader);
					if (obj3 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					list.Add(obj3);
				}
				if (arrayMetadata.IsArray)
				{
					int count = list.Count;
					obj2 = Array.CreateInstance(elementType, count);
					for (int i = 0; i < count; i++)
					{
						((Array)obj2).SetValue(list[i], i);
					}
				}
				else
				{
					obj2 = list;
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				AddObjectMetadata(inst_type);
				ObjectMetadata objectMetadata = object_metadata[inst_type];
				obj2 = Activator.CreateInstance(inst_type);
				while (true)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string text2 = (string)reader.Value;
					if (objectMetadata.Properties.ContainsKey(text2))
					{
						PropertyMetadata propertyMetadata = objectMetadata.Properties[text2];
						if (propertyMetadata.IsField)
						{
							((FieldInfo)propertyMetadata.Info).SetValue(obj2, ReadValue(propertyMetadata.Type, reader));
							continue;
						}
						PropertyInfo propertyInfo = (PropertyInfo)propertyMetadata.Info;
						if (propertyInfo.CanWrite)
						{
							propertyInfo.SetValue(obj2, ReadValue(propertyMetadata.Type, reader), null);
						}
						else
						{
							ReadValue(propertyMetadata.Type, reader);
						}
					}
					else if (!objectMetadata.IsDictionary)
					{
						if (!reader.SkipNonMembers)
						{
							throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, text2));
						}
						ReadSkip(reader);
					}
					else
					{
						((IDictionary)obj2).Add(text2, ReadValue(objectMetadata.ElementType, reader));
					}
				}
			}
			if (obj2 is IHasJsonCallback)
			{
				((IHasJsonCallback)obj2).JsonCallback();
			}
			return obj2;
		}

		private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
			{
				return null;
			}
			IJsonWrapper jsonWrapper = factory();
			if (reader.Token == JsonToken.String)
			{
				jsonWrapper.SetString((string)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Double)
			{
				jsonWrapper.SetDouble((double)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Int)
			{
				jsonWrapper.SetInt((int)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Long)
			{
				jsonWrapper.SetLong((long)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				jsonWrapper.SetBoolean((bool)reader.Value);
				return jsonWrapper;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				jsonWrapper.SetJsonType(JsonType.Array);
				while (true)
				{
					IJsonWrapper jsonWrapper2 = ReadValue(factory, reader);
					if (jsonWrapper2 == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					jsonWrapper.Add(jsonWrapper2);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				jsonWrapper.SetJsonType(JsonType.Object);
				while (true)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string key = (string)reader.Value;
					jsonWrapper[key] = ReadValue(factory, reader);
				}
			}
			return jsonWrapper;
		}

		private static void ReadSkip(JsonReader reader)
		{
			if (_003C_003Ef__am_0024cache10 == null)
			{
				_003C_003Ef__am_0024cache10 = _003CReadSkip_003Em__118;
			}
			ToWrapper(_003C_003Ef__am_0024cache10, reader);
		}

		private static void RegisterBaseExporters()
		{
			IDictionary<Type, ExporterFunc> dictionary = base_exporters_table;
			Type typeFromHandle = typeof(byte);
			if (_003C_003Ef__am_0024cache11 == null)
			{
				_003C_003Ef__am_0024cache11 = _003CRegisterBaseExporters_003Em__119;
			}
			dictionary[typeFromHandle] = _003C_003Ef__am_0024cache11;
			IDictionary<Type, ExporterFunc> dictionary2 = base_exporters_table;
			Type typeFromHandle2 = typeof(char);
			if (_003C_003Ef__am_0024cache12 == null)
			{
				_003C_003Ef__am_0024cache12 = _003CRegisterBaseExporters_003Em__11A;
			}
			dictionary2[typeFromHandle2] = _003C_003Ef__am_0024cache12;
			IDictionary<Type, ExporterFunc> dictionary3 = base_exporters_table;
			Type typeFromHandle3 = typeof(DateTime);
			if (_003C_003Ef__am_0024cache13 == null)
			{
				_003C_003Ef__am_0024cache13 = _003CRegisterBaseExporters_003Em__11B;
			}
			dictionary3[typeFromHandle3] = _003C_003Ef__am_0024cache13;
			IDictionary<Type, ExporterFunc> dictionary4 = base_exporters_table;
			Type typeFromHandle4 = typeof(decimal);
			if (_003C_003Ef__am_0024cache14 == null)
			{
				_003C_003Ef__am_0024cache14 = _003CRegisterBaseExporters_003Em__11C;
			}
			dictionary4[typeFromHandle4] = _003C_003Ef__am_0024cache14;
			IDictionary<Type, ExporterFunc> dictionary5 = base_exporters_table;
			Type typeFromHandle5 = typeof(sbyte);
			if (_003C_003Ef__am_0024cache15 == null)
			{
				_003C_003Ef__am_0024cache15 = _003CRegisterBaseExporters_003Em__11D;
			}
			dictionary5[typeFromHandle5] = _003C_003Ef__am_0024cache15;
			IDictionary<Type, ExporterFunc> dictionary6 = base_exporters_table;
			Type typeFromHandle6 = typeof(short);
			if (_003C_003Ef__am_0024cache16 == null)
			{
				_003C_003Ef__am_0024cache16 = _003CRegisterBaseExporters_003Em__11E;
			}
			dictionary6[typeFromHandle6] = _003C_003Ef__am_0024cache16;
			IDictionary<Type, ExporterFunc> dictionary7 = base_exporters_table;
			Type typeFromHandle7 = typeof(ushort);
			if (_003C_003Ef__am_0024cache17 == null)
			{
				_003C_003Ef__am_0024cache17 = _003CRegisterBaseExporters_003Em__11F;
			}
			dictionary7[typeFromHandle7] = _003C_003Ef__am_0024cache17;
			IDictionary<Type, ExporterFunc> dictionary8 = base_exporters_table;
			Type typeFromHandle8 = typeof(uint);
			if (_003C_003Ef__am_0024cache18 == null)
			{
				_003C_003Ef__am_0024cache18 = _003CRegisterBaseExporters_003Em__120;
			}
			dictionary8[typeFromHandle8] = _003C_003Ef__am_0024cache18;
			IDictionary<Type, ExporterFunc> dictionary9 = base_exporters_table;
			Type typeFromHandle9 = typeof(ulong);
			if (_003C_003Ef__am_0024cache19 == null)
			{
				_003C_003Ef__am_0024cache19 = _003CRegisterBaseExporters_003Em__121;
			}
			dictionary9[typeFromHandle9] = _003C_003Ef__am_0024cache19;
			IDictionary<Type, ExporterFunc> dictionary10 = base_exporters_table;
			Type typeFromHandle10 = typeof(float);
			if (_003C_003Ef__am_0024cache1A == null)
			{
				_003C_003Ef__am_0024cache1A = _003CRegisterBaseExporters_003Em__122;
			}
			dictionary10[typeFromHandle10] = _003C_003Ef__am_0024cache1A;
		}

		private static void RegisterBaseImporters()
		{
			if (_003C_003Ef__am_0024cache1B == null)
			{
				_003C_003Ef__am_0024cache1B = _003CRegisterBaseImporters_003Em__123;
			}
			ImporterFunc importer = _003C_003Ef__am_0024cache1B;
			RegisterImporter(base_importers_table, typeof(int), typeof(byte), importer);
			if (_003C_003Ef__am_0024cache1C == null)
			{
				_003C_003Ef__am_0024cache1C = _003CRegisterBaseImporters_003Em__124;
			}
			importer = _003C_003Ef__am_0024cache1C;
			RegisterImporter(base_importers_table, typeof(int), typeof(ulong), importer);
			if (_003C_003Ef__am_0024cache1D == null)
			{
				_003C_003Ef__am_0024cache1D = _003CRegisterBaseImporters_003Em__125;
			}
			importer = _003C_003Ef__am_0024cache1D;
			RegisterImporter(base_importers_table, typeof(int), typeof(sbyte), importer);
			if (_003C_003Ef__am_0024cache1E == null)
			{
				_003C_003Ef__am_0024cache1E = _003CRegisterBaseImporters_003Em__126;
			}
			importer = _003C_003Ef__am_0024cache1E;
			RegisterImporter(base_importers_table, typeof(int), typeof(short), importer);
			if (_003C_003Ef__am_0024cache1F == null)
			{
				_003C_003Ef__am_0024cache1F = _003CRegisterBaseImporters_003Em__127;
			}
			importer = _003C_003Ef__am_0024cache1F;
			RegisterImporter(base_importers_table, typeof(int), typeof(ushort), importer);
			if (_003C_003Ef__am_0024cache20 == null)
			{
				_003C_003Ef__am_0024cache20 = _003CRegisterBaseImporters_003Em__128;
			}
			importer = _003C_003Ef__am_0024cache20;
			RegisterImporter(base_importers_table, typeof(int), typeof(uint), importer);
			if (_003C_003Ef__am_0024cache21 == null)
			{
				_003C_003Ef__am_0024cache21 = _003CRegisterBaseImporters_003Em__129;
			}
			importer = _003C_003Ef__am_0024cache21;
			RegisterImporter(base_importers_table, typeof(int), typeof(float), importer);
			if (_003C_003Ef__am_0024cache22 == null)
			{
				_003C_003Ef__am_0024cache22 = _003CRegisterBaseImporters_003Em__12A;
			}
			importer = _003C_003Ef__am_0024cache22;
			RegisterImporter(base_importers_table, typeof(int), typeof(double), importer);
			if (_003C_003Ef__am_0024cache23 == null)
			{
				_003C_003Ef__am_0024cache23 = _003CRegisterBaseImporters_003Em__12B;
			}
			importer = _003C_003Ef__am_0024cache23;
			RegisterImporter(base_importers_table, typeof(double), typeof(decimal), importer);
			if (_003C_003Ef__am_0024cache24 == null)
			{
				_003C_003Ef__am_0024cache24 = _003CRegisterBaseImporters_003Em__12C;
			}
			importer = _003C_003Ef__am_0024cache24;
			RegisterImporter(base_importers_table, typeof(long), typeof(uint), importer);
			if (_003C_003Ef__am_0024cache25 == null)
			{
				_003C_003Ef__am_0024cache25 = _003CRegisterBaseImporters_003Em__12D;
			}
			importer = _003C_003Ef__am_0024cache25;
			RegisterImporter(base_importers_table, typeof(string), typeof(char), importer);
			if (_003C_003Ef__am_0024cache26 == null)
			{
				_003C_003Ef__am_0024cache26 = _003CRegisterBaseImporters_003Em__12E;
			}
			importer = _003C_003Ef__am_0024cache26;
			RegisterImporter(base_importers_table, typeof(string), typeof(DateTime), importer);
			if (_003C_003Ef__am_0024cache27 == null)
			{
				_003C_003Ef__am_0024cache27 = _003CRegisterBaseImporters_003Em__12F;
			}
			importer = _003C_003Ef__am_0024cache27;
			RegisterImporter(base_importers_table, typeof(double), typeof(float), importer);
		}

		private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, new Dictionary<Type, ImporterFunc>());
			}
			table[json_type][value_type] = importer;
		}

		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			if (depth > max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (writer_is_private)
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
				}
				else
				{
					((IJsonWrapper)obj).ToJson(writer);
				}
				return;
			}
			if (obj is string)
			{
				writer.Write((string)obj);
				return;
			}
			if (obj is double)
			{
				writer.Write((double)obj);
				return;
			}
			if (obj is int)
			{
				writer.Write((int)obj);
				return;
			}
			if (obj is bool)
			{
				writer.Write((bool)obj);
				return;
			}
			if (obj is long)
			{
				writer.Write((long)obj);
				return;
			}
			if (obj is Array)
			{
				writer.WriteArrayStart();
				foreach (object item in (Array)obj)
				{
					WriteValue(item, writer, writer_is_private, depth + 1);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IList)
			{
				writer.WriteArrayStart();
				foreach (object item2 in (IList)obj)
				{
					WriteValue(item2, writer, writer_is_private, depth + 1);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj is IDictionary)
			{
				writer.WriteObjectStart();
				foreach (DictionaryEntry item3 in (IDictionary)obj)
				{
					writer.WritePropertyName((string)item3.Key);
					WriteValue(item3.Value, writer, writer_is_private, depth + 1);
				}
				writer.WriteObjectEnd();
				return;
			}
			Type type = obj.GetType();
			if (custom_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc = custom_exporters_table[type];
				exporterFunc(obj, writer);
				return;
			}
			if (base_exporters_table.ContainsKey(type))
			{
				ExporterFunc exporterFunc2 = base_exporters_table[type];
				exporterFunc2(obj, writer);
				return;
			}
			if (obj is Enum)
			{
				writer.Write(obj.ToString());
				return;
			}
			AddTypeProperties(type);
			IList<PropertyMetadata> list = type_properties[type];
			writer.WriteObjectStart();
			foreach (PropertyMetadata item4 in list)
			{
				if (item4.IsField)
				{
					writer.WritePropertyName(getPropertyName(item4.Info));
					WriteValue(((FieldInfo)item4.Info).GetValue(obj), writer, writer_is_private, depth + 1);
					continue;
				}
				PropertyInfo propertyInfo = (PropertyInfo)item4.Info;
				if (propertyInfo.CanRead)
				{
					writer.WritePropertyName(item4.Info.Name);
					WriteValue(propertyInfo.GetValue(obj, null), writer, writer_is_private, depth + 1);
				}
			}
			writer.WriteObjectEnd();
		}

		private static string getPropertyName(MemberInfo memInfo)
		{
			object[] customAttributes = memInfo.GetCustomAttributes(typeof(JsonName), true);
			if (customAttributes.Length > 0)
			{
				return ((JsonName)customAttributes[0]).Name;
			}
			return memInfo.Name;
		}

		public static string ToJson(object obj)
		{
			//Discarded unreachable code: IL_0033
			lock (static_writer_lock)
			{
				static_writer.Reset();
				WriteValue(obj, static_writer, true, 0);
				return static_writer.ToString();
			}
		}

		public static void ToJson(object obj, JsonWriter writer)
		{
			WriteValue(obj, writer, false, 0);
		}

		public static JsonData ToObject(JsonReader reader)
		{
			if (_003C_003Ef__am_0024cache28 == null)
			{
				_003C_003Ef__am_0024cache28 = _003CToObject_003Em__130;
			}
			return (JsonData)ToWrapper(_003C_003Ef__am_0024cache28, reader);
		}

		public static JsonData ToObject(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			if (_003C_003Ef__am_0024cache29 == null)
			{
				_003C_003Ef__am_0024cache29 = _003CToObject_003Em__131;
			}
			return (JsonData)ToWrapper(_003C_003Ef__am_0024cache29, reader2);
		}

		public static JsonData ToObject(string json)
		{
			if (_003C_003Ef__am_0024cache2A == null)
			{
				_003C_003Ef__am_0024cache2A = _003CToObject_003Em__132;
			}
			return (JsonData)ToWrapper(_003C_003Ef__am_0024cache2A, json);
		}

		public static T ToObject<T>(JsonReader reader)
		{
			return (T)ReadValue(typeof(T), reader);
		}

		public static T ToObject<T>(TextReader reader)
		{
			JsonReader reader2 = new JsonReader(reader);
			return (T)ReadValue(typeof(T), reader2);
		}

		public static T ToObject<T>(string json)
		{
			JsonReader reader = new JsonReader(json);
			return (T)ReadValue(typeof(T), reader);
		}

		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return ReadValue(factory, reader);
		}

		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			JsonReader reader = new JsonReader(json);
			return ReadValue(factory, reader);
		}

		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			_003CRegisterExporter_003Ec__AnonStorey19E<T> _003CRegisterExporter_003Ec__AnonStorey19E = new _003CRegisterExporter_003Ec__AnonStorey19E<T>();
			_003CRegisterExporter_003Ec__AnonStorey19E.exporter = exporter;
			ExporterFunc value = _003CRegisterExporter_003Ec__AnonStorey19E._003C_003Em__133;
			custom_exporters_table[typeof(T)] = value;
		}

		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			_003CRegisterImporter_003Ec__AnonStorey19F<TJson, TValue> _003CRegisterImporter_003Ec__AnonStorey19F = new _003CRegisterImporter_003Ec__AnonStorey19F<TJson, TValue>();
			_003CRegisterImporter_003Ec__AnonStorey19F.importer = importer;
			ImporterFunc importer2 = _003CRegisterImporter_003Ec__AnonStorey19F._003C_003Em__134;
			RegisterImporter(custom_importers_table, typeof(TJson), typeof(TValue), importer2);
		}

		public static void UnregisterExporters()
		{
			custom_exporters_table.Clear();
		}

		public static void UnregisterImporters()
		{
			custom_importers_table.Clear();
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CReadSkip_003Em__118()
		{
			return new JsonMockWrapper();
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__119(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((byte)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__11A(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToString((char)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__11B(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToString((DateTime)obj, datetime_format));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__11C(object obj, JsonWriter writer)
		{
			writer.Write((decimal)obj);
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__11D(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((sbyte)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__11E(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((short)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__11F(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToInt32((ushort)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__120(object obj, JsonWriter writer)
		{
			writer.Write(Convert.ToUInt64((uint)obj));
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__121(object obj, JsonWriter writer)
		{
			writer.Write((ulong)obj);
		}

		[CompilerGenerated]
		private static void _003CRegisterBaseExporters_003Em__122(object obj, JsonWriter writer)
		{
			writer.Write((float)obj);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__123(object input)
		{
			return Convert.ToByte((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__124(object input)
		{
			return Convert.ToUInt64((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__125(object input)
		{
			return Convert.ToSByte((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__126(object input)
		{
			return Convert.ToInt16((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__127(object input)
		{
			return Convert.ToUInt16((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__128(object input)
		{
			return Convert.ToUInt32((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__129(object input)
		{
			return Convert.ToSingle((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12A(object input)
		{
			return Convert.ToDouble((int)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12B(object input)
		{
			return Convert.ToDecimal((double)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12C(object input)
		{
			return Convert.ToUInt32((long)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12D(object input)
		{
			return Convert.ToChar((string)input);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12E(object input)
		{
			return Convert.ToDateTime((string)input, datetime_format);
		}

		[CompilerGenerated]
		private static object _003CRegisterBaseImporters_003Em__12F(object input)
		{
			return Convert.ToSingle((float)(double)input);
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CToObject_003Em__130()
		{
			return new JsonData();
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CToObject_003Em__131()
		{
			return new JsonData();
		}

		[CompilerGenerated]
		private static IJsonWrapper _003CToObject_003Em__132()
		{
			return new JsonData();
		}
	}
}
