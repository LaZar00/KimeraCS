using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using HDC = System.IntPtr;
using HGLRC = System.IntPtr;

namespace KimeraCS
{
    using Defines;

    using static FF7PModel;

    using static Utils;
    using static GDI32;
    using static GLExt;

    namespace Defines
    {
        #region The OpenGL constant definitions.
        [Flags()]
        public enum OpenGLVersionIdentifier : uint
        {
            GL_VERSION_1_1 = 1,
        }

        [Flags()]
        public enum AccumOp : uint
        {
            GL_ACCUM = 0x0100,
            GL_LOAD = 0x0101,
            GL_RETURN = 0x0102,
            GL_MULT = 0x0103,
            GL_ADD = 0x0104,
        }

        [Flags()]
        public enum GLBufferMask : uint
        {
            GL_NONE = 0x00000000,
            GL_DEPTH_BUFFER_BIT = 0x00000100,
            GL_STENCIL_BUFFER_BIT = 0x00000400,
            GL_COLOR_BUFFER_BIT = 0x00004000,
        }

        [Flags()]
        public enum AttribMask : uint
        {
            GL_CURRENT_BIT = 0x00000001,
            GL_POINT_BIT = 0x00000002,
            GL_LINE_BIT = 0x00000004,
            GL_POLYGON_BIT = 0x00000008,
            GL_POLYGON_STIPPLE_BIT = 0x00000010,
            GL_PIXEL_MODE_BIT = 0x00000020,
            GL_LIGHTING_BIT = 0x00000040,
            GL_FOG_BIT = 0x00000080,
            GL_DEPTH_BUFFER_BIT = 0x00000100,
            GL_ACCUM_BUFFER_BIT = 0x00000200,
            GL_STENCIL_BUFFER_BIT = 0x00000400,
            GL_VIEWPORT_BIT = 0x00000800,
            GL_TRANSFORM_BIT = 0x00001000,
            GL_ENABLE_BIT = 0x00002000,
            GL_COLOR_BUFFER_BIT = 0x00004000,
            GL_HINT_BIT = 0x00008000,
            GL_EVAL_BIT = 0x00010000,
            GL_LIST_BIT = 0x00020000,
            GL_TEXTURE_BIT = 0x00040000,
            GL_SCISSOR_BIT = 0x00080000,
            GL_ALL_ATTRIB_BITS = 0x000fffff,
        }

        [Flags()]
        public enum GLBlendFuncFactor : uint
        {
            GL_ZERO = 0,
            GL_ONE = 1,
            GL_SRC_COLOR = 0x0300,
            GL_ONE_MINUS_SRC_COLOR = 0x0301,
            GL_SRC_ALPHA = 0x0302,
            GL_ONE_MINUS_SRC_ALPHA = 0x0303,
            GL_DST_ALPHA = 0x0304,
            GL_ONE_MINUS_DST_ALPHA = 0x0305,
            GL_DST_COLOR = 0x0306,
            GL_ONE_MINUS_DST_COLOR = 0x0307,
            GL_SRC_ALPHA_SATURATE = 0x0308,
            GL_CONSTANT_COLOR = 0x8001,
            GL_ONE_MINUS_CONSTANT_COLOR = 0x8002,
            GL_CONSTANT_ALPHA = 0x8003,
            GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004,
            GL_SRC1_COLOR = 0x88F9,
            GL_ONE_MINUS_SRC1_COLOR = 0x88FA,
            GL_SRC1_ALPHA = 0x8589,
            GL_ONE_MINUS_SRC1_ALPHA = 0x88FB,
        }

        [Flags()]
        public enum GL_Boolean : byte
        {
            GL_TRUE = 1,
            GL_FALSE = 0,
        }

        [Flags()]
        public enum ClipPlaneName : uint
        {
            GL_CLIP_PLANE0 = 0x3000,
            GL_CLIP_PLANE1 = 0x3001,
            GL_CLIP_PLANE2 = 0x3002,
            GL_CLIP_PLANE3 = 0x3003,
            GL_CLIP_PLANE4 = 0x3004,
            GL_CLIP_PLANE5 = 0x3005,
        }

        [Flags()]
        public enum GLPixelDataType : uint
        {
            GL_NONE = 0,
            GL_BYTE = 0x1400,
            GL_UNSIGNED_BYTE = 0x1401,
            GL_SHORT = 0x1402,
            GL_UNSIGNED_SHORT = 0x1403,
            GL_INT = 0x1404,
            GL_UNSIGNED_INT = 0x1405,
            GL_FLOAT = 0x1406,
            GL_2_BYTES = 0x1407,
            GL_3_BYTES = 0x1408,
            GL_4_BYTES = 0x1409,
            GL_DOUBLE = 0x140A,
        }

        [Flags()]
        public enum GLDrawMode : uint
        {
            GL_POINTS = 0x0000,
            GL_LINES = 0x0001,
            GL_LINE_LOOP = 0x0002,
            GL_LINE_STRIP = 0x0003,
            GL_TRIANGLES = 0x0004,
            GL_TRIANGLE_STRIP = 0x0005,
            GL_TRIANGLE_FAN = 0x0006,
            GL_QUADS = 0x0007,
            GL_LINES_ADJACENCY = 0x000A,
            GL_LINE_STRIP_ADJACENCY = 0x000B,
            GL_TRIANGLES_ADJACENCY = 0x000C,
            GL_TRIANGLE_STRIP_ADJACENCY = 0x000D,
            GL_PATCHES = 0x000E,
        }

        [Flags()]
        public enum GLFace : uint
        {
            GL_FRONT = 0x0404,
            GL_BACK = 0x0405,
            GL_FRONT_AND_BACK = 0x0408,
        }

        public enum GLFunc : uint
        {
            GL_NEVER = 0x0200,
            GL_LESS = 0x0201,
            GL_EQUAL = 0x0202,
            GL_LEQUAL = 0x0203,
            GL_GREATER = 0x0204,
            GL_NOTEQUAL = 0x0205,
            GL_GEQUAL = 0x0206,
            GL_ALWAYS = 0x0207,
        }

        [Flags()]
        public enum GLPolygon : uint
        {
            GL_POINT = 0x1B00,
            GL_LINE = 0x1B01,
            GL_FILL = 0x1B02,
        }

        public enum GLBuffer : uint
        {
            GL_NONE = 0,
            GL_FRONT_LEFT = 0x0400,
            GL_FRONT_RIGHT = 0x0401,
            GL_BACK_LEFT = 0x0402,
            GL_BACK_RIGHT = 0x0403,
            GL_FRONT = 0x0404,
            GL_BACK = 0x0405,
            GL_LEFT = 0x0406,
            GL_RIGHT = 0x0407,
            GL_FRONT_AND_BACK = 0x0408,
            GL_AUX0 = 0x0409,
            GL_AUX1 = 0x040A,
            GL_AUX2 = 0x040B,
            GL_AUX3 = 0x040C,
            GL_COLOR = 0x1800,
            GL_DEPTH = 0x1801,
            GL_STENCIL = 0x1802,
            GL_DEPTH_STENCIL = 0x84F9,
            GL_COLOR_ATTACHMENT0 = 0x8CE0,
            GL_COLOR_ATTACHMENT1 = 0x8CE1,
            GL_COLOR_ATTACHMENT2 = 0x8CE2,
            GL_COLOR_ATTACHMENT3 = 0x8CE3,
            GL_COLOR_ATTACHMENT4 = 0x8CE4,
            GL_COLOR_ATTACHMENT5 = 0x8CE5,
            GL_COLOR_ATTACHMENT6 = 0x8CE6,
            GL_COLOR_ATTACHMENT7 = 0x8CE7,
            GL_COLOR_ATTACHMENT8 = 0x8CE8,
            GL_COLOR_ATTACHMENT9 = 0x8CE9,
            GL_COLOR_ATTACHMENT10 = 0x8CEA,
            GL_COLOR_ATTACHMENT11 = 0x8CEB,
            GL_COLOR_ATTACHMENT12 = 0x8CEC,
            GL_COLOR_ATTACHMENT13 = 0x8CED,
            GL_COLOR_ATTACHMENT14 = 0x8CEE,
            GL_COLOR_ATTACHMENT15 = 0x8CEF,
            GL_COLOR_ATTACHMENT16 = 0x8CF0,
            GL_COLOR_ATTACHMENT17 = 0x8CF1,
            GL_COLOR_ATTACHMENT18 = 0x8CF2,
            GL_COLOR_ATTACHMENT19 = 0x8CF3,
            GL_COLOR_ATTACHMENT20 = 0x8CF4,
            GL_COLOR_ATTACHMENT21 = 0x8CF5,
            GL_COLOR_ATTACHMENT22 = 0x8CF6,
            GL_COLOR_ATTACHMENT23 = 0x8CF7,
            GL_COLOR_ATTACHMENT24 = 0x8CF8,
            GL_COLOR_ATTACHMENT25 = 0x8CF9,
            GL_COLOR_ATTACHMENT26 = 0x8CFA,
            GL_COLOR_ATTACHMENT27 = 0x8CFB,
            GL_COLOR_ATTACHMENT28 = 0x8CFC,
            GL_COLOR_ATTACHMENT29 = 0x8CFD,
            GL_COLOR_ATTACHMENT30 = 0x8CFE,
            GL_COLOR_ATTACHMENT31 = 0x8CFF,
            GL_DEPTH_ATTACHMENT = 0x8D00,
            GL_STENCIL_ATTACHMENT = 0x8D20,
            GL_DEPTH_STENCIL_ATTACHMENT = 0x821A,
        }

        [Flags()]
        public enum ErrorCode : uint
        {
            GL_NO_ERROR = 0,
            GL_INVALID_ENUM = 0x0500,
            GL_INVALID_VALUE = 0x0501,
            GL_INVALID_OPERATION = 0x0502,
            GL_STACK_OVERFLOW = 0x0503,
            GL_STACK_UNDERFLOW = 0x0504,
            GL_OUT_OF_MEMORY = 0x0505,
        }

        [Flags()]
        public enum FeedBackMode : uint
        {
            GL_2D = 0x0600,
            GL_3D = 0x0601,
            GL_4D_COLOR = 0x0602,
            GL_3D_COLOR_TEXTURE = 0x0603,
            GL_4D_COLOR_TEXTURE = 0x0604,
        }

        [Flags()]
        public enum FeedBackToken : uint
        {
            GL_PASS_THROUGH_TOKEN = 0x0700,
            GL_POINT_TOKEN = 0x0701,
            GL_LINE_TOKEN = 0x0702,
            GL_POLYGON_TOKEN = 0x0703,
            GL_BITMAP_TOKEN = 0x0704,
            GL_DRAW_PIXEL_TOKEN = 0x0705,
            GL_COPY_PIXEL_TOKEN = 0x0706,
            GL_LINE_RESET_TOKEN = 0x0707,
        }

        [Flags()]
        public enum FogMode : uint
        {
            GL_EXP = 0x0800,
            GL_EXP2 = 0x0801,
        }

        [Flags()]
        public enum GLFrontFaceDirection : uint
        {
            GL_CW = 0x0900,
            GL_CCW = 0x0901,
        }

        [Flags()]
        public enum GetMapTarget : uint
        {
            GL_COEFF = 0x0A00,
            GL_ORDER = 0x0A01,
            GL_DOMAIN = 0x0A02,
        }

        [Flags()]
        public enum GLTextureTarget : uint
        {
            GL_NONE = 0,
            GL_TEXTURE_1D = 0x0DE0,
            GL_TEXTURE_1D_ARRAY = 0x8C18,
            GL_TEXTURE_2D = 0x0DE1,
            GL_TEXTURE_2D_ARRAY = 0x8C1A,
            GL_TEXTURE_2D_MULTISAMPLE = 0x9100,
            GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102,
            GL_TEXTURE_3D = 0x806F,
            GL_TEXTURE_BUFFER = 0x8C2A,
            GL_TEXTURE_CUBE_MAP = 0x8513,
            GL_TEXTURE_CUBE_MAP_ARRAY = 0x9009,
            GL_TEXTURE_RECTANGLE = 0x84F5,
            GL_RENDERBUFFER = 0x8D41,
        }

        [Flags()]
        public enum GLTexture2DProxyTarget : uint
        {
            GL_NONE = 0,
            GL_TEXTURE_1D = 0x0DE0,
            GL_TEXTURE_2D = 0x0DE1,
            GL_PROXY_TEXTURE_2D = 0x8064,
            GL_TEXTURE_RECTANGLE = 0x84F5,
            GL_PROXY_TEXTURE_RECTANGLE = 0x84F7,
            GL_TEXTURE_CUBE_MAP = 0x8513,
            GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515,
            GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516,
            GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517,
            GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518,
            GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519,
            GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A,
            GL_PROXY_TEXTURE_CUBE_MAP = 0x851B,
            GL_TEXTURE_1D_ARRAY = 0x8C18,
            GL_PROXY_TEXTURE_1D_ARRAY = 0x8C19,
            GL_TEXTURE_2D_MULTISAMPLE = 0x9100,
            GL_PROXY_TEXTURE_2D_MULTISAMPLE = 0x9101,
        }

        [Flags()]
        public enum GLPixelStoreParameter : uint
        {
            GL_UNPACK_SWAP_BYTES = 0x0CF0,
            [Obsolete("Deprecated since 4.3")]
            GL_UNPACK_LSB_FIRST = 0x0CF1,
            GL_UNPACK_ROW_LENGTH = 0x0CF2,
            GL_UNPACK_SKIP_ROWS = 0x0CF3,
            GL_UNPACK_SKIP_PIXELS = 0x0CF4,
            GL_UNPACK_ALIGNMENT = 0x0CF5,
            GL_UNPACK_SKIP_IMAGES = 0x806D,
            GL_UNPACK_IMAGE_HEIGHT = 0x806E,
            GL_PACK_SWAP_BYTES = 0x0D00,
            [Obsolete("Deprecated since 4.3")]
            GL_PACK_LSB_FIRST = 0x0D01,
            GL_PACK_ROW_LENGTH = 0x0D02,
            GL_PACK_SKIP_ROWS = 0x0D03,
            GL_PACK_SKIP_PIXELS = 0x0D04,
            GL_PACK_ALIGNMENT = 0x0D05,
            GL_PACK_SKIP_IMAGES = 0x806B,
            GL_PACK_IMAGE_HEIGHT = 0x806C,
            GL_UNPACK_COMPRESSED_BLOCK_WIDTH = 0x9127,
            GL_UNPACK_COMPRESSED_BLOCK_HEIGHT = 0x9128,
            GL_UNPACK_COMPRESSED_BLOCK_DEPTH = 0x9129,
            GL_UNPACK_COMPRESSED_BLOCK_SIZE = 0x912A,
            GL_PACK_COMPRESSED_BLOCK_WIDTH = 0x912B,
            GL_PACK_COMPRESSED_BLOCK_HEIGHT = 0x912C,
            GL_PACK_COMPRESSED_BLOCK_DEPTH = 0x912D,
            GL_PACK_COMPRESSED_BLOCK_SIZE = 0x912E,
        }

