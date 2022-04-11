using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Defines;

    using static frmSkeletonEditor;

    using static FF7Skeleton;
    using static FF7FieldSkeleton;
    using static FF7PModel;

    using static FF7BattleSkeleton;
    using static FF7BattleAnimation;
    using static FF7BattleAnimationsPack;

    using static Utils;
    using static OpenGL32;

    class Lighting
    {

        public const int LIGHT_STEPS = 20;

        public static void SetLights()
        {
            float light_x, light_y, light_z;

            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            float scene_diameter;

            if (!bchkFrontLight && !bchkRearLight &&
                !bchkRightLight && !bchkLeftLight)
            {
                glDisable(glCapability.GL_LIGHTING);
                return;
            }

            switch (modelType)
            {
                case K_P_FIELD_MODEL:
                case K_P_BATTLE_MODEL:
                case K_P_MAGIC_MODEL:
                case K_3DS_MODEL:
                    ComputePModelBoundingBox(fPModel, ref p_min, ref p_max);
                    break;

                case K_HRC_SKELETON:
                    ComputeFieldBoundingBox(fSkeleton, fAnimation.frames[iCurrentFrameScroll], ref p_min, ref p_max);
                    break;

                case K_AA_SKELETON:
                    //ComputeAABoundingBox aa_sk, DAAnims.BodyAnimations(anim_index).Frames(CurrentFrameScroll.value), p_min, p_max
                    ComputeBattleBoundingBox(bSkeleton, bAnimationsPack.SkeletonAnimations[ianimIndex].frames[iCurrentFrameScroll],
                                             ref p_min, ref p_max);
                    break;
            }

            scene_diameter = -(float)(2 * ComputeSceneRadius(p_min, p_max));

            light_x = scene_diameter / LIGHT_STEPS * fLightPosXScroll;
            light_y = scene_diameter / LIGHT_STEPS * fLightPosYScroll;
            light_z = scene_diameter / LIGHT_STEPS * fLightPosZScroll;

            if (bchkRightLight) SetLighting(glCapability.GL_LIGHT0, light_z, light_y, light_x, 0.5f, 0.5f, 0.5f, infinityFarQ);
            else
                glDisable(glCapability.GL_LIGHT0);

            if (bchkLeftLight) SetLighting(glCapability.GL_LIGHT1, -light_z, light_y, light_x, 0.5f, 0.5f, 0.5f, infinityFarQ);
            else
                glDisable(glCapability.GL_LIGHT1);

            if (bchkFrontLight) SetLighting(glCapability.GL_LIGHT2, light_x, light_y, light_z, 1f, 1f, 1f, infinityFarQ);
            else
            glDisable(glCapability.GL_LIGHT2);

            if (bchkRearLight) SetLighting(glCapability.GL_LIGHT3, light_x, light_y, -light_z, 0.75f, 0.75f, 0.75f, infinityFarQ);
            else
            glDisable(glCapability.GL_LIGHT3);
        }

        public static void SetLighting(glCapability lightNumber, float x, float y, float z, float red, float green, float blue, bool infinityFarQ)
        {
            float[] l_color = new float[4];
            float[] l_pos = new float[4];

            l_pos[0] = x;
            l_pos[1] = y;
            l_pos[2] = z;

            if (infinityFarQ) l_pos[3] = 0;
            else l_pos[3] = 1;

            l_color[0] = red;
            l_color[1] = green;
            l_color[2] = blue;
            l_color[3] = 1;

            glEnable(glCapability.GL_LIGHTING);
            glDisable(lightNumber);

            glLightfv(lightNumber, glLightParameter.GL_POSITION, l_pos);
            glLightfv(lightNumber, glLightParameter.GL_DIFFUSE, l_color);

            glEnable(lightNumber);
        }
    }
}
