using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Linq;

namespace GamePatcher
{
	public class LegacyLauncherCallPatch : IPatch
	{
		public string Name => "Remove Legacy Launcher Process Call";

		private const string TargetString = "/../../Portail/Portail.exe";

		public int Apply(AssemblyDefinition assembly)
		{
			int patches = 0;
			int local = 0;

			foreach (TypeDefinition type in assembly.MainModule.Types)
			{
				local = 0;

				Log.Info($"Checking type {type.Name}");

				foreach (MethodDefinition method in type.Methods)
				{
					Log.Info($"Checking {type.Name}::{method.Name}");

					if (!method.HasBody)
					{
						Log.Warn($"{type.Name}::{method.Name} has an empty body. Skipping.");
						continue;
					}

					Collection<Instruction> il = method.Body.Instructions;

					for (int i = 0; i < il.Count; i++)
					{
						Instruction instr = il[i];

						// Look for ldstr ".../Portail/Portail.exe"
						if (instr.OpCode == OpCodes.Ldstr && instr.Operand is string s && s.Contains(TargetString))
						{
							// Find following Process.Start call
							int startCallIndex = il.FindIndex(i + 1, ins =>
								(ins.OpCode == OpCodes.Call || ins.OpCode == OpCodes.Callvirt) &&
								ins.Operand is MethodReference mr &&
								mr.FullName.Contains("System.Diagnostics.Process::Start"));

							if (startCallIndex != -1)
							{
								Log.Success($"Patched {type.Name}::{method.Name}");

								// NOP everything from ldstr to Process.Start inclusive
								for (int j = i; j <= startCallIndex; j++)
								{
									il[j].OpCode = OpCodes.Nop;
									il[j].Operand = null;
								}

								// Optional: clean up empty methods
								if (MethodIsEmpty(method))
								{
									method.Body.Instructions.Clear();
									method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
								}

								local++;
								patches++;
							}
						}
					}
				}

				Log.Info($"{local} patch(es) applied to {type.Name}\n");
			}

			return patches;
		}

		private bool MethodIsEmpty(MethodDefinition method)
		{
			// Checks if after NOPing, there's nothing meaningful left
			return method.Body.Instructions.All(ins => ins.OpCode == OpCodes.Nop);
		}
	}

	// Helper extension for Cecil instruction list
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
