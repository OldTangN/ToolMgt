using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToolMgt.Model
{
    public class ObjectCopier
    {
        /// <summary>
        /// 使用二进制流序列化、反序列化克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        //public static T CloneJson<T>(this T source)
        //{
        //    if (Object.ReferenceEquals(source, null))
        //    {
        //        return default(T);
        //    }
        //    var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        //    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        //}

        /// <summary>
        /// 使用反射克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflect<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;
            object retval = Activator.CreateInstance(obj.GetType());
            PropertyInfo[] fields = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (PropertyInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        /// <summary>
        /// Xml序列化、反序列化克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string xmlData) where T : new()
        {
            if (string.IsNullOrEmpty(xmlData))
                return default(T);

            TextReader tr = new StringReader(xmlData);
            T DocItms = new T();
            XmlSerializer xms = new XmlSerializer(DocItms.GetType());
            DocItms = (T)xms.Deserialize(tr);

            return DocItms == null ? default(T) : DocItms;
        }
    }
}