        [Flags()]
        public enum GLCapability : uint
        {
            GL_CURRENT_COLOR = 0x0B00,
            GL_CURRENT_INDEX = 0x0B01,
            GL_CURRENT_NORMAL = 0x0B02,
            GL_CURRENT_TEXTURE_COORDS = 0x0B03,
            GL_CURRENT_RASTER_COLOR = 0x0B04,
            GL_CURRENT_RASTER_INDEX = 0x0B05,
            GL_CURRENT_RASTER_TEXTURE_COORDS = 0x0B06,
            GL_CURRENT_RASTER_POSITION = 0x0B07,
            GL_CURRENT_RASTER_POSITION_VALID = 0x0B08,
            GL_CURRENT_RASTER_DISTANCE = 0x0B09,
            GL_POINT_SMOOTH = 0x0B10,
            GL_POINT_SIZE = 0x0B11,
            GL_POINT_SIZE_RANGE = 0x0B12,
            GL_POINT_SIZE_GRANULARITY = 0x0B13,
            GL_LINE_SMOOTH = 0x0B20,
            GL_LINE_WIDTH = 0x0B21,
            GL_LINE_WIDTH_RANGE = 0x0B22,
            GL_LINE_WIDTH_GRANULARITY = 0x0B23,
            GL_LINE_STIPPLE = 0x0B24,
            GL_LINE_STIPPLE_PATTERN = 0x0B25,
            GL_LINE_STIPPLE_REPEAT = 0x0B26,
            GL_LIST_MODE = 0x0B30,
            GL_MAX_LIST_NESTING = 0x0B31,
            GL_LIST_BASE = 0x0B32,
            GL_LIST_INDEX = 0x0B33,
            GL_POLYGON_MODE = 0x0B40,
            GL_POLYGON_SMOOTH = 0x0B41,
            GL_POLYGON_STIPPLE = 0x0B42,
            GL_EDGE_FLAG = 0x0B43,
            GL_CULL_FACE = 0x0B44,
            GL_CULL_FACE_MODE = 0x0B45,
            GL_FRONT_FACE = 0x0B46,
            GL_LIGHTING = 0x0B50,
            GL_LIGHT_MODEL_LOCAL_VIEWER = 0x0B51,
            GL_LIGHT_MODEL_TWO_SIDE = 0x0B52,
            GL_LIGHT_MODEL_AMBIENT = 0x0B53,
            GL_SHADE_MODEL = 0x0B54,
            GL_COLOR_MATERIAL_FACE = 0x0B55,
            GL_COLOR_MATERIAL_PARAMETER = 0x0B56,
            GL_COLOR_MATERIAL = 0x0B57,
            GL_FOG = 0x0B60,
            GL_FOG_INDEX = 0x0B61,
            GL_FOG_DENSITY = 0x0B62,
            GL_FOG_START = 0x0B63,
            GL_FOG_END = 0x0B64,
            GL_FOG_MODE = 0x0B65,
            GL_FOG_COLOR = 0x0B66,
            GL_DEPTH_RANGE = 0x0B70,
            GL_DEPTH_TEST = 0x0B71,
            GL_DEPTH_WRITEMASK = 0x0B72,
            GL_DEPTH_CLEAR_VALUE = 0x0B73,
            GL_DEPTH_FUNC = 0x0B74,
            GL_ACCUM_CLEAR_VALUE = 0x0B80,
            GL_STENCIL_TEST = 0x0B90,
            GL_STENCIL_CLEAR_VALUE = 0x0B91,
            GL_STENCIL_FUNC = 0x0B92,
            GL_STENCIL_VALUE_MASK = 0x0B93,
            GL_STENCIL_FAIL = 0x0B94,
            GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95,
            GL_STENCIL_PASS_DEPTH_PASS = 0x0B96,
            GL_STENCIL_REF = 0x0B97,
            GL_STENCIL_WRITEMASK = 0x0B98,
            GL_MATRIX_MODE = 0x0BA0,
            GL_NORMALIZE = 0x0BA1,
            GL_VIEWPORT = 0x0BA2,
            GL_MODELVIEW_STACK_DEPTH = 0x0BA3,
            GL_PROJECTION_STACK_DEPTH = 0x0BA4,
            GL_TEXTURE_STACK_DEPTH = 0x0BA5,
            GL_MODELVIEW_MATRIX = 0x0BA6,
            GL_PROJECTION_MATRIX = 0x0BA7,
            GL_TEXTURE_MATRIX = 0x0BA8,
            GL_ATTRIB_STACK_DEPTH = 0x0BB0,
            GL_CLIENT_ATTRIB_STACK_DEPTH = 0x0BB1,
            GL_ALPHA_TEST = 0x0BC0,
            GL_ALPHA_TEST_FUNC = 0x0BC1,
            GL_ALPHA_TEST_REF = 0x0BC2,
            GL_DITHER = 0x0BD0,
            GL_BLEND_DST = 0x0BE0,
            GL_BLEND_SRC = 0x0BE1,
            GL_BLEND = 0x0BE2,
            GL_LOGIC_OP_MODE = 0x0BF0,
            GL_INDEX_LOGIC_OP = 0x0BF1,
            GL_COLOR_LOGIC_OP = 0x0BF2,
            GL_AUX_BUFFERS = 0x0C00,
            GL_DRAW_BUFFER = 0x0C01,
            GL_READ_BUFFER = 0x0C02,
            GL_SCISSOR_BOX = 0x0C10,
            GL_SCISSOR_TEST = 0x0C11,
            GL_INDEX_CLEAR_VALUE = 0x0C20,
            GL_INDEX_WRITEMASK = 0x0C21,
            GL_COLOR_CLEAR_VALUE = 0x0C22,
            GL_COLOR_WRITEMASK = 0x0C23,
            GL_INDEX_MODE = 0x0C30,
            GL_RGBA_MODE = 0x0C31,
            GL_DOUBLEBUFFER = 0x0C32,
            GL_STEREO = 0x0C33,
            GL_RENDER_MODE = 0x0C40,
            GL_PERSPECTIVE_CORRECTION_HINT = 0x0C50,
            GL_POINT_SMOOTH_HINT = 0x0C51,
            GL_LINE_SMOOTH_HINT = 0x0C52,
            GL_POLYGON_SMOOTH_HINT = 0x0C53,
            GL_FOG_HINT = 0x0C54,
            GL_TEXTURE_GEN_S = 0x0C60,
            GL_TEXTURE_GEN_T = 0x0C61,
            GL_TEXTURE_GEN_R = 0x0C62,
            GL_TEXTURE_GEN_Q = 0x0C63,
            GL_PIXEL_MAP_I_TO_I = 0x0C70,
            GL_PIXEL_MAP_S_TO_S = 0x0C71,
            GL_PIXEL_MAP_I_TO_R = 0x0C72,
            GL_PIXEL_MAP_I_TO_G = 0x0C73,
            GL_PIXEL_MAP_I_TO_B = 0x0C74,
            GL_PIXEL_MAP_I_TO_A = 0x0C75,
            GL_PIXEL_MAP_R_TO_R = 0x0C76,
            GL_PIXEL_MAP_G_TO_G = 0x0C77,
            GL_PIXEL_MAP_B_TO_B = 0x0C78,
            GL_PIXEL_MAP_A_TO_A = 0x0C79,
            GL_PIXEL_MAP_I_TO_I_SIZE = 0x0CB0,
            GL_PIXEL_MAP_S_TO_S_SIZE = 0x0CB1,
            GL_PIXEL_MAP_I_TO_R_SIZE = 0x0CB2,
            GL_PIXEL_MAP_I_TO_G_SIZE = 0x0CB3,
            GL_PIXEL_MAP_I_TO_B_SIZE = 0x0CB4,
            GL_PIXEL_MAP_I_TO_A_SIZE = 0x0CB5,
            GL_PIXEL_MAP_R_TO_R_SIZE = 0x0CB6,
            GL_PIXEL_MAP_G_TO_G_SIZE = 0x0CB7,
            GL_PIXEL_MAP_B_TO_B_SIZE = 0x0CB8,
            GL_PIXEL_MAP_A_TO_A_SIZE = 0x0CB9,
            GL_MAP_COLOR = 0x0D10,
            GL_MAP_STENCIL = 0x0D11,
            GL_INDEX_SHIFT = 0x0D12,
            GL_INDEX_OFFSET = 0x0D13,
            GL_RED_SCALE = 0x0D14,
            GL_RED_BIAS = 0x0D15,
            GL_ZOOM_X = 0x0D16,
            GL_ZOOM_Y = 0x0D17,
            GL_GREEN_SCALE = 0x0D18,
            GL_GREEN_BIAS = 0x0D19,
            GL_BLUE_SCALE = 0x0D1A,
            GL_BLUE_BIAS = 0x0D1B,
            GL_ALPHA_SCALE = 0x0D1C,
            GL_ALPHA_BIAS = 0x0D1D,
            GL_DEPTH_SCALE = 0x0D1E,
            GL_DEPTH_BIAS = 0x0D1F,
            GL_MAX_EVAL_ORDER = 0x0D30,
            GL_MAX_LIGHTS = 0x0D31,
            GL_MAX_CLIP_PLANES = 0x0D32,
            GL_MAX_TEXTURE_SIZE = 0x0D33,
            GL_MAX_PIXEL_MAP_TABLE = 0x0D34,
            GL_MAX_ATTRIB_STACK_DEPTH = 0x0D35,
            GL_MAX_MODELVIEW_STACK_DEPTH = 0x0D36,
            GL_MAX_NAME_STACK_DEPTH = 0x0D37,
            GL_MAX_PROJECTION_STACK_DEPTH = 0x0D38,
            GL_MAX_TEXTURE_STACK_DEPTH = 0x0D39,
            GL_MAX_VIEWPORT_DIMS = 0x0D3A,
            GL_MAX_CLIENT_ATTRIB_STACK_DEPTH = 0x0D3B,
            GL_SUBPIXEL_BITS = 0x0D50,
            GL_INDEX_BITS = 0x0D51,
            GL_RED_BITS = 0x0D52,
            GL_GREEN_BITS = 0x0D53,
            GL_BLUE_BITS = 0x0D54,
            GL_ALPHA_BITS = 0x0D55,
            GL_DEPTH_BITS = 0x0D56,
            GL_STENCIL_BITS = 0x0D57,
            GL_ACCUM_RED_BITS = 0x0D58,
            GL_ACCUM_GREEN_BITS = 0x0D59,
            GL_ACCUM_BLUE_BITS = 0x0D5A,
            GL_ACCUM_ALPHA_BITS = 0x0D5B,
            GL_NAME_STACK_DEPTH = 0x0D70,
            GL_AUTO_NORMAL = 0x0D80,
            GL_MAP1_COLOR_4 = 0x0D90,
            GL_MAP1_INDEX = 0x0D91,
            GL_MAP1_NORMAL = 0x0D92,
            GL_MAP1_TEXTURE_COORD_1 = 0x0D93,
            GL_MAP1_TEXTURE_COORD_2 = 0x0D94,
            GL_MAP1_TEXTURE_COORD_3 = 0x0D95,
            GL_MAP1_TEXTURE_COORD_4 = 0x0D96,
            GL_MAP1_VERTEX_3 = 0x0D97,
            GL_MAP1_VERTEX_4 = 0x0D98,
            GL_MAP2_COLOR_4 = 0x0DB0,
            GL_MAP2_INDEX = 0x0DB1,
            GL_MAP2_NORMAL = 0x0DB2,
            GL_MAP2_TEXTURE_COORD_1 = 0x0DB3,
            GL_MAP2_TEXTURE_COORD_2 = 0x0DB4,
            GL_MAP2_TEXTURE_COORD_3 = 0x0DB5,
            GL_MAP2_TEXTURE_COORD_4 = 0x0DB6,
            GL_MAP2_VERTEX_3 = 0x0DB7,
            GL_MAP2_VERTEX_4 = 0x0DB8,
            GL_MAP1_GRID_DOMAIN = 0x0DD0,
            GL_MAP1_GRID_SEGMENTS = 0x0DD1,
            GL_MAP2_GRID_DOMAIN = 0x0DD2,
            GL_MAP2_GRID_SEGMENTS = 0x0DD3,
            GL_TEXTURE_1D = 0x0DE0,
            GL_TEXTURE_2D = 0x0DE1,
            GL_FEEDBACK_BUFFER_POINTER = 0x0DF0,
            GL_FEEDBACK_BUFFER_SIZE = 0x0DF1,
            GL_FEEDBACK_BUFFER_TYPE = 0x0DF2,
            GL_SELECTION_BUFFER_POINTER = 0x0DF3,
            GL_SELECTION_BUFFER_SIZE = 0x0DF4,
            CLIP_DISTANCE0 = 0x3000,
            CLIP_DISTANCE1 = 0x3001,
            CLIP_DISTANCE2 = 0x3002,
            CLIP_DISTANCE3 = 0x3003,
            CLIP_DISTANCE4 = 0x3004,
            CLIP_DISTANCE5 = 0x3005,
            CLIP_DISTANCE6 = 0x3006,
            CLIP_DISTANCE7 = 0x3007,
            GL_LIGHT0 = 0x4000,
            GL_LIGHT1 = 0x4001,
            GL_LIGHT2 = 0x4002,
            GL_LIGHT3 = 0x4003,
            GL_LIGHT4 = 0x4004,
            GL_LIGHT5 = 0x4005,
            GL_LIGHT6 = 0x4006,
            GL_LIGHT7 = 0x4007,
            GL_POLYGON_OFFSET_FACTOR = 0x8038,
            GL_POLYGON_OFFSET_UNITS = 0x2A00,
            GL_POLYGON_OFFSET_POINT = 0x2A01,
            GL_POLYGON_OFFSET_LINE = 0x2A02,
            GL_POLYGON_OFFSET_FILL = 0x8037,
        }

        [Flags()]
        public enum GetTextureParameter : uint
        {
            GL_TEXTURE_WIDTH = 0x1000,
            GL_TEXTURE_HEIGHT = 0x1001,
            GL_TEXTURE_INTERNAL_FORMAT = 0x1003,
            GL_TEXTURE_BORDER_COLOR = 0x1004,
            GL_TEXTURE_BORDER = 0x1005,
        }

