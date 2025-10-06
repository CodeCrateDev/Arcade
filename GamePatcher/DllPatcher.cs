using Mono.Cecil;

namespace GamePatcher
{
	public class DllPatcher
	{
		private readonly string _path;
		private readonly AssemblyDefinition _assembly;
		private readonly List<IPatch> _patches = new List<IPatch>();

		public DllPatcher(string path)
		{
			_path = path;

			ReaderParameters readerParams = new ReaderParameters
			{
				ReadingMode = ReadingMode.Immediate,
				InMemory = true
			};

			_assembly = AssemblyDefinition.ReadAssembly(path, readerParams);
		}

		public void AddPatch(IPatch patch)
		{
			_patches.Add(patch);
		}

		public int ApplyPatches()
		{
			int totalApplied = 0;

			foreach (IPatch patch in _patches)
			{
				Log.Info($"Applying patch: {patch.Name}\n");
				totalApplied += patch.Apply(_assembly);
			}

			_assembly.Write(_path);
			return totalApplied;
		}
	}
}
