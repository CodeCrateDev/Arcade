// Copyright (c) 2025 - Bastien "CodeCrate" Fortier

#include "utils.h"

const int EXIT_OK = 0;
const int EXIT_ARG_ERROR = 1;
const int EXIT_GAME_ERROR = 2;
const int EXIT_LAUNCHER_ERROR = 3;

int main(int argc, char* argv[])
{
	// Check if the user has passed the correct amount of arguments
	if (argc < 3)
	{
		std::cout << "Usage: launcherstub <GAMEPATH> <LAUNCHERPATH>" << std::endl;
		return EXIT_ARG_ERROR;
	}

	// Convert and format wstring
	std::wstring gamePathW = Utils::NormalizePath(Utils::CharToWide(argv[1]));
	std::wstring launcherPathW = Utils::NormalizePath(Utils::CharToWide(argv[2]));

	// Convert to native C99 wchar_t*
	const wchar_t* gamePath = gamePathW.c_str();
	const wchar_t* launcherPath = launcherPathW.c_str();

	// Game
	if (!Utils::LaunchAndWait(gamePath))
	{
		std::cerr << "Failed to launch game executable: " << argv[1] << std::endl;
		return EXIT_GAME_ERROR;
	}

	// Launcher
	if (!Utils::LaunchNoWait(launcherPath))
	{
		std::cerr << "Failed to launch launcher executable: " << argv[2] << std::endl;
		return EXIT_LAUNCHER_ERROR;
	}

	return EXIT_OK;
}