        [Flags()]
        public enum HintMode : uint
        {
            GL_DONT_CARE = 0x1100,
            GL_FASTEST = 0x1101,
            GL_NICEST = 0x1102,
        }

        [Flags()]
        public enum GLLightParameter : uint
        {
            GL_AMBIENT = 0x1200,
            GL_DIFFUSE = 0x1201,
            GL_SPECULAR = 0x1202,
            GL_POSITION = 0x1203,
            GL_SPOT_DIRECTION = 0x1204,
            GL_SPOT_EXPONENT = 0x1205,
            GL_SPOT_CUTOFF = 0x1206,
            GL_CONSTANT_ATTENUATION = 0x1207,
            GL_LINEAR_ATTENUATION = 0x1208,
            GL_QUADRATIC_ATTENUATION = 0x1209,
        }

        [Flags()]
        public enum GLListMode : uint
        {
            GL_COMPILE = 0x1300,
            GL_COMPILE_AND_EXECUTE = 0x1301,
        }

        [Flags()]
        public enum GLLogicOpMode : uint
        {
            GL_CLEAR = 0x1500,
            GL_AND = 0x1501,
            GL_AND_REVERSE = 0x1502,
            GL_COPY = 0x1503,
            GL_AND_INVERTED = 0x1504,
            GL_NOOP = 0x1505,
            GL_XOR = 0x1506,
            GL_OR = 0x1507,
            GL_NOR = 0x1508,
            GL_EQUIV = 0x1509,
            GL_INVERT = 0x150A,
            GL_OR_REVERSE = 0x150B,
            GL_COPY_INVERTED = 0x150C,
            GL_OR_INVERTED = 0x150D,
            GL_NAND = 0x150E,
            GL_SET = 0x150F,
        }

        [Flags()]
        public enum GLMaterialParameter : uint
        {
            GL_EMISSION = 0x1600,
            GL_SHININESS = 0x1601,
            GL_AMBIENT_AND_DIFFUSE = 0x1602,
            GL_COLOR_INDEXES = 0x1603,
        }

        [Flags()]
        public enum GLMatrixModeList : uint
        {
            GL_MODELVIEW = 0x1700,
            GL_PROJECTION = 0x1701,
            GL_TEXTURE = 0x1702,
        }

        [Flags()]
        public enum PixelCopyType : uint
        {
            GL_COLOR = 0x1800,
            GL_DEPTH = 0x1801,
            GL_STENCIL = 0x1802,
        }

        public enum GLInternalFormat : uint
        {
            GL_NONE = 0,

            #region Base internal formats
            GL_DEPTH_COMPONENT = 0x1902,
            GL_DEPTH_STENCIL = 0x84F9,
            GL_STENCIL_INDEX = 0x1901,
            GL_RED = 0x1903,
            GL_RG = 0x8227,
            GL_RGB = 0x1907,
            GL_RGBA = 0x1908,
            GL_SRGB = 0x8C40,
            GL_SRGB_ALPHA = 0x8C42,
            #endregion

            #region Sized internal formats
            GL_DEPTH_COMPONENT16 = 0x81A5,
            GL_DEPTH_COMPONENT24 = 0x81A6,
            GL_DEPTH_COMPONENT32 = 0x81A7,
            GL_DEPTH_COMPONENT32F = 0x8CAC,
            GL_DEPTH24_STENCIL8 = 0x88F0,
            GL_DEPTH32F_STENCIL8 = 0x8CAD,
            GL_STENCIL_INDEX1 = 0x8D46,
            GL_STENCIL_INDEX4 = 0x8D47,
            GL_STENCIL_INDEX8 = 0x8D48,
            GL_STENCIL_INDEX16 = 0x8D49,
            GL_R8 = 0x8229,
            GL_R8_SNORM = 0x8F94,
            GL_R16 = 0x822A,
            GL_R16_SNORM = 0x8F98,
            GL_RG8 = 0x822B,
            GL_RG8_SNORM = 0x8F95,
            GL_RG16 = 0x822C,
            GL_RG16_SNORM = 0x8F99,
            GL_R3_G3_B2 = 0x2A10,
            GL_RGB4 = 0x804F,
            GL_RGB5 = 0x8050,
            GL_RGB565 = 0x8D62,
            GL_RGB8 = 0x8051,
            GL_RGB8_SNORM = 0x8F96,
            GL_RGB10 = 0x8052,
            GL_RGB12 = 0x8053,
            GL_RGB16 = 0x8054,
            GL_RGB16_SNORM = 0x8F9A,
            GL_RGBA2 = 0x8055,
            GL_RGBA4 = 0x8056,
            GL_RGB5_A1 = 0x8057,
            GL_RGBA8 = 0x8058,
            GL_RGBA8_SNORM = 0x8F97,
            GL_RGB10_A2 = 0x8059,
            GL_RGB10_A2UI = 0x906F,
            GL_RGBA12 = 0x805A,
            GL_RGBA16 = 0x805B,
            GL_RGBA16_SNORM = 0x8F9B,
            GL_SRGB8 = 0x8C41,
            GL_SRGB8_ALPHA8 = 0x8C43,
            GL_R16F = 0x822D,
            GL_RG16F = 0x822F,
            GL_RGB16F = 0x881B,
            GL_RGBA16F = 0x881A,
            GL_R32F = 0x822E,
            GL_RG32F = 0x8230,
            GL_RGB32F = 0x8815,
            GL_RGBA32F = 0x8814,
            GL_R11F_G11F_B10F = 0x8C3A,
            GL_RGB9_E5 = 0x8C3D,
            GL_R8I = 0x8231,
            GL_R8UI = 0x8232,
            GL_R16I = 0x8233,
            GL_R16UI = 0x8234,
            GL_R32I = 0x8235,
            GL_R32UI = 0x8236,
            GL_RG8I = 0x8237,
            GL_RG8UI = 0x8238,
            GL_RG16I = 0x8239,
            GL_RG16UI = 0x823A,
            GL_RG32I = 0x823B,
            GL_RG32UI = 0x823C,
            GL_RGB8I = 0x8D8F,
            GL_RGB8UI = 0x8D7D,
            GL_RGB16I = 0x8D89,
            GL_RGB16UI = 0x8D77,
            GL_RGB32I = 0x8D83,
            GL_RGB32UI = 0x8D71,
            GL_RGBA8I = 0x8D8E,
            GL_RGBA8UI = 0x8D7C,
            GL_RGBA16I = 0x8D88,
            GL_RGBA16UI = 0x8D76,
            GL_RGBA32I = 0x8D82,
            GL_RGBA32UI = 0x8D70,
            #endregion

            #region Generic compressed internal formats
            GL_COMPRESSED_RED = 0x8225,
            GL_COMPRESSED_RG = 0x8226,
            GL_COMPRESSED_RGB = 0x84ED,
            GL_COMPRESSED_RGBA = 0x84EE,
            GL_COMPRESSED_SRGB = 0x8C48,
            GL_COMPRESSED_SRGB_ALPHA = 0x8C49,
            #endregion

            #region Specific compressed internal formats
            GL_COMPRESSED_RED_RGTC1 = 0x8DBB,
            GL_COMPRESSED_SIGNED_RED_RGTC1 = 0x8DBC,
            GL_COMPRESSED_RG_RGTC2 = 0x8DBD,
            GL_COMPRESSED_SIGNED_RG_RGTC2 = 0x8DBE,
            GL_COMPRESSED_RGBA_BPTC_UNORM = 0x8E8C,
            GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM = 0x8E8D,
            GL_COMPRESSED_RGB_BPTC_SIGNED_FLOAT = 0x8E8E,
            GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = 0x8E8F,
            GL_COMPRESSED_RGB8_ETC2 = 0x9274,
            GL_COMPRESSED_SRGB8_ETC2 = 0x9275,
            GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9276,
            GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277,
            GL_COMPRESSED_RGBA8_ETC2_EAC = 0x9278,
            GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = 0x9279,
            GL_COMPRESSED_R11_EAC = 0x9270,
            GL_COMPRESSED_SIGNED_R11_EAC = 0x9271,
            GL_COMPRESSED_RG11_EAC = 0x9272,
            GL_COMPRESSED_SIGNED_RG11_EAC = 0x9273,
            #endregion
        }

        [Flags()]
        public enum GLPixelFormat : uint
        {
            GL_NONE = 0,
            GL_COLOR_INDEX = 0x1900,
            GL_STENCIL_INDEX = 0x1901,
            GL_DEPTH_COMPONENT = 0x1902,
            GL_RED = 0x1903,
            GL_GREEN = 0x1904,
            GL_BLUE = 0x1905,
            GL_ALPHA = 0x1906,
            GL_RGB = 0x1907,
            GL_RGBA = 0x1908,
            GL_LUMINANCE = 0x1909,
            GL_LUMINANCE_ALPHA = 0x190A,
            GL_DEPTH_STENCIL = 0x84F9,
            GL_RG = 0x8227,
            GL_BGR = 0x80E0,
            GL_BGRA = 0x80E1,
            GL_RED_INTEGER = 0x8D94,
            GL_GREEN_INTEGER = 0x8D95,
            GL_BLUE_INTEGER = 0x8D96,
            GL_RG_INTEGER = 0x8228,
            GL_RGB_INTEGER = 0x8D98,
            GL_RGBA_INTEGER = 0x8D99,
            GL_BGR_INTEGER = 0x8D9A,
            GL_BGRA_INTEGER = 0x8D9B,
        }

        [Flags()]
        public enum PixelType : uint
        {
            GL_BITMAP = 0x1A00,
        }

        [Flags()]
        public enum PolygonMode : uint
        {
            GL_POINT = 0x1B00,
            GL_LINE = 0x1B01,
            GL_FILL = 0x1B02,
        }

        [Flags()]
        public enum GLRenderingMode : uint
        {
            GL_RENDER = 0x1C00,
            GL_FEEDBACK = 0x1C01,
            GL_SELECT = 0x1C02,
        }

        [Flags()]
        public enum GLShadingModel : uint
        {
            GL_FLAT = 0x1D00,
            GL_SMOOTH = 0x1D01,
        }

        [Flags()]
        public enum StencilOp : uint
        {
            GL_KEEP = 0x1E00,
            GL_REPLACE = 0x1E01,
            GL_INCR = 0x1E02,
            GL_DECR = 0x1E03,
        }

        [Flags()]
        public enum GL_StringName : uint
        {
            GL_VENDOR = 0x1F00,
            GL_RENDERER = 0x1F01,
            GL_VERSION = 0x1F02,
            GL_EXTENSIONS = 0x1F03,
        }

        [Flags()]
        public enum TextureCoordName : uint
        {
            GL_S = 0x2000,
            GL_T = 0x2001,
            GL_R = 0x2002,
            GL_Q = 0x2003,
        }

        [Flags()]
        public enum TextureEnvMode : uint
        {
            GL_MODULATE = 0x2100,
            GL_DECAL = 0x2101,
        }

        [Flags()]
        public enum TextureEnvParameter : uint
        {
            GL_TEXTURE_ENV_MODE = 0x2200,
            GL_TEXTURE_ENV_COLOR = 0x2201,
        }

        [Flags()]
        public enum TextureEnvTarget : uint
        {
            GL_TEXTURE_ENV = 0x2300,
        }

        [Flags()]
        public enum TextureGenMode : uint
        {
            GL_EYE_LINEAR = 0x2400,
            GL_OBJECT_LINEAR = 0x2401,
            GL_SPHERE_MAP = 0x2402,
        }

        [Flags()]
        public enum TextureGenParameter : uint
        {
            GL_TEXTURE_GEN_MODE = 0x2500,
            GL_OBJECT_PLANE = 0x2501,
            GL_EYE_PLANE = 0x2502,
        }

        [Flags()]
        public enum GLTextureMagFilter : uint
        {
            GL_NEAREST = 0x2600,
            GL_LINEAR = 0x2601,
        }

        [Flags()]
        public enum TextureMinFilter : uint
        {
            GL_NEAREST_MIPMAP_NEAREST = 0x2700,
            GL_LINEAR_MIPMAP_NEAREST = 0x2701,
            GL_NEAREST_MIPMAP_LINEAR = 0x2702,
            GL_LINEAR_MIPMAP_LINEAR = 0x2703,
        }

        [Flags()]
        public enum TextureParameterName : uint
        {
            GL_TEXTURE_MAG_FILTER = 0x2800,
            GL_TEXTURE_MIN_FILTER = 0x2801,
            GL_TEXTURE_WRAP_S = 0x2802,
            GL_TEXTURE_WRAP_T = 0x2803,
        }

        [Flags()]
        public enum TextureWrapMode : uint
        {
            GL_CLAMP = 0x2900,
            GL_REPEAT = 0x2901,
        }

        [Flags()]
        public enum GLTextureParameter : uint
        {
            GL_DEPTH_STENCIL_TEXTURE_MODE = 0x90EA,
            GL_TEXTURE_BASE_LEVEL = 0x813C,
            GL_TEXTURE_BORDER_COLOR = 0x1004,
            GL_TEXTURE_COMPARE_FUNC = 0x884D,
            GL_TEXTURE_COMPARE_MODE = 0x884C,
            GL_TEXTURE_LOD_BIAS = 0x8501,
            GL_TEXTURE_MAG_FILTER = 0x2800,
            GL_TEXTURE_MAX_LEVEL = 0x813D,
            GL_TEXTURE_MAX_LOD = 0x813B,
            GL_TEXTURE_MIN_FILTER = 0x2801,
            GL_TEXTURE_MIN_LOD = 0x813A,
            GL_TEXTURE_SWIZZLE_A = 0x8E45,
            GL_TEXTURE_SWIZZLE_B = 0x8E44,
            GL_TEXTURE_SWIZZLE_G = 0x8E43,
            GL_TEXTURE_SWIZZLE_R = 0x8E42,
            GL_TEXTURE_SWIZZLE_RGBA = 0x8E46,
            GL_TEXTURE_WRAP_R = 0x8072,
            GL_TEXTURE_WRAP_S = 0x2802,
            GL_TEXTURE_WRAP_T = 0x2803,
            GL_IMAGE_FORMAT_COMPATIBILITY_TYPE = 0x90C7,
            GL_TEXTURE_IMMUTABLE_FORMAT = 0x912F,
            GL_TEXTURE_IMMUTABLE_LEVELS = 0x82DF,
            GL_TEXTURE_VIEW_MIN_LEVEL = 0x82DB,
            GL_TEXTURE_VIEW_NUM_LEVELS = 0x82DC,
            GL_TEXTURE_VIEW_MIN_LAYER = 0x82DD,
            GL_TEXTURE_VIEW_NUM_LAYERS = 0x82DE,
            GL_TEXTURE_TARGET = 0x1006,
        }

