using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace GamePatcher
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string path = args[0];
			int patched = 0;

			ReaderParameters readerParameters = new ReaderParameters
			{
				ReadingMode = ReadingMode.Immediate,
				InMemory = true,
			};

			try
			{
				// Load assembly
				Log.Info("Reading assembly...");
				AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path, readerParameters);
				ModuleDefinition module = assembly.MainModule;
				Log.Info("Assembly successfully read.");

				// Loop through every type and method
				foreach (TypeDefinition type in module.Types)
				{
					Log.Info($"Checking type: {type.FullName}");

					foreach (MethodDefinition method in type.Methods)
					{
						// Check if method has an empty body for optimization
						if (!method.HasBody)
						{
							Log.Warn($"{type.FullName}::{method.Name} has an empty body. Skipping.");
							continue;
						}

						// Loop through the IL instructions
						Collection<Instruction> il = method.Body.Instructions;
						for (int i = 0; i < il.Count; i++)
						{
							Instruction instr = il[i];

							// OpCodes.Ldstr = IL instruction that loads a string literal onto the stack
							if (instr.OpCode == OpCodes.Ldstr && instr.Operand is string s && s.Contains("/../../Portail/Portail.exe"))
							{
								Log.Success($"Patching method {type.Name}::{method.Name}");

								// NOP all instructions around this
								for (int j = Math.Max(0, i - 3); j < Math.Min(il.Count, i + 3); j++)
								{
									// Replaces the process start instruction with a NOP (No Operation) instruction
									il[j].OpCode = OpCodes.Nop;
									il[j].Operand = null;
								}

								patched++;
							}
						}
					}

					Log.Info($"Parsed type: {type.FullName}");
				}

				// Output results
				if (patched > 0)
				{
					// Save assembly
					Log.Info("Applying changes...");
					assembly.Write(path);

					Log.Success($"Patched successfully. ({patched} methods patched)");
					Log.Info($"Patched assembly has been outputed at: \"{path}\"");
				}
				else
				{
					// No modifications found
					Log.Info("Coudn't find any methods to patch.");
					Log.Info("No patched assembly outputed.");
				}
			}
			catch (Exception ex)
			{
				Log.Error($"An error occured during the patching process!\nError: {ex.Message}");
			}
		}
	}
}