using GamePatcher.Interfaces;
using GamePatcher.Utils;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace GamePatcher.Patches
{
	public class LegacyLauncherCallPatch : IPatch
	{
		public string Name => "Remove Legacy Launcher Process Call";

		private const string TARGET_LITERAL = "/../../Portail/Portail.exe";

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
						if (instr.OpCode == OpCodes.Ldstr && instr.Operand is string s && s.Contains(TARGET_LITERAL))
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
	}
}