        [Flags()]
        public enum ClientAttribMask : uint
        {
            GL_CLIENT_PIXEL_STORE_BIT = 0x00000001,
            GL_CLIENT_VERTEX_ARRAY_BIT = 0x00000002,
            GL_CLIENT_ALL_ATTRIB_BITS = 0xffffffff,
        }

        [Flags()]
        public enum PolygonOffset : uint
        {
            GL_POLYGON_OFFSET_FACTOR = 0x8038,
            GL_POLYGON_OFFSET_UNITS = 0x2A00,
            GL_POLYGON_OFFSET_POINT = 0x2A01,
            GL_POLYGON_OFFSET_LINE = 0x2A02,
            GL_POLYGON_OFFSET_FILL = 0x8037,
        }

        [Flags()]
        public enum GLTextureFormats : uint
        {
            GL_RED = 0x1903,
            GL_RGB = 0x1907,
            GL_RGBA = 0x1908,
            GL_LUMINANCE = 0x1909,
            GL_LUMINANCE8 = 0x8040,
            GL_RGB4 = 0x804F,
            GL_RGB5 = 0x8050,
            GL_RGB8 = 0x8051,
            GL_RGBA8 = 0x8058,
            GL_RGBA4 = 0x8056,
            GL_RGB5_A1 = 0x8057,
            GL_RGB10_A2_EXT = 0x8059,
            GL_RGBA16 = 0x805B,
            GL_BGR = 0x80E0,            // GLExt
            GL_BGR_EXT = 0x80E0,        // GLExt
            GL_BGRA = 0x80E1,
            GL_DEPTH_COMPONENT16 = 0x81A5,
            GL_DEPTH_COMPONENT24 = 0x81A6,
            GL_RG = 0x8227,
            GL_RG8 = 0x822B,
            GL_RG16 = 0x822C,
            GL_R16F = 0x822D,
            GL_R32F = 0x822E,
            GL_RG16F = 0x822F,
            GL_RG32F = 0x8230,
            GL_RGBA32F = 0x8814,
            GL_RGBA16F = 0x881A,
            GL_DEPTH24_STENCIL8 = 0x88F0,
            GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3,
            GL_COMPRESSED_RGBA_S3TC_DXT1_EXT = 0x83F1,
            GL_COMPRESSED_RGBA_S3TC_DXT3_EXT = 0x83F2,
            GL_COMPRESSED_RGBA_S3TC_DXT5_EXT = 0x83F3,
        }

        [Flags()]
        public enum GLTextures : uint
        {
            GL_TEXTURE_WRAP_S = 0x2802,
            GL_TEXTURE_WRAP_T = 0x2803,
            GL_TEXTURE_WRAP_R = 0x8072,
            GL_TEXTURE_MAG_FILTER = 0x2800,
            GL_TEXTURE_MIN_FILTER = 0x2801,
            GL_TEXTURE_MAX_ANISOTROPY_EXT = 0x84FE,
            GL_TEXTURE_BASE_LEVEL = 0x813C,
            GL_TEXTURE_MAX_LEVEL = 0x813D,
            GL_TEXTURE_LOD_BIAS = 0x8501,
        }

        [Flags()]
        public enum Texture : uint
        {
            GL_ALPHA4 = 0x803B,
            GL_ALPHA8 = 0x803C,
            GL_ALPHA12 = 0x803D,
            GL_ALPHA16 = 0x803E,
            GL_LUMINANCE4 = 0x803F,
            GL_LUMINANCE8 = 0x8040,
            GL_LUMINANCE12 = 0x8041,
            GL_LUMINANCE16 = 0x8042,
            GL_LUMINANCE4_ALPHA4 = 0x8043,
            GL_LUMINANCE6_ALPHA2 = 0x8044,
            GL_LUMINANCE8_ALPHA8 = 0x8045,
            GL_LUMINANCE12_ALPHA4 = 0x8046,
            GL_LUMINANCE12_ALPHA12 = 0x8047,
            GL_LUMINANCE16_ALPHA16 = 0x8048,
            GL_INTENSITY = 0x8049,
            GL_INTENSITY4 = 0x804A,
            GL_INTENSITY8 = 0x804B,
            GL_INTENSITY12 = 0x804C,
            GL_INTENSITY16 = 0x804D,
            GL_R3_G3_B2 = 0x2A10,
            GL_RGB4 = 0x804F,
            GL_RGB5 = 0x8050,
            GL_RGB8 = 0x8051,
            GL_RGB10 = 0x8052,
            GL_RGB12 = 0x8053,
            GL_RGB16 = 0x8054,
            GL_RGBA2 = 0x8055,
            GL_RGBA4 = 0x8056,
            GL_RGB5_A1 = 0x8057,
            GL_RGBA8 = 0x8058,
            GL_RGB10_A2 = 0x8059,
            GL_RGBA12 = 0x805A,
            GL_RGBA16 = 0x805B,
            GL_TEXTURE_RED_SIZE = 0x805C,
            GL_TEXTURE_GREEN_SIZE = 0x805D,
            GL_TEXTURE_BLUE_SIZE = 0x805E,
            GL_TEXTURE_ALPHA_SIZE = 0x805F,
            GL_TEXTURE_LUMINANCE_SIZE = 0x8060,
            GL_TEXTURE_INTENSITY_SIZE = 0x8061,
            GL_PROXY_TEXTURE_1D = 0x8063,
            GL_PROXY_TEXTURE_2D = 0x8064,
        }

        [Flags()]
        public enum TextureObject : uint
        {
            GL_TEXTURE_PRIORITY = 0x8066,
            GL_TEXTURE_RESIDENT = 0x8067,
            GL_TEXTURE_BINDING_1D = 0x8068,
            GL_TEXTURE_BINDING_2D = 0x8069,
        }

        [Flags()]
        public enum VertexArray : uint
        {
            GL_VERTEX_ARRAY = 0x8074,
            GL_NORMAL_ARRAY = 0x8075,
            GL_COLOR_ARRAY = 0x8076,
            GL_INDEX_ARRAY = 0x8077,
            GL_TEXTURE_COORD_ARRAY = 0x8078,
            GL_EDGE_FLAG_ARRAY = 0x8079,
            GL_VERTEX_ARRAY_SIZE = 0x807A,
            GL_VERTEX_ARRAY_TYPE = 0x807B,
            GL_VERTEX_ARRAY_STRIDE = 0x807C,
            GL_NORMAL_ARRAY_TYPE = 0x807E,
            GL_NORMAL_ARRAY_STRIDE = 0x807F,
            GL_COLOR_ARRAY_SIZE = 0x8081,
            GL_COLOR_ARRAY_TYPE = 0x8082,
            GL_COLOR_ARRAY_STRIDE = 0x8083,
            GL_INDEX_ARRAY_TYPE = 0x8085,
            GL_INDEX_ARRAY_STRIDE = 0x8086,
            GL_TEXTURE_COORD_ARRAY_SIZE = 0x8088,
            GL_TEXTURE_COORD_ARRAY_TYPE = 0x8089,
            GL_TEXTURE_COORD_ARRAY_STRIDE = 0x808A,
            GL_EDGE_FLAG_ARRAY_STRIDE = 0x808C,
            GL_VERTEX_ARRAY_POINTER = 0x808E,
            GL_NORMAL_ARRAY_POINTER = 0x808F,
            GL_COLOR_ARRAY_POINTER = 0x8090,
            GL_INDEX_ARRAY_POINTER = 0x8091,
            GL_TEXTURE_COORD_ARRAY_POINTER = 0x8092,
            GL_EDGE_FLAG_ARRAY_POINTER = 0x8093,
            GL_V2F = 0x2A20,
            GL_V3F = 0x2A21,
            GL_C4UB_V2F = 0x2A22,
            GL_C4UB_V3F = 0x2A23,
            GL_C3F_V3F = 0x2A24,
            GL_N3F_V3F = 0x2A25,
            GL_C4F_N3F_V3F = 0x2A26,
            GL_T2F_V3F = 0x2A27,
            GL_T4F_V4F = 0x2A28,
            GL_T2F_C4UB_V3F = 0x2A29,
            GL_T2F_C3F_V3F = 0x2A2A,
            GL_T2F_N3F_V3F = 0x2A2B,
            GL_T2F_C4F_N3F_V3F = 0x2A2C,
            GL_T4F_C4F_N3F_V4F = 0x2A2D,
        }

        [Flags()]
        public enum Extensions : uint
        {
            GL_EXT_vertex_array = 1,
            GL_EXT_bgra = 1,
            GL_EXT_paletted_texture = 1,
            GL_WIN_swap_hint = 1,
            GL_WIN_draw_range_elements = 1,
        }

        [Flags()]
        public enum ExVertexArray : uint
        {
            GL_VERTEX_ARRAY_EXT = 0x8074,
            GL_NORMAL_ARRAY_EXT = 0x8075,
            GL_COLOR_ARRAY_EXT = 0x8076,
            GL_INDEX_ARRAY_EXT = 0x8077,
            GL_TEXTURE_COORD_ARRAY_EXT = 0x8078,
            GL_EDGE_FLAG_ARRAY_EXT = 0x8079,
            GL_VERTEX_ARRAY_SIZE_EXT = 0x807A,
            GL_VERTEX_ARRAY_TYPE_EXT = 0x807B,
            GL_VERTEX_ARRAY_STRIDE_EXT = 0x807C,
            GL_VERTEX_ARRAY_COUNT_EXT = 0x807D,
            GL_NORMAL_ARRAY_TYPE_EXT = 0x807E,
            GL_NORMAL_ARRAY_STRIDE_EXT = 0x807F,
            GL_NORMAL_ARRAY_COUNT_EXT = 0x8080,
            GL_COLOR_ARRAY_SIZE_EXT = 0x8081,
            GL_COLOR_ARRAY_TYPE_EXT = 0x8082,
            GL_COLOR_ARRAY_STRIDE_EXT = 0x8083,
            GL_COLOR_ARRAY_COUNT_EXT = 0x8084,
            GL_INDEX_ARRAY_TYPE_EXT = 0x8085,
            GL_INDEX_ARRAY_STRIDE_EXT = 0x8086,
            GL_INDEX_ARRAY_COUNT_EXT = 0x8087,
            GL_TEXTURE_COORD_ARRAY_SIZE_EXT = 0x8088,
            GL_TEXTURE_COORD_ARRAY_TYPE_EXT = 0x8089,
            GL_TEXTURE_COORD_ARRAY_STRIDE_EXT = 0x808A,
            GL_TEXTURE_COORD_ARRAY_COUNT_EXT = 0x808B,
            GL_EDGE_FLAG_ARRAY_STRIDE_EXT = 0x808C,
            GL_EDGE_FLAG_ARRAY_COUNT_EXT = 0x808D,
            GL_VERTEX_ARRAY_POINTER_EXT = 0x808E,
            GL_NORMAL_ARRAY_POINTER_EXT = 0x808F,
            GL_COLOR_ARRAY_POINTER_EXT = 0x8090,
            GL_INDEX_ARRAY_POINTER_EXT = 0x8091,
            GL_TEXTURE_COORD_ARRAY_POINTER_EXT = 0x8092,
            GL_EDGE_FLAG_ARRAY_POINTER_EXT = 0x8093,
            GL_DOUBLE_EXT = 1,/*DOUBLE*/
        }

        [Flags()]
        public enum ExPalettedTexture : uint
        {
            GL_COLOR_TABLE_FORMAT_EXT = 0x80D8,
            GL_COLOR_TABLE_WIDTH_EXT = 0x80D9,
            GL_COLOR_TABLE_RED_SIZE_EXT = 0x80DA,
            GL_COLOR_TABLE_GREEN_SIZE_EXT = 0x80DB,
            GL_COLOR_TABLE_BLUE_SIZE_EXT = 0x80DC,
            GL_COLOR_TABLE_ALPHA_SIZE_EXT = 0x80DD,
            GL_COLOR_TABLE_LUMINANCE_SIZE_EXT = 0x80DE,
            GL_COLOR_TABLE_INTENSITY_SIZE_EXT = 0x80DF,
            GL_COLOR_INDEX1_EXT = 0x80E2,
            GL_COLOR_INDEX2_EXT = 0x80E3,
            GL_COLOR_INDEX4_EXT = 0x80E4,
            GL_COLOR_INDEX8_EXT = 0x80E5,
            GL_COLOR_INDEX12_EXT = 0x80E6,
            GL_COLOR_INDEX16_EXT = 0x80E7,
        }

        [Flags()]
        public enum WinDrawRangeElements : uint
        {
            GL_MAX_ELEMENTS_VERTICES_WIN = 0x80E8,
            GL_MAX_ELEMENTS_INDICES_WIN = 0x80E9,
        }

        [Flags()]
        public enum WIN_phong_shading : uint
        {
            GL_PHONG_WIN = 0x80EA,
            GL_PHONG_HINT_WIN = 0x80EB,
        }

        [Flags()]
        public enum WIN_specular_fog : uint
        {
            FOG_SPECULAR_TEXTURE_WIN = 0x80EC,
        }
        #endregion
        #region The GLU DLL Constant Definitions.
        [Flags()]
        public enum Version : uint
        {
            GLU_VERSION_1_1 = 1,
            GLU_VERSION_1_2 = 1,
        }
        [Flags()]
        public enum Errors : uint
        {
            GLU_INVALID_ENUM = 100900,
            GLU_INVALID_VALUE = 100901,
            GLU_OUT_OF_MEMORY = 100902,
            GLU_INCOMPATIBLE_GL_VERSION = 100903,
        }
        [Flags()]
        public enum GLU_StringName : uint
        {
            GLU_VERSION = 100800,
            GLU_EXTENSIONS = 100801,
        }
        [Flags()]
        public enum GLU_Boolean : uint
        {
            GLU_TRUE = 1,
            GLU_FALSE = 0,
        }
        [Flags()]
        public enum QuadricNormal : uint
        {
            GLU_SMOOTH = 100000,
            GLU_FLAT = 100001,
            GLU_NONE = 100002,
        }
        [Flags()]
        public enum QuadricDrawStyle : uint
        {
            GLU_POINT = 100010,
            GLU_LINE = 100011,
            GLU_FILL = 100012,
            GLU_SILHOUETTE = 100013,
        }
        [Flags()]
        public enum QuadricOrientation : uint
        {
            GLU_OUTSIDE = 100020,
            GLU_INSIDE = 100021,
        }

