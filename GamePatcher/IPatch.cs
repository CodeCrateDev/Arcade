using Mono.Cecil;

namespace GamePatcher
{
	public interface IPatch
	{
		string Name { get; }
		int Apply(AssemblyDefinition assembly);
	}
}
