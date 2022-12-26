
using System;
using Game.Common;
using UnityEngine;

namespace BallBlast
{
    public class BBBorderManager : SingletonObserverBase<BBBorderManager>
    {
        BBBorderRefHolderMB m_BorderHolderInstance;


        public void Init(BBBorderRefHolderMB inInstance)
        {
            m_BorderHolderInstance = inInstance;

            RePositionBorder(m_BorderHolderInstance);
        }

        private void RePositionBorder(BBBorderRefHolderMB inBorderHolderInstance)
        {
            var spriteSize = inBorderHolderInstance.Borders[0].GetComponent<SpriteRenderer>().bounds.size;
            var spriteHalfWidth = spriteSize.x / 2;

            var borderArry = m_BorderHolderInstance.Borders;
            int totalBorderObject = borderArry.Length;

            //Four sides so
            for (int i = 0; i <= totalBorderObject - 4; i++)
            {
                Vector2 positionVector = Vector2.zero;
                //Left And Right
                BBBorder borderObject = inBorderHolderInstance.Borders[i]; //Left

                positionVector.x = -(Mathf.Abs(BBScreenSize.Instance().Left) + spriteHalfWidth);
                borderObject.transform.position = positionVector;
                borderObject.BorderPlacementDirection = Direction.eLeft;

                borderObject = inBorderHolderInstance.Borders[i + 1]; //Right
                positionVector.x = Mathf.Abs(positionVector.x);
                borderObject.transform.position = positionVector;
                borderObject.BorderPlacementDirection = Direction.eRight;

                // Bottom and Top
                // Roate z = 90;

                borderObject = inBorderHolderInstance.Borders[i + 2]; //Right
                positionVector.x = 0;
                var angleOFRotation = Quaternion.Euler(0, 0, 90); ;
                positionVector.y = (BBScreenSize.Instance().Top) + spriteHalfWidth;
                borderObject.transform.position = positionVector;
                borderObject.transform.rotation = angleOFRotation;
                borderObject.BorderPlacementDirection = Direction.eTop;


                positionVector.y = -(positionVector.y);
                borderObject = inBorderHolderInstance.Borders[i + 3]; //Right
                borderObject.transform.position = positionVector;
                borderObject.transform.rotation = angleOFRotation;
                borderObject.BorderPlacementDirection = Direction.eBottom;
            }
        }

        public void DidWeHitBorder(Transform inTransform, out Direction outDir)
        {
            outDir = Direction.eNone;

            int objectHashCode = inTransform.GetHashCode();

            var borderObject = m_BorderHolderInstance.Borders;

            foreach (var border in borderObject)
            {
                if (border.transform.GetHashCode() == objectHashCode)
                {
                    outDir = border.BorderPlacementDirection;
                    break;
                }
            }
        }
    }
}