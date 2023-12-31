#pragma once

#include <windows.h>
#include <sstream>
#include <fstream>


enum class LogMode {
    Console,
    File
};

class DebugLogger {
public:
    DebugLogger(LogMode mode = LogMode::File) : mode(mode) {
        if (mode == LogMode::File) {
            file.open("debug_log.txt", std::ios::out | std::ios::app);
        }
    }

    // Destructor to output the final message
    ~DebugLogger() {
        if (mode == LogMode::Console) {
            OutputDebugString(stream.str().c_str());
        }
        else if (mode == LogMode::File) {
            if (file.is_open()) {
                file << stream.str();
                file.close();
            }
        }
    }

    // Overload the << operator
    template<typename T>
    DebugLogger& operator<<(const T& value) {
        stream << value;
        return *this;
    }

    DebugLogger& operator<<(const std::string& value) {
        // Convert std::string to std::wstring
        std::wstring wideValue(value.begin(), value.end());
        stream << wideValue;
        return *this;
    }

private:
    LogMode mode = LogMode::File;
    std::wostringstream stream;
    std::wofstream file;
};


inline DebugLogger log() {
    return DebugLogger(LogMode::Console);
}