        [Flags()]
        public enum TessProperty : uint
        {
            GLU_TESS_WINDING_RULE = 100140,
            GLU_TESS_BOUNDARY_ONLY = 100141,
            GLU_TESS_TOLERANCE = 100142,
        }
        [Flags()]
        public enum TessWinding : uint
        {
            GLU_TESS_WINDING_ODD = 100130,
            GLU_TESS_WINDING_NONZERO = 100131,
            GLU_TESS_WINDING_POSITIVE = 100132,
            GLU_TESS_WINDING_NEGATIVE = 100133,
            GLU_TESS_WINDING_ABS_GEQ_TWO = 100134,
        }
        [Flags()]
        public enum TessCallback : uint
        {
            GLU_TESS_BEGIN = 100100,
            GLU_TESS_VERTEX = 100101,
            GLU_TESS_END = 100102,
            GLU_TESS_ERROR = 100103,
            GLU_TESS_EDGE_FLAG = 100104,
            GLU_TESS_COMBINE = 100105,
            GLU_TESS_BEGIN_DATA = 100106,
            GLU_TESS_VERTEX_DATA = 100107,
            GLU_TESS_END_DATA = 100108,
            GLU_TESS_ERROR_DATA = 100109,
            GLU_TESS_EDGE_FLAG_DATA = 100110,
            GLU_TESS_COMBINE_DATA = 100111,
        }
        [Flags()]
        public enum TessError : uint
        {
            GLU_TESS_ERROR1 = 100151,
            GLU_TESS_ERROR2 = 100152,
            GLU_TESS_ERROR3 = 100153,
            GLU_TESS_ERROR4 = 100154,
            GLU_TESS_ERROR5 = 100155,
            GLU_TESS_ERROR6 = 100156,
            GLU_TESS_ERROR7 = 100157,
            GLU_TESS_ERROR8 = 100158,
            GLU_TESS_MISSING_BEGIN_POLYGON = 100151,
            GLU_TESS_MISSING_BEGIN_CONTOUR = 100152,
            GLU_TESS_MISSING_END_POLYGON = 100153,
            GLU_TESS_MISSING_END_CONTOUR = 100154,
            GLU_TESS_COORD_TOO_LARGE = 100155,
            GLU_TESS_NEED_COMBINE_CALLBACK = 100156,
        }
        [Flags()]
        public enum NurbsProperty : uint
        {
            GLU_AUTO_LOAD_MATRIX = 100200,
            GLU_CULLING = 100201,
            GLU_SAMPLING_TOLERANCE = 100203,
            GLU_DISPLAY_MODE = 100204,
            GLU_PARAMETRIC_TOLERANCE = 100202,
            GLU_SAMPLING_METHOD = 100205,
            GLU_U_STEP = 100206,
            GLU_V_STEP = 100207,
        }
        [Flags()]
        public enum NurbsSampling : uint
        {
            GLU_PATH_LENGTH = 100215,
            GLU_PARAMETRIC_ERROR = 100216,
            GLU_DOMAIN_DISTANCE = 100217,
        }
        [Flags()]
        public enum NurbsTrim : uint
        {
            GLU_MAP1_TRIM_2 = 100210,
            GLU_MAP1_TRIM_3 = 100211,
        }
        [Flags()]
        public enum NurbsOutline : uint
        {
            GLU_OUTLINE_POLYGON = 100240,
            GLU_OUTLINE_PATCH = 100241,
        }
        [Flags()]
        public enum NurbsCallback : uint
        {
            GLU_ERROR = 100103,
        }
        [Flags()]
        public enum NurbsError : uint
        {
            GLU_NURBS_ERROR1 = 100251,
            GLU_NURBS_ERROR2 = 100252,
            GLU_NURBS_ERROR3 = 100253,
            GLU_NURBS_ERROR4 = 100254,
            GLU_NURBS_ERROR5 = 100255,
            GLU_NURBS_ERROR6 = 100256,
            GLU_NURBS_ERROR7 = 100257,
            GLU_NURBS_ERROR8 = 100258,
            GLU_NURBS_ERROR9 = 100259,
            GLU_NURBS_ERROR10 = 100260,
            GLU_NURBS_ERROR11 = 100261,
            GLU_NURBS_ERROR12 = 100262,
            GLU_NURBS_ERROR13 = 100263,
            GLU_NURBS_ERROR14 = 100264,
            GLU_NURBS_ERROR15 = 100265,
            GLU_NURBS_ERROR16 = 100266,
            GLU_NURBS_ERROR17 = 100267,
            GLU_NURBS_ERROR18 = 100268,
            GLU_NURBS_ERROR19 = 100269,
            GLU_NURBS_ERROR20 = 100270,
            GLU_NURBS_ERROR21 = 100271,
            GLU_NURBS_ERROR22 = 100272,
            GLU_NURBS_ERROR23 = 100273,
            GLU_NURBS_ERROR24 = 100274,
            GLU_NURBS_ERROR25 = 100275,
            GLU_NURBS_ERROR26 = 100276,
            GLU_NURBS_ERROR27 = 100277,
            GLU_NURBS_ERROR28 = 100278,
            GLU_NURBS_ERROR29 = 100279,
            GLU_NURBS_ERROR30 = 100280,
            GLU_NURBS_ERROR31 = 100281,
            GLU_NURBS_ERROR32 = 100282,
            GLU_NURBS_ERROR33 = 100283,
            GLU_NURBS_ERROR34 = 100284,
            GLU_NURBS_ERROR35 = 100285,
            GLU_NURBS_ERROR36 = 100286,
            GLU_NURBS_ERROR37 = 100287,
        }

        public enum GLGetFloatParameter : uint
        {
            ALIASED_LINE_WIDTH_RANGE = 0x846E,
            BLEND_COLOR = 0x8005,
            COLOR_CLEAR_VALUE = 0x0C22,
            DEPTH_CLEAR_VALUE = 0x0B73,
            LINE_WIDTH = 0x0B21,
            MAX_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5C,
            MAX_TEXTURE_LOD_BIAS = 0x84FD,
            MAX_VIEWPORT_DIMS = 0x0D3A,
            MIN_FRAGMENT_INTERPOLATION_OFFSET = 0x8E5B,
            MIN_SAMPLE_SHADING_VALUE = 0x8C37,
            PATCH_DEFAULT_INNER_LEVEL = 0x8E73,
            PATCH_DEFAULT_OUTER_LEVEL = 0x8E74,
            POINT_FADE_THRESHOLD_SIZE = 0x8128,
            POINT_SIZE = 0x0B11,
            POINT_SIZE_RANGE = 0x0B12,
            POINT_SIZE_GRANULARITY = 0x0B13,
            POLYGON_OFFSET_FACTOR = 0x8038,
            POLYGON_OFFSET_UNITS = 0x2A00,
            SAMPLE_COVERAGE_VALUE = 0x80AA,
            SMOOTH_LINE_WIDTH_RANGE = 0x0B22,
            SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23,
            VIEWPORT_BOUNDS_RANGE = 0x825D,
        }
        #endregion

        public enum BLEND_MODE
        {
            BLEND_AVG = 0,
            BLEND_ADD,
            BLEND_SUB,
            BLEND_25P,
            BLEND_NONE,
            BLEND_DISABLED = 999
        };
    }

    public class OpenGL32
    {
        internal const string DLLName = "OPENGL32.DLL";

        //Tesselation
        public const double GLU_TESS_MAX_COORD = 1.0e150;

        //public const uint GL_FRAMEBUFFER = 0x8D40;
        //public const uint GL_TEXTURE_2D = 0x0DE1;
        //public const uint GL_RGBA8 = 0x8058;
        //public const uint GL_COLOR_ATTACHMENT0 = 0x8CE0;
        //public const uint GL_RENDERBUFFER = 0x8D41;
        //public const uint GL_DEPTH_COMPONENT16 = 0x81A5;
        //public const uint GL_DEPTH_ATTACHMENT = 0x8D00;
        //public const uint GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
        //public const uint GL_FRAMEBUFFER_UNDEFINED = 0x8219;
        //public const uint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
        //public const uint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
        //public const uint GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x8CDB;
        //public const uint GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x8CDC;
        //public const uint GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
        //public const uint GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
        //public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = 0x8DA8;
        //public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
        //public const uint GL_DEPTH_BUFFER_BIT = 0x00000100;


        [DllImport(DLLName, EntryPoint = "wglCreateContext")]
        public static extern IntPtr wglCreateContext(HDC hdc);
        [DllImport(DLLName, EntryPoint = "wglMakeCurrent")]
        public static extern bool wglMakeCurrent(HDC hdc, HGLRC hglrc);
        [DllImport(DLLName, EntryPoint = "wglDeleteContext")]
        public static extern bool wglDeleteContext(HGLRC hglrc);
        [DllImport(DLLName, EntryPoint = "wglGetCurrentContext")]
        public static extern HGLRC wglGetCurrentContext();
        //[DllImport("opengl32.dll", EntryPoint = "wglSwapBuffers")]
        //internal extern static bool wglSwapBuffers(IntPtr hdc);


        #region The OpenGL DLL Functions (Exactly the same naming).
        [DllImport(DLLName)]
        public static extern void glAccum(uint op, float value);

        //[DllImport(DLLName)]
        //public static extern void glAlphaFunc(uint func, float ref_notkeword);
        [DllImport(DLLName, EntryPoint = "glAlphaFunc")]
        public static extern void glAlphaFunc(GLFunc Function, float Reference);

        [DllImport(DLLName)]
        public static extern byte glAreTexturesResident(int n, uint[] textures, byte[] residences);
        [DllImport(DLLName)]
        public static extern void glArrayElement(int i);
        [DllImport(DLLName, EntryPoint = "glBegin")]
        public static extern void glBegin(GLDrawMode mode);

        //[DllImport(DLLName)]
        //public static extern void glBindTexture(uint target, uint texture);
        [DllImport(DLLName, EntryPoint = "glBindTexture")]
        public static extern void glBindTexture(GLTextureTarget target, uint texture);

        [DllImport(DLLName)]
        public static extern void glBitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte[] bitmap);
        [DllImport(DLLName, EntryPoint = "glBlendFunc")]
        public static extern void glBlendFunc(GLBlendFuncFactor sfactor, GLBlendFuncFactor dfactor);
        [DllImport(DLLName)]
        public static extern void glCallList(uint list);
        [DllImport(DLLName)]
        public static extern void glCallLists(int n, uint type, IntPtr lists);
        [DllImport(DLLName)]
        public static extern void glCallLists(int n, uint type, uint[] lists);
        [DllImport(DLLName)]
        public static extern void glCallLists(int n, uint type, byte[] lists);
        [DllImport(DLLName, EntryPoint = "glClear")]
        public static extern void glClear(GLBufferMask mask);
        [DllImport(DLLName)]
        public static extern void glClearAccum(float red, float green, float blue, float alpha);
        [DllImport(DLLName)]
        public static extern void glClearColor(float red, float green, float blue, float alpha);
        [DllImport(DLLName)]
        public static extern void glClearDepth(double depth);
        [DllImport(DLLName)]
        public static extern void glClearIndex(float c);
        [DllImport(DLLName)]
        public static extern void glClearStencil(int s);
        [DllImport(DLLName)]
        public static extern void glClipPlane(uint plane, double[] equation);
        [DllImport(DLLName)]
        public static extern void glColor3b(byte red, byte green, byte blue);
        [DllImport(DLLName)]
        public static extern void glColor3bv(byte[] v);
        [DllImport(DLLName)]
        public static extern void glColor3d(double red, double green, double blue);
        [DllImport(DLLName)]
        public static extern void glColor3dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glColor3f(float red, float green, float blue);
        [DllImport(DLLName)]
        public static extern void glColor3fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glColor3i(int red, int green, int blue);
        [DllImport(DLLName)]
        public static extern void glColor3iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glColor3s(short red, short green, short blue);
        [DllImport(DLLName)]
        public static extern void glColor3sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glColor3ub(byte red, byte green, byte blue);
        [DllImport(DLLName)]
        public static extern void glColor3ubv(byte[] v);
        [DllImport(DLLName)]
        public static extern void glColor3ui(uint red, uint green, uint blue);
        [DllImport(DLLName)]
        public static extern void glColor3uiv(uint[] v);
        [DllImport(DLLName)]
        public static extern void glColor3us(ushort red, ushort green, ushort blue);
        [DllImport(DLLName)]
        public static extern void glColor3usv(ushort[] v);
        [DllImport(DLLName)]
        public static extern void glColor4b(byte red, byte green, byte blue, byte alpha);
        [DllImport(DLLName)]
        public static extern void glColor4bv(byte[] v);
        [DllImport(DLLName)]
        public static extern void glColor4d(double red, double green, double blue, double alpha);
        [DllImport(DLLName)]
        public static extern void glColor4dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glColor4f(float red, float green, float blue, float alpha);
        [DllImport(DLLName)]
        public static extern void glColor4fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glColor4i(int red, int green, int blue, int alpha);
        [DllImport(DLLName)]
        public static extern void glColor4iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glColor4s(short red, short green, short blue, short alpha);
        [DllImport(DLLName)]
        public static extern void glColor4sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glColor4ub(byte red, byte green, byte blue, byte alpha);
        [DllImport(DLLName)]
        public static extern void glColor4ubv(byte[] v);
        [DllImport(DLLName)]
        public static extern void glColor4ui(uint red, uint green, uint blue, uint alpha);
        [DllImport(DLLName)]
        public static extern void glColor4uiv(uint[] v);
        [DllImport(DLLName)]
        public static extern void glColor4us(ushort red, ushort green, ushort blue, ushort alpha);
        [DllImport(DLLName)]
        public static extern void glColor4usv(ushort[] v);

        //[DllImport(DLLName)]
        //public static extern void glColorMask(byte red, byte green, byte blue, byte alpha);
        [DllImport(DLLName, EntryPoint = "glColorMask")]
        //public static extern void glColorMask(byte red, byte green, byte blue, byte alpha);
        public static extern void glColorMask([MarshalAs(UnmanagedType.I1)] GL_Boolean red, [MarshalAs(UnmanagedType.I1)] GL_Boolean green,
                                              [MarshalAs(UnmanagedType.I1)] GL_Boolean blue, [MarshalAs(UnmanagedType.I1)] GL_Boolean alpha);

        //[DllImport(DLLName)]
        //public static extern void glColorMaterial(uint face, uint mode);
        [DllImport("opengl32.dll", EntryPoint = "glColorMaterial")]
        public static extern void glColorMaterial(GLFace face, GLMaterialParameter mode); // 

