using Mono.Cecil;

namespace GamePatcher.Interfaces
{
	public interface IPatch
	{
		string Name { get; }
		int Apply(AssemblyDefinition assembly);
	}
}
