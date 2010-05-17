using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Raven.Client.Util;
using System.Linq;

namespace Raven.Client.Document
{
	public class DocumentConvention
	{
		public DocumentConvention()
		{
			FindIdentityProperty = q => q.Name == "Id";
			FindTypeTagName = t => DefaultTypeTagName(t);
		    JsonContractResolver = new DefaultContractResolver(shareCache: true);
		}

		public static string GenerateDocumentKeyUsingIdentity(DocumentConvention conventions, object entity)
		{
			return conventions.FindTypeTagName(entity.GetType()).ToLowerInvariant() + "/";
		}

		public static string DefaultTypeTagName(Type t)
		{
			return Inflector.Pluralize(t.Name);
		}

		public string GetTypeTagName(Type type)
		{
			return FindTypeTagName(type) ?? DefaultTypeTagName(type);
		}

		public string GenerateDocumentKey(object entity)
		{
			return DocumentKeyGenerator(entity);
		}

		public PropertyInfo GetIdentityProperty(Type type)
		{
			return type.GetProperties().FirstOrDefault(FindIdentityProperty);
		}

        public IContractResolver JsonContractResolver { get; set; }

		public Func<Type, string> FindTypeTagName { get; set; }
		public Func<PropertyInfo, bool> FindIdentityProperty { get; set; }

		public Func<object, string> DocumentKeyGenerator { get; set; }
	}
}