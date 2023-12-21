#pragma once

#include <wrl/client.h>
#include <minwindef.h>
#include <cstdint>
#include "logger.h"

template<typename T>
using ComPtr = Microsoft::WRL::ComPtr<T>;



