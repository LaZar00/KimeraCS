#region Copyright and License
// Copyright (c) 2013-2014 The Khronos Group Inc.
// Copyright (c) of C# port 2014 by Shinta <shintadono@googlemail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and/or associated documentation files (the
// "Materials"), to deal in the Materials without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Materials, and to
// permit persons to whom the Materials are furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Materials.
//
// THE MATERIALS ARE PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// MATERIALS OR THE USE OR OTHER DEALINGS IN THE MATERIALS.
#endregion

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Delegates;
	using Defines;

	namespace Defines
    {
		[Flags()]
		public enum glBlendEquationMode : uint
		{
			GL_FUNC_ADD = 0x8006,
			GL_FUNC_SUBTRACT = 0x800A,
			GL_FUNC_REVERSE_SUBTRACT = 0x800B,
			GL_MIN = 0x8007,
			GL_MAX = 0x8008,
		}

		public enum glBGRAEXT : uint
		{
			GL_BGR = 0x80E0,
			GL_BGR_EXT = 0x80E0,
			GL_BGRA = 0x80E1,
			GL_BGRA_EXT = 0x80E1,
		}

		[Flags()]
		public enum glDrawElementsType : uint
		{
			UNSIGNED_BYTE = 0x1401,
			UNSIGNED_SHORT = 0x1403,
			UNSIGNED_INT = 0x1405,
		}

		[Flags()]
		public enum glOrientationOrigin : uint
		{
			LOWER_LEFT = 0x8CA1,
			UPPER_LEFT = 0x8CA2,
		}

		[Flags()]
		public enum glPointParameter : uint
		{
			POINT_FADE_THRESHOLD_SIZE = 0x8128,
			POINT_SPRITE_COORD_ORIGIN = 0x8CA0,
		}
	}

	namespace Delegates
	{
		/// <summary>
		/// Sets the blend function factors separately for color and alpha.
		/// </summary>
		/// <param name="srcRGB">Factor to the source color value.</param>
		/// <param name="dstRGB">Factor to the destination color value.</param>
		/// <param name="srcAlpha">Factor to the source alpha value.</param>
		/// <param name="dstAlpha">Factor to the destination alpha value.</param>
		public delegate void glBlendFuncSeparate(glBlendFuncFactor srcRGB, glBlendFuncFactor dstRGB, glBlendFuncFactor srcAlpha, glBlendFuncFactor dstAlpha);

		/// <summary>
		/// Renders primitives from array.
		/// </summary>
		/// <param name="mode">A <see cref="glDrawMode"/> specifying the type of primitive to be rendered.</param>
		/// <param name="first">Start indices in the array.</param>
		/// <param name="count">Numbers of elements to be rendered.</param>
		/// <param name="drawcount">Number of draws (Length of <paramref name="first"/> and <paramref name="count"/>.</param>
		public delegate void glMultiDrawArrays(glDrawMode mode, int[] first, int[] count, int drawcount);

		internal delegate void glMultiDrawElements(glDrawMode mode, int[] count, glDrawElementsType type, IntPtr[] indices, int primcount);

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		/// <param name="pname">A <see cref="glPointParameter"/> specifying the parameter.</param>
		/// <param name="param">The value the point parameter will be set to.</param>
		public delegate void glPointParameterf(glPointParameter pname, float param);

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		/// <param name="pname">A <see cref="glPointParameter"/> specifying the parameter.</param>
		/// <param name="params">The values the point parameter will be set to.</param>
		public delegate void glPointParameterfv(glPointParameter pname, params float[] @params);

		internal delegate void glPointParameteri(glPointParameter pname, int param);

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		/// <param name="pname">A <see cref="glPointParameter"/> specifying the parameter.</param>
		/// <param name="params">The values the point parameter will be set to.</param>
		public delegate void glPointParameteriv(glPointParameter pname, params int[] @params);

		/// <summary>
		/// Sets the blend color.
		/// </summary>
		/// <param name="red">The red component of the blend color.</param>
		/// <param name="green">The green component of the blend color.</param>
		/// <param name="blue">The blue component of the blend color.</param>
		/// <param name="alpha">The alpha component for alpha blending.</param>
		public delegate void glBlendColor(float red, float green, float blue, float alpha);

		/// <summary>
		/// Sets the equation for blending of color and alpha values.
		/// </summary>
		/// <param name="mode">A <see cref="glBlendEquationMode"/> specifying the blend equation.</param>
		public delegate void glBlendEquation(glBlendEquationMode mode);
	}

	public static partial class GLExt
    {
		/// <summary>
		/// Indicates if OpenGL version 1.4 is available.
		/// </summary>
		public static bool VERSION_1_4;

		internal const string DLLName = "OPENGL32.DLL";

		const string platformErrorString = "Value to big for 32bit platform.";
		static string PlatformErrorString { get { return platformErrorString; } }

		const string platformWrongTypeErrorString = "Plattform is 64bit and value need 64bit to store, but argument is only 32bit.";
		static string PlatformWrongTypeErrorString { get { return platformWrongTypeErrorString; } }

		const string platformArrayErrorString = "A value in the array to big for 32bit platform.";
		static string PlatformArrayErrorString { get { return platformErrorString; } }


		#region WGL binding
		[DllImport(DLLName, CharSet = CharSet.Ansi)]
		private static extern IntPtr wglGetProcAddress(string lpszProc);

		private static TDelegate GetAddress<TDelegate>(string name) where TDelegate : class
		{
			IntPtr addr = wglGetProcAddress(name);
			if (addr == IntPtr.Zero) return null;
			return (TDelegate)(object)Marshal.GetDelegateForFunctionPointer(addr, typeof(TDelegate));
		}
		#endregion

		#region Delegates
		/// <summary>
		/// Sets the blend function factors separately for color and alpha.
		/// </summary>
		public static glBlendFuncSeparate glBlendFuncSeparate;

		/// <summary>
		/// Renders primitives from array.
		/// </summary>
		public static glMultiDrawArrays glMultiDrawArrays;

		private static glMultiDrawElements _MultiDrawElements;

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		public static glPointParameterf glPointParameterf;

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		public static glPointParameterfv glPointParameterfv;

		private static glPointParameteri _PointParameteri;

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		public static glPointParameteriv glPointParameteriv;

		/// <summary>
		/// Sets the blend color.
		/// </summary>
		public static glBlendColor glBlendColor;

		/// <summary>
		/// Sets the equation for blending of color an alpha values.
		/// </summary>
		public static glBlendEquation glBlendEquation;
		#endregion

		#region Overloads
		#region MultiDrawElements
		/// <summary>
		/// Renders primitives from array via indices.
		/// </summary>
		/// <param name="mode">A <see cref="glDrawMode"/> specifying the type of primitive to render.</param>
		/// <param name="count">Numbers of indices.</param>
		/// <param name="type">A <see cref="glDrawElementsType"/> specifying the data type of the indices.</param>
		/// <param name="offsets">The offsets into the array bound to <see cref="glBufferTarget.ELEMENT_ARRAY_BUFFER"/>.</param>
		/// <param name="drawcount">Number of draws (Length of <paramref name="count"/> and <paramref name="offsets"/>.</param>
		public static void MultiDrawElements(glDrawMode mode, int[] count, glDrawElementsType type, int[] offsets, int drawcount)
		{
			IntPtr[] iOffsets = new IntPtr[drawcount];
			for (int i = 0; i < drawcount; i++) iOffsets[i] = (IntPtr)offsets[i];
			_MultiDrawElements(mode, count, type, iOffsets, drawcount);
		}

		/// <summary>
		/// Renders primitives from array via indices.
		/// </summary>
		/// <param name="mode">A <see cref="glDrawMode"/> specifying the type of primitive to render.</param>
		/// <param name="count">Numbers of indices.</param>
		/// <param name="type">A <see cref="glDrawElementsType"/> specifying the data type of the indices.</param>
		/// <param name="offsets">The offsets into the array bound to <see cref="glBufferTarget.ELEMENT_ARRAY_BUFFER"/>.</param>
		/// <param name="drawcount">Number of draws (Length of <paramref name="count"/> and <paramref name="offsets"/>.</param>
		public static void glMultiDrawElements(glDrawMode mode, int[] count, glDrawElementsType type, long[] offsets, int drawcount)
		{
			IntPtr[] iOffsets = new IntPtr[drawcount];
			for (int i = 0; i < drawcount; i++)
			{
				if (IntPtr.Size == 4 && ((long)offsets[i] >> 32) != 0) throw new ArgumentOutOfRangeException("offsets", PlatformArrayErrorString);
				iOffsets[i] = (IntPtr)offsets[i];
			}
			_MultiDrawElements(mode, count, type, iOffsets, drawcount);
		}
		#endregion

		#region PointParameteri
		/// <summary>
		/// Sets point parameters.
		/// </summary>
		/// <param name="pname">A <see cref="glPointParameter"/> specifying the parameter.</param>
		/// <param name="param">The value the point parameter will be set to.</param>
		public static void PointParameteri(glPointParameter pname, int param)
		{
			_PointParameteri(pname, param);
		}

		/// <summary>
		/// Sets point parameters.
		/// </summary>
		/// <param name="pname">Must be <see cref="glPointParameter.POINT_SPRITE_COORD_ORIGIN"/>.</param>
		/// <param name="param">The <see cref="glOrientationOrigin"/> value <see cref="glPointParameter.POINT_SPRITE_COORD_ORIGIN">POINT_SPRITE_COORD_ORIGIN</see> will be set to.</param>
		public static void PointParameteri(glPointParameter pname, glOrientationOrigin param)
		{
			_PointParameteri(pname, (int)param);
		}
		#endregion
		#endregion

		private static void Load_VERSION_1_4()
		{
			glBlendFuncSeparate = GetAddress<glBlendFuncSeparate>("glBlendFuncSeparate");
			glMultiDrawArrays = GetAddress<glMultiDrawArrays>("glMultiDrawArrays");
			_MultiDrawElements = GetAddress<glMultiDrawElements>("glMultiDrawElements");
			glPointParameterf = GetAddress<glPointParameterf>("glPointParameterf");
			glPointParameterfv = GetAddress<glPointParameterfv>("glPointParameterfv");
			_PointParameteri = GetAddress<glPointParameteri>("glPointParameteri");
			glPointParameteriv = GetAddress<glPointParameteriv>("glPointParameteriv");
			glBlendColor = GetAddress<glBlendColor>("glBlendColor");
			glBlendEquation = GetAddress<glBlendEquation>("glBlendEquation");

			VERSION_1_4 = glBlendFuncSeparate != null && glMultiDrawArrays != null && _MultiDrawElements != null &&
				glPointParameterf != null && _PointParameteri != null && glBlendColor != null && glBlendEquation != null;
		}

		public static void Load_Extensions()
        {
			Load_VERSION_1_4();
        }
	}
}
