using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace GamePatcher.Utils
{
	static class CecilExtensions
	{
		public static int FindIndex(this Collection<Instruction> instructions, int startIndex, Func<Instruction, bool> predicate)
		{
			for (int i = startIndex; i < instructions.Count; i++)
			{
				if (predicate(instructions[i]))
					return i;
			}
			return -1;
		}
	}
}