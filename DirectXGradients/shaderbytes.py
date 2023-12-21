with open(r'C:\Users\rando\source\repos\WPF\XamlSpinners\x64\Debug\ConicGradient.cso', 'rb') as f:
    bytecode = f.read()

with open('ShaderBytecode.h', 'w') as f:
    f.write('const BYTE ConicGradientPixelShader[] = {\n')
    f.write(', '.join([str(b) for b in bytecode]))
    f.write('\n};')
