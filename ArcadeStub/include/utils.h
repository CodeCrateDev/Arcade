// Copyright (c) 2025 - Bastien "CodeCrate" Fortier

#ifndef UTILS_H
#define UTILS_H

#include <windows.h>
#include <iostream>
#include <string>

namespace Utils
{
	std::wstring NormalizePath(std::wstring path);
	std::wstring CharToWide(const char* str);

	bool LaunchAndWait(const wchar_t* exePath);
	bool LaunchNoWait(const wchar_t* exePath);
}
#endif