#include "pch.h"

/*
#include <d2d1_1.h>
#include <d3d11.h>
#include <wrl/client.h>
using Microsoft::WRL::ComPtr;

// Step 1: Initialize the COM Library
HRESULT hr = CoInitializeEx(nullptr, COINITBASE_MULTITHREADED);

// Step 2: Create the Direct3D Device and Context
UINT createDeviceFlags = 0;
#ifdef _DEBUG
createDeviceFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

D3D_FEATURE_LEVEL featureLevels[] = {
    D3D_FEATURE_LEVEL_11_1,
    D3D_FEATURE_LEVEL_11_0,
    // Additional feature levels as needed
};

ComPtr<ID3D11Device> d3dDevice;
ComPtr<ID3D11DeviceContext> d3dContext;
D3D_FEATURE_LEVEL featureLevel;

hr = D3D11CreateDevice(
    nullptr, // Default adapter
    D3D_DRIVER_TYPE_HARDWARE,
    nullptr, // No software device
    createDeviceFlags,
    featureLevels,
    _countof(featureLevels),
    D3D11_SDK_VERSION,
    &d3dDevice,
    &featureLevel,
    &d3dContext);

// Step 3: Create the Direct2D Factory
ComPtr<ID2D1Factory1> d2dFactory;
hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &d2dFactory);

// Step 4: Create the Direct2D Device
ComPtr<IDXGIDevice> dxgiDevice;
hr = d3dDevice.As(&dxgiDevice);

ComPtr<ID2D1Device> d2dDevice;
hr = d2dFactory->CreateDevice(dxgiDevice.Get(), &d2dDevice);

// Step 5: Create the Direct2D Device Context
ComPtr<ID2D1DeviceContext> d2dContext;
hr = d2dDevice->CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &d2dContext);

// Step 6: Register the Custom Effect
hr = d2dFactory->RegisterEffectFromString(
    CLSID_CustomEffect,
    L"xml representation of effect",
    // Factory methods for the effect
    ...
);

// Step 7: Create the effect
ComPtr<ID2D1Effect> effect;
hr = d2dContext->CreateEffect(CLSID_CustomEffect, &effect);

// Step 8: Configure the Effect
// (Set necessary properties or input images for the effect)

// Step 9: Create a Direct2D Bitmap
ComPtr<ID2D1Bitmap1> d2dBitmap;
D2D1_BITMAP_PROPERTIES1 bitmapProperties = { ... }; // Define bitmap properties
hr = d2dContext->CreateBitmap(D2D1::SizeU(width, height), nullptr, 0, &bitmapProperties, &d2dBitmap);

// Step 10: Render the Effect to the Bitmap
d2dContext->BeginDraw();
d2dContext->SetTarget(d2dBitmap.Get());
d2dContext->DrawImage(effect.Get());
hr = d2dContext->EndDraw();

// Step 11: Use the Bitmap as needed

// Step 12: Cleanup - Release all COM objects and resources
*/


/*
#include <d2d1_1.h>
#include <d3d11.h>
#include <wrl/client.h>
using Microsoft::WRL::ComPtr;

// Initialize the COM Library
HRESULT hr = CoInitializeEx(nullptr, COINITBASE_MULTITHREADED);

// Create the Direct3D Device and Context
UINT createDeviceFlags = 0;
#ifdef _DEBUG
createDeviceFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

D3D_FEATURE_LEVEL featureLevels[] = {
    D3D_FEATURE_LEVEL_11_1,
    D3D_FEATURE_LEVEL_11_0,
    // Additional feature levels as needed
};

ComPtr<ID3D11Device> d3dDevice;
ComPtr<ID3D11DeviceContext> d3dContext;
D3D_FEATURE_LEVEL featureLevel;

hr = D3D11CreateDevice(
    nullptr, // Default adapter
    D3D_DRIVER_TYPE_HARDWARE,
    nullptr, // No software device
    createDeviceFlags,
    featureLevels,
    _countof(featureLevels),
    D3D11_SDK_VERSION,
    &d3dDevice,
    &featureLevel,
    &d3dContext);

// Create the Direct2D Factory
ComPtr<ID2D1Factory1> d2dFactory;
hr = D2D1CreateFactory(D2D1_FACTORY_TYPE_SINGLE_THREADED, &d2dFactory);

// Create the Direct2D Device
ComPtr<IDXGIDevice> dxgiDevice;
hr = d3dDevice.As(&dxgiDevice);

ComPtr<ID2D1Device> d2dDevice;
hr = d2dFactory->CreateDevice(dxgiDevice.Get(), &d2dDevice);

// Create the Direct2D Device Context
ComPtr<ID2D1DeviceContext> d2dContext;
hr = d2dDevice->CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &d2dContext);

// Register the Custom Effect
hr = d2dFactory->RegisterEffectFromString(
    CLSID_CustomEffect,
    L"xml representation of effect",
    // Factory methods for the effect
    ...
);

// Create the effect
ComPtr<ID2D1Effect> effect;
hr = d2dContext->CreateEffect(CLSID_CustomEffect, &effect);

// Configure the Effect
// (Set necessary properties or input images for the effect)

// Create the ID2D1CommandList
ComPtr<ID2D1CommandList> commandList;
hr = d2dContext->CreateCommandList(&commandList);

// Start recording commands
d2dContext->SetTarget(commandList.Get());
d2dContext->BeginDraw();

// Draw using Device Context
// For example, drawing an effect
d2dContext->DrawImage(effect.Get());

// End drawing and close the command list
hr = d2dContext->EndDraw();
hr = commandList->Close();

// Create a Direct2D Bitmap
D2D1_BITMAP_PROPERTIES1 bitmapProperties = { ... }; // Define bitmap properties
ComPtr<ID2D1Bitmap1> d2dBitmap;
hr = d2dContext->CreateBitmap(D2D1::SizeU(width, height), nullptr, 0, &bitmapProperties, &d2dBitmap);

// Draw the command list into the bitmap
d2dContext->SetTarget(d2dBitmap.Get());
d2dContext->BeginDraw();
d2dContext->DrawImage(commandList.Get());
hr = d2dContext->EndDraw();

// Use the Bitmap as needed

// Cleanup - Release all COM objects and resources
*/
