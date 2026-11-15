"""Headless Linux EGL/OpenGL validation harness. GPL-2.0-or-later."""

import ctypes as C
import os
import sys

SCALE = int(sys.argv[1])
WIDTH = int(sys.argv[2]) if len(sys.argv) > 2 else 8
HEIGHT = int(sys.argv[3]) if len(sys.argv) > 3 else 8
OUT_W = WIDTH * SCALE
OUT_H = HEIGHT * SCALE
from pathlib import Path

os.environ["LIBGL_ALWAYS_SOFTWARE"] = "1"
os.environ["EGL_PLATFORM"] = "surfaceless"
os.environ.setdefault("MESA_SHADER_CACHE_DISABLE", "true")
E = C.CDLL("libEGL.so.1")
G = C.CDLL("libGL.so.1")
I, U, P, F = C.c_int, C.c_uint, C.c_void_p, C.c_float


def fn(lib, name, result, *args):
    f = getattr(lib, name)
    f.restype, f.argtypes = result, args
    return f


display = fn(E, "eglGetPlatformDisplay", P, U, P, C.POINTER(I))(0x31DD, None, None)
major, minor = I(), I()
assert fn(E, "eglInitialize", U, P, C.POINTER(I), C.POINTER(I))(
    display, C.byref(major), C.byref(minor)
), "EGL init failed"
assert fn(E, "eglBindAPI", U, U)(0x30A2)
attrs = (I * 7)(0x3033, 1, 0x3040, 8, 0x3024, 8, 0x3038)
config, count = P(), I()
assert (
    fn(E, "eglChooseConfig", U, P, C.POINTER(I), C.POINTER(P), I, C.POINTER(I))(
        display, attrs, C.byref(config), 1, C.byref(count)
    )
    and count.value
)
ctx = fn(E, "eglCreateContext", P, P, P, P, C.POINTER(I))(
    display, config, None, (I * 1)(0x3038)
)
assert ctx
assert fn(E, "eglMakeCurrent", U, P, P, P, P)(display, None, None, ctx)
print("OpenGL:", fn(G, "glGetString", C.c_char_p, U)(0x1F02).decode())


def compile_shader(source, kind):
    shader = fn(G, "glCreateShader", U, U)(kind)
    data = C.c_char_p(source.encode())
    fn(G, "glShaderSource", None, U, I, C.POINTER(C.c_char_p), P)(
        shader, 1, C.byref(data), None
    )
    fn(G, "glCompileShader", None, U)(shader)
    ok = I()
    fn(G, "glGetShaderiv", None, U, U, C.POINTER(I))(shader, 0x8B81, C.byref(ok))
    log = C.create_string_buffer(65536)
    fn(G, "glGetShaderInfoLog", None, U, I, P, P)(shader, len(log), None, log)
    assert ok.value, log.value.decode()
    return shader


vs = compile_shader(
    "#version 150 core\nin vec4 position;\nout vec2 texCoord;\nvoid main(){gl_Position=position;texCoord=(position.st+vec2(1.0))*0.5;}",
    0x8B31,
)
header = "#version 150 core\n#define varying in\n#define texture2D texture\nout vec4 compat_FragColor;\n#define gl_FragColor compat_FragColor\n"
fs = compile_shader(
    header
    + (
        Path(__file__).resolve().parents[1]
        / "shaders"
        / f"hq{SCALE}x.shader"
        / f"hq{SCALE}x.fs"
    ).read_text(),
    0x8B30,
)
program = fn(G, "glCreateProgram", U)()
attach = fn(G, "glAttachShader", None, U, U)
attach(program, vs)
attach(program, fs)
fn(G, "glLinkProgram", None, U)(program)
ok = I()
fn(G, "glGetProgramiv", None, U, U, C.POINTER(I))(program, 0x8B82, C.byref(ok))
log = C.create_string_buffer(65536)
fn(G, "glGetProgramInfoLog", None, U, I, P, P)(program, len(log), None, log)
assert ok.value, log.value.decode()
print(f"PASS: HQ{SCALE}X compiles and links with mGBA 0.10.5 OpenGL shader header")


def texture(width, height, data=None):
    tex = U()
    fn(G, "glGenTextures", None, I, C.POINTER(U))(1, C.byref(tex))
    fn(G, "glBindTexture", None, U, U)(0x0DE1, tex)
    param = fn(G, "glTexParameteri", None, U, U, I)
    for key, val in [
        (0x2801, 0x2600),
        (0x2800, 0x2600),
        (0x2802, 0x812F),
        (0x2803, 0x812F),
    ]:
        param(0x0DE1, key, val)
    fn(G, "glTexImage2D", None, U, I, I, I, I, I, U, U, P)(
        0x0DE1, 0, 0x8058, width, height, 0, 0x1908, 0x1401, data
    )
    return tex


output = texture(OUT_W, OUT_H)
fbo = U()
fn(G, "glGenFramebuffers", None, I, C.POINTER(U))(1, C.byref(fbo))
fn(G, "glBindFramebuffer", None, U, U)(0x8D40, fbo)
fn(G, "glFramebufferTexture2D", None, U, U, U, U, I)(0x8D40, 0x8CE0, 0x0DE1, output, 0)
assert fn(G, "glCheckFramebufferStatus", U, U)(0x8D40) == 0x8CD5
vao, vbo = U(), U()
fn(G, "glGenVertexArrays", None, I, C.POINTER(U))(1, C.byref(vao))
fn(G, "glBindVertexArray", None, U)(vao)
fn(G, "glGenBuffers", None, I, C.POINTER(U))(1, C.byref(vbo))
fn(G, "glBindBuffer", None, U, U)(0x8892, vbo)
vertices = (F * 8)(-1, -1, -1, 1, 1, 1, 1, -1)
fn(G, "glBufferData", None, U, C.c_ssize_t, P, U)(
    0x8892, C.sizeof(vertices), vertices, 0x88E4
)
pos = fn(G, "glGetAttribLocation", I, U, C.c_char_p)(program, b"position")
fn(G, "glEnableVertexAttribArray", None, U)(pos)
fn(G, "glVertexAttribPointer", None, U, I, U, U, I, P)(pos, 2, 0x1406, 0, 0, None)
fn(G, "glUseProgram", None, U)(program)
loc = fn(G, "glGetUniformLocation", I, U, C.c_char_p)
fn(G, "glUniform1i", None, I, I)(loc(program, b"tex"), 0)
fn(G, "glUniform2f", None, I, F, F)(loc(program, b"texSize"), WIDTH, HEIGHT)
fn(G, "glViewport", None, I, I, I, I)(0, 0, OUT_W, OUT_H)


def render(pixels):
    data = (C.c_ubyte * (WIDTH * HEIGHT * 4))(
        *(channel for color in pixels for channel in (*color, 255))
    )
    tex = texture(WIDTH, HEIGHT, data)
    fn(G, "glDrawArrays", None, U, I, I)(0x0006, 0, 4)
    result = (C.c_ubyte * (OUT_W * OUT_H * 4))()
    fn(G, "glReadPixels", None, I, I, I, I, U, U, P)(
        0, 0, OUT_W, OUT_H, 0x1908, 0x1401, result
    )
    assert fn(G, "glGetError", U)() == 0
    fn(G, "glDeleteTextures", None, I, C.POINTER(U))(1, C.byref(tex))
    return [tuple(result[i : i + 4]) for i in range(0, OUT_W * OUT_H * 4, 4)]
