// Copyright (c) 2025 - Bastien "CodeCrate" Fortier

#include "utils.h"

std::wstring Utils::NormalizePath(std::wstring path)
{
    for (auto& ch : path)
    {
        if (ch == L'/') ch = L'\\';
    }
    return path;
}

std::wstring Utils::CharToWide(const char* str)
{
    if (!str)
    {
        return L"";
    }

    int len = MultiByteToWideChar(CP_UTF8, 0, str, -1, nullptr, 0);
    if (len == 0)
    {
        return L"";
    }

    std::wstring wstr(len - 1, L'\0');
    MultiByteToWideChar(CP_UTF8, 0, str, -1, &wstr[0], len);
    return wstr.c_str();
}

bool Utils::LaunchAndWait(const wchar_t* exePath)
{
    // Create the startup handle and process handle
    STARTUPINFOW si;
    PROCESS_INFORMATION pi;

    // Clear the memory of those handles
    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    ZeroMemory(&pi, sizeof(pi));

    bool ok = CreateProcessW(exePath, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi);

    if (!ok)
    {
        std::cerr << "Failed to start executable. Error: " << GetLastError() << std::endl;
        return false;
    }

    // Wait till closure and dispose
    WaitForSingleObject(pi.hThread, INFINITE);
    CloseHandle(pi.hProcess);
    CloseHandle(pi.hThread);

    return true;
}

bool Utils::LaunchNoWait(const wchar_t* exePath)
{
    // Create the startup handle and process handle
    STARTUPINFOW si;
    PROCESS_INFORMATION pi;

    // Clear the memory of those handles
    ZeroMemory(&si, sizeof(si));
    si.cb = sizeof(si);
    ZeroMemory(&pi, sizeof(pi));

    bool ok = CreateProcessW(exePath, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &si, &pi);

    if (!ok)
    {
        std::cerr << "Failed to start executable. Error: " << GetLastError() << std::endl;
        return false;
    }

    // Dispose of handles
    CloseHandle(pi.hProcess);
    CloseHandle(pi.hThread);

    return true;
}
