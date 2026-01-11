// https://github.com/ds5678/KeyboardUtilities/blob/master/KeyboardUtilities/ReflectionHelpers.cs
using System.Reflection;

namespace QoL
{
    internal static class ReflectionHelpers
	{
		public const BindingFlags CommonFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;

		public static Type? GetTypeByName(string fullName)
		{
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in asm.TryGetTypes())
				{
					if (type.FullName == fullName)
					{
						return type;
					}
				}
			}

			return null;
		}

		public static void SearchTypesAndDo(string keyworld, Action<Type> action)
		{
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in asm.TryGetTypes())
				{
					if (type.FullName.Contains(keyworld))
					{
						action(type);
					}
				}
			}
		}

		private static IEnumerable<Type> TryGetTypes(this Assembly asm)
		{
			try
			{
				return asm.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				try
				{
					return asm.GetExportedTypes();
				}
				catch
				{
					return e.Types.Where(t => t != null)!;
				}
			}
			catch
			{
				return Enumerable.Empty<Type>();
			}
		}

		internal static void TryLoadGameModules()
		{
			LoadModule("Il2CppAssembly-CSharp");
			LoadModule("Il2CppAssembly-CSharp-firstpass");
		}

		public static bool LoadModule(string module)
		{
			string path = $@"MelonLoader\Il2CppAssemblies\{module}.dll";
			return LoadModuleInternal(path);
		}

		internal static bool LoadModuleInternal(string fullPath)
		{
			if (!File.Exists(fullPath))
			{
				return false;
			}

			try
			{
				Assembly.Load(File.ReadAllBytes(fullPath));
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.GetType() + ", " + e.Message);
			}

			return false;
		}
	}
}