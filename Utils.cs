using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{

    using Defines;

    using static FrmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7PModel;

    using static FF7BattleSkeleton;

    using static OpenGL32;

    public class Utils
    {
        public struct Point2D
        {
            public float x;
            public float y;

            public Point2D(float in_x, float in_y)
            {
                x = in_x;
                y = in_y;
            }
        }

        public struct Point3D
        {
            public float x;
            public float y;
            public float z;

            public Point3D(float in_x, float in_y, float in_z)
            {
                x = in_x;
                y = in_y;
                z = in_z;
            }
        }

        public struct OrderPair
        {
            public float d;
        }

        public struct STIntVector
        {
            public int length;
            public int[] vector;
        }

        public struct Quaternion
        {
            public double x;
            public double y;
            public double z;
            public double w;
        }


        //  This is for PEditor
        public struct PairIB
        {
            public int I;
            public float B;
        }


        public const double PI = 3.14159265358979;

        public const double PIOVER180 = PI / 180;
        public const double QUAT_NORM_TOLERANCE = 0.00001;
        public const double INFINITY_SINGLE = 3.4028234E+38;

        public const double EulRepYes = 1;
        public const double EulParOdd = 1;
        public const double EulFrmR = 1;
        public const float FLT_EPSILON = 1.192092896e-07f;
        public const float MAX_DELTA_SQUARED = 0.001f * 0.001f;

        public const double PI_180 = 3.141593 / 180;

        //private int[] Onbits = new int[32];

        public static string strGlobalExceptionMessage;

        // Helper Functions
        public static bool IsNumeric(string val) => Int32.TryParse(val, out int _);

        public static void BuildQuaternionFromAxis(ref Point3D vec, double angle, ref Quaternion res_quat)
        {
            double sinAngle;
            angle = angle * PIOVER180 / 2;

            sinAngle = Math.Sin(angle);

            res_quat.x = vec.x * sinAngle;
            res_quat.y = vec.y * sinAngle;
            res_quat.z = vec.z * sinAngle;
            res_quat.w = Math.Cos(angle);
        }

        public static void MultiplyQuaternions(Quaternion quat_a, Quaternion quat_b, ref Quaternion res_quat)
        {
            res_quat.x = quat_a.w * quat_b.x + quat_a.x * quat_b.w + quat_a.y * quat_b.z - quat_a.z * quat_b.y;
            res_quat.y = quat_a.w * quat_b.y + quat_a.y * quat_b.w + quat_a.z * quat_b.x - quat_a.x * quat_b.z;
            res_quat.z = quat_a.w * quat_b.z + quat_a.z * quat_b.w + quat_a.x * quat_b.y - quat_a.y * quat_b.x;
            res_quat.w = quat_a.w * quat_b.w - quat_a.x * quat_b.x - quat_a.y * quat_b.y - quat_a.z * quat_b.z;
        }

        //  Convert Quaternion to Matrix
        public static void BuildMatrixFromQuaternion(Quaternion quat, ref double[] mat_res)
        {
            double x2, y2, z2;
            double xy, xz, yz;
            double wx, wy, wz;

            x2 = quat.x * quat.x;
            y2 = quat.y * quat.y;
            z2 = quat.z * quat.z;

            xy = quat.x * quat.y;
            xz = quat.x * quat.z;
            yz = quat.y * quat.z;

            wx = quat.w * quat.x;
            wy = quat.w * quat.y;
            wz = quat.w * quat.z;

            //  This calculation would be a lot more complicated for non-unit length quaternions
            //  Note: The constructor of Matrix4 expects the Matrix in column-major format like expected by
            //  OpenGL
            mat_res[0] = 1 - 2 * (y2 + z2);
            mat_res[4] = 2 * (xy - wz);
            mat_res[8] = 2 * (xz + wy);
            mat_res[12] = 0;
            mat_res[1] = 2 * (xy + wz);
            mat_res[5] = 1 - 2 * (x2 + z2);
            mat_res[9] = 2 * (yz - wx);
            mat_res[13] = 0;
            mat_res[2] = 2 * (xz - wy);
            mat_res[6] = 2 * (yz + wx);
            mat_res[10] = 1 - 2 * (x2 + y2);
            mat_res[14] = 0;
            mat_res[3] = 0;
            mat_res[7] = 0;
            mat_res[11] = 0;
            mat_res[15] = 1;
        }

        public static void BuildRotationMatrixWithQuaternions(double alpha, double beta, double gamma, ref double[] mat_res)
        {
            Quaternion quat_x = new Quaternion();
            Quaternion quat_y = new Quaternion();
            Quaternion quat_z = new Quaternion();
            Quaternion quat_xy = new Quaternion();
            Quaternion quat_xyz = new Quaternion();

            Point3D px = new Point3D(1, 0, 0);
            Point3D py = new Point3D(0, 1, 0);
            Point3D pz = new Point3D(0, 0, 1);

            BuildQuaternionFromAxis(ref px, alpha, ref quat_x);
            BuildQuaternionFromAxis(ref py, beta, ref quat_y);
            BuildQuaternionFromAxis(ref pz, gamma, ref quat_z);

            MultiplyQuaternions(quat_y, quat_x, ref quat_xy);
            MultiplyQuaternions(quat_xy, quat_z, ref quat_xyz);

            BuildMatrixFromQuaternion(quat_xyz, ref mat_res);
        }


        public static void MultiplyPoint3DByOGLMatrix(double[] matA, Point3D p_in, ref Point3D p_out)
        {
            p_out.x = (float)(p_in.x * matA[0] + p_in.y * matA[4] + p_in.z * matA[8] + matA[12]);
            p_out.y = (float)(p_in.x * matA[1] + p_in.y * matA[5] + p_in.z * matA[9] + matA[13]);
            p_out.z = (float)(p_in.x * matA[2] + p_in.y * matA[6] + p_in.z * matA[10] + matA[14]);
        }

        public static void ComputeTransformedBoxBoundingBox(double[] MV_matrix, ref Point3D p_min, ref Point3D p_max,
                                                            ref Point3D p_min_trans, ref Point3D p_max_trans)
        {

            Point3D[] box_pointsV = new Point3D[8];
            Point3D p_aux_trans = new Point3D();
            int iBoxPoints;

            p_max_trans.x = -(float)INFINITY_SINGLE;
            p_max_trans.y = -(float)INFINITY_SINGLE;
            p_max_trans.z = -(float)INFINITY_SINGLE;

            p_min_trans.x = (float)INFINITY_SINGLE;
            p_min_trans.y = (float)INFINITY_SINGLE;
            p_min_trans.z = (float)INFINITY_SINGLE;

            box_pointsV[0] = p_min;

            box_pointsV[1].x = p_min.x;
            box_pointsV[1].y = p_min.y;
            box_pointsV[1].z = p_max.z;

            box_pointsV[2].x = p_min.x;
            box_pointsV[2].y = p_max.y;
            box_pointsV[2].z = p_min.z;

            box_pointsV[3].x = p_min.x;
            box_pointsV[3].y = p_max.y;
            box_pointsV[3].z = p_max.z;

            box_pointsV[4] = p_max;

            box_pointsV[5].x = p_max.x;
            box_pointsV[5].y = p_max.y;
            box_pointsV[5].z = p_min.z;

            box_pointsV[6].x = p_max.x;
            box_pointsV[6].y = p_min.y;
            box_pointsV[6].z = p_max.z;

            box_pointsV[7].x = p_max.x;
            box_pointsV[7].y = p_min.y;
            box_pointsV[7].z = p_min.z;

            for (iBoxPoints = 0; iBoxPoints < 8; iBoxPoints++)
            {
                MultiplyPoint3DByOGLMatrix(MV_matrix, box_pointsV[iBoxPoints], ref p_aux_trans);

                if (p_max_trans.x < p_aux_trans.x) p_max_trans.x = p_aux_trans.x;
                if (p_max_trans.y < p_aux_trans.y) p_max_trans.y = p_aux_trans.y;
                if (p_max_trans.z < p_aux_trans.z) p_max_trans.z = p_aux_trans.z;

                if (p_min_trans.x > p_aux_trans.x) p_min_trans.x = p_aux_trans.x;
                if (p_min_trans.y > p_aux_trans.y) p_min_trans.y = p_aux_trans.y;
                if (p_min_trans.z > p_aux_trans.z) p_min_trans.z = p_aux_trans.z;
            }
        }

        public static void BuildRotationMatrixWithQuaternionsXYZ(double alpha, double beta, double gamma, ref double[] mat_res)
        {
            Quaternion quat_x = new Quaternion();
            Quaternion quat_y = new Quaternion();
            Quaternion quat_z = new Quaternion();
            Quaternion quat_xy = new Quaternion();
            Quaternion quat_xyz = new Quaternion();
            
            Point3D px = new Point3D(1, 0, 0);
            Point3D py = new Point3D(0, 1, 0);
            Point3D pz = new Point3D(0, 0, 1);

            BuildQuaternionFromAxis(ref px, alpha, ref quat_x);
            BuildQuaternionFromAxis(ref py, beta, ref quat_y);
            BuildQuaternionFromAxis(ref pz, gamma, ref quat_z);

            MultiplyQuaternions(quat_x, quat_y, ref quat_xy);
            MultiplyQuaternions(quat_xy, quat_z, ref quat_xyz);

            BuildMatrixFromQuaternion(quat_xyz, ref mat_res);
        }

        public static Quaternion GetQuaternionFromEulerUniversal(double y, double x, double z, int i, int j, int k, int n, int s, int f)
        {
            double[] a = new double[3];
            double ti, tj, th, ci, cj, ch, si, sj, sh, cc, cs, sc, ss;

            double t;

            Quaternion quat_GetQuaternionFromEulerUniversalResult = new Quaternion();

            if (f == EulFrmR)
            {
                t = x;
                x = z;
                z = t;
            }

            if (n == EulParOdd) y = -y;

            ti = x * 0.5;
            tj = y * 0.5;
            th = z * 0.5;
            ci = Math.Cos(ti);
            cj = Math.Cos(tj);
            ch = Math.Cos(th);
            si = Math.Sin(ti);
            sj = Math.Sin(tj);
            sh = Math.Sin(th);
            cc = ci * ch;
            cs = ci * sh;
            sc = si * ch;
            ss = si * sh;

            if (s == EulRepYes)
            {
                a[i] = cj * (cs + sc); // Could speed up with trig identities.
                a[j] = sj * (cc + ss);
                a[k] = sj * (cs - sc);
                quat_GetQuaternionFromEulerUniversalResult.w = cj * (cc - ss);
            }
            else
            {
                a[i] = cj * sc - sj * cs; // Could speed up with trig identities.
                a[j] = cj * ss + sj * cc;
                a[k] = cj * cs - sj * sc;
                quat_GetQuaternionFromEulerUniversalResult.w = cj * cc + sj * ss;
            }

            if (n == EulParOdd) a[j] = -a[j];

            quat_GetQuaternionFromEulerUniversalResult.x = a[0];
            quat_GetQuaternionFromEulerUniversalResult.y = a[1];
            quat_GetQuaternionFromEulerUniversalResult.z = a[2];

            return quat_GetQuaternionFromEulerUniversalResult;
        }

        public static double QuaternionsDot(ref Quaternion q1, ref Quaternion q2)
        {
            return q1.x * q2.x + q1.y * q2.y + q1.z * q2.z + q1.w * q2.w;
        }

        public static void NormalizeQuaternion(ref Quaternion quat)
        {
            // Don't normalize if we don't have to
            double mag, mag2;

            mag2 = quat.w * quat.w + quat.x * quat.x + quat.y * quat.y + quat.z * quat.z;

            mag = Math.Sqrt(mag2);

            quat.w /= mag;
            quat.x /= mag;
            quat.y /= mag;
            quat.z /= mag;

            //        NEW UDPATE vertex2995 fix for Hojo/Heidegger animations (by L@Zar0)
            //        If Abs(mag2 - 1#) > QUAT_NORM_TOLERANCE Then
            //            mag = Sqr(mag2)
            //            .w = .w / mag
            //            .x = .x / mag
            //            .y = .y / mag
            //            .z = .z / mag
            //        End If

            //        If .w > 1# Then
            //            .w = 1
            //        End If
        }

        public static Quaternion QuaternionLerp(ref Quaternion q1, ref Quaternion q2, double t)
        {
            double one_minus_t;
            Quaternion quat_QuaternionLerpResult = new Quaternion();

            one_minus_t = 1f - t;

            quat_QuaternionLerpResult.x = q1.x * one_minus_t + q2.x * t;
            quat_QuaternionLerpResult.y = q1.y * one_minus_t + q2.y * t;
            quat_QuaternionLerpResult.z = q1.z * one_minus_t + q2.z * t;
            quat_QuaternionLerpResult.w = q1.w * one_minus_t + q2.w * t;

            NormalizeQuaternion(ref quat_QuaternionLerpResult);

            return quat_QuaternionLerpResult;
        }

        public static Quaternion QuaternionSlerp2(ref Quaternion q1, ref Quaternion q2, double t)
        {
            Quaternion q3 = new Quaternion();
            Quaternion quat_QuaternionSlerp2Result = new Quaternion();

            double dot, angle, one_minus_t, sin_angle, sin_angle_by_t, sin_angle_by_one_t;

            dot = QuaternionsDot(ref q1, ref q2);
            //    dot = cos(theta)
            //    if (dot < 0), q1 and q2 are more than 90 degrees apart,
            //    so we can invert one to reduce spinning
            if (dot < 0)
            {
                dot = -dot;
                q3.x = -q2.x;
                q3.y = -q2.y;
                q3.z = -q2.z;
                q3.w = -q2.w;
            }
            else
            {
                q3.x = q2.x;
                q3.y = q2.y;
                q3.z = q2.z;
                q3.w = q2.w;
            }

            if (dot < 0.95)
            {
                angle = Math.Acos(dot);
                one_minus_t = 1f - t;
                sin_angle = Math.Sin(angle);
                sin_angle_by_t = Math.Sin(angle * t);
                sin_angle_by_one_t = Math.Sin(angle * one_minus_t);

                quat_QuaternionSlerp2Result.x = ((q1.x * sin_angle_by_one_t) + q3.x * sin_angle_by_t) / sin_angle;
                quat_QuaternionSlerp2Result.y = ((q1.y * sin_angle_by_one_t) + q3.y * sin_angle_by_t) / sin_angle;
                quat_QuaternionSlerp2Result.z = ((q1.z * sin_angle_by_one_t) + q3.z * sin_angle_by_t) / sin_angle;
                quat_QuaternionSlerp2Result.w = ((q1.w * sin_angle_by_one_t) + q3.w * sin_angle_by_t) / sin_angle;
            }
            else
            {
                quat_QuaternionSlerp2Result = QuaternionLerp(ref q1, ref q3, t);
            }

            return quat_QuaternionSlerp2Result;
        }

        public static double DegToRad(double x)
        {
            return x * PI / 180f;
        }

        public static double RadToDeg(double x)
        {
            return x * 180f / PI;
        }

        public static Quaternion GetQuaternionFromEulerXYZr(double x, double y, double z)
        {
            return GetQuaternionFromEulerUniversal(DegToRad(x), DegToRad(y), DegToRad(z), 2, 1, 0, 1, 0, 1);
        }

        public static Quaternion GetQuaternionFromEulerYXZr(double x, double y, double z)
        {
            return GetQuaternionFromEulerUniversal(DegToRad(x), DegToRad(y), DegToRad(z), 2, 0, 1, 0, 0, 1);
        }

        public static Point3D GetEulerFormMatrixUniversal(double[] mat, int i, int j, int k, int n, int s, int f)
        {
            double sy, cy, t;
            Point3D up3DGetEulerFormMatrixUniversalResult = new Point3D();

            if (s == EulRepYes)
            {
                sy = Math.Sqrt(mat[i + 4 * j] * mat[i + 4 * j] + mat[i + 4 * k] * mat[i + 4 * k]);
                if (sy > 16f * FLT_EPSILON)
                {
                    up3DGetEulerFormMatrixUniversalResult.x = (float)Math.Atan2(mat[i + 4 * j], mat[i + 4 * k]);
                    up3DGetEulerFormMatrixUniversalResult.y = (float)Math.Atan2(sy, mat[i + 4 * i]);
                    up3DGetEulerFormMatrixUniversalResult.z = (float)Math.Atan2(mat[j + 4 * i], -mat[k + 4 * i]);
                }
                else
                {
                    up3DGetEulerFormMatrixUniversalResult.x = (float)Math.Atan2(-mat[j + 4 * k], mat[j + 4 * j]);
                    up3DGetEulerFormMatrixUniversalResult.y = (float)Math.Atan2(sy, mat[i + 4 * i]);
                    up3DGetEulerFormMatrixUniversalResult.z = 0;
                }
            }
            else
            {
                cy = Math.Sqrt(mat[i + 4 * i] * mat[i + 4 * i] + mat[j + 4 * i] * mat[j + 4 * i]);
                if (cy >16f * FLT_EPSILON)
                {
                    up3DGetEulerFormMatrixUniversalResult.x = (float)Math.Atan2(mat[k + 4 * j], mat[k + 4 * k]);
                    up3DGetEulerFormMatrixUniversalResult.y = (float)Math.Atan2(-mat[k + 4 * i], cy);
                    up3DGetEulerFormMatrixUniversalResult.z = (float)Math.Atan2(mat[j + 4 * i], mat[i + 4 * i]);
                }
                else
                {
                    up3DGetEulerFormMatrixUniversalResult.x = (float)Math.Atan2(-mat[j + 4 * k], mat[j + 4 * j]);
                    up3DGetEulerFormMatrixUniversalResult.y = (float)Math.Atan2(-mat[k + 4 * i], cy);
                    up3DGetEulerFormMatrixUniversalResult.z = 0;
                }
            }

            if (n == EulParOdd)
            {
                up3DGetEulerFormMatrixUniversalResult.x = -up3DGetEulerFormMatrixUniversalResult.x;
                up3DGetEulerFormMatrixUniversalResult.y = -up3DGetEulerFormMatrixUniversalResult.y;
                up3DGetEulerFormMatrixUniversalResult.z = -up3DGetEulerFormMatrixUniversalResult.z;
            }

            if (f == EulFrmR)
            {
                t = up3DGetEulerFormMatrixUniversalResult.x;
                up3DGetEulerFormMatrixUniversalResult.x = up3DGetEulerFormMatrixUniversalResult.z;
                up3DGetEulerFormMatrixUniversalResult.z = (float)t;
            }

            up3DGetEulerFormMatrixUniversalResult.x = (float)RadToDeg(up3DGetEulerFormMatrixUniversalResult.x);
            up3DGetEulerFormMatrixUniversalResult.y = (float)RadToDeg(up3DGetEulerFormMatrixUniversalResult.y);
            up3DGetEulerFormMatrixUniversalResult.z = (float)RadToDeg(up3DGetEulerFormMatrixUniversalResult.z);

            return up3DGetEulerFormMatrixUniversalResult;
        }

        public static Point3D GetEulerXYZrFromMatrix(double[] mat)
        {
            return GetEulerFormMatrixUniversal(mat, 2, 1, 0, 1, 0, 1);
        }

        public static Point3D GetEulerYXZrFromMatrix(double[] mat)
        {
            return GetEulerFormMatrixUniversal(mat, 2, 0, 1, 0, 0, 1);
        }

        public static Quaternion GetQuaternionConjugate(ref Quaternion quat)
        {
            Quaternion quat_GetQuaternionConjugateResult = new Quaternion()
            {
                x = -quat.x,
                y = -quat.y,
                z = -quat.z,
                w = quat.w,
            };

            return quat_GetQuaternionConjugateResult;
        }

        //  Convert from Euler Angles
        public static void BuildQuaternionFromEuler(double alpha, double beta, double gamma, ref Quaternion res_quat)
        {
            //  Basically we create 3 Quaternions, one for pitch, one for yaw, one for roll
            //  and multiply those together.

            Quaternion quat_x = new Quaternion();
            Quaternion quat_y = new Quaternion();
            Quaternion quat_z = new Quaternion();
            Quaternion quat_xy = new Quaternion();

            Point3D px = new Point3D(1, 0, 0);
            Point3D py = new Point3D(0, 1, 0);
            Point3D pz = new Point3D(0, 0, 1);

            BuildQuaternionFromAxis(ref px, alpha, ref quat_x);
            BuildQuaternionFromAxis(ref py, beta, ref quat_y);
            BuildQuaternionFromAxis(ref pz, gamma, ref quat_z);

            MultiplyQuaternions(quat_y, quat_x, ref quat_xy);
            MultiplyQuaternions(quat_xy, quat_z, ref res_quat);

            NormalizeQuaternion(ref res_quat);
        }


        ///////////////////////////////////////////
        // Camera things
        public static void ConcatenateCameraModelView(float cX, float cY, float cZ,
                                                      float alpha, float beta, float gamma,
                                                      float rszX, float rszY, float rszZ)
        {
            double[] rot_mat = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            //glLoadIdentity();
            glTranslatef(cX, cY, cZ);

            BuildRotationMatrixWithQuaternionsXYZ(alpha, beta, gamma, ref rot_mat);

            glMultMatrixd(rot_mat);
            glScalef(rszX, rszY, rszZ);
        }

        public static void ConcatenateCameraModelViewQuat(float cX, float cY, float cZ,
                                                          Quaternion quat,
                                                          float rszX, float rszY, float rszZ)
        {
            double[] rot_mat = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glLoadIdentity();
            glTranslatef(cX, cY, cZ);

            BuildMatrixFromQuaternion(quat, ref rot_mat);

            glMultMatrixd(rot_mat);
            glScalef(rszX, rszY, rszZ);
        }

        public static void SetCameraModelView(float cX, float cY, float cZ, 
                                              float alpha, float beta, float gamma,
                                              float rszX, float rszY, float rszZ)
        {
            double[] rot_mat = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glLoadIdentity();

            glTranslatef(cX, cY, cZ);

            BuildRotationMatrixWithQuaternionsXYZ(alpha, beta, gamma, ref rot_mat);

            glMultMatrixd(rot_mat);

            glScalef(rszX, rszY, rszZ);
        }

        public static void SetCameraModelViewQuat(float cX, float cY, float cZ,
                                                  Quaternion quat,
                                                  float rszX, float rszY, float rszZ)
        {
            double[] rot_mat = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glLoadIdentity();

            glTranslatef(cX, cY, cZ);

            BuildMatrixFromQuaternion(quat, ref rot_mat);

            glMultMatrixd(rot_mat);

            glScalef(rszX, rszY, rszZ);
        }

        public static void SetCameraPModel(PModel Model, float cX, float cY, float cZ,
                                           float alpha, float beta, float gamma,
                                           float rszX, float rszY, float rszZ)
        {

            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();
            Point3D center_model, origin;

            int width, height;

            float model_radius, distance_origin, scene_radius;
            int[] vp = new int[4];

            ComputePModelBoundingBox(Model, ref p_min, ref p_max);

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glLoadIdentity();

            center_model = new Point3D((p_min.x + p_max.x) / 2,
                                       (p_min.y + p_max.y) / 2,
                                       (p_min.z + p_max.z) / 2);

            origin = new Point3D();

            model_radius = CalculateDistance(p_min, p_max) / 2;
            distance_origin = CalculateDistance(center_model, origin);
            scene_radius = model_radius + distance_origin;
            gluPerspective(60, (float)width / height, Math.Max(0.1, -cZ - scene_radius), Math.Max(0.1, -cZ + scene_radius));

            SetCameraModelView(cX, cY, cZ, alpha, beta, gamma, rszX, rszY, rszZ);
        }


        public static void SetCameraAroundModel(ref Point3D p_min, ref Point3D p_max, 
                                                float cX, float cY, float cZ, 
                                                float alpha, float beta, float gamma, 
                                                float rszX, float rszY, float rszZ)
        {
            float width, height;
            float scene_radius;
            int[] vp = new int[4];

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glLoadIdentity();

            scene_radius = ComputeSceneRadius(p_min, p_max);

            gluPerspective(60, (float)width / height, Math.Max(0.1, -cZ - scene_radius), Math.Max(0.1, -cZ + scene_radius));

            SetCameraModelView(cX, cY, cZ, alpha, beta, gamma, rszX, rszY, rszZ);
        }

        public static void SetCameraAroundModelQuat(ref Point3D p_min, ref Point3D p_max,
                                                    float cX, float cY, float cZ,
                                                    Quaternion quat,
                                                    float rszX, float rszY, float rszZ)
        {
            float width, height;
            float scene_radius;
            int[] vp = new int[4];

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glLoadIdentity();

            scene_radius = ComputeSceneRadius(p_min, p_max);

            gluPerspective(60, (float)width / height, Math.Max(0.1, -cZ - scene_radius), Math.Max(0.1, -cZ + scene_radius));

            SetCameraModelViewQuat(cX, cY, cZ, quat, rszX, rszY, rszZ);
        }

        public static bool IsCameraUnderGround()
        {
            Point3D origin = new Point3D();
            Point3D originTrans = new Point3D();
            double[] MV_matrix = new double[16];

            glGetDoublev((uint)GLCapability.GL_MODELVIEW_MATRIX, MV_matrix);

            InvertMatrix(ref MV_matrix);

            MultiplyPoint3DByOGLMatrix(MV_matrix, origin, ref originTrans);

            return originTrans.y > -1;
        }

        public static void ResetCamera(ref double alpha, ref double beta, ref double gamma,
                                       ref float panX, ref float panY, ref float panZ,
                                       ref double DIST)
        {
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            //int animIndex;

            if (bLoaded)
            {
                switch (modelType)
                {
                    case K_HRC_SKELETON:
                        ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[iCurrentFrameScroll],
                                                ref p_min, ref p_max);
                        break;

                    case K_AA_SKELETON:
                    case K_MAGIC_SKELETON:
                        //if (!bSkeleton.IsBattleLocation)
                        //{
                        ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                                 ref p_min, ref p_max);
                        //}
                        break;

                    case K_P_FIELD_MODEL:
                    case K_P_BATTLE_MODEL:
                    case K_P_MAGIC_MODEL:
                    case K_3DS_MODEL:
                        ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                        break;
                }

                alpha = 200;
                beta = 45;
                gamma = 0;
                panX = 0;
                panY = 0;
                panZ = 0;
                DIST = -2 * ComputeSceneRadius(p_min, p_max);
            }
        }



        ///////////////////////////////////////////
        // Geometric
        public static float CalculateLength3D(Point3D v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        }

        public static Point3D AddPoint3D(Point3D v1, Point3D v2)
        {
            return new Point3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Point3D SubstractPoint3D(Point3D v1, Point3D v2)
        {
            return new Point3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static float DotProduct3D(Point3D v1, Point3D v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static Point3D CrossProduct3D(Point3D v1, Point3D v2)
        {
            return new Point3D(v1.y * v2.z - v1.z * v2.y,
                               v1.z * v2.x - v1.x * v2.z,
                               v1.x * v2.y - v1.y * v2.x);
        }

        public static Point3D DividePoint3D(Point3D v, float fScalar)
        {
            return new Point3D(v.x / fScalar, v.y / fScalar, v.z / fScalar);
        }

        public static float CalculateAngle2Vectors3D(Point3D v1, Point3D v2)
        {
            double dAngleRadians;
            float fCalculateAngle2Vectors3DResult;

            dAngleRadians = Math.Acos(DotProduct3D(v1, v2) / (CalculateLength3D(v1) * CalculateLength3D(v2)));

            fCalculateAngle2Vectors3DResult = (float)RadToDeg(dAngleRadians);

            if (float.IsNaN(fCalculateAngle2Vectors3DResult) ||
                float.IsInfinity(fCalculateAngle2Vectors3DResult))
                    fCalculateAngle2Vectors3DResult = 0;

            return fCalculateAngle2Vectors3DResult;
        }

        public static float CalculateAreaPoly3D(Point3D v0, Point3D v1, Point3D v2)
        {
            float a = CalculateDistance(v0, v1);
            float b = CalculateDistance(v1, v2);
            float c = CalculateDistance(v2, v0);
            float s = (a + b + c) / 2;

            return (float)Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        public static Point3D Normalize(Point3D v)
        {
            float fLength;

            fLength = CalculateLength3D(v);

            return DividePoint3D(v, fLength);


            //fLength = CalculateLength3D(v);

            //if (fLength > 0)
            //{
            //    fLength = 1 / fLength;

            //    return new Point3D(v.x / fLength, v.y / fLength, v.z / fLength);
            //}

            //else return new Point3D(0.0f, 0.0f, 0.0f);
        }

        public static float CalculateDistance(Point3D v0, Point3D v1)
        {
            float fDeltaX = v1.x - v0.x;
            float fDeltaY = v1.y - v0.y;
            float fDeltaZ = v1.z - v0.z;

            return (float)Math.Sqrt(fDeltaX * fDeltaX + fDeltaY * fDeltaY + fDeltaZ * fDeltaZ);
        }

        public static float ComputeSceneRadius(Point3D p_min, Point3D p_max)
        {
            float model_radius, distance_origin;

            Point3D center_model = new Point3D((p_min.x + p_max.x) / 2.0f,
                                               (p_min.y + p_max.y) / 2.0f,
                                               (p_min.z + p_max.z) / 2.0f);

            Point3D origin = new Point3D(0, 0, 0);

            model_radius = CalculateDistance(p_min, p_max) / 2;
            distance_origin = CalculateDistance(center_model, origin);

            return model_radius + distance_origin;
        }

        public static Point3D CalculateNormal(Point3D p1, Point3D p2, Point3D p3)
        {
            Point3D v1, v2;

            v1 = SubstractPoint3D(p2, p1);
            v2 = SubstractPoint3D(p3, p1);

            return CrossProduct3D(v1, v2);
        }

        public static bool ComparePoints3D(Point3D a, Point3D b)
        {
            return (a.x == b.x) && (a.y == b.y) && (a.z == b.z);
        }

        public static Point3D CalculateCenteroid(Point3D p1, Point3D p2, Point3D p3)
        {
            return new Point3D((p1.x + p2.x + p3.x) / 3.0f,
                               (p1.y + p2.y + p3.y) / 3.0f,
                               (p1.z + p2.z + p3.z) / 3.0f);
        }

         
        ///////////////////////////////////////////
        // Maths
        public static bool CompareLongs(long val1, long val2)
        {
            if ((val1 ^ val2) < 0) return val1 < 0;
            else return val1 > val2;
        }

        public static void GetSubMatrix(double[] mat, int i, int j, ref double[] mat_out)
        {
            int i2, j2, order, pos;

            order = (int)Math.Sqrt(mat.Length);

            mat_out = new double[(int)Math.Pow(order - 1, 2)];

            for (i2 = 0; i2 < order; i2++)
            {
                if (i2 != i)
                {
                    for (j2 = 0; j2 < order; j2++)
                    {
                        if (j2 != j)
                        {
                            pos = i2 + j2 * (order - 1);
                            if (i2 > i) pos--;
                            if (j2 > j) pos = pos - order + 1;
                            mat_out[pos] = mat[i2 + j2 * order];
                        }
                    }
                }
            }
        }

        public static double GetMatrixDeterminant(ref double[] mat)
        {
            double iGetMatrixDeterminantResult = 0;

            int i, order;
            double det_aux;
            double[] mat_aux = null;

            order = (int)Math.Sqrt(mat.Length);

            if (order > 2)
            {
                for (i = 0; i < order; i++)
                {
                    if (mat[i] != 0)
                    {
                        GetSubMatrix(mat, i, 0, ref mat_aux);
                        det_aux = GetMatrixDeterminant(ref mat_aux) * Math.Pow(-1, i) * mat[i];
                        iGetMatrixDeterminantResult += det_aux;
                    }
                }
            }
            else
            {
                iGetMatrixDeterminantResult = mat[0] * mat[3] - mat[1] * mat[2];
            }

            return iGetMatrixDeterminantResult;
        }

        public static void GetAtachedMatrix(double[] mat, ref double[] mat_out)
        {
            int i, j, order;
            double[] mat_aux = null;

            order = (int)Math.Sqrt(mat.Length);

            mat_out = new double[(int)Math.Pow(order, 2)];

            for (i =  0; i < order; i++)
            {
                for (j = 0; j < order; j++)
                {
                    GetSubMatrix(mat, i, j, ref mat_aux);
                    mat_out[i + j * order] = Math.Pow(-1, i + j) * GetMatrixDeterminant(ref mat_aux);
                }
            }
        }

        public static void TransposeMatrix(ref double[] mat)
        {
            int i, j, order;
            double temp;

            order = (int)Math.Sqrt(mat.Length);

            for (i = 0; i < order; i++)
            {
                for (j = 0; j <= i; j++)
                {
                    temp = mat[i * order + j];
                    mat[i * order + j] = mat[i + j * order];
                    mat[i + j * order] = temp;
                }
            }
        }

        public static void InvertMatrix(ref double[] mat)
        {
            int i, j, order;
            double[] mat_aux = null;
            double det;

            order = (int)Math.Sqrt(mat.Length);
            det = GetMatrixDeterminant(ref mat);

            GetAtachedMatrix(mat, ref mat_aux);

            for (i = 0; i < order; i++)
            {
                for (j = 0; j < order; j++)
                {
                    mat[i + j * order] = mat_aux[i + j * order] / det;
                }
            }

            TransposeMatrix(ref mat);
        }

        //  The value is considered unsigned
        public static int GetBitBlockVUnsigned(byte[] vect, int nBits, ref int FBit)
        {
            int iGetBitBlockVUnsignedResult = 0;
            int baseByte, bi, res, nBytes, unalignedByBits, firstAlignedByte, lastAlignedByte, endBits;
            bool isAligned, cleanEnd;

            if (nBits > 0)
            {
                baseByte = FBit / 8;
                unalignedByBits = FBit % 8;

                if (unalignedByBits + nBits > 8)
                {
                    isAligned = (unalignedByBits == 0);

                    endBits = (FBit + nBits) % 8;
                    cleanEnd = (endBits == 0);

                    nBytes = (nBits - (isAligned ? 0 : 8 - unalignedByBits) - (cleanEnd ? 0 : endBits)) / 8 +
                             (isAligned ? 0 : 1) + (cleanEnd ? 0 : 1);
                    lastAlignedByte = nBytes - (cleanEnd ? 0 : 1) - 1;
                    firstAlignedByte = 0;

                    res = 0;
                    //  Unaligned prefix
                    //  Stored at the begining of the byte
                    if (!isAligned)
                    {
                        res = vect[baseByte];
                        res &= (int)(Math.Pow(2, (8 - unalignedByBits)) - 1);
                        firstAlignedByte = 1;
                    }

                    //  Aligned bytes
                    for (bi = firstAlignedByte; bi <= lastAlignedByte; bi++)
                    {
                        res *= 256;
                        res |= vect[baseByte + bi];
                    }

                    //  Sufix
                    //  Stored at the end of the byte
                    if (!cleanEnd)
                    {
                        res *= (int)Math.Pow(2, endBits);
                        res |= ((vect[baseByte + lastAlignedByte + 1]) / (int)(Math.Pow(2, 8 - endBits)) & (int)(Math.Pow(2, endBits) - 1));
                    }
                }
                else
                {
                    res = vect[baseByte];
                    res /= (int)Math.Pow(2, 8 - (unalignedByBits + nBits));
                    res &= (int)(Math.Pow(2, nBits) - 1);
                }

                iGetBitBlockVUnsignedResult = (short)res;

                FBit += nBits;
            }

            return iGetBitBlockVUnsignedResult;
        }

        public static int ExtendSignInteger(int val, int len)
        {
            int iExtendSignIntegerResult, auxRes;

            //KimeraCS VB6 has this lines but they don't seem to have any effect, right?
            //if (len != 12)
            //{
            //    auxRes = auxRes;
            //}

            if ((val & (int)Math.Pow(2, (len - 1))) != 0)
            {
                auxRes = (int)Math.Pow(2, 16) - 1;
                auxRes ^= (int)(Math.Pow(2, len) - 1);
                auxRes |= val;

                iExtendSignIntegerResult = auxRes;
            }
            else
            {
                iExtendSignIntegerResult = val;
            }

            return iExtendSignIntegerResult;
        }

        public static int GetSignExtendedShort(int src, int valLen)
        {
            int iGetSignExtendedShortResult = 0;

            if (valLen > 0)
            {
                if (valLen < 16)
                {
                    iGetSignExtendedShortResult = ExtendSignInteger(src, valLen);
                }
                else
                {
                    iGetSignExtendedShortResult = src;
                }
            }

            return iGetSignExtendedShortResult;
        }

        public static int GetBitBlockV(byte[] vect, int nBits, ref int FBit)
        {
            int tmpValue;

            tmpValue = GetBitBlockVUnsigned(vect, nBits, ref FBit);

            return GetSignExtendedShort(tmpValue, nBits);
        }

        public static void PutBitBlockV(ref byte[] vect, int nBits, ref int FBit, int iValue)
        {
            int bi, baseByte, nBytes, unalignedByBits;
            int firstAlignedByte, lastAlignedByte, endBits, tmpValue;
            bool isAligned, cleanEnd;

            //  Deal with it as some raw positive value.
            //  Divisions can't be used for bit shifting negative values,
            //  since they round towards 0 instead of minus infinity
            iValue &= (int)(Math.Pow(2, nBits) - 1);

            if (nBits > 0)
            {
                baseByte = FBit / 8;
                unalignedByBits = FBit % 8;

                if (unalignedByBits + nBits > 8)
                {
                    isAligned = (unalignedByBits == 0);

                    endBits = (FBit + nBits) % 8;
                    cleanEnd = (endBits == 0);

                    nBytes = (nBits - (isAligned ? 0 : (8 - unalignedByBits)) - (cleanEnd ? 0 : endBits)) / 8 + 
                             (isAligned ? 0 : 1) + (cleanEnd ? 0 : 1);

                    lastAlignedByte = nBytes - (cleanEnd ? 0 : 1) - 1;
                    firstAlignedByte = 0;

                    Array.Resize(ref vect, baseByte + nBytes);

                    //  Unaligned prefix
                    if (!isAligned)
                    {
                        tmpValue = iValue / (int)(Math.Pow(2, nBits - (8 - unalignedByBits)));
                        tmpValue &= ((int)(Math.Pow(2, (8 - unalignedByBits)) - 1));
                        vect[baseByte] = (byte)(vect[baseByte] | tmpValue);
                        firstAlignedByte = 1;
                    }

                    //  Aligned bytes
                    for (bi = firstAlignedByte; bi <= lastAlignedByte; bi++)
                    {
                        tmpValue = iValue / (int)(Math.Pow(2, ((lastAlignedByte - bi) * 8 + endBits)));
                        vect[baseByte + bi] = (byte)(tmpValue & 0xFF);
                    }

                    // Suffix
                    if (!cleanEnd)
                    {
                        tmpValue = iValue & (int)(Math.Pow(2, endBits) - 1);
                        vect[baseByte + lastAlignedByte + 1] = (byte)(tmpValue * (int)(Math.Pow(2, 8 - endBits)));                            
                    }
                }
                else
                {
                    if (vect.Length - 1 < baseByte)
                    {
                        Array.Resize(ref vect, baseByte + 1);
                        vect[baseByte] = 0;
                    }

                    tmpValue = iValue & (int)Math.Pow(2, nBits) - 1;
                    tmpValue *= (int)Math.Pow(2, 8 - (unalignedByBits + nBits));
                    vect[baseByte] = (byte)(vect[baseByte] | tmpValue);
                }
            }

            FBit += nBits;
        }

        public static float NormalizeAngle180(float fValue)
        {
            float fNormalizeAngle180Result;
            float fDec;

            if (fValue > 0) fDec = 360f;
            else fDec = -360f;

            fNormalizeAngle180Result = fValue;
            while ((fNormalizeAngle180Result > 0 && fValue > 0) || (fNormalizeAngle180Result < 0 && fValue < 0))
                fNormalizeAngle180Result -= fDec;

            if (Math.Abs(fNormalizeAngle180Result) > Math.Abs(fNormalizeAngle180Result + fDec)) fNormalizeAngle180Result += fDec;

            if (fNormalizeAngle180Result >= 180f) fNormalizeAngle180Result -= 360f;

            return fNormalizeAngle180Result;
        }

        public static float GetDegreesFromRaw(int iValue, short key)
        {
            //return (iValue / (float)Math.Pow(2, 12 - key)) * 360;
            float fVal = iValue;
            //fVal = fVal / 4096;
            fVal /= (float)Math.Pow(2, 12 - key);
            fVal *= 360;
            return fVal;
            //return ((float)iValue / 4096) * 360;
        }

        public static int GetRawFromDegrees(float fValue, int key)
        {
            //return (int)((fValue / 360f) * Math.Pow(2, 12 - key));
            float fVal = fValue;
            fVal /= 360;
            //fVal = fVal * 4096;
            fVal *= (float)Math.Pow(2, 12 - key);
            int iVal = (int)Math.Round(fVal);
            return iVal;
            //return (int)(fValue / 360f) * 4096;
        }

        public static int GetBitInteger(int iValue, int iBitIndex)
        {
            return (iValue & (int)Math.Pow(2, iBitIndex)) != 0 ? 1 : 0;
        }

        public static int SetBitInteger(int iValue, int iBitIndex, int iBitValue)
        {
            int iSetBitIntegerResult;

            if (iBitValue == 0) iSetBitIntegerResult = iValue & (~(int)Math.Pow(2, iBitIndex));
            else iSetBitIntegerResult = iValue | ((int)Math.Pow(2, iBitIndex));

            return iSetBitIntegerResult;
        }

        public static int InvertBitInteger(int iValue, int iBitIndex)
        {
            int iInvertBitIntegerResult;

            if (GetBitInteger(iValue, iBitIndex) == 1) iInvertBitIntegerResult = SetBitInteger(iValue, iBitIndex, 0);
            else iInvertBitIntegerResult = SetBitInteger(iValue, iBitIndex, 1);

            return iInvertBitIntegerResult;
        }



        //  -------------------------------------------------------------------------------------------------
        //  ======================================= PEDITOR PROCEDURES ======================================
        //  -------------------------------------------------------------------------------------------------
        public static int GetBrightness(int iR, int iG, int iB)
        {
            return (int)((iR + iG + iB) / 3f);
        }

        public static void FillColorTable(PModel Model, ref List<Color> colorTable,
                                                        ref PairIB[] translationTableVertex, ref PairIB[] translationTablePolys,
                                                        byte iThreshold)
        {
            float v;
            double dv;
            int iC, it, i, iDiff;
            Color cColor;

            colorTable.Clear();

            //colorTable = new Color[Model.Header.numVerts + Model.Header.numPolys];
            translationTablePolys = new PairIB[Model.Header.numPolys];
            translationTableVertex = new PairIB[Model.Header.numVerts];

            for (it = 0; it < Model.Header.numVerts; it++)
            {
                cColor = Model.Vcolors[it];

                v = GetBrightness(cColor.R, cColor.G, cColor.B);
                //  Debug.Print "Brightness(" + Str$(it) + "):" + Str$(v)

                if (v == 0) dv = 255;
                else dv = Math.Round(128 / v, 15);

                iC = -1;
                iDiff = 765;

                for (i = 0; i < colorTable.Count; i++)
                {
                    if (colorTable[i].R <= Math.Min(255, cColor.R + iThreshold) &&
                        colorTable[i].R >= Math.Max(0, cColor.R - iThreshold) &&
                        colorTable[i].G <= Math.Min(255, cColor.G + iThreshold) &&
                        colorTable[i].G >= Math.Max(0, cColor.G - iThreshold) &&
                        colorTable[i].B <= Math.Min(255, cColor.B + iThreshold) &&
                        colorTable[i].B >= Math.Max(0, cColor.B - iThreshold))
                    {
                        if (Math.Abs(cColor.R - colorTable[i].R) +
                            Math.Abs(cColor.G - colorTable[i].G) +
                            Math.Abs(cColor.B - colorTable[i].B) < iDiff)
                        {
                            iDiff = Math.Abs(cColor.R - colorTable[i].R) +
                                    Math.Abs(cColor.G - colorTable[i].G) +
                                    Math.Abs(cColor.B - colorTable[i].B);

                            iC = i;
                        }
                    }
                }

                if (iC == -1)
                {
                    colorTable.Add(Color.FromArgb(255, cColor.R, cColor.G, cColor.B));
                    iC = colorTable.Count - 1;
                }

                translationTableVertex[it].I = iC;
                translationTableVertex[it].B = (float)Math.Round(dv, 7);
            }

            for (it = 0; it < Model.Header.numPolys; it++)
            {
                cColor = Model.Pcolors[it];

                v = GetBrightness(cColor.R, cColor.G, cColor.B);

                if (v == 0) dv = 255;
                else dv = Math.Round(128 / v, 15);

                iC = -1;
                iDiff = 765;

                for (i = 0; i < colorTable.Count; i++)
                {
                    if (colorTable[i].R <= Math.Min(255, cColor.R + iThreshold) &&
                        colorTable[i].R >= Math.Max(0, cColor.R - iThreshold) &&
                        colorTable[i].G <= Math.Min(255, cColor.G + iThreshold) &&
                        colorTable[i].G >= Math.Max(0, cColor.G - iThreshold) &&
                        colorTable[i].B <= Math.Min(255, cColor.B + iThreshold) &&
                        colorTable[i].B >= Math.Max(0, cColor.B - iThreshold))
                    {
                        if (Math.Abs(cColor.R - colorTable[i].R) +
                            Math.Abs(cColor.G - colorTable[i].G) +
                            Math.Abs(cColor.B - colorTable[i].B) < iDiff)
                        {
                            iDiff = Math.Abs(cColor.R - colorTable[i].R) +
                                    Math.Abs(cColor.G - colorTable[i].G) +
                                    Math.Abs(cColor.B - colorTable[i].B);

                            iC = i;
                        }
                    }
                }

                if (iC == -1)
                {
                    colorTable.Add(Color.FromArgb(255, cColor.R, cColor.G, cColor.B));
                    iC = colorTable.Count - 1;
                }

                translationTablePolys[it].I = iC;

                if (dv == 0) translationTablePolys[it].B = 0.001f;
                else translationTablePolys[it].B = (float)Math.Round(dv, 7);
            }
        }

        // -- This function is changed from KimeraVB6. I use direct palette color (no Brightness)
        //public static void FillColorTable(PModel Model, ref List<Color> colorTable, ref int nColors,
        //                                  ref pairIB[] translationTableVertex, ref pairIB[] translationTablePolys,
        //                                  byte iThreshold)
        //{
        //    float v;
        //    double dv;
        //    int tmpR, tmpG, tmpB, iC, it, i, iDiff;
        //    Color cColor;

        //    //colorTable = new Color[Model.Header.numVerts + Model.Header.numPolys];
        //    translationTablePolys = new pairIB[Model.Header.numPolys];
        //    translationTableVertex = new pairIB[Model.Header.numVerts];

        //    for (it = 0; it < Model.Header.numVerts; it++)
        //    {
        //        cColor = Model.Vcolors[it];

        //        v = GetBrightness(cColor.R, cColor.G, cColor.B);
        //        //  Debug.Print "Brightness(" + Str$(it) + "):" + Str$(v)

        //        if (v == 0) dv = 255;
        //        else dv = 128 / v;

        //        tmpR = Math.Min(255, (int)Math.Truncate(cColor.R * dv));
        //        tmpG = Math.Min(255, (int)Math.Truncate(cColor.G * dv));
        //        tmpB = Math.Min(255, (int)Math.Truncate(cColor.B * dv));
        //        iC = -1;
        //        iDiff = 765;

        //        for (i = 0; i < nColors; i++)
        //        {
        //            if ((colorTable[i].R <= Math.Min(255, tmpR + iThreshold) &&
        //                 colorTable[i].R >= Math.Max(0, tmpR - iThreshold)) &&
        //                (colorTable[i].G <= Math.Min(255, tmpG + iThreshold) &&
        //                 colorTable[i].G >= Math.Max(0, tmpG - iThreshold)) &&
        //                (colorTable[i].B <= Math.Min(255, tmpB + iThreshold) &&
        //                 colorTable[i].B >= Math.Max(0, tmpB - iThreshold)))
        //            {
        //                if (Math.Abs(tmpR - colorTable[i].R) +
        //                    Math.Abs(tmpG - colorTable[i].G) +
        //                    Math.Abs(tmpB - colorTable[i].B) < iDiff)
        //                {
        //                    iDiff = Math.Abs(tmpR - colorTable[i].R) +
        //                            Math.Abs(tmpG - colorTable[i].G) +
        //                            Math.Abs(tmpB - colorTable[i].B);

        //                    iC = i;
        //                }
        //            }
        //        }

        //        if (iC == -1)
        //        {
        //            colorTable.Add(Color.FromArgb(255, tmpR, tmpG, tmpB));

        //            nColors++;
        //        }

        //        if (iC == -1) iC = nColors - 1;

        //        translationTableVertex[it].I = iC;
        //        translationTableVertex[it].B = (float)dv;
        //    }

        //    for (it = 0; it < Model.Header.numPolys; it++)
        //    {
        //        cColor = Model.Pcolors[it];

        //        v = GetBrightness(cColor.R, cColor.G, cColor.B);

        //        if (v == 0) dv = 128;
        //        else dv = 128 / v;

        //        tmpR = Math.Min(255, (int)Math.Truncate(cColor.R * dv));
        //        tmpG = Math.Min(255, (int)Math.Truncate(cColor.G * dv));
        //        tmpB = Math.Min(255, (int)Math.Truncate(cColor.B * dv));

        //        iC = -1;
        //        iDiff = 765;

        //        for (i = 0; i < nColors; i++)
        //        {
        //            if ((colorTable[i].R <= Math.Min(255, tmpR + iThreshold) &&
        //                 colorTable[i].R >= Math.Max(0, tmpR - iThreshold)) &&
        //                (colorTable[i].G <= Math.Min(255, tmpG + iThreshold) &&
        //                 colorTable[i].G >= Math.Max(0, tmpG - iThreshold)) &&
        //                (colorTable[i].B <= Math.Min(255, tmpB + iThreshold) &&
        //                 colorTable[i].B >= Math.Max(0, tmpB - iThreshold)))
        //            {
        //                if (Math.Abs(tmpR - colorTable[i].R) +
        //                    Math.Abs(tmpG - colorTable[i].G) +
        //                    Math.Abs(tmpB - colorTable[i].B) < iDiff)
        //                {
        //                    iDiff = Math.Abs(tmpR - colorTable[i].R) +
        //                            Math.Abs(tmpG - colorTable[i].G) +
        //                            Math.Abs(tmpB - colorTable[i].B);

        //                    iC = i;
        //                }
        //            }
        //        }

        //        if (iC == -1)
        //        {
        //            colorTable.Add(Color.FromArgb(255, tmpR, tmpG, tmpB));

        //            iC = nColors - 1;
        //        }

        //        translationTablePolys[it].I = iC;

        //        if (dv == 0) translationTablePolys[it].B = 0.001f;
        //        else translationTablePolys[it].B = (float)dv;
        //    }
        //}

        public static void ApplyColorTable(ref PModel Model, List<Color> colorTable, PairIB[] translationTableVertex,
                                                                                     PairIB[] translationTablePolys)
        {
            int iPolyIdx, iVertIdx;
            Color cColor;

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                cColor = colorTable[translationTableVertex[iVertIdx].I];

                if (!Model.Groups[GetVertexGroup(Model, iVertIdx)].HiddenQ)
                {
                    Model.Vcolors[iVertIdx] = Color.FromArgb(255, cColor.R, cColor.G, cColor.B);
                }
            }

            // -- This is not in KimeraVB6
            for (iPolyIdx = 0; iPolyIdx < Model.Header.numPolys; iPolyIdx++)
            {
                cColor = colorTable[translationTablePolys[iPolyIdx].I];

                if (!Model.Groups[GetPolygonGroup(Model, iPolyIdx)].HiddenQ)
                {
                    Model.Pcolors[iPolyIdx] = Color.FromArgb(255, cColor.R, cColor.G, cColor.B);
                }
            }
        }

        // -- This function is changed from KimeraVB6. I use direct palette color (no Brightness)
        //public static void ApplyColorTable(ref PModel Model, List<Color> colorTable, pairIB[] translationTableVertex,
        //                                                                             pairIB[] translationTablePolys)
        //{
        //    int vi, pi;
        //    Color cColor;
        //    float dv;

        //    for (vi = 0; vi < Model.Header.numVerts; vi++)
        //    {
        //        cColor = colorTable[translationTableVertex[iVertIdx].I];
        //        dv = translationTableVertex[iVertIdx].B;

        //        Model.Vcolors[iVertIdx] = Color.FromArgb(255,
        //                                           (byte)Math.Max(0, Math.Min(255, Math.Ceiling(cColor.R / dv))),
        //                                           (byte)Math.Max(0, Math.Min(255, Math.Ceiling(cColor.G / dv))),
        //                                           (byte)Math.Max(0, Math.Min(255, Math.Ceiling(cColor.B / dv))));
        //    }

        //}

        //public static void ChangeBrightness(ref PModel Model, int iFactor, Color[] vcolorsOriginal, Color[] pcolorsOriginal)
        public static void ChangeBrightness(ref PModel Model, int iFactor, Color[] vcolorsOriginal)
        {
            int iVertIdx;

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {

                Model.Vcolors[iVertIdx] = Color.FromArgb(vcolorsOriginal[iVertIdx].A,       // 255
                                Math.Max(0, Math.Min(255, vcolorsOriginal[iVertIdx].R + iFactor)),
                                Math.Max(0, Math.Min(255, vcolorsOriginal[iVertIdx].G + iFactor)),
                                Math.Max(0, Math.Min(255, vcolorsOriginal[iVertIdx].B + iFactor)));
            }

            ComputePColors(ref Model);

            //for (iPolyIdx = 0; iPolyIdx < Model.Header.numPolys; iPolyIdx++)
            //{
            //    Model.Pcolors[iPolyIdx] = Color.FromArgb(255,
            //                         Math.Max(0, Math.Min(255, pcolorsOriginal[iPolyIdx].R + iFactor)),
            //                         Math.Max(0, Math.Min(255, pcolorsOriginal[iPolyIdx].G + iFactor)),
            //                         Math.Max(0, Math.Min(255, pcolorsOriginal[iPolyIdx].B + iFactor)));
            //}
        }

        public static void UpdateTranslationTable(ref PairIB[] translationTableVertex, 
                                                  PModel Model, int pIndex, int cIndex)
        {
            int iVertIdx, iGroupIdx, iDiff, baseVert;

            iDiff = Model.Header.numVerts - 1 - (translationTableVertex.Length - 1);

            iGroupIdx = GetPolygonGroup(Model, pIndex);
            baseVert = Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert - 1 - iDiff;

            Array.Resize(ref translationTableVertex, Model.Header.numVerts);

            for (iVertIdx = Model.Header.numVerts - 1; iVertIdx >= baseVert + 1; iVertIdx--)
            {
                translationTableVertex[iVertIdx].I = translationTableVertex[iVertIdx - iDiff].I;
                translationTableVertex[iVertIdx].B = translationTableVertex[iVertIdx - iDiff].B;
            }

            for (iVertIdx = baseVert + 1; iVertIdx <= baseVert + iDiff; iVertIdx++)
            {
                translationTableVertex[iVertIdx].I = cIndex;
                translationTableVertex[iVertIdx].B = 1;
            }
        }

        public static float CalculatePoint2LineProjectionPosition(Point3D q, Point3D p1, Point3D p2)
        {
            float alpha;
            Point3D vdUP3D = new Point3D(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);

            alpha = (float)((vdUP3D.x * (q.x - p1.x) + vdUP3D.y * (q.y - p1.y) + vdUP3D.z * (q.z - p1.z)) /
                            (Math.Pow(vdUP3D.x, 2) + Math.Pow(vdUP3D.y, 2) + Math.Pow(vdUP3D.z, 2)));

            if (alpha > 1) alpha = 1;
            if (alpha < -1) alpha = -1;

            return alpha;
        }

        public static Point3D CalculatePoint2LineProjection(Point3D q, Point3D p1, Point3D p2)
        {
            float alpha;

            alpha = CalculatePoint2LineProjectionPosition(q, p1, p2);

            return GetPointInLine(p1, p2, alpha);
        }

        public static bool FindWindowOpened (string strWindowName)
        {
            foreach (Form itmFrm in Application.OpenForms)
            {
                if (itmFrm.Name == strWindowName) return true;
            }

            return false;
        }

        // This function will check if there are any duplicated vertices or duplicated polys indexes
        // in Add Polygon feature of PEditor.
        // iArrayVNP = VertexNewPoly        iVCNP = VertexCountNewPoly
        public static bool ValidateAddPolygonVertices(PModel Model, ushort[] iArrayVNP, int iVCNP)
        {
            bool bValidateVerts = true;
            int iGrpv0, iGrpv1, iGrpv2;
            Point3D p3Dv0, p3Dv1, p3Dv2;

            if (iVCNP > 1)
            {
                switch (iVCNP)
                {
                    case 2:
                        p3Dv0 = Model.Verts[iArrayVNP[0]];
                        p3Dv1 = Model.Verts[iArrayVNP[1]];

                        iGrpv0 = GetVertexGroup(Model, iArrayVNP[0]);
                        iGrpv1 = GetVertexGroup(Model, iArrayVNP[1]);

                        if (iArrayVNP[0] == iArrayVNP[1] || ComparePoints3D(p3Dv0, p3Dv1) || iGrpv0 != iGrpv1)
                            bValidateVerts = false;

                        break;

                    case 3:
                        p3Dv0 = Model.Verts[iArrayVNP[0]];
                        p3Dv1 = Model.Verts[iArrayVNP[1]];
                        p3Dv2 = Model.Verts[iArrayVNP[2]];

                        iGrpv0 = GetVertexGroup(Model, iArrayVNP[0]);
                        iGrpv1 = GetVertexGroup(Model, iArrayVNP[1]);
                        iGrpv2 = GetVertexGroup(Model, iArrayVNP[2]);

                        if (iArrayVNP[0] == iArrayVNP[1] || iArrayVNP[0] == iArrayVNP[2] || iArrayVNP[1] == iArrayVNP[2] ||
                            ComparePoints3D(p3Dv0, p3Dv1) || ComparePoints3D(p3Dv0, p3Dv2) || ComparePoints3D(p3Dv1, p3Dv2) ||
                            iGrpv0 != iGrpv1 || iGrpv0 != iGrpv2)
                            bValidateVerts = false;

                        break;
                }
            }

            return bValidateVerts;
        }






    }
}