        [DllImport(DLLName)]
        public static extern void glColorPointer(int size, uint type, int stride, int[] pointer);
        [DllImport(DLLName)]
        public static extern void glCopyPixels(int x, int y, int width, int height, uint type);
        [DllImport(DLLName)]
        public static extern void glCopyTexImage1D(uint target, int level, uint internalFormat, int x, int y, int width, int border);
        [DllImport(DLLName)]
        public static extern void glCopyTexImage2D(uint target, int level, uint internalFormat, int x, int y, int width, int height, int border);
        [DllImport(DLLName)]
        public static extern void glCopyTexSubImage1D(uint target, int level, int xoffset, int x, int y, int width);
        [DllImport(DLLName)]
        public static extern void glCopyTexSubImage2D(uint target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        
        //[DllImport(DLLName)]
        //public static extern void glCullFace(uint mode);
        [DllImport(DLLName, EntryPoint = "glCullFace")]
        public static extern void glCullFace(GLFace mode);

        [DllImport(DLLName)]
        public static extern void glDeleteLists(uint list, int range);
        
        [DllImport(DLLName, EntryPoint = "glDeleteTextures")]
        public static extern void glDeleteTextures(int n, uint[] textures);

        [DllImport(DLLName, EntryPoint = "glDepthFunc")]
        public static extern void glDepthFunc(GLFunc func);
        [DllImport(DLLName)]
        public static extern void glDepthMask(byte flag);
        [DllImport(DLLName)]
        public static extern void glDepthRange(double zNear, double zFar);
        [DllImport(DLLName, EntryPoint = "glDisable")]
        public static extern void glDisable(GLCapability cap);
        [DllImport(DLLName)]
        public static extern void glDisableClientState(uint array);
        [DllImport(DLLName)]
        public static extern void glDrawArrays(uint mode, int first, int count);
        [DllImport(DLLName)]
        public static extern void glDrawBuffer(uint mode);
        [DllImport(DLLName)]
        public static extern void glDrawElements(uint mode, int count, uint type, int[] indices);
        [DllImport(DLLName)]
        public static extern void glDrawPixels(int width, int height, uint format, uint type, float[] pixels);
        [DllImport(DLLName)]
        public static extern void glEdgeFlag(byte flag);
        [DllImport(DLLName)]
        public static extern void glEdgeFlagPointer(int stride, int[] pointer);
        [DllImport(DLLName)]
        public static extern void glEdgeFlagv(byte[] flag);
        [DllImport(DLLName, EntryPoint = "glEnable")]
        public static extern void glEnable(GLCapability cap);
        [DllImport(DLLName)]
        public static extern void glEnableClientState(uint array);
        [DllImport(DLLName)]
        public static extern void glEnd();
        [DllImport(DLLName)]
        public static extern void glEndList();
        [DllImport(DLLName)]
        public static extern void glEvalCoord1d(double u);
        [DllImport(DLLName)]
        public static extern void glEvalCoord1dv(double[] u);
        [DllImport(DLLName)]
        public static extern void glEvalCoord1f(float u);
        [DllImport(DLLName)]
        public static extern void glEvalCoord1fv(float[] u);
        [DllImport(DLLName)]
        public static extern void glEvalCoord2d(double u, double v);
        [DllImport(DLLName)]
        public static extern void glEvalCoord2dv(double[] u);
        [DllImport(DLLName)]
        public static extern void glEvalCoord2f(float u, float v);
        [DllImport(DLLName)]
        public static extern void glEvalCoord2fv(float[] u);
        [DllImport(DLLName)]
        public static extern void glEvalMesh1(uint mode, int i1, int i2);
        [DllImport(DLLName)]
        public static extern void glEvalMesh2(uint mode, int i1, int i2, int j1, int j2);
        [DllImport(DLLName)]
        public static extern void glEvalPoint1(int i);
        [DllImport(DLLName)]
        public static extern void glEvalPoint2(int i, int j);
        [DllImport(DLLName)]
        public static extern void glFeedbackBuffer(int size, uint type, float[] buffer);
        [DllImport(DLLName)]
        public static extern void glFinish();
        [DllImport(DLLName)]
        public static extern void glFlush();

        [DllImport(DLLName, EntryPoint = "glFogf")]
        public static extern void glFogf(GLCapability pname, float param); // 
        [DllImport(DLLName, EntryPoint = "glFogfv")]
        public static extern void glFogfv(GLCapability pname, float[] @params); // 
        [DllImport(DLLName, EntryPoint = "glFogi")]
        public static extern void glFogi(GLCapability pname, int param); // 
        [DllImport(DLLName, EntryPoint = "glFogiv")]
        public static extern void glFogiv(GLCapability pname, GLTextureMagFilter[] @params); // 

        [DllImport(DLLName)]
        public static extern void glFrontFace(GLFrontFaceDirection mode);
        [DllImport(DLLName)]
        public static extern void glFrustum(double left, double right, double bottom, double top, double zNear, double zFar);
        [DllImport(DLLName)]
        public static extern uint glGenLists(int range);

        //[DllImport(DLLName)]
        //public static extern void glGenTextures(int n, uint[] textures);
        //[DllImport(DLLName, EntryPoint = "glGenTextures")]
        //public static extern void glGenTextures(int count, uint[] textures);
        [DllImport(DLLName, EntryPoint = "glGenTextures")]
        public static extern void glGenTextures(int one, ref uint texture);

        [DllImport(DLLName)]
        public static extern void glGetBooleanv(uint pname, byte[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetClipPlane(uint plane, double[] equation);
        
        //[DllImport(DLLName, EntryPoint = "glGetDoublev")]
        //public static extern void glGetDoublev(glGetFloatParameter pname, out double param);
        //[DllImport(DLLName, EntryPoint = "glGetDoublev")]
        //public static extern void glGetDoublev(glGetFloatParameter pname, double[] @params);
        [DllImport(DLLName, EntryPoint = "glGetDoublev")]
        public static extern void glGetDoublev(uint pname, double[] params_notkeyword);
        
        [DllImport(DLLName)]
        public static extern uint glGetError();
        [DllImport(DLLName)]
        public static extern void glGetFloatv(uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetIntegerv(uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetLightfv(uint light, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetLightiv(uint light, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetMapdv(uint target, uint query, double[] v);
        [DllImport(DLLName)]
        public static extern void glGetMapfv(uint target, uint query, float[] v);
        [DllImport(DLLName)]
        public static extern void glGetMapiv(uint target, uint query, int[] v);
        [DllImport(DLLName)]
        public static extern void glGetMaterialfv(uint face, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetMaterialiv(uint face, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetPixelMapfv(uint map, float[] values);
        [DllImport(DLLName)]
        public static extern void glGetPixelMapuiv(uint map, uint[] values);
        [DllImport(DLLName)]
        public static extern void glGetPixelMapusv(uint map, ushort[] values);
        [DllImport(DLLName)]
        public static extern void glGetPointerv(uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetPolygonStipple(byte[] mask);
        //[DllImport(DLLName)]
        //public unsafe static extern sbyte* glGetString(uint name);
        [DllImport(DLLName)]
        public static extern void glGetTexEnvfv(uint target, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexEnviv(uint target, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexGendv(uint coord, uint pname, double[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexGenfv(uint coord, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexGeniv(uint coord, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexImage(uint target, int level, uint format, uint type, int[] pixels);
        [DllImport(DLLName)]
        public static extern void glGetTexLevelParameterfv(uint target, int level, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexLevelParameteriv(uint target, int level, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexParameterfv(uint target, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glGetTexParameteriv(uint target, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glHint(uint target, uint mode);
        [DllImport(DLLName)]
        public static extern void glIndexMask(uint mask);
        [DllImport(DLLName)]
        public static extern void glIndexPointer(uint type, int stride, int[] pointer);
        [DllImport(DLLName)]
        public static extern void glIndexd(double c);
        [DllImport(DLLName)]
        public static extern void glIndexdv(double[] c);
        [DllImport(DLLName)]
        public static extern void glIndexf(float c);
        [DllImport(DLLName)]
        public static extern void glIndexfv(float[] c);
        [DllImport(DLLName)]
        public static extern void glIndexi(int c);
        [DllImport(DLLName)]
        public static extern void glIndexiv(int[] c);
        [DllImport(DLLName)]
        public static extern void glIndexs(short c);
        [DllImport(DLLName)]
        public static extern void glIndexsv(short[] c);
        [DllImport(DLLName)]
        public static extern void glIndexub(byte c);
        [DllImport(DLLName)]
        public static extern void glIndexubv(byte[] c);
        [DllImport(DLLName)]
        public static extern void glInitNames();
        [DllImport(DLLName)]
        public static extern void glInterleavedArrays(uint format, int stride, int[] pointer);

        //[DllImport(DLLName)]
        //public static extern byte glIsEnabled(uint cap);
        [DllImport(DLLName, EntryPoint = "glIsEnabled")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool glIsEnabled(GLCapability cap);

        [DllImport(DLLName, EntryPoint = "glIsList")]
        public static extern bool glIsList(uint list);
        
        //[DllImport(DLLName)]
        //public static extern byte glIsTexture(uint texture);
        [DllImport(DLLName, EntryPoint = "glIsTexture")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool glIsTexture(uint texture);

        [DllImport(DLLName)]
        public static extern void glLightModelf(uint pname, float param);
        [DllImport(DLLName)]
        public static extern void glLightModelfv(uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glLightModeli(uint pname, int param);
        [DllImport(DLLName)]
        public static extern void glLightModeliv(uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glLightf(GLCapability light, GLLightParameter pname, float param);
        [DllImport(DLLName)]
        public static extern void glLightfv(GLCapability light, GLLightParameter pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glLighti(GLCapability light, GLLightParameter pname, int param);
        [DllImport(DLLName)]
        public static extern void glLightiv(GLCapability light, GLLightParameter pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glLineStipple(int factor, ushort pattern);
        [DllImport(DLLName)]
        public static extern void glLineWidth(float width);
        [DllImport(DLLName)]
        public static extern void glListBase(uint base_notkeyword);
        [DllImport(DLLName)]
        public static extern void glLoadIdentity();
        [DllImport(DLLName)]
        public static extern void glLoadMatrixd(double[] m);
        [DllImport(DLLName)]
        public static extern void glLoadMatrixf(float[] m);
        [DllImport(DLLName)]
        public static extern void glLoadName(uint name);
        [DllImport(DLLName, EntryPoint = "glLogicOp")]
        public static extern void LogicOp(GLLogicOpMode opcode);
        [DllImport(DLLName)]
        public static extern void glMap1d(uint target, double u1, double u2, int stride, int order, double[] points);
        [DllImport(DLLName)]
        public static extern void glMap1f(uint target, float u1, float u2, int stride, int order, float[] points);
        [DllImport(DLLName)]
        public static extern void glMap2d(uint target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double[] points);
        [DllImport(DLLName)]
        public static extern void glMap2f(uint target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float[] points);
        [DllImport(DLLName)]
        public static extern void glMapGrid1d(int un, double u1, double u2);
        [DllImport(DLLName)]
        public static extern void glMapGrid1f(int un, float u1, float u2);
        [DllImport(DLLName)]
        public static extern void glMapGrid2d(int un, double u1, double u2, int vn, double v1, double v2);
        [DllImport(DLLName)]
        public static extern void glMapGrid2f(int un, float u1, float u2, int vn, float v1, float v2);
        [DllImport(DLLName)]
        public static extern void glMaterialf(uint face, uint pname, float param);
        [DllImport(DLLName)]
        public static extern void glMaterialfv(uint face, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glMateriali(uint face, uint pname, int param);
        [DllImport(DLLName)]
        public static extern void glMaterialiv(uint face, uint pname, int[] params_notkeyword);
        [DllImport(DLLName, EntryPoint = "glMatrixMode")]
        public static extern void glMatrixMode(GLMatrixModeList mode);
        [DllImport(DLLName)]
        public static extern void glMultMatrixd(double[] m);
        [DllImport(DLLName)]
        public static extern void glMultMatrixf(float[] m);
        
        //[DllImport(DLLName)]
        //public static extern void glNewList(uint list, uint mode);
        [DllImport(DLLName, EntryPoint = "glNewList")]
        public static extern void glNewList(uint list, GLListMode mode);

        [DllImport(DLLName)]
        public static extern void glNormal3b(byte nx, byte ny, byte nz);
        [DllImport(DLLName)]
        public static extern void glNormal3bv(byte[] v);
        [DllImport(DLLName)]
        public static extern void glNormal3d(double nx, double ny, double nz);
        [DllImport(DLLName)]
        public static extern void glNormal3dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glNormal3f(float nx, float ny, float nz);
        [DllImport(DLLName)]
        public static extern void glNormal3fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glNormal3i(int nx, int ny, int nz);
        [DllImport(DLLName)]
        public static extern void glNormal3iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glNormal3s(short nx, short ny, short nz);
        [DllImport(DLLName)]
        public static extern void glNormal3sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glNormalPointer(uint type, int stride, float[] pointer);
        [DllImport(DLLName)]
        public static extern void glOrtho(double left, double right, double bottom, double top, double zNear, double zFar);
        [DllImport(DLLName)]
        public static extern void glPassThrough(float token);
        [DllImport(DLLName)]
        public static extern void glPixelMapfv(uint map, int mapsize, float[] values);
        [DllImport(DLLName)]
        public static extern void glPixelMapuiv(uint map, int mapsize, uint[] values);
        [DllImport(DLLName)]
        public static extern void glPixelMapusv(uint map, int mapsize, ushort[] values);
        [DllImport(DLLName)]
        public static extern void glPixelStoref(uint pname, float param);

        //[DllImport(DLLName)]
        //public static extern void glPixelStorei(uint pname, int param);
        [DllImport(DLLName, EntryPoint = "glPixelStorei")]
        public static extern void glPixelStorei(GLPixelStoreParameter pname, int param);

        [DllImport(DLLName)]
        public static extern void glPixelTransferf(uint pname, float param);
        [DllImport(DLLName)]
        public static extern void glPixelTransferi(uint pname, int param);
        [DllImport(DLLName)]
        public static extern void glPixelZoom(float xfactor, float yfactor);
        [DllImport(DLLName)]
        public static extern void glPointSize(float size);
        [DllImport(DLLName, EntryPoint = "glPolygonMode")]
        public static extern void glPolygonMode(GLFace face, GLPolygon mode);
        [DllImport(DLLName)]
        public static extern void glPolygonOffset(float factor, float units);
        [DllImport(DLLName)]
        public static extern void glPolygonStipple(byte[] mask);
        [DllImport(DLLName)]
        public static extern void glPopAttrib();
        [DllImport(DLLName)]
        public static extern void glPopClientAttrib();
        [DllImport(DLLName)]
        public static extern void glPopMatrix();
        [DllImport(DLLName)]
        public static extern void glPopName();
        [DllImport(DLLName)]
        public static extern void glPrioritizeTextures(int n, uint[] textures, float[] priorities);
        [DllImport(DLLName)]
        public static extern void glPushAttrib(uint mask);
        [DllImport(DLLName)]
        public static extern void glPushClientAttrib(uint mask);
        [DllImport(DLLName)]
        public static extern void glPushMatrix();
        [DllImport(DLLName)]
        public static extern void glPushName(uint name);
        [DllImport(DLLName)]
        public static extern void glRasterPos2d(double x, double y);
        [DllImport(DLLName)]
        public static extern void glRasterPos2dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos2f(float x, float y);
        [DllImport(DLLName)]
        public static extern void glRasterPos2fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos2i(int x, int y);
        [DllImport(DLLName)]
        public static extern void glRasterPos2iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos2s(short x, short y);
        [DllImport(DLLName)]
        public static extern void glRasterPos2sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos3d(double x, double y, double z);
        [DllImport(DLLName)]
        public static extern void glRasterPos3dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos3f(float x, float y, float z);
        [DllImport(DLLName)]
        public static extern void glRasterPos3fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos3i(int x, int y, int z);
        [DllImport(DLLName)]
        public static extern void glRasterPos3iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos3s(short x, short y, short z);
        [DllImport(DLLName)]
        public static extern void glRasterPos3sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos4d(double x, double y, double z, double w);
        [DllImport(DLLName)]
        public static extern void glRasterPos4dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos4f(float x, float y, float z, float w);
        [DllImport(DLLName)]
        public static extern void glRasterPos4fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos4i(int x, int y, int z, int w);
        [DllImport(DLLName)]
        public static extern void glRasterPos4iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glRasterPos4s(short x, short y, short z, short w);
        [DllImport(DLLName)]
        public static extern void glRasterPos4sv(short[] v);
        
        //[DllImport(DLLName)]
        //public static extern void glReadBuffer(uint mode);
        [DllImport(DLLName, EntryPoint = "glReadBuffer")]
        public static extern void glReadBuffer(GLBuffer src);

        //[DllImport(DLLName, EntryPoint = "glReadPixels")]
        //public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, byte[] pixels);
        [DllImport(DLLName, EntryPoint = "glReadPixels")]
        public static extern void glReadPixels(int x, int y, int width, int height, GLPixelFormat format, GLPixelDataType type, byte[] pixels);

        //[DllImport(DLLName)]
        //public static extern void glReadPixels(int x, int y, int width, int height, uint format, uint type, IntPtr pixels);
        [DllImport(DLLName, EntryPoint = "glReadPixels")]
        public static extern void glReadPixels(int x, int y, int width, int height, GLPixelFormat format, GLPixelDataType type, IntPtr pixels);

        [DllImport(DLLName)]
        public static extern void glRectd(double x1, double y1, double x2, double y2);
        [DllImport(DLLName)]
        public static extern void glRectdv(double[] v1, double[] v2);
        [DllImport(DLLName)]
        public static extern void glRectf(float x1, float y1, float x2, float y2);
        [DllImport(DLLName)]
        public static extern void glRectfv(float[] v1, float[] v2);
        [DllImport(DLLName)]
        public static extern void glRecti(int x1, int y1, int x2, int y2);
        [DllImport(DLLName)]
        public static extern void glRectiv(int[] v1, int[] v2);
        [DllImport(DLLName)]
        public static extern void glRects(short x1, short y1, short x2, short y2);
        [DllImport(DLLName)]
        public static extern void glRectsv(short[] v1, short[] v2);

        [DllImport(DLLName)]
        public static extern int glRenderMode(GLRenderingMode mode);

        [DllImport(DLLName)]
        public static extern void glRotated(double angle, double x, double y, double z);
        [DllImport(DLLName)]
        public static extern void glRotatef(float angle, float x, float y, float z);
        [DllImport(DLLName)]
        public static extern void glScaled(double x, double y, double z);
        [DllImport(DLLName)]
        public static extern void glScalef(float x, float y, float z);
        [DllImport(DLLName)]
        public static extern void glScissor(int x, int y, int width, int height);

        [DllImport(DLLName, EntryPoint = "glSelectBuffer")]
        public static extern void glSelectBuffer(int size, int[] buffer);

        [DllImport(DLLName, EntryPoint = "glShadeModel")]
        public static extern void glShadeModel(GLShadingModel mode);

        //[DllImport(DLLName)]
        //public static extern void glStencilFunc(uint func, int ref_notkeword, uint mask);
        [DllImport(DLLName, EntryPoint = "glStencilFunc")]
        public static extern void glStencilFunc(GLFunc func, int refvalue, uint mask);

        [DllImport(DLLName)]
        public static extern void glStencilMask(uint mask);
        [DllImport(DLLName)]
        public static extern void glStencilOp(uint fail, uint zfail, uint zpass);
        [DllImport(DLLName)]
        public static extern void glTexCoord1d(double s);
        [DllImport(DLLName)]
        public static extern void glTexCoord1dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord1f(float s);
        [DllImport(DLLName)]
        public static extern void glTexCoord1fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord1i(int s);
        [DllImport(DLLName)]
        public static extern void glTexCoord1iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord1s(short s);
        [DllImport(DLLName)]
        public static extern void glTexCoord1sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord2d(double s, double t);
        [DllImport(DLLName)]
        public static extern void glTexCoord2dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord2f(float s, float t);
        [DllImport(DLLName)]
        public static extern void glTexCoord2fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord2i(int s, int t);
        [DllImport(DLLName)]
        public static extern void glTexCoord2iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord2s(short s, short t);
        [DllImport(DLLName)]
        public static extern void glTexCoord2sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord3d(double s, double t, double r);
        [DllImport(DLLName)]
        public static extern void glTexCoord3dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord3f(float s, float t, float r);
        [DllImport(DLLName)]
        public static extern void glTexCoord3fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord3i(int s, int t, int r);
        [DllImport(DLLName)]
        public static extern void glTexCoord3iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord3s(short s, short t, short r);
        [DllImport(DLLName)]
        public static extern void glTexCoord3sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord4d(double s, double t, double r, double q);
        [DllImport(DLLName)]
        public static extern void glTexCoord4dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord4f(float s, float t, float r, float q);
        [DllImport(DLLName)]
        public static extern void glTexCoord4fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord4i(int s, int t, int r, int q);
        [DllImport(DLLName)]
        public static extern void glTexCoord4iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoord4s(short s, short t, short r, short q);
        [DllImport(DLLName)]
        public static extern void glTexCoord4sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glTexCoordPointer(int size, uint type, int stride, float[] pointer);
        [DllImport(DLLName)]
        public static extern void glTexEnvf(uint target, uint pname, float param);
        [DllImport(DLLName)]
        public static extern void glTexEnvfv(uint target, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glTexEnvi(uint target, uint pname, int param);
        [DllImport(DLLName)]
        public static extern void glTexEnviv(uint target, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glTexGend(uint coord, uint pname, double param);
        [DllImport(DLLName)]
        public static extern void glTexGendv(uint coord, uint pname, double[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glTexGenf(uint coord, uint pname, float param);
        [DllImport(DLLName)]
        public static extern void glTexGenfv(uint coord, uint pname, float[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glTexGeni(uint coord, uint pname, int param);
        [DllImport(DLLName)]
        public static extern void glTexGeniv(uint coord, uint pname, int[] params_notkeyword);

        //[DllImport(DLLName)]
        //public static extern void glTexImage1D(uint target, int level, int internalformat, int width, int border, uint format, uint type, byte[] pixels);
        //[DllImport(DLLName)]
        //public static extern void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, byte[] pixels);
        //[DllImport(DLLName)]
        //public static extern void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        [DllImport(DLLName, EntryPoint = "glTexImage2D")]
        public static extern void glTexImage2D(GLTexture2DProxyTarget target, int level, GLInternalFormat internalformat, int width, int height, int border, GLPixelFormat format, GLPixelDataType type, IntPtr pixels);
        //[DllImport(DLLName, EntryPoint = "glTexImage2D")]
        //public static extern void glTexImage2D(glTexture2DProxyTarget target, int level, GLInternalFormat internalformat, int width, int height, int border, GLPixelFormat format, glPixelDataType type, byte[] pixels);

        //[DllImport(DLLName)]
        //public static extern void glTexParameterf(uint target, uint pname, float param);
        [DllImport(DLLName, EntryPoint = "glTexParameterf")]
        public static extern void glTexParameterf(GLTextureTarget target, GLTextureParameter pname, float param);

        [DllImport(DLLName)]
        public static extern void glTexParameterfv(uint target, uint pname, float[] params_notkeyword);
        
        //[DllImport(DLLName)]
        //public static extern void glTexParameteri(uint target, uint pname, int param);
        [DllImport(DLLName, EntryPoint = "glTexParameteri")]
        public static extern void glTexParameteri(GLTextureTarget target, GLTextureParameter pname, int param);

        [DllImport(DLLName)]
        public static extern void glTexParameteriv(uint target, uint pname, int[] params_notkeyword);
        [DllImport(DLLName)]
        public static extern void glTexSubImage1D(uint target, int level, int xoffset, int width, uint format, uint type, int[] pixels);
        [DllImport(DLLName)]
        public static extern void glTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, byte[] pixels);
        [DllImport(DLLName)]
        public static extern void glTranslated(double x, double y, double z);
        [DllImport(DLLName)]
        public static extern void glTranslatef(float x, float y, float z);
        [DllImport(DLLName)]
        public static extern void glVertex2d(double x, double y);
        [DllImport(DLLName)]
        public static extern void glVertex2dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glVertex2f(float x, float y);
        [DllImport(DLLName)]
        public static extern void glVertex2fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glVertex2i(int x, int y);
        [DllImport(DLLName)]
        public static extern void glVertex2iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glVertex2s(short x, short y);
        [DllImport(DLLName)]
        public static extern void glVertex2sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glVertex3d(double x, double y, double z);
        [DllImport(DLLName)]
        public static extern void glVertex3dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glVertex3f(float x, float y, float z);
        [DllImport(DLLName)]
        public static extern void glVertex3fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glVertex3i(int x, int y, int z);
        [DllImport(DLLName)]
        public static extern void glVertex3iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glVertex3s(short x, short y, short z);
        [DllImport(DLLName)]
        public static extern void glVertex3sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glVertex4d(double x, double y, double z, double w);
        [DllImport(DLLName)]
        public static extern void glVertex4dv(double[] v);
        [DllImport(DLLName)]
        public static extern void glVertex4f(float x, float y, float z, float w);
        [DllImport(DLLName)]
        public static extern void glVertex4fv(float[] v);
        [DllImport(DLLName)]
        public static extern void glVertex4i(int x, int y, int z, int w);
        [DllImport(DLLName)]
        public static extern void glVertex4iv(int[] v);
        [DllImport(DLLName)]
        public static extern void glVertex4s(short x, short y, short z, short w);
        [DllImport(DLLName)]
        public static extern void glVertex4sv(short[] v);
        [DllImport(DLLName)]
        public static extern void glVertexPointer(int size, uint type, int stride, float[] pointer);
        [DllImport(DLLName)]
        public static extern void glViewport(int x, int y, int width, int height);
        #endregion


        #region The GLU DLL Functions (Exactly the same naming).
        //[DllImport("Glu32.dll")]
        //public static unsafe extern sbyte* gluErrorString(uint errCode);
        //[DllImport("Glu32.dll")]
        //public static unsafe extern sbyte* gluGetString(int name);
        [DllImport("Glu32.dll")]
        public static extern void gluOrtho2D(double left, double right, double bottom, double top);
        
        //[DllImport("Glu32.dll")]
        //public static extern void gluPerspective(double fovy, double aspect, double zNear, double zFar);
        [DllImport("glu32.dll", EntryPoint = "gluPerspective")]
        public static extern void gluPerspective(double fov, double aspect, double zNear, double zFar);

        [DllImport("Glu32.dll")]
        public static extern void gluPickMatrix(double x, double y, double width, double height, int[] viewport);
        [DllImport("Glu32.dll")]
        public static extern void gluLookAt(double eyex, double eyey, double eyez, double centerx, double centery, double centerz, double upx, double upy, double upz);

        //[DllImport("Glu32.dll")]
        //public static extern void gluProject(double objx, double objy, double objz, double[] modelMatrix, double[] projMatrix, int[] viewport, double winx, double winy, double winz);
        [DllImport("glu32.dll", EntryPoint = "gluProject")]
        public static extern int gluProject(double objx, double objy, double objz,
                                            double[] modelMatrix, double[] projectionMatrix, int[] viewPort,
                                            ref double winX, ref double winY, ref double winZ);

        //      [DllImport("Glu32.dll")]
        //      public static extern void gluUnProject(double winx, double winy, double winz, double[] modelMatrix, double[] projMatrix, int[] viewport, ref double objx, ref double objy, ref double objz);
        //[DllImport("glu32.dll", EntryPoint = "gluUnProject")]
        //public static extern int gluUnProject(double winx, double winy, double winz, 
        //                                      double[] modelViewMatrix, double[] projectionMatrix, int[] viewPort, 
        //                                      double[] posX, double[] posY, double[] posZ);
        [DllImport("glu32.dll", EntryPoint = "gluUnProject")]
        public static extern int gluUnProject(double winx, double winy, double winz,
                                              double[] modelViewMatrix, double[] projectionMatrix, int[] viewPort,
                                              ref double posX, ref double posY, ref double posZ);

        [DllImport("Glu32.dll")]
        public static extern void gluScaleImage(int format, int widthin, int heightin, int typein, int[] datain, int widthout, int heightout, int typeout, int[] dataout);
        [DllImport("Glu32.dll")]
        public static extern void gluBuild1DMipmaps(uint target, uint components, int width, uint format, uint type, IntPtr data);
        [DllImport("Glu32.dll")]
        public static extern void gluBuild2DMipmaps(uint target, uint components, int width, int height, uint format, uint type, IntPtr data);
        [DllImport("Glu32.dll")]
        public static extern IntPtr gluNewQuadric();
        [DllImport("Glu32.dll")]
        public static extern void gluDeleteQuadric(IntPtr state);
        [DllImport("Glu32.dll")]
        public static extern void gluQuadricNormals(IntPtr quadObject, uint normals);
        [DllImport("Glu32.dll")]
        public static extern void gluQuadricTexture(IntPtr quadObject, int textureCoords);
        [DllImport("Glu32.dll")]
        public static extern void gluQuadricOrientation(IntPtr quadObject, int orientation);
        [DllImport("Glu32.dll")]
        public static extern void gluQuadricDrawStyle(IntPtr quadObject, uint drawStyle);
        [DllImport("Glu32.dll")]
        public static extern void gluCylinder(IntPtr qobj, double baseRadius, double topRadius, double height, int slices, int stacks);
        [DllImport("Glu32.dll")]
        public static extern void gluDisk(IntPtr qobj, double innerRadius, double outerRadius, int slices, int loops);
        [DllImport("Glu32.dll")]
        public static extern void gluPartialDisk(IntPtr qobj, double innerRadius, double outerRadius, int slices, int loops, double startAngle, double sweepAngle);
        [DllImport("Glu32.dll")]
        public static extern void gluSphere(IntPtr qobj, double radius, int slices, int stacks);
        [DllImport("Glu32.dll")]
        public static extern IntPtr gluNewTess();
        [DllImport("Glu32.dll")]
        public static extern void gluDeleteTess(IntPtr tess);
        [DllImport("Glu32.dll")]
        public static extern void gluTessBeginPolygon(IntPtr tess, IntPtr polygonData);
        [DllImport("Glu32.dll")]
        public static extern void gluTessBeginContour(IntPtr tess);
        [DllImport("Glu32.dll")]
        public static extern void gluTessVertex(IntPtr tess, double[] coords, double[] data);
        [DllImport("Glu32.dll")]
        public static extern void gluTessEndContour(IntPtr tess);
        [DllImport("Glu32.dll")]
        public static extern void gluTessEndPolygon(IntPtr tess);
        [DllImport("Glu32.dll")]
        public static extern void gluTessProperty(IntPtr tess, int which, double value);
        [DllImport("Glu32.dll")]
        public static extern void gluTessNormal(IntPtr tess, double x, double y, double z);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Begin callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.BeginData callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Combine callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.CombineData callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EdgeFlag callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EdgeFlagData callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.End callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.EndData callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Error callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.ErrorData callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.Vertex callback);
        //[DllImport("Glu32.dll")] public static extern void gluTessCallback(IntPtr tess, int which, SharpGL.Delegates.Tesselators.VertexData callback);
        [DllImport("Glu32.dll")]
        public static extern void gluGetTessProperty(IntPtr tess, int which, double value);
        [DllImport("Glu32.dll")]
        public static extern IntPtr gluNewNurbsRenderer();
        [DllImport("Glu32.dll")]
        public static extern void gluDeleteNurbsRenderer(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluBeginSurface(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluBeginCurve(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluEndCurve(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluEndSurface(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluBeginTrim(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluEndTrim(IntPtr nobj);
        [DllImport("Glu32.dll")]
        public static extern void gluPwlCurve(IntPtr nobj, int count, float array, int stride, uint type);
        [DllImport("Glu32.dll")]
        public static extern void gluNurbsCurve(IntPtr nobj, int nknots, float[] knot, int stride, float[] ctlarray, int order, uint type);
        [DllImport("Glu32.dll")]
        public static extern void gluNurbsSurface(IntPtr nobj, int sknot_count, float[] sknot, int tknot_count, float[] tknot, int s_stride, int t_stride, float[] ctlarray, int sorder, int torder, uint type);
        [DllImport("Glu32.dll")]
        public static extern void gluLoadSamplingMatrices(IntPtr nobj, float[] modelMatrix, float[] projMatrix, int[] viewport);
        [DllImport("Glu32.dll")]
        public static extern void gluNurbsProperty(IntPtr nobj, int property, float value);
        [DllImport("Glu32.dll")]
        public static extern void gluGetNurbsProperty(IntPtr nobj, int property, float value);
        [DllImport("Glu32.dll")]
        public static extern void IntPtrCallback(IntPtr nobj, int which, IntPtr Callback);
        #endregion

        //#region GenTexture(s)
        ///// <summary>
        ///// Generates one texture name and returns it.
        ///// </summary>
        ///// <returns>The new texture name.</returns>
        //public static uint GenTexture()
        //{
        //    uint i = 0;
        //    GenTextures(1, out i);
        //    return i;
        //}

        ///// <summary>
        ///// Generates one texture name and returns it.
        ///// </summary>
        ///// <param name="texture">The new texture name.</param>
        //public static void GenTexture(out uint texture)
        //{
        //    GenTextures(1, out texture);
        //}

        ///// <summary>
        ///// Generates <paramref name="count"/> many texture names and returns them as array.
        ///// </summary>
        ///// <param name="count">The number of texture names to be generated.</param>
        ///// <returns>The new texture names as array.</returns>
        //public static uint[] GenTextures(int count)
        //{
        //    uint[] res = new uint[count];
        //    GenTextures(count, res);
        //    return res;
        //}
        //#endregion

        //
        //
        //
        //      OpenGL function utilities
        //
        //
        //
        public static HDC CreateOGLContext(HDC hDC)
        {
            HDC hRC = HDC.Zero;
            PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
            int pixelFormat;

            pixelFormat = ChoosePixelFormat(hDC, ref pfd);

            if (pixelFormat == 0)
            {
                MessageBox.Show("Can't create OpenGL context (ChoosePixelFormat)!");
            }
            else
            {
                if (!SetPixelFormat(hDC, pixelFormat, ref pfd))
                {
                    MessageBox.Show("Can't create OpenGL context (SetPixelFormat)!");
                }
                else
                {
                    hRC = wglCreateContext(hDC);

                    if (hRC == null)
                    {
                        MessageBox.Show("Can't create OpenGL context (wglCreateContext)!");
                    }
                    else 
                    {
                        if (!wglMakeCurrent(hDC, hRC))
                        {
                            MessageBox.Show("Can't create OpenGL context (wglMakeCurrent)!");
                        }
                        else
                        {
                            Load_Extensions();

                            //frmSkeletonEditor.OpenGLValid = true;
                        }
                    }
                }
            }

            return hRC;
        }

        public static void SetOGLContext(HDC hDC, HDC hRC)
        {
            wglMakeCurrent(hDC, hRC);
        }

        public static HDC GetOGLContext()
        {
            return wglGetCurrentContext();
        }

        public static void SetDefaultOGLRenderState()
        {

            glPolygonMode(GLFace.GL_FRONT_AND_BACK, GLPolygon.GL_FILL);
            glCullFace(GLFace.GL_FRONT);
            glEnable(GLCapability.GL_CULL_FACE);

            glDepthFunc(GLFunc.GL_LEQUAL);
            glEnable(GLCapability.GL_DEPTH_TEST);
            glDepthMask((byte)GL_Boolean.GL_TRUE);

            glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MIN_FILTER,
                            (float)GLTextureMagFilter.GL_LINEAR);
            glTexParameterf(GLTextureTarget.GL_TEXTURE_2D, GLTextureParameter.GL_TEXTURE_MAG_FILTER,
                            (float)GLTextureMagFilter.GL_LINEAR);

            // BLEND NONE
            SetBlendMode(BLEND_MODE.BLEND_DISABLED);

        }

        public static void DisableOpenGL(HDC hRC)
        {
            wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
            wglDeleteContext(hRC);
        }

        public static void ClearPanel()
        {
            //glClearColor(0.20f, 0.20f, 0.28f, 0);
            glClearColor(0.4f, 0.4f, 0.65f, 0);
            glClear(GLBufferMask.GL_COLOR_BUFFER_BIT | GLBufferMask.GL_DEPTH_BUFFER_BIT);
        }



        // -----------------------------------------------------------------------------
        // --------------------------COORDINATES CONVERSIONS----------------------------
        // -----------------------------------------------------------------------------
        public static Point3D GetUnProjectedCoords(Point3D p)
        {
            double[] mm = new double[16];
            double[] pm = new double[16];
            int[] vp = new int[4];

            double xIn, yIn, zIn;
            double tmpX = 0, tmpY = 0, tmpZ = 0;

            xIn = p.x;
            yIn = p.y;
            zIn = p.z;

            glGetDoublev((uint)GLCapability.GL_MODELVIEW_MATRIX, mm);
            glGetDoublev((uint)GLCapability.GL_PROJECTION_MATRIX, pm);
            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);

            gluUnProject(xIn, vp[3] - yIn, zIn, mm, pm, vp, ref tmpX, ref tmpY, ref tmpZ);

            return new Point3D((float)tmpX, (float)tmpY, (float)tmpZ);
        }

        public static Point3D GetProjectedCoords(Point3D p)
        {
            double[] mm = new double[16];
            double[] pm = new double[16];
            int[] vp = new int[4];

            double xIn, yIn, zIn;
            double tmpX = 0, tmpY = 0, tmpZ = 0;

            xIn = p.x;
            yIn = p.y;
            zIn = p.z;

            glGetDoublev((uint)GLCapability.GL_MODELVIEW_MATRIX, mm);
            glGetDoublev((uint)GLCapability.GL_PROJECTION_MATRIX, pm);
            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);

            gluProject(xIn, yIn, zIn, mm, pm, vp, ref tmpX, ref tmpY, ref tmpZ);

            return new Point3D((float)tmpX, (float)tmpY, (float)tmpZ);
        }

        public static Point3D GetVertexProjectedCoords(Point3D[] lstVerts, int iVertIdx)
        {
            glClear(GLBufferMask.GL_DEPTH_BUFFER_BIT);
            return GetProjectedCoords(lstVerts[iVertIdx]);
        }

        public static float GetDepthZ(Point3D pUP3D)
        {
            return GetProjectedCoords(pUP3D).z;
        }

        public static Point3D GetEyeSpaceCoords(Point3D p)
        {
            double[] mm = new double[16];

            glGetDoublev((uint)GLCapability.GL_MODELVIEW_MATRIX, mm);

            return new Point3D((float)(p.x * mm[0] + p.y * mm[4] + p.z * mm[8] + mm[12]),
                               (float)(p.x * mm[1] + p.y * mm[5] + p.z * mm[9] + mm[13]),
                               (float)(p.x * mm[2] + p.y * mm[6] + p.z * mm[10] + mm[14]));

        }

        public static Color GetVertColor(Point3D p, Point3D n, Color c)
        {
            byte[] pcolor = new byte[4];
            int[] vp0 = new int[4];
            int[] vp = new int[4];
            //double[] P_matrix = new double[16];
            Color colGetVertColorResult;

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp0);
            glViewport(0, 0, 3, 3);

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPushMatrix();

            //  Debug.Print p.X, p.Y, p.z, .X, .Y, c.r, c.g, c.b
            //glGetDoublev((uint)GLCapability.GL_PROJECTION_MATRIX, P_matrix);
            //glLoadIdentity();
            //gluPickMatrix(GetProjectedCoords(p).x - 1, GetProjectedCoords(p).y - 1, 3, 3, vp);
            //glMultMatrixd(P_matrix);

            glClear(GLBufferMask.GL_COLOR_BUFFER_BIT | GLBufferMask.GL_DEPTH_BUFFER_BIT);

            glPointSize(100.0f);

            glBegin(GLDrawMode.GL_POINTS);
                glColor4f(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, 1.0f);
                glColorMaterial(GLFace.GL_FRONT_AND_BACK, GLMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                glNormal3f(n.x, n.y, n.z);

                glVertex3f(p.x, p.y, p.z);
            glEnd();

            glFlush();
            glReadBuffer(GLBuffer.GL_BACK);

            glReadPixels(1, 1, 1, 1, GLPixelFormat.GL_RGB, GLPixelDataType.GL_BYTE, pcolor);

            colGetVertColorResult = Color.FromArgb(255, pcolor[0] * 2, pcolor[1] * 2, pcolor[2] * 2);

            glPopMatrix();
            glViewport(vp0[0], vp0[1], vp0[2], vp0[3]);

            return colGetVertColorResult;
        }

        public static void SetBlendMode(BLEND_MODE bmMode)
        {

            if (bmMode == BLEND_MODE.BLEND_DISABLED)
            {
                glDisable(GLCapability.GL_BLEND);
            }
            else
            {
                glEnable(GLCapability.GL_BLEND);
                glBlendEquation(GLBlendEquationMode.GL_FUNC_ADD);

                switch (bmMode)
                {
                    case BLEND_MODE.BLEND_AVG:
                        glBlendFunc(GLBlendFuncFactor.GL_SRC_ALPHA, GLBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);
                        //glBlendFunc((GLBlendFuncFactor)Enum.GetValues(typeof(GLBlendFuncFactor)).GetValue(iSrcBlend),
                        //            (GLBlendFuncFactor)Enum.GetValues(typeof(GLBlendFuncFactor)).GetValue(iDestBlend));

                        break;

                    case BLEND_MODE.BLEND_ADD:
                        glBlendFunc(GLBlendFuncFactor.GL_ONE, GLBlendFuncFactor.GL_ONE);

                        break;

                    case BLEND_MODE.BLEND_SUB:
                        glBlendFunc(GLBlendFuncFactor.GL_ONE, GLBlendFuncFactor.GL_ONE);
                        glBlendEquation(GLBlendEquationMode.GL_FUNC_REVERSE_SUBTRACT);

                        break;

                    case BLEND_MODE.BLEND_25P:
                        glBlendFunc(GLBlendFuncFactor.GL_SRC_ALPHA, GLBlendFuncFactor.GL_ONE);

                        break;

                    case BLEND_MODE.BLEND_NONE:
                        // For internal (vanilla)
                        // If needed we will change this to external
                        //glBlendEquation(GLBlendEquationMode.GL_FUNC_ADD);
                        glBlendFunc(GLBlendFuncFactor.GL_ONE, GLBlendFuncFactor.GL_ZERO);
                        // For external (hd textures)
                        //glBlendEquation(GLBlendEquationMode.GL_FUNC_ADD);
                        //glBlendFunc(GLBlendFuncFactor.GL_SRC_ALPHA, GLBlendFuncFactor.GL_ONE_MINUS_SRC_ALPHA);

                        break;

                }
            }



        }




        // HELP FUNCTIONS for C#
        //public static void TexImage2D(glTexture2DProxyTarget target, int level, GLInternalFormat internalformat, 
        //                              int width, int height, int border, GLPixelFormat format, glPixelDataType type, object pixels)
        //{
        //    GCHandle pp_pixels = GCHandle.Alloc(pixels, GCHandleType.Pinned);
        //    try
        //    {
        //        if (pglTexImage2D != null)
        //            pglTexImage2D(target, level, internalformat, width, height, border, format, type, pp_pixels.AddrOfPinnedObject());
        //        else
        //            throw new InvalidOperationException("binding point TexImage2D cannot be found");
        //    }
        //    finally
        //    {
        //        pp_pixels.Free();
        //    }
        //}
    }
}
